using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Video;

public class InitLogInEvent : UnityEvent<bool> { }

public class MovieManager : MonoBehaviour {

    /*************--事件的声明--*******************/
    [HideInInspector]
    public InitLogInEvent m_InitLogInEvent;
    [HideInInspector]
    public UnityEvent m_PlayIdleEvent;

    private UGUIManager _UIManager;

    #region

    /**************-VideoClips-*******************/
    private VideoClip _mainGreetClip;//打招呼视频
    private VideoClip _mainIdleClip;//坐着循环视频
    private VideoClip _mainNodClip;//点头视频
    private VideoClip _mainShakeHeadClip;//摇头视频
    private VideoClip _mainOnHeadClip;//触摸头部视频
    private VideoClip _mainOnBodyClip;//触摸身体视频

    private VideoClip _sleepOneClip;//睡眠视频1
    private VideoClip _sleepTwoClip;//睡眠视频2

    private VideoClip _levelUpClip;//升级视频


    private VideoClip _idle;//游戏idle视频
    private VideoClip _shearclip;//剪刀视频
    private VideoClip _clowrapclip;//包袱视频
    private VideoClip _maulerclip;//拳头视频
    private VideoClip _gameVictoryClip;//游戏胜利视频
    private VideoClip _gameLostClip;//游戏失败视频

    #endregion

    #region 播放器

    //摄像机
    private Camera _mainCamera;//Movie渲染摄像机
    private RenderTexture _mainRenderTexture;//主渲染纹理
    //播放器组
    private Transform _mainVideoPlayers;//主界面播放器组
    private Transform _mainScenePlayers;//主界面播放器
    private Transform _otherPlayers;//到招呼/升级/睡眠播放器组
    private Transform _gameVideoPlayers;//游戏界面播放器组
    private Transform _CoaxSleepPlayers;//哄睡播放器组


    private VideoPlayer _leveUpPlayer;

    private VideoPlayer _otherPlayer;//其他播放器
    private AudioSource _audioSource;//其他播放器的声音组件
    private AudioSource _gameAudioSource;//游戏声音组件
    //播放器

    private VideoPlayer _idlePlayer;// 主界面播放器
    private VideoPlayer _greetPlayer;//打招呼播放器
    private VideoPlayer _onHeadPlayer;//摸头播放器
    private VideoPlayer _onBodyPlayer;//摸身体播放器
    private VideoPlayer _levelUpPlayer;//升级播放器
    private VideoPlayer _sleepPlayer;//睡眠播放器

    private VideoPlayer _idelplayer; //idle播放器
    private VideoPlayer _assiVideoplayer1;//游戏视频播放器1
    private VideoPlayer _assiVideoplayer2;//游戏视频播放器2
    private VideoPlayer _assiVideoplayer3;//游戏视频播放器3
    private VideoPlayer _gameVictory;//游戏成功视频播放器
    private VideoPlayer _gameLost;//游戏失败视频播放器
    #endregion

    private VideoPlayer vplayer;//用于接收是否播放完毕的播放器

    private bool isWakeUp = false;// 睡眠是否结束

    private Coroutine movieCoro;//用于接受播放视频协程

    [HideInInspector]
    public bool isStart = false;//用于切换开头视频


    private void Awake()
    {






        //主渲染摄像机
        _mainCamera = GetComponent<Camera>();
        //主场景播放器
        _mainScenePlayers = this.transform.GetChild(0);
        //其他播放器
        _otherPlayers = this.transform.GetChild(1);
        //加载游戏播放器组
        _gameVideoPlayers = this.transform.GetChild(2);
        //加载哄睡播放器组
        _CoaxSleepPlayers = this.transform.GetChild(3);

        _otherPlayer = _otherPlayers.GetComponent<VideoPlayer>();//加载多用途播放器
        _audioSource = _otherPlayers.GetComponent<AudioSource>();//加载多用途播放器声音组件
        /*
        foreach (var item in _mainVideoPlayers.GetComponentsInChildren<VideoPlayer>())
        {

            m_VideoPlayers.Add(item);


        }

        _leveUpPlayer = m_VideoPlayers[4];

        _leveUpPlayer.clip = Resources.Load<VideoClip>("Movies/levelUpVideo/Uplevel");
        _leveUpPlayer.SetTargetAudioSource(0, _leveUpPlayer.transform.GetComponent<AudioSource>());
        _leveUpPlayer.Play();
        _leveUpPlayer.Pause();

        */

        MainCacheMovieMethod(false);

        //加载视频
        //MainLoadMovie(false);

        // LoadingCoro = StartCoroutine(  MainCacheMovieMethod(false, "load"));

        //UI管理器
        _UIManager = GameObject.Find("Canvas").GetComponent<UGUIManager>();


        #region 自动睡眠事件 播放打招呼视频或睡眠视频

        if (m_InitLogInEvent == null)
        {
            m_InitLogInEvent = new InitLogInEvent();

            m_InitLogInEvent.AddListener((bool isSleep) => {

                if (isSleep)// true： 播放睡眠视频
                {
                    //isWakeUp = isSleep;

                    // movieCoro = StartCoroutine(MainPlay(_sleepVideoPlayer, "sleep"));//播放自动睡眠视频

                    StartCoroutine("MainLoopPlay", "sleep");

                }
                else //false ： 播放打招呼视频
                {
                    // isWakeUp = true;

                    StartCoroutine("MainLoopPlay", "greet");

                }






            });

        }



        #endregion

        #region 自动睡眠结束后播放idle视频

        if (m_PlayIdleEvent == null)
        {
            m_PlayIdleEvent = new UnityEvent();

            m_PlayIdleEvent.AddListener(() => {

                isWakeUp = true;


            });

        }

        #endregion



        #region 主界面按钮事件管理
        //触摸头部事件
        _UIManager._touchHeadBtn.onClick.AddListener(() => {



            //播放触摸头部视频
            // movieCoro = StartCoroutine(MainPlay(_onHeadVideoPlayer, "head"));

            StartCoroutine("MainLoopPlay", "head");


        });
        //触摸身体事件
        _UIManager._touchBodyBtn.onClick.AddListener(() => {

            //播放触摸身体视频
            // movieCoro = StartCoroutine(MainPlay(_onBodyVideoPlayer, "body"));

            StartCoroutine("MainLoopPlay", "body");


        });

        //升级事件
        _UIManager._levelUpBtn.onClick.AddListener(() => {

            //播放升级视频
            // movieCoro = StartCoroutine(MainPlay(_levelUpVideoPlayer, "levelUp"));

            StartCoroutine("MainLoopPlay", "levelUp");




        });



        #endregion

        #region 游戏界面按钮事件

        //按下玩耍按钮 进入猜拳小游戏界面
        _UIManager._gameBtn.onClick.AddListener(() => {

            //调用加载页面
            StartCoroutine("LoadingGame", true);


        });

        //按下哄睡按钮， 进入猜拳小游戏界面
        _UIManager._sleepBtn.onClick.AddListener(() => {

            /*
             * 
            //停止主界面视频播放，释放主界面视频空间
            _mainVideoPlayer.Stop();
            _mainVideoPlayer.targetCamera = null;
            //释放主界面资源
            MainLoadMovie(true);
            //调用淡入淡出方法
            // StartCoroutine("FadeInOut");
            //调用加载页面

            /***开始预加载视频***
            //调用预加载视频方法
            PreLoadMovie(false);

            //播放idle视频

            StartCoroutine("DealVideo", 0);
            */

        });

        //按下剪刀图标按钮事件
        _UIManager._gameJBtn.onClick.AddListener(() => {

            StartCoroutine("DealVideo", 1);

        });
        //按下包袱图标按钮事件
        _UIManager._gameBBtn.onClick.AddListener(() => {

            StartCoroutine("DealVideo", 2);

        });
        //按下拳头图标按钮事件
        _UIManager._gameQBtn.onClick.AddListener(() => {

            StartCoroutine("DealVideo", 3);

        });
        //按下游戏界面返回图标按钮事件
        _UIManager._gameReturn.onClick.AddListener(() => {



            //调用加载页面
            StartCoroutine("LoadingGame", false);


        });
        #endregion


        // MainCacheMovieMethod2(false);


    }
    //VideoClip veclip;
    void Start() {

        // StartCoroutine("MainLoopPlay", "sleep");

        //MainCacheMovieMethod2(false);




    }

    #region 视频切换方法
    IEnumerator SwitchVideo()
    {
        _UIManager._loadingPanel.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        _UIManager.StartCoroutine("ExecuteUp");//上隐

        yield return new WaitUntil(() => _UIManager.UpFinished == true);//等到上隐完成


        //这里播放切换后的视频
        StartCoroutine("MainLoopPlay", "sleep");

        _UIManager.StartCoroutine("ExecuteDown");//下现

        yield return new WaitUntil(() => _UIManager.DownFinished == true);//等到下现完成

        _UIManager._loadingPanel.gameObject.SetActive(false);


        StopCoroutine("SwitchVideo");


    }
    #endregion

    #region //加载游戏场景方法
    IEnumerator LoadingGame(bool loadOrquit) // true 为进入游戏， false为离开游戏
    {

        _UIManager._loadingPanel.gameObject.SetActive(true);


        yield return new WaitForSeconds(0.3f);

        _UIManager.StartCoroutine("ExecuteUp");//上隐

        yield return new WaitForSeconds(1.4f);


        //释放主界面资源

        if (loadOrquit)
        {
            // ReleseCoro = StartCoroutine(MainCacheMovieMethod(loadOrquit, "relese"));

            MainCacheMovieMethod(true); //释放主界面视频资源

        }
        else
        {

            // LoadingCoro = StartCoroutine(MainCacheMovieMethod(loadOrquit, "load"));

            MainCacheMovieMethod(false);//加载主界面视频资源

        }



        // StartCoroutine("MainCacheMovieMethod", loadOrquit);

        PreLoadMovie(!loadOrquit); // 加载或释放游戏资源

        _UIManager.SwitchScene(!loadOrquit); //关闭游戏面板


        // yield return new WaitUntil(() => isLoading == true );

        yield return new WaitForSeconds(2f);

        _UIManager.SwitchScene(loadOrquit); //打开游戏面板

        _UIManager.StartCoroutine("ExecuteDown");//下现


        if (loadOrquit)
        {
            StartCoroutine("DealVideo", 0);

        }
        else
        {
            // movieCoro = StartCoroutine(MainPlay(_mainVideoPlayer, "idle"));

            StartCoroutine("MainLoopPlay", "idle");

        }





        yield return new WaitForSeconds(2f);

        _UIManager._loadingPanel.gameObject.SetActive(false);


        StopCoroutine("LoadingGame");


    }
    #endregion

   

    #region//换装方法 Unity 2.3
    // private Coroutine ReleseCoro;
    // private Coroutine LoadingCoro;

    IEnumerator ReLoadingMain()
    {

        _UIManager._loadingPanel.gameObject.SetActive(true);//显示加载页面


        // yield return new WaitForSeconds(0.3f);

        _UIManager.StartCoroutine("ExecuteUp");//上隐

        yield return new WaitUntil(() => _UIManager.UpFinished);//直到上隐完毕


        //释放主界面资源
        // MainCacheMovieMethod(true);//释放当前视频

        //  ReleseCoro = StartCoroutine(MainCacheMovieMethod(true, "relese"));

        MainCacheMovieMethod(true);


        // MainCacheMovieMethod(false);//加载下一套视频 -----播放器不用释放，只是替换不同的视频资源--待完善

        // LoadingCoro = StartCoroutine(MainCacheMovieMethod(false, "load"));
        MainCacheMovieMethod(false);


        yield return new WaitForSeconds(1.6f);

        StartCoroutine("MainLoopPlay", "greet");

        _UIManager.StartCoroutine("ExecuteDown");//下现



        yield return new WaitUntil(() => _UIManager.DownFinished);//直到下现完毕

        yield return new WaitForSeconds(0.5f);

        isStart = true;//开始切换

        yield return new WaitForSeconds(2f);

        _UIManager._loadingPanel.gameObject.SetActive(false);


        StopCoroutine("ReLoadingMain");


    }
    #endregion 

    #region 主界面轮播 Unity2.2版
    [HideInInspector]
    public bool isLevelUpFinished = false; //升级是否结束
    [HideInInspector]
    public bool IsSleepFinished = false; //睡眠是否结束
    IEnumerator MainLoopPlay(string videoName)
    {


        switch (videoName)
        {
            case "greet":

                PlayerCurVideo(m_VideoPlayers[0], true);//播放idle视频并循环

                StartCoroutine("CacheGreetVideo", 0);//准备打招呼视频

                yield return new WaitUntil(() => isStart == true); // 开始切换到打招呼视频

                StopCurVideo(m_VideoPlayers[0]);//停止idle视频


                PlayerCurVideo(_otherPlayer);//播放打招呼视频



                yield return new WaitUntil(IsFinished);//打招呼播放完毕后切换到idle视频


                StopCurVideo(_otherPlayer, true); //停止打招呼视频

                PlayerCurVideo(m_VideoPlayers[0], true);//播放idle视频


                break;

            case "idle":

                //IsInteractableControl(true);//启用触摸功能
                PlayerCurVideo(m_VideoPlayers[0], true); //播放idle视频

                break;
            case "head":

                StopCurVideo(m_VideoPlayers[0]);//停止idle视频
                IsInteractableControl(false);//禁用触摸功能
                PlayerCurVideo(m_VideoPlayers[2]);//播放触摸头部视频

                yield return new WaitUntil(IsFinished);//触摸头部或身体视频播放完毕后

                StopCurVideo(m_VideoPlayers[2]);//停止播放触摸头部视频
                PlayerCurVideo(m_VideoPlayers[0], true);//播放idle视频
                IsInteractableControl(true);//启用触摸功能

                break;
            case "body":

                StopCurVideo(m_VideoPlayers[0]);//停止idle视频
                IsInteractableControl(false);//禁用触摸功能
                PlayerCurVideo(m_VideoPlayers[1]);//播放触摸身体视频

                yield return new WaitUntil(IsFinished);//触摸头部或身体视频播放完毕后

                StopCurVideo(m_VideoPlayers[1]);//停止播放触摸身体视频
                PlayerCurVideo(m_VideoPlayers[0], true);//播放idle视频
                IsInteractableControl(true);//启用触摸功能

                break;
            case "levelUp":

                StartCoroutine("CacheLevelUpVideo", 0);//准备升级视频

                yield return new WaitForSeconds(1.2f);

                isLevelUpFinished = false;

                StopCurVideo(m_VideoPlayers[0]);//停止idle视频
                IsInteractableControl(false);//禁用触摸功能
                _UIManager.HideUI(false);//隐藏UI
                PlayerCurVideo(_otherPlayer);//播放升级视频----
                                             // Debug.Log(m_VideoPlayers[4].name);
                yield return new WaitUntil(IsFinished);//升级视频播放完毕后

                //调用加载页面切换
                isLevelUpFinished = true;

                StopCurVideo(_otherPlayer, true);//停止播放升级视频
                IsInteractableControl(true);//启用触摸功能
                _UIManager.HideUI(true);//显示UI
                                        //准备视频，准备下一个升级视频
                                        // PrepareNextVideo(m_VideoPlayers[4]);

                // PlayerCurVideo(m_VideoPlayers[1], true);//播放idle视频               
                break;
            case "sleep": //在登陆的时候 如果用到需要加载。。。。。
                StartCoroutine("CacheSleepVideo"); // 准备睡眠视频
                                                   //判断是否停止播放idle视频


                //等到睡眠视频准备完毕
                yield return new WaitUntil(() => IsPrepSleepVideo == true);
                StopCurVideo(m_VideoPlayers[0]);//停止播放idle视频
                IsPrepSleepVideo = false;//准备睡眠视频状态初始化

                IsInteractableControl(false);//禁用触摸功能
                PlayerCurVideo(_otherPlayer, true);//播放睡眠视频
                                                   // yield return new WaitUntil(IsFinished);//睡眠视频播放完毕后

                //调用加载页面切换,播放idle视频
                // StartCoroutine("SwitchVideo");

                yield return new WaitUntil(() => IsSleepFinished == true); //等得到结束睡眠通知后

                IsSleepFinished = false;// 初始化
                StopCurVideo(_otherPlayer, true);//停止播放睡眠视频
                IsInteractableControl(true);//启用触摸功能
                //PlayerCurVideo(m_VideoPlayers[0], true);
                //StartCoroutine("MainLoopPlay", "idle");
                Debug.Log("21212122122122");
                break;

                // 等级提升与睡眠，要根据等级即将提升和即将自动睡眠时，在加载，可节约内存资源。






        }

        StopCoroutine("MainLoopPlay");


    }


    #endregion

    #region// 猜拳游戏 随机播放&判断播放完毕 协程方法。
    IEnumerator DealVideo(int num)
    {


        if (num == 0)
        {


            // StopCurVideo(m_VideoPlayers[1]);//停止主界面视频

            PlayerCurVideo(m_gamePlayer[0], true);//播放游戏idle视频



        }
        else
        {


            // _idelplayer.targetMaterialRenderer = null;
            // _idelplayer.targetCamera = null;

            int index = Random.Range(1, 4);

            _UIManager.GameButtonInteractable(false);

            switch (index) // index 随机数字
            {
                case 1: // 播放剪刀视频

                    /*
                    //_assiVideoplayer1.targetMaterialRenderer = _movieMeshRender;
                    _assiVideoplayer1.targetCamera = _mainCamera;//让摄像机渲染镜头1
                    _assiVideoplayer1.Play();//播放剪刀视频                  
                    vplayer = _assiVideoplayer1;// 判断是否播放完毕

                    /**缓存idle视频**
                    _idelplayer.Play();
                    _idelplayer.Pause();

                    /****/

                    StopCurVideo(m_gamePlayer[0]); // 停止游戏idle视频

                    PlayerCurVideo(m_gamePlayer[1]); //播放剪刀视频



                    yield return new WaitUntil(IsFinished);//播放完毕后执行下面代码


                    StopCurVideo(m_gamePlayer[1]);//停止播放剪刀视频


                    /*
                    /****重新预加载镜头1视频*****
                    _assiVideoplayer1.Play();
                    _assiVideoplayer1.Pause();
                    _assiVideoplayer1.targetCamera = null;
                    //_assiVideoplayer1.targetMaterialRenderer = null;
                    /*******************************
                    */

                    //判断输赢
                    if (num == 1)
                    {
                        //平了
                        Debug.Log("平了。。。。");

                    }
                    else if (num == 2)
                    {
                        Debug.Log("输了。。。。");
                        // 用户输了,少女高兴
                        // _gameLost.targetMaterialRenderer = _movieMeshRender;
                        /*
                         _gameVictory.targetCamera = _mainCamera;//让摄像机渲染失败镜头
                         _gameVictory.Play();//播放失败视频
                         */
                        PlayerCurVideo(m_gamePlayer[4]); //播放胜利视频

                        /*

                        vplayer = _gameVictory;//判断是否播放完毕

                        //红心减一
                        _UIManager.RedPngState(true);
                      
                        */
                    }
                    else if (num == 3)
                    {
                        Debug.Log("胜了。。。。");
                        // 用户胜了,少女不高兴
                        /*
                        //_gameVictory.targetMaterialRenderer = _movieMeshRender;
                        _gameLost.targetCamera = _mainCamera;
                        _gameLost.Play();
                        vplayer = _gameLost;
                       

                        //胜利次数加一
                        _UIManager.GameVictoryMethod();
                        */

                        PlayerCurVideo(m_gamePlayer[5]);//播放失败视频


                    }


                    yield return new WaitUntil(IsFinished);//等待输赢结果视频播放完毕后



                    StopCurVideo(m_gamePlayer[4]);//停止播放胜利视频
                    StopCurVideo(m_gamePlayer[5]);//停止播放成功视频

                    PlayerCurVideo(m_gamePlayer[0], true);//播放idle视频

                    _UIManager.GameButtonInteractable(true);

                    /*
                    /****重新预加载输赢视频*****
                    vplayer.Play();
                    vplayer.Pause();
                     vplayer.targetCamera = null;
                    //vplayer.targetMaterialRenderer = null;
                    /*******************************




                    /****播放idle视频*****
                    //_idelplayer.targetMaterialRenderer = _movieMeshRender;
                    _idelplayer.targetCamera = _mainCamera;                    
                    _idelplayer.Play();
                    /*********
                    _UIManager.GameButtonInteractable(true);

                    if(_UIManager.redCount == 3)
                    {

                        //红心耗尽
                        //调用返回按钮回到主界面
                        _UIManager._gameReturn.onClick.Invoke();


                    }
                    */
                    break;
                case 2: //包袱视频

                    /*
                    //_assiVideoplayer2.targetMaterialRenderer = _movieMeshRender;
                    _assiVideoplayer2.targetCamera = _mainCamera;
                    _assiVideoplayer2.Play();
                    vplayer = _assiVideoplayer2; // 判断是否播放完毕

                    /**缓存idle视频**
                    _idelplayer.Play();
                    _idelplayer.Pause();
                    /****/

                    StopCurVideo(m_gamePlayer[0]); // 停止游戏idle视频

                    PlayerCurVideo(m_gamePlayer[2]); //播放剪刀视频


                    yield return new WaitUntil(IsFinished);//播放完毕后执行下面代码

                    StopCurVideo(m_gamePlayer[2]);//停止播放剪刀视频

                    /*
                    /****重新预加载镜头2视频*****
                    _assiVideoplayer2.Play();
                    _assiVideoplayer2.Pause();
                     _assiVideoplayer2.targetCamera = null;
                    //_assiVideoplayer2.targetMaterialRenderer = null;
                    /*******************************/

                    //判断输赢
                    if (num == 1)
                    {
                        Debug.Log("胜了。。。。");
                        //用户胜了，少女不高兴
                        /*
                       // _gameVictory.targetMaterialRenderer = _movieMeshRender;
                        _gameLost.targetCamera = _mainCamera;
                        _gameLost.Play();
                        vplayer = _gameLost;

                        //胜利次数加一
                        _UIManager.GameVictoryMethod();
                        */

                        PlayerCurVideo(m_gamePlayer[4]); //播放胜利视频


                    }
                    else if (num == 2)
                    {
                        // 平了
                        Debug.Log("平了。。。。");

                    }
                    else if (num == 3)
                    {
                        Debug.Log("输了。。。。");
                        // 用户输了，少女高兴
                        /*
                       // _gameLost.targetMaterialRenderer = _movieMeshRender;
                        _gameVictory.targetCamera = _mainCamera;//让摄像机渲染失败镜头
                        _gameVictory.Play();//播放失败视频
                        vplayer = _gameVictory;//判断是否播放完毕

                        //红心减一
                        _UIManager.RedPngState(true);
                        */

                        PlayerCurVideo(m_gamePlayer[5]);//播放失败视频

                    }




                    yield return new WaitUntil(IsFinished);//等待输赢结果视频播放完毕后


                    StopCurVideo(m_gamePlayer[4]);//停止播放胜利视频
                    StopCurVideo(m_gamePlayer[5]);//停止播放成功视频

                    PlayerCurVideo(m_gamePlayer[0], true);//播放idle视频

                    _UIManager.GameButtonInteractable(true);

                    /*
                    /****重新预加载输赢视频*****
                    vplayer.Play();
                    vplayer.Pause();
                    vplayer.targetCamera = null;
                    //vplayer.targetMaterialRenderer = null;
                    /*******************************

                    /****播放idle视频*****
                    //_idelplayer.targetMaterialRenderer = _movieMeshRender;
                    _idelplayer.targetCamera = _mainCamera;      
                    _idelplayer.Play();
                    /*********
                    _UIManager.GameButtonInteractable(true);

                    if (_UIManager.redCount == 3)
                    {

                        //红心耗尽
                        //调用返回按钮回到主界面
                        _UIManager._gameReturn.onClick.Invoke();


                    }
                    */
                    break;
                case 3://拳头视频

                    /*
                    //_assiVideoplayer3.targetMaterialRenderer = _movieMeshRender;
                    _assiVideoplayer3.targetCamera = _mainCamera;
                    _assiVideoplayer3.Play();                  
                    vplayer = _assiVideoplayer3;// 判断是否播放完毕


                    /**缓存idle视频**
                    _idelplayer.Play();
                    _idelplayer.Pause();
                    /****/
                    StopCurVideo(m_gamePlayer[0]); // 停止游戏idle视频

                    PlayerCurVideo(m_gamePlayer[3]); //播放剪刀视频


                    yield return new WaitUntil(IsFinished);//播放完毕后执行下面代码

                    StopCurVideo(m_gamePlayer[3]);//停止播放剪刀视频

                    /****重新预加载镜头3视频*****
                    _assiVideoplayer3.Play();
                    _assiVideoplayer3.Pause();
                     _assiVideoplayer3.targetCamera = null;
                    //_assiVideoplayer3.targetMaterialRenderer = null;
                    /*******************************/

                    //判断输赢
                    if (num == 1)
                    {
                        Debug.Log("输了。。。。");
                        //用户输了，少女高兴
                        /*
                        // _gameLost.targetMaterialRenderer = _movieMeshRender;
                        _gameVictory.targetCamera = _mainCamera;//让摄像机渲染失败镜头
                        _gameVictory.Play();//播放失败视频
                        vplayer = _gameVictory;//判断是否播放完毕

                        //红心减一
                        _UIManager.RedPngState(true);
                        */
                        PlayerCurVideo(m_gamePlayer[5]);//播放失败视频

                    }
                    else if (num == 2)
                    {
                        Debug.Log("胜了。。。。");
                        // 用户胜了，少女不高兴
                        /*
                        // _gameVictory.targetMaterialRenderer = _movieMeshRender;
                        _gameLost.targetCamera = _mainCamera;
                        _gameLost.Play();
                        vplayer = _gameLost;

                        //胜利次数加一
                        _UIManager.GameVictoryMethod();
                        */
                        PlayerCurVideo(m_gamePlayer[4]); //播放胜利视频

                    }
                    else if (num == 3)
                    {
                        // 平了
                        Debug.Log("平了。。。。");
                    }




                    yield return new WaitUntil(IsFinished);//等待输赢结果视频播放完毕后


                    StopCurVideo(m_gamePlayer[4]);//停止播放胜利视频
                    StopCurVideo(m_gamePlayer[5]);//停止播放成功视频

                    PlayerCurVideo(m_gamePlayer[0], true);//播放idle视频

                    _UIManager.GameButtonInteractable(true);


                    /****重新预加载输赢视频*****
                    vplayer.Play();
                    vplayer.Pause();
                    vplayer.targetCamera = null;
                    //vplayer.targetMaterialRenderer = null;
                    /*******************************

                    /****播放idle视频*****
                    //_idelplayer.targetMaterialRenderer = _movieMeshRender;
                    _idelplayer.targetCamera = _mainCamera;  
                    _idelplayer.Play();
                    /*********

                    _UIManager.GameButtonInteractable(true);

                    if (_UIManager.redCount == 3)
                    {

                        //红心耗尽
                        //调用返回按钮回到主界面
                        _UIManager._gameReturn.onClick.Invoke();


                    }
                    */



                    break;




            }


            // yield return new WaitUntil(TestWait);


        }



    }
    #endregion

    #region 哄睡视频切换处理
    IEnumerator CoaxSleepmethod()
    {

        CoaxSleepLoadMethod(true);

        yield return null;

        PlayerCurVideo(m_CoaxSleepPlayers[4], true);


        








    }

    #endregion

    #region 视频播放准备阶段

    //播放视频
    private void PlayerCurVideo(VideoPlayer curPlayer, bool isLoop = false)
    {
        vplayer = curPlayer;
        //播放 并且循环
        curPlayer.targetCamera = _mainCamera;
        curPlayer.isLooping = isLoop;
        curPlayer.Play();


    }
    //停止并准备视频
    private void StopCurVideo(VideoPlayer curPlayer, bool isRelese = false)
    {
        curPlayer.targetCamera = null;
        // curPlayer.SetTargetAudioSource(0, null);
        curPlayer.isLooping = false;
        curPlayer.Stop();
        if (!isRelese) //isrelese = false
        {
            curPlayer.frame = 0;
            curPlayer.Play();
            curPlayer.Pause();

        }
        else // isrelese = true
        {
           
            Resources.UnloadAsset(curPlayer.clip);

            curPlayer.clip = null;
        }
       


    }

    //准备升级视频
    [HideInInspector]
    public float levelVideo = 0;//与等级相对应的视频
    private void PrepareNextVideo(VideoPlayer levelPlayer)
    {
        string path = "Movies/levelUpVideo/" + levelVideo.ToString();

        levelPlayer.targetCamera = null;
        levelPlayer.Stop();
        if (levelVideo != 3)
        {
            levelPlayer.clip = Resources.Load<VideoClip>(path);

            levelVideo++;

        }



       // levelPlayer.frame = 0;
        levelPlayer.Play();
        levelPlayer.Pause();



    }
    #endregion

    #region//主界面 视频加载/释放 方法 Unity 2.3版
    private List<VideoClip> m_MainVideoClips = new List<VideoClip>();
    List<VideoPlayer> m_VideoPlayers = new List<VideoPlayer>();
    int index = -1;
    public void MainCacheMovieMethod(bool isRelease)// 根据索引index的值来加载不同的视频 true:释放资源 false：加载资源
    {

        


        if (isRelease)//释放资源
        {
            /*
            //释放播放器对象
            foreach (var item in m_VideoPlayers)
            {

                Destroy(item); //销毁组建

            }
            */

            foreach (var item in m_VideoPlayers)
            {

              


                item.clip = null;

                item.targetCamera = null;

            

                

            }


            m_VideoPlayers.Clear(); //清空播放器列表

          

            //卸载视频资源
            foreach (var item in m_MainVideoClips)
            {

                Resources.UnloadAsset(item); // 卸载资源

            }

            m_MainVideoClips.Clear();//清空视频列表



        }
        else//加载资源
        {

            /*
            index++;

            if (index == 3)
            {
                index = 0;
            }
            */

            string path = "Movies/movie2";// + index.ToString();//视频路径

           
            object[] clipObj = Resources.LoadAll<VideoClip>(path); //加载所有的视频资源
          
            foreach (var item in clipObj)
            {

                m_MainVideoClips.Add(item as VideoClip);//添加视频片段（idle/onBody/onHead）


            }

            VideoPlayer[] vp = _mainScenePlayers.GetComponents<VideoPlayer>();//加载主界面的4个播放器（idle/onBody/onHead）

            for (int i = 0; i < vp.Length; i++)
            {

                vp[i].clip = m_MainVideoClips[i];
                vp[i].SetTargetAudioSource(0, vp[i].GetComponent<AudioSource>());
                vp[i].Play();
                vp[i].Pause();
                m_VideoPlayers.Add(vp[i]);//添加到主播放器组

                if (i != 4)
                {
                    /*
                    m_VideoPlayers[i].clip = m_MainVideoClips[i];
                    m_VideoPlayers[i].SetTargetAudioSource(0, m_VideoPlayers[i].transform.GetComponent<AudioSource>());
                    m_VideoPlayers[i].Play();
                    m_VideoPlayers[i].Pause();
                    */

                }
                
            }

        }
        

    }
    #endregion

    #region 主界面加载 打招呼 / 升级 / 睡眠视频

    //打招呼视频
    private IEnumerator CacheGreetVideo(int index)//根据索引加载不同的打招呼视频
    {
        string path = "Movies/greetVideo/greet" + index.ToString();

        ResourceRequest request = Resources.LoadAsync<VideoClip>(path);

        yield return request;

        _otherPlayer.clip = request.asset as VideoClip;
        _otherPlayer.SetTargetAudioSource(0, _audioSource);
       // _otherPlayer.targetCamera = _mainCamera;
        _otherPlayer.Play();
        _otherPlayer.Pause();

        StopCoroutine("CacheGreetVideo");


    }
    //升级视频
    private IEnumerator  CacheLevelUpVideo(int index )//根据索引加载不同的升级视频
    {
        string path = "Movies/levelUpVideo/level" + index.ToString();

        //异步加载升级视频
        ResourceRequest request = Resources.LoadAsync<VideoClip>(path);

        yield return new WaitUntil(() => request.isDone);//等待资源加载完毕后

        _otherPlayer.clip = request.asset as VideoClip;
        _otherPlayer.SetTargetAudioSource(0, _audioSource);
       // _otherPlayer.targetCamera = _mainCamera;
        _otherPlayer.Play();
        _otherPlayer.Pause();

        StopCoroutine("CacheLevelUpVideo");


    }
    //睡眠视频
    [HideInInspector]
    bool IsPrepSleepVideo = false;
    private IEnumerator CacheSleepVideo()
    {
        string path = "Movies/sleep/idle";

        ResourceRequest request = Resources.LoadAsync<VideoClip>(path);

        yield return  request;

        _otherPlayer.clip = request.asset as VideoClip;
        _otherPlayer.SetTargetAudioSource(0, _audioSource);
        // _otherPlayer.targetCamera = _mainCamera;
        _otherPlayer.Play();
        _otherPlayer.Pause();

        IsPrepSleepVideo = true;

        StopCoroutine("CacheLevelUpVideo");



    }
    //视频视频
    private void CacheReleaseVideo()
    {



    }



    #endregion

    #region//游戏页面 预加载/释放 视频方法
    List<VideoClip> m_gameClips = new List<VideoClip>();
    List<VideoPlayer> m_gamePlayer = new List<VideoPlayer>();
    private void PreLoadMovie(bool isRelease)
    {

        if (isRelease)//isRelease = true 释放资源
        {

            //初始化红心状态
            _UIManager.RedPngState(false);


            /*
           //释放播放器对象
           foreach (var item in m_VideoPlayers)
           {

               Destroy(item); //销毁组建

           }
           */

            foreach (var item in m_gamePlayer)
            {
                
                    item.clip = null;

                    item.targetCamera = null;
                
            }


             m_gamePlayer.Clear(); //清空播放器列表

            // Destroy(GetComponent<AudioSource>());//释放声音组件

            //卸载视频资源
            foreach (var item in m_gameClips)
            {

                Resources.UnloadAsset(item); // 卸载资源

            }

            m_gameClips.Clear();//清空视频列表



            /*
            //释放播放器加载的视频
            _idelplayer.Stop();
            _idelplayer.clip = null;  
            _assiVideoplayer1.Stop();
            _assiVideoplayer1.clip = null;           
            _assiVideoplayer2.Stop();
            _assiVideoplayer2.clip = null;          
            _assiVideoplayer3.Stop();
            _assiVideoplayer3.clip = null;          
            _gameLost.Stop();
            _gameLost.clip = null;          
            _gameVictory.Stop();
            _gameVictory.clip = null;

            //释放渲染摄像机
            _idelplayer.targetCamera = null;

            //释放播放器对象
            
            _idelplayer = null;
            _assiVideoplayer1 = null;
            _assiVideoplayer2 = null;
            _assiVideoplayer3 = null;
            _gameLost = null;
            _gameVictory = null;

            //释放加载的视频
            Resources.UnloadAsset(_idle);
            Resources.UnloadAsset(_shearclip);
            Resources.UnloadAsset(_clowrapclip);
            Resources.UnloadAsset(_maulerclip);
            Resources.UnloadAsset(_gameLostClip);
            Resources.UnloadAsset(_gameVictoryClip);

            */

        }
        else //isRelease = false 加载资源
        {

            string path = "Movies/game/";//视频路径


            object[] clipObj = Resources.LoadAll<VideoClip>(path); //加载所有的视频资源



            foreach (var item in clipObj)
            {

                m_gameClips.Add(item as VideoClip);//列表顺序同资源文件在编辑器里的顺序。
               
            }
            foreach (var gamePlayer in _gameVideoPlayers.GetComponents<VideoPlayer>())
            {

                m_gamePlayer.Add(gamePlayer);
               

            }


            for (int i = 0; i < m_gameClips.Count; i++)
            {

                m_gamePlayer[i].clip = m_gameClips[i];
               // m_gamePlayer[i].SetTargetAudioSource(0, m_gamePlayer[i].transform.GetComponent<AudioSource>());
                m_gamePlayer[i].Play();
                m_gamePlayer[i].Pause();

            }

           



            /*

            //加载播放器
            _idelplayer = GameObject.Find("mainVideoPlay").GetComponent<VideoPlayer>(); //idle播放器
            _assiVideoplayer1 = GameObject.Find("game_J").GetComponent<VideoPlayer>(); //镜头1
            _assiVideoplayer2 = GameObject.Find("game_B").GetComponent<VideoPlayer>(); //镜头2
            _assiVideoplayer3 = GameObject.Find("game_Q").GetComponent<VideoPlayer>(); //镜头3
            _gameVictory = GameObject.Find("game_V").GetComponent<VideoPlayer>(); //胜利播放器
            _gameLost = GameObject.Find("game_L").GetComponent<VideoPlayer>(); //失败播放器
            /*
            //加载视频
            _idle = Resources.Load("Movies/idle_00", typeof(VideoClip)) as VideoClip;
            _shearclip = Resources.Load("Movies/game_J", typeof(VideoClip)) as VideoClip;
            _clowrapclip = Resources.Load("Movies/game_B", typeof(VideoClip)) as VideoClip;
            _maulerclip = Resources.Load("Movies/game_Q", typeof(VideoClip)) as VideoClip;
            _gameLostClip = Resources.Load("Movies/game_fail", typeof(VideoClip)) as VideoClip;
            _gameVictoryClip = Resources.Load("Movies/game_victorys", typeof(VideoClip)) as VideoClip;
            
            _idle = Resources.Load("Movies/game/game_Ready", typeof(VideoClip)) as VideoClip;
            _shearclip = Resources.Load("Movies/game/game_JianDao", typeof(VideoClip)) as VideoClip;
            _clowrapclip = Resources.Load("Movies/game/game_Bu", typeof(VideoClip)) as VideoClip;
            _maulerclip = Resources.Load("Movies/game/game_ShiTou", typeof(VideoClip)) as VideoClip;
            _gameLostClip = Resources.Load("Movies/game/game_Fail", typeof(VideoClip)) as VideoClip;
            _gameVictoryClip = Resources.Load("Movies/game/game_Victory", typeof(VideoClip)) as VideoClip;


            //准备播放器
            _idelplayer.clip = _idle;//idle播放器固定播放idl视频
            _idelplayer.Play();
            _idelplayer.Pause();
            _assiVideoplayer1.clip = _shearclip;//镜头1固定播放剪刀视频
            _assiVideoplayer1.Play();
            _assiVideoplayer1.Pause();
            _assiVideoplayer2.clip = _clowrapclip;//镜头2固定播放包袱视频
            _assiVideoplayer2.Play();
            _assiVideoplayer2.Pause();
            _assiVideoplayer3.clip = _maulerclip;//镜头3固定包袱拳头视频
            _assiVideoplayer3.Play();
            _assiVideoplayer3.Pause();
            _gameVictory.clip = _gameVictoryClip; //预加载胜利视频
            _gameVictory.Play();
            _gameVictory.Pause();
            _gameLost.clip = _gameLostClip; //预加载失败视频
            _gameLost.Play();
            _gameLost.Pause();

         */

        }

      

       



        

        
    }

    #endregion

    #region 哄睡界面加载/释放方法

    private List<VideoPlayer> m_CoaxSleepPlayers = new List<VideoPlayer>();
    private List<VideoClip> m_CoaxSleepClip = new List<VideoClip>();
    public void CoaxSleepLoadMethod(bool isRelease)
    {
        if (isRelease) // true 加载
        {
            string path = "Movies/sleep/";

            VideoClip[] obj = Resources.LoadAll<VideoClip>(path);

            foreach (var item in obj)
            {
                m_CoaxSleepClip.Add(item);

                 VideoPlayer player = _CoaxSleepPlayers.gameObject.AddComponent<VideoPlayer>();
                player.clip = item;
                player.playOnAwake = false;
                player.waitForFirstFrame = false;
                player.isLooping = false;
                player.Play();
                player.Pause();
                player.renderMode = VideoRenderMode.CameraFarPlane;
                player.targetCamera = null;

                m_CoaxSleepPlayers.Add(player);

            }

            //加载触摸管理脚本
            _CoaxSleepPlayers.gameObject.AddComponent<TouchManager>();



        }
        else //false 释放
        {

           
            

            foreach (var item in m_CoaxSleepPlayers)
            {


                // Resources.UnloadUnusedAssets(item.clip);

                

                item.clip = null;

                item.targetCamera = null;

                Resources.UnloadAsset(m_CoaxSleepClip[m_CoaxSleepPlayers.IndexOf(item)]); // 卸载资源 

                Destroy(item); //销毁组建

            }
            m_CoaxSleepClip.Clear();
            m_CoaxSleepPlayers.Clear();

        }
        
    }


    #endregion
    //主界面触摸按钮交互控制
    private void IsInteractableControl(bool Actable)
    {
        _UIManager._touchHeadBtn.interactable = Actable;
        _UIManager._touchBodyBtn.interactable = Actable;


    }

    // 协程调用的延迟方法      
    bool IsFinished()
    {

        return IsPlay(vplayer);

    }
    //判断是否播放完毕方法
    bool IsPlay(VideoPlayer vplayer)
    {
        
            if (vplayer != null && vplayer.isPlaying)
            {
                return false;
            }
            else
            {

                return true;

            }

    }

    // Update is called once per frame
    void Update () {

        //调用 判断是否播放完毕方法
        IsPlay(vplayer);
       
	}








}
