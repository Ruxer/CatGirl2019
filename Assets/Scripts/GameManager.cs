using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.UI;
using UnityEngine.Video;

public class GameManager : MonoBehaviour {


    //预定义的一些数值
    [Tooltip("多长时间变化一次")]
    public float oneHour;//= 3600f;//等待1小时 3600s
    private float oneSecond = 1f; //常量值 1秒
    private float checkTimeSpan = 300f;//每5分钟检查一次300s
    private float subMoodPointPerHour = 1f;//心情低于33时，每小时降低1点心情
    private float maxSubMoodPoint = 5f;//心情低于33时，最多降低5点心情
    private float subBellyPointPerHour = 5f;//每小时降低的饱食度
    private float subSleepingPointPerHour = 3f;//每小时降低的睡眠度
    private float lowerLimitBellyPoint = 33f;//当饱腹度低于33时心情开始降低
    private float recoverSleepingPoint = 60f;//自动睡眠后恢复60点
    private float subMoodPointAutoSleeping = 3f;//自动睡眠后降低3点心情
    private float sleepingDuration = 8f;//睡眠时长

    private PlayerDataUtils playerDataUtils;//工具类
    private int subtractedMoodPoint = 0;
    //private IEnumerator oldUpdateMoodStatusEnum;
    //private IEnumerator updateMoodStatusEnum;
    //private IEnumerator updateBellyStatusEnum;
    //private IEnumerator updateSleepingStatusEnum;
    //private IEnumerator checkStatusEnum;

    private UGUIManager _UIManager;//UI布局管理类

    private MovieManager _MovieManager;//视频播放管理器

    private WorkDialog _WorkDialogManger;//打工对话管理器

    private GameObject movieManagerObject;//视频播放对象
    private GameObject CanvasManagerObject;//画布对象
    private GameObject DialogueManagerObject;//对话对象

   
  
    private void Awake()
    {
        //类对象实例化
        if (_UIManager == null && _MovieManager == null )
        {

            #region 实例化UI布局管理器与视频播放管理器
            CanvasManagerObject = GameObject.FindGameObjectWithTag("UIManager");
            movieManagerObject = GameObject.FindGameObjectWithTag("MovieManager");
           // DialogueManagerObject = GameObject.FindGameObjectWithTag("DialogueManager");

            _UIManager = CanvasManagerObject.AddComponent<UGUIManager>();
            _MovieManager = movieManagerObject.AddComponent<MovieManager>();
            _WorkDialogManger = CanvasManagerObject.transform.GetChild(8).GetComponentInChildren<WorkDialog>();

            #endregion

        }

      


        //加载工具类----
        playerDataUtils = PlayerDataUtils.getInstance();

        //正式发布时，要有一个开关，决定自动睡眠系统的开启与关闭
        //--假设 上次退出的时候，正在自动睡眠--isAutoSleep = true 且 remainSleepTime > 0 (未苏醒状态)(默认剩余睡眠时间2分钟)
        
        float timeSpan = Mathf.Floor( playerDataUtils.getTimeSpan());//上次退出到再次登录的时间间隔，单位为秒-------需要与剩余自动睡眠对比 差值     
        //Debug.Log(timeSpan);
        float hours = playerDataUtils.Hours(timeSpan);//时
        float minutes = playerDataUtils.Minutes(timeSpan);//分
        float seconds = playerDataUtils.Seconds(timeSpan);//秒
     
        float remainSleepTime = playerDataUtils.getRemainSleepingTime();//剩余睡眠时间

        float DValue = timeSpan - remainSleepTime; // 间隔时间减去剩余睡眠时间的差值

        float curReaminSleepTime = 0; // 当前退出再登陆后的剩余睡眠时间

        // StartCoroutine("updateSleepingStatusCor");
        // StartCoroutine("updateBellyStatusCor");

        #region 计算启动后，是睡眠状态还是苏醒状态

        //启动后当前的剩余睡眠时间：remainSleepTime
        //间隔时间：timeSpan
        //限制时间：8小时
        //8-remainSleepTime = 上次退出时已经睡眠了多少时间。
        //判断剩余睡眠时间与间隔时间的大小
        //time-remainSleepTime = Dvalue 的绝对值差值

        //考虑极端情况
        //1.退出时是苏醒状态，再次进入时是睡眠状态
        //2.退出时是苏醒状态，再次进入时是苏醒状态
        //3.退出时是睡眠状态，再次进入时是苏醒状态
        //4.退出时是睡眠状态，再次进入时是睡眠状态

        if (playerDataUtils.isAutoSleep())//退出时是睡眠状态
        {




        }
        else //退出时是苏醒状态
        {

            //1.进入后为睡眠状态





            //2.进入后为苏醒状态

            //情况一：很短时间内再次登录，未进入睡眠状态。间隔时间timeSpan 
            //情况二：退出后进入了至少一次睡眠，再次登录后为苏醒状态。//间隔时间timeSpan > 8

            //三大数据基础变化
            //1.心情：饱食度低于33，心情每小时变化：1，2，3，4，5，5，5最多到5.
            //2.饱食：每小时降低10点。睡眠不降低。
            //3.睡眠：每小时降低3点，哄睡进入睡眠状态，并慢慢回满睡眠值，8小时静止操作。达到0会自动睡眠，恢复到60，心情降低3.



        }



        if (DValue >= 0)//间隔时间大于剩余睡眠时间
        {

            //当前状态为苏醒状态
            playerDataUtils.setAutoSleep(false);
            //并且已经苏醒了DValue时间



        }
        else//剩余睡眠时间大于间隔时间
        {

            //当前状态为睡眠状态
            playerDataUtils.setAutoSleep(true);
            //还剩余DValue时间苏醒 (判断是否值过于小)

            playerDataUtils.setRemainSleepingTime(Mathf.Abs(DValue));


        }


        #endregion
        

        #region 根据睡眠状态来算

       

        if (playerDataUtils.isAutoSleep()) // 处于睡眠状态
        {
            
            //判断剩余睡眠时间
            #region 一阶判断--判断到此次登录后的自动睡眠状态
            if (DValue < 0) //判断此次登陆后睡眠状态是否结束
            {
                curReaminSleepTime = Mathf.Abs(DValue); // 当前退出再登陆后的剩余睡眠时间,判断是否值过于小
                //Debug.Log(curReaminSleepTime);
                //设置当前剩余睡眠时间
                playerDataUtils.setRemainSleepingTime(curReaminSleepTime);

                //启动自动睡眠协程
                //StartCoroutine("AutoSleepingCor");

                //禁止播放打招呼视频等
                //播放睡眠视频
                 StartCoroutine("SleepLoad", false);//等待加载完成在播放视频  //根据isAutoSleep的值 在MovieManager里面判断主页面是播放打招呼视频还是睡眠视频


            }
            else if(DValue >= 0)// 自动睡眠结束了
            {
                MainCache(true);
                //StartCoroutine("LoadingMethod", true);                                                                                                                                                                                                                                                                                                                                        
                //判断当前进度条的动画播放时机

               // _UIManager.UpdateSleepPmgressbar(recoverSleepingPoint);//更新睡眠进度条
               // _UIManager.UpdateMoodPmgressbar(-3);//跟新心情进度条

                playerDataUtils.WakeUp();



            }

            #endregion




        } 
        //已经苏醒状态
        else
        {
             
            //上次退出之前一直处于正常状态或是退出之前已经睡眠结束
            //正常启动状态

            //游戏初始化页面与加载页面与开头视频
            // StartCoroutine("LoadingMethod", false);//等待加载完成在播放视频  //根据isAutoSleep的值 在MovieManager里面判断主页面是播放打招呼视频还是睡眠视频
            MainCache();
            // StartCoroutine("checkStatusCor");//

            #region  //根据上次游戏的退出时间计算各状态值， 处于睡眠状态这些值是不会变化的

            //如果 协程每小时的变化都计入数据表单中，然后根据再次登录后的差值（每小时是3600秒来变化的，可以保存登录后剩余时间，再次运行来执行协程，可减少数据错误），
            //当前心情/喂食/睡眠 分别使用一个数值
            float remainChangeTime = playerDataUtils.GetSleepTime();//协程剩余变化时间

            float TotalTime = remainChangeTime + timeSpan;//总共退出的时间：上次退出时剩余的变化时间 + 本次的间隔时间
            float hourSpan = Mathf.Floor(TotalTime / 3600f);//小时差 // 这是要变化的时间
           //float hourSpan = 45;
            float secondSpan = (TotalTime - hourSpan * 3600);//剩余的不足1小时的时间 //这是不足一个小时的变化时间，要重新赋给剩余变化时间
          //  playerDataUtils.SetSleepTime(secondSpan);//设置剩余变化时间


            float curSleepPoint = playerDataUtils.getSleepingPoint(); // 当前变化前的睡眠度
            float curMoodPoint = playerDataUtils.getMoodPoint(); // 变化前的心情点
            float curBellyPoint = playerDataUtils.getBellyPoint(); //变化前的饱食度 每小时降低5点
           // Debug.Log(curSleepPoint);
            //循环减少，如果降到0，则进入自动睡眠状态

            //睡眠  每小时降低3点
           //判断间隔时间是否足够
            if(hourSpan != 0) //说明总间隔时间至少1个小时
            {
                int index = 0;
                //开始变化
                for (int i = 0; i < hourSpan; i++)
                {

                    curSleepPoint -= 3f;//睡眠度
                    curBellyPoint -= 10f;
                   // curBellyPoint -= 5f;
                   //降低心情度
                    if(curBellyPoint <= 33)
                    {
                        index++;
                        switch (index)
                        {
                            case 1:
                                curMoodPoint -= 1f;
                                break;
                            case 2:
                                curMoodPoint -= 2f;
                                break;
                            case 3:
                                curMoodPoint -= 3f;
                                break;
                            case 4:
                                curMoodPoint -= 4f;
                                break;
                            default:
                                curMoodPoint -= 5f;
                                break;

                        }
                        
                    }

                    //在退出期间进入了自动睡眠，心情/饱食/睡眠停止变化
                    if (curSleepPoint <= 0)//在间隔时间内降到了0 //在退出期间开始进入睡眠状态
                    {

                        float remainSpan = (hourSpan - i) * 3600f + secondSpan; // 在退出期间开始睡眠，remainSpan是自动睡眠开始后到本次登录的间隔时间
                                                                                //自动睡眠开始---计算此次剩余睡眠时间   ( hourSpan - hour) + secondSpan
                        float a = ((8 * 3600) - remainSpan); //根据a的大小来判断在退出后到自动睡眠等的时间是否足够

                        if (a > 0) // 判断在退出期间，自动睡眠是否结束， 自动睡眠需要8个小时
                        {
                            //登录后还有剩余睡眠时间
                            //更新剩余睡眠时间
                            playerDataUtils.setRemainSleepingTime(a);
                            //启动自动睡眠流程
                            //播放睡眠视频
                            StartCoroutine("SleepLoad", false); //播放自动睡眠视频，停止一切活动。

                            break;



                        }
                        else // 登录后自动睡眠时间已经结束，只更新进度条即可
                        {
                            //跟新进度条
                            //睡眠到60， 心情-3





                        }



                        // playerDataUtils.SetSleepTime((hourSpan - hour) + secondSpan);//设置剩余变化时间




                    }
                    else //在退出期间没有进入自动睡眠状态
                    {




                    }




                }

                

                //此次登录后，饱食变化后的值
               // playerDataUtils.subtractBellyPoint(hourSpan * 10f); //降低饱食度，可能降低到0。。

                playerDataUtils.SetSleepPoint(curSleepPoint);
                playerDataUtils.SetBellyPoint(curBellyPoint);
                playerDataUtils.SetMoodPoint(curMoodPoint);
                //需要考虑进度条小于0的条件
                _UIManager.UpdateSleepPmgressbar(curSleepPoint);
                _UIManager.UpdateBellyPmgressbar(curBellyPoint);
                _UIManager.UpdateMoodPmgressbar(curMoodPoint);

           
            }
            else // 总间隔时间不足1小时
            {
               
                //剩余变化时间减去间隔时间
                playerDataUtils.SetSleepTime(remainChangeTime - timeSpan);

            }

            #endregion




        }



        #endregion




        #region//设置弹窗事件
        //实际运行
        _UIManager._set1.onClick.AddListener(() => {

            oneHour = 3600f;

        });
        //数值测试
        _UIManager._set2.onClick.AddListener(() => {

            _UIManager._SetPanel.gameObject.SetActive(false);

            _UIManager._InputPanel.gameObject.SetActive(true);



        });

        _UIManager._set3.onClick.AddListener(() => {



        });
        _UIManager._set4.onClick.AddListener(() => {



        });
        //
        _UIManager._confirmBtn.onClick.AddListener(() => {

            oneHour = float.Parse(_UIManager._inputText.text);

            _UIManager._InputPanel.gameObject.SetActive(false);
            _UIManager._PopPanel.gameObject.SetActive(false);

        });

        #endregion

        #region//检查约会与玩耍 子项是还有冷却未完成
        //读取检查playerData的计时器中的剩余时间。





        #endregion

    }


    void Start () {

       // playerDataUtils.DataInit();


        // StartCoroutine("LoadingMethod");


        //判断是否进入睡眠状态

        // _UIManager.UpdateIndicate();

        //_MovieManager.m_AutoSleepEvent.Invoke(false);


        //设置事件
        _UIManager._setBtn.onClick.AddListener(() => {                                                                                                                                                                                                                                      


             


        });


        //约会事件
        foreach (var Btn in _UIManager._dateList)
        {
            

            Btn.onClick.AddListener(() => {


            //调用点击列表子项变化方法
            dateCoro = StartCoroutine( itemClick(Btn, _UIManager._dateList.IndexOf(Btn)));

            });
            
        }

        //打工事件
        foreach (var Btn in _UIManager._workList)
        {

            Btn.onClick.AddListener(() => {

                //Debug.Log("点击了打工子项" + Btn.name);
                //调用点击列表子项变化方法
                
                workCoro =  StartCoroutine(itemClick(Btn, _UIManager._workList.IndexOf(Btn)));
            


            });

        }

        //喂食事件
        foreach (var Btn in _UIManager._feedList)
        {
            
            Btn.onClick.AddListener(() => {

                //调用点击列表子项变化方法
              feedCoro = StartCoroutine(itemClick(Btn, _UIManager._feedList.IndexOf(Btn)));

            });


        }

        //哄睡小游戏结束后的统计事件
        _UIManager._gameReturn.onClick.AddListener(() => {


           
           

            //根据游戏胜利次数给出奖励

            //回满体力和睡眠度

            //回满体力----根据胜利次数
            //当前等级的体力最大值
            //  float gameVictoryHealth =  playerDataUtils.completeGame(playerDataUtils.GameVictoryTimes());

            // _UIManager.UpdatePowerPmgressbar(gameVictoryHealth - playerDataUtils.getHealthPoint());


            //回满睡眠度
            //当前等级的睡眠最大值
            //_UIManager.UpdateSleepPmgressbar(playerDataUtils.getMaxSleepingPoint() - playerDataUtils.getSleepingPoint());


            //进入睡眠状态，主界面换成少女睡眠视频
            //8小时内不可进行
            //约会/打工/喂食/玩耍等。
            //睡眠时，睡眠度，饱食度不降低。

            //根据哄睡小游戏结果，给出超出体力上限20%~100%的体力奖励



        });

        //哄睡事件
        _UIManager._sleepBtn.onClick.AddListener(() => {

            //哄睡
            //_MovieManager.CoaxSleepLoadMethod(true);

            StartCoroutine("LoadingCoaxSleep");

            //停止其他按钮功能


        });
        //钻石点击事件
        _UIManager._DmBtn.onClick.AddListener(() => {

            _MovieManager.CoaxSleepLoadMethod(false);


        });


        //换装事件
        _UIManager._shopBtn.onClick.AddListener(() => {


             _MovieManager.StartCoroutine("ReLoadingMain");

            

        });

        //升级事件
        playerDataUtils.levelUpEvent.AddListener((curFavorPoint, curDima, curLevel) => {


           
                //通知列表项当前不更新进度条
                isLevelUp = true;

                //在此进行进度条等的变化
                //更新好感，钻石，等级

              //  float preLevelMaxPoint = playerDataUtils.getMaxFavorPoint(curLevel - 1);//上一等级的最大值

                if (curFavorPoint != 0) //表示要进行两端更新
                {


                    //_UIManager.UpdateFavorPmgressbar(preLevelMaxPoint, preLevelMaxPoint );//先变化到满值，

                   // _UIManager.UpdateDiamText(curDima);//更新钻石数

                   // _UIManager.UpdateFavorLevelText(curLevel);//更新等级

                    // _UIManager.UpdateFavorPmgressbar(curFavorPoint, playerDataUtils.getMaxFavorPoint(), preLevelMaxPoint,true);//变到0

                    // _UIManager.UpdateFavorPmgressbar(curFavorPoint, playerDataUtils.getMaxFavorPoint());//再在下一等级下开始变化到当前值

                }
                else
                {

                    // _UIManager.UpdateFavorPmgressbar(preLevelMaxPoint, preLevelMaxPoint);//变化到满值


                }

                //播放升级视频

                // _MovieManager.StartCoroutine("MainLoopPlay", "levelUp");

                StartCoroutine("LevelCache", false );



            

            
           

        });


    }



    //判断如果是第一次登陆的时候，以及以后登录的情况
    void MainCache(bool state = false)  
    {
        playerDataUtils.SetLogTimes();//设置登陆次数， 累计加1

        if (playerDataUtils.LogTimes() != 1) //判断当前登陆总次数是否是第一次登陆游戏，默认为0，这里不是第一次登陆
        {
            

            //_MovieManager.m_InitLogInEvent.Invoke(playerDataUtils.isAutoSleep()); //播放打招呼或睡眠视频视频

            StartCoroutine("SleepLoad", true);

           // StartCoroutine("LoadingMethod", state);

           // StartCoroutine("updateBellyStatusCor");//启动饱食度协程
          //  StartCoroutine("updateSleepingStatusCor");//启动睡眠度协程

        }
        else //当前登陆次数为1， 第一次登陆，播放剧情（非升级）视频
        {
            // _MovieManager.StartCoroutine("MainLoopPlay", "greet");//播放打招呼视频

            // _MovieManager.m_InitLogInEvent.Invoke(playerDataUtils.isAutoSleep()); //播放打招呼或睡眠视频视频

            //_MovieManager.StartCoroutine("MainLoopPlay", "levelUp");//播放升级视频

            StartCoroutine("LevelCache", true); //播放第一次登录时的剧情视频


        }



    }

    //加载场景方法
    IEnumerator LoadingMethod(bool sleepFinished) // sleepFinished:默认为false，当为true的时候，代表此次登陆后上次退出后自动睡眠在此处结束
    {

      //  playerDataUtils.SetLogTimes();//设置登陆次数， 累计加1

        _UIManager._loadingPanel.gameObject.SetActive(true);
        _UIManager._IniPanel.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.4f);

        _UIManager.StartCoroutine("ExecuteUp");//上隐

        //yield return new WaitForSeconds(1.2f);
        yield return new WaitUntil(() => _UIManager.UpFinished == true);//等到上隐完成
        

       

    //    if (playerDataUtils.LogTimes() != 1) //判断当前登陆总次数是否是第一次登陆游戏，默认为0，这里不是第一次登陆
     //   {



            _MovieManager.m_InitLogInEvent.Invoke(playerDataUtils.isAutoSleep()); //播放打招呼或睡眠视频视频






    //    }
    //    else //当前登陆次数为1， 第一次登陆，播放剧情（升级）视频
    //    {
           // _MovieManager.StartCoroutine("MainLoopPlay", "greet");//播放打招呼视频

           // _MovieManager.m_InitLogInEvent.Invoke(playerDataUtils.isAutoSleep()); //播放打招呼或睡眠视频视频

       //      _MovieManager.StartCoroutine("MainLoopPlay", "levelUp");//播放升级视频






      //  }
        

        yield return new WaitForSeconds(1.2f);//视频加载时间。。。

        _UIManager._IniPanel.gameObject.SetActive(false);


        _UIManager.StartCoroutine("ExecuteDown");//下现


        // yield return new WaitForSeconds(1.4f);
        yield return new WaitUntil(() => _UIManager.DownFinished == true);//等到下现完成

        if (sleepFinished) // 此次登陆，自动睡眠结束，所以进度条要变化，睡眠恢复到60，心情降低3点。
        {

            _UIManager.UpdateSleepPmgressbar(recoverSleepingPoint);//更新睡眠进度条
            _UIManager.UpdateMoodPmgressbar(playerDataUtils.getMoodPoint() - 3f);//跟新心情进度条

        }

        _MovieManager.isStart = true;//开始切换

        yield return new WaitForSeconds(1f);

        _MovieManager.isStart = false;

        _UIManager._loadingPanel.gameObject.SetActive(false);
        
        StopCoroutine("LoadingMethod");

        
    }

    //加载睡眠/睡醒 视频方法
    IEnumerator SleepLoad(bool WakeOrSleep)
    {

        //等待一段时间，在开始切换，这段时间可以弹窗提示等一些其他的东西
        yield return new WaitForSeconds(0.4f);

        _UIManager._loadingPanel.gameObject.SetActive(true);
        _UIManager._IniPanel.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.4f);

        _UIManager.StartCoroutine("ExecuteUp");//上隐

        yield return new WaitUntil(() => _UIManager.UpFinished == true);//等到上隐完成

       

        if (WakeOrSleep) // 睡眠与idle视频的切换：Ture：播放idle False：播放睡眠
        {

            //播放idle 睡醒视频

            //播放idle视频
            _MovieManager.StartCoroutine("MainLoopPlay", "idle");
                       
            StartCoroutine("AutoSleepingCor");//进入自动睡眠状态

        }
        else //默认播放睡眠视频
        {

            // _MovieManager.IsSleepFinished = true;//睡眠结束

            _MovieManager.StartCoroutine("MainLoopPlay", "sleep");


        }

       


        yield return new WaitForSeconds(0.4f);
        _UIManager._IniPanel.gameObject.SetActive(false);


        _UIManager.StartCoroutine("ExecuteDown");//下现

        yield return new WaitUntil(() => _UIManager.DownFinished == true);//等到下现完成

        if (WakeOrSleep)
        {

            _UIManager.UpdateSleepPmgressbar(recoverSleepingPoint);//更新睡眠进度条

            if(playerDataUtils.getMoodPoint() - 3f >= 0)
            {
                _UIManager.UpdateMoodPmgressbar(playerDataUtils.getMoodPoint() - 3f);//跟新心情进度条
            }
            else
            {
                _UIManager.UpdateMoodPmgressbar(0);//跟新心情进度条

            }
            


        }

        _UIManager._loadingPanel.gameObject.SetActive(false);

        StopCoroutine("SleepLoad");

    }

    //加载剧情/升级视频方法--- true：播放 / false：返回主场景
    IEnumerator LevelCache(bool isFirstLevel)
    {

        _UIManager._loadingPanel.gameObject.SetActive(true);
        _UIManager._IniPanel.gameObject.SetActive(true);

       

        _UIManager.StartCoroutine("ExecuteUp");//上隐

        yield return new WaitUntil(() => _UIManager.UpFinished == true);//等到上隐完成




        _MovieManager.StartCoroutine("MainLoopPlay", "levelUp");
      
        
      


        

        yield return new WaitForSeconds(1.4f);


        _UIManager._IniPanel.gameObject.SetActive(false);


        _UIManager.StartCoroutine("ExecuteDown");//下现

        //yield return new WaitUntil(() => _UIManager.DownFinished == true);//等到下现完成

        yield return new WaitUntil( () => _MovieManager.isLevelUpFinished == true);//升级视频播放完全后


        if (isFirstLevel) // 判断是否是剧情视频还是升级视频
        {

            StartCoroutine("updateBellyStatusCor");//启动饱食度协程
            StartCoroutine("updateSleepingStatusCor");//启动睡眠度协程

        }

        _UIManager.StartCoroutine("ExecuteUp");//上隐

        yield return new WaitForSeconds(1.2f);




        _MovieManager.StartCoroutine("MainLoopPlay", "idle"); // 播放打招呼视频

        

        yield return new WaitForSeconds(1.4f);


        _UIManager._IniPanel.gameObject.SetActive(false);


        _UIManager.StartCoroutine("ExecuteDown");//下现





        yield return new WaitForSeconds(1f);

       

        _UIManager._loadingPanel.gameObject.SetActive(false);

        StopCoroutine("LoadingMethod");


    }

    //加载哄睡视频方法
    IEnumerator LoadingCoaxSleep()
    {
        //等待一段时间，在开始切换，这段时间可以弹窗提示等一些其他的东西
        yield return new WaitForSeconds(0.4f);

        _UIManager._loadingPanel.gameObject.SetActive(true);
        _UIManager._IniPanel.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.4f);

        _UIManager.StartCoroutine("ExecuteUp");//上隐

        yield return new WaitUntil(() => _UIManager.UpFinished == true);//等到上隐完成

        //这里是加载哄睡视频的。
        _MovieManager.CoaxSleepLoadMethod(true);

        //开始哄睡视频播放以及切换
        _MovieManager.StartCoroutine("CoaxSleepmethod");




        yield return new WaitForSeconds(0.4f);
        _UIManager._IniPanel.gameObject.SetActive(false);


        _UIManager.StartCoroutine("ExecuteDown");//下现

        yield return new WaitUntil(() => _UIManager.DownFinished == true);//等到下现完成

       

        _UIManager._loadingPanel.gameObject.SetActive(false);

        StopCoroutine("LoadingCoaxSleep");



    }




    //列表选择方法（约会/打工/喂食）
    #region//列表选择事件方法
    private Coroutine dateCoro;
    private Coroutine workCoro;
    private Coroutine feedCoro;
    
    float[] ResoultList;
    private bool isLevelUp = false;
    IEnumerator itemClick(Button btn, int index)
    {
       

        // if(btn.transform.parent.name == "")
        // Debug.Log(btn.transform.parent.name);
        string str = btn.transform.parent.name;
        //int index = Convert.ToInt32(btn.transform.GetChild(0).GetComponent<Text>().text);//获得当前按钮的在列表中的索引index
        float[] floats = null;

        //约会事件
        if(string.Equals(str, "DateContent"))
        {
           // float[] levelUps;
            // 好感增加，金钱减少
            switch (index)
            {
                case 0:
                    //更新好感度和金钱

                    #region Untiy2.1版本
                    /*
                    //更新好感度和金钱
                    floats = playerDataUtils.getDateChangeValue(index);//获取当前将要修改的数据
                    playerDataUtils.goToDate(floats[0], floats[1]);//将变化写入数据文件

                    //返回主界面
                    _UIManager._homeBtn.onClick.Invoke();

                    //进行当前好感度是否超过上限。
                    levelUps = playerDataUtils.LevelUp();
                    if(levelUps[0] == 1)//升级了
                    {
                        //升级下的UI更新
                        _UIManager.UpdateFavorPmgressbar(playerDataUtils.getMaxFavorPoint() - 5f, playerDataUtils.getMaxFavorPoint() - 5f);
                        _UIManager.UpdateFavorPmgressbar(levelUps[1], playerDataUtils.getMaxFavorPoint());

                        _UIManager.UpdateDiamText(levelUps[2]);

                        _UIManager.UpdateFavorLevelText(levelUps[3]);


                    }
                    else
                    {


                        _UIManager.UpdateFavorPmgressbar(floats[0], playerDataUtils.getMaxFavorPoint());
                        _UIManager.UpdateCoinsText(-floats[1]);

                    }



                    //加载特效
                    if (floats[0] != 0) _UIManager.StartEffect(_UIManager._favorEffect, "+" + floats[0].ToString());//好感特效
                    if (floats[1] != 0) _UIManager.StartEffect(_UIManager._moneyEffect, "-" + floats[1].ToString());//金钱特效                                      
                    _UIManager.InitWaitConunt();//初始化特效间隔
                    */

                    #endregion

                    #region Unity2.3版本
                    ResoultList =  playerDataUtils.GoToDate(index); //接受返回结果
                   //0:状态码 /1: 返回变化后的金币 / 2:变化后的好感度 /3: 当前的约会次数 /4:  当前项的冷却时间 /5: 当前等级最大的好感度 /6: 金币差值 /7: 好感度差值

                    if (ResoultList[0] == 1) // 金钱足够，正在工作
                    {
                      
                        //获取冷却时间
                        //开始冷却计时
                        //_UIManager.TimeCDMehod(btn, ResoultList[3]);//参数为：当前item（button） 冷却时间

                        //返回主界面
                        _UIManager._homeBtn.onClick.Invoke();
                        
                        playerDataUtils.SetIsDating(false);//当前没有在约会
                        
                        // isLevelUp 为true：表示当前正在升级，在升级事件里更新进度条，不在这里更新，所以要跳过。
                        if (isLevelUp == false) //当前已经升级了，
                        {
                            _UIManager.UpdateFavorPmgressbar(ResoultList[1], ResoultList[4], 0); //更新好感度进度条

                            _UIManager.StartEffect(_UIManager._effectPrefabs[3], "-" + ResoultList[6]); //金币特效
                            _UIManager.StartEffect(_UIManager._effectPrefabs[2], "+" + ResoultList[7]); //好感度特效

                            _UIManager.InitWaitConunt();//初始化特效间隔

                        }
                        else
                        {
                            Debug.Log("升级了。。。。。");
                            //初始化升级状态
                            isLevelUp = false;


                        }

                       
                        

                    }
                    else  if(ResoultList[0] == 0)// 金钱不足，不能工作
                    {
 
                            Debug.Log("金币不足，不能约会。。。。。。");

                            //调用弹出窗体

                    }

                    #endregion

                    break;
                case 1:

                    #region Unity2.3版本
                    ResoultList = playerDataUtils.GoToDate(index); //接受返回结果
                                                                   //0:状态码 /1: 返回变化后的金币 / 2:变化后的好感度 /3: 当前的约会次数 /4:  当前项的冷却时间 /5: 当前等级最大的好感度 /6: 金币差值 /7: 好感度差值

                    if (ResoultList[0] == 1) // 金钱足够，正在工作
                    {

                        //获取冷却时间
                        //开始冷却计时
                        //_UIManager.TimeCDMehod(btn, ResoultList[3]);//参数为：当前item（button） 冷却时间

                        //返回主界面
                        _UIManager._homeBtn.onClick.Invoke();

                        playerDataUtils.SetIsDating(false);//当前没有在约会

                        // isLevelUp 为true：表示当前正在升级，在升级事件里更新进度条，不在这里更新，所以要跳过。
                        if (isLevelUp == false) //当前已经升级了，
                        {
                            _UIManager.UpdateFavorPmgressbar(ResoultList[1], ResoultList[4], 0); //更新好感度进度条


                            _UIManager.StartEffect(_UIManager._effectPrefabs[3], "-" + ResoultList[6]); //金币特效
                            _UIManager.StartEffect(_UIManager._effectPrefabs[2], "+" + ResoultList[7]); //好感度特效

                            _UIManager.InitWaitConunt();//初始化特效间隔

                        }
                        else
                        {
                            Debug.Log("升级了。。。。。");
                            //初始化升级状态
                            isLevelUp = false;


                        }



                    }
                    else if (ResoultList[0] == 0)// 金钱不足，不能工作
                    {

                        Debug.Log("金币不足，不能约会。。。。。。");

                        //调用弹出窗体

                    }

                    #endregion

                    break;
                case 2:

                    #region Unity2.3版本
                    ResoultList = playerDataUtils.GoToDate(index); //接受返回结果
                                                                   //0:状态码 /1: 返回变化后的金币 / 2:变化后的好感度 /3: 当前的约会次数 /4:  当前项的冷却时间 /5: 当前等级最大的好感度 /6: 金币差值 /7: 好感度差值

                    if (ResoultList[0] == 1) // 金钱足够，正在工作
                    {

                        //获取冷却时间
                        //开始冷却计时
                        //_UIManager.TimeCDMehod(btn, ResoultList[3]);//参数为：当前item（button） 冷却时间

                        //返回主界面
                        _UIManager._homeBtn.onClick.Invoke();

                        playerDataUtils.SetIsDating(false);//当前没有在约会

                        // isLevelUp 为true：表示当前正在升级，在升级事件里更新进度条，不在这里更新，所以要跳过。
                        if (isLevelUp == false) //当前已经升级了，
                        {
                            _UIManager.UpdateFavorPmgressbar(ResoultList[1], ResoultList[4], 0); //更新好感度进度条

                            _UIManager.StartEffect(_UIManager._effectPrefabs[3], "-" + ResoultList[6]); //金币特效
                            _UIManager.StartEffect(_UIManager._effectPrefabs[2], "+" + ResoultList[7]); //好感度特效

                            _UIManager.InitWaitConunt();//初始化特效间隔

                        }
                        else
                        {
                            Debug.Log("升级了。。。。。");
                            //初始化升级状态
                            isLevelUp = false;


                        }

                       


                    }
                    else if (ResoultList[0] == 0)// 金钱不足，不能工作
                    {

                        Debug.Log("金币不足，不能约会。。。。。。");

                        //调用弹出窗体

                    }

                    #endregion

                    break;
                case 3:

                    #region Unity2.3版本
                    ResoultList = playerDataUtils.GoToDate(index); //接受返回结果
                                                                   //0:状态码 /1: 返回变化后的金币 / 2:变化后的好感度 /3: 当前的约会次数 /4:  当前项的冷却时间 /5: 当前等级最大的好感度 /6: 金币差值 /7: 好感度差值

                    if (ResoultList[0] == 1) // 金钱足够，正在工作
                    {

                        //获取冷却时间
                        //开始冷却计时
                        //_UIManager.TimeCDMehod(btn, ResoultList[3]);//参数为：当前item（button） 冷却时间

                        //返回主界面
                        _UIManager._homeBtn.onClick.Invoke();

                        playerDataUtils.SetIsDating(false);//当前没有在约会

                        // isLevelUp 为true：表示当前正在升级，在升级事件里更新进度条，不在这里更新，所以要跳过。
                        if (isLevelUp == false) //当前已经升级了，
                        {
                            _UIManager.UpdateFavorPmgressbar(ResoultList[1], ResoultList[4], 0); //更新好感度进度条

                            _UIManager.StartEffect(_UIManager._effectPrefabs[3], "-" + ResoultList[6]); //金币特效
                            _UIManager.StartEffect(_UIManager._effectPrefabs[2], "+" + ResoultList[7]); //好感度特效

                            _UIManager.InitWaitConunt();//初始化特效间隔

                        }
                        else
                        {
                            Debug.Log("升级了。。。。。");
                            //初始化升级状态
                            isLevelUp = false;


                        }

                       


                    }
                    else if (ResoultList[0] == 0)// 金钱不足，不能工作
                    {

                        Debug.Log("金币不足，不能约会。。。。。。");

                        //调用弹出窗体

                    }

                    #endregion

                    break;


            }


           // Debug.Log("开始约会");

        }
        //打工事件
        else if(string.Equals(str, "WorkContent"))
        {

            
                //金钱增加，心情减少，体力减少
                switch (index)
                {
                    case 0:
                    //更新金钱，心情，体力

                    #region Unity2.1版本
                    /*
                    floats = playerDataUtils.getWorkChnageValue(index);


                        //进行体力判断，可工作则。。。不可则。。。。
                        //根据变化后的体力进行判断 当前体力 - 需要的体力
                        //体力判断
                        if (playerDataUtils.getHealthPoint() - floats[2] >= 0)//体力足够
                        {


                            //调用对话功能模块
                            //指定进行哪段对话



                            yield return null; //等待对话完成


                            #region

                            //打工次数+1
                            //当前的item 熟练度更新，开始打工时间倒计时
                            // _UIManager.WorkTest(btn, 2, 1);
                            //yield return new WaitUntil(() => _UIManager.isWorkFinished == true); //等待倒计时完成后


                            // Debug.Log("该调用对话了。。。。。。。");// 如果当前正在打工，其他工作不可进行
                            //调用对话功能
                            //指定对话
                            #endregion




                            yield return null;//等待对话完成


                            //开始数值变化

                            playerDataUtils.goToWork(floats[0], floats[1], floats[2]);//将数据写入数据库

                            //返回主界面
                            _UIManager._homeBtn.onClick.Invoke();

                            //更新各UI进度条，数值等的变化
                            _UIManager.UpdateCoinsText(floats[0]);
                            _UIManager.UpdateMoodPmgressbar(-floats[1]);
                            _UIManager.UpdatePowerPmgressbar(-floats[2]);

                            //加载特效                  
                            if (floats[0] != 0) _UIManager.StartEffect(_UIManager._moneyEffect, "+" + floats[0].ToString());//金钱特效
                            if (floats[1] != 0) _UIManager.StartEffect(_UIManager._moodEffect, "-" + floats[1].ToString());//心情特效
                            if (floats[2] != 0) _UIManager.StartEffect(_UIManager._powerEffect, "-" + floats[2].ToString());//体力特效                  
                            _UIManager.InitWaitConunt();//初始化特效间隔


                            // }

                        }
                        else//体力不足
                        {
                            //当前工作页面弹出 体力不足提示
                            _UIManager.PopHintPower(true);

                            yield return new WaitForSeconds(1.5f);

                            _UIManager.PopHintPower(false);

                        }
                        */

                    #endregion

                    #region Unity2.3版本
                    ResoultList = playerDataUtils.goToWork(index); //调用打工方法，返回变化后的数值

                        //进行体力判断，可工作则。。。不可则。。。。
                        //根据变化后的体力进行判断 当前体力 - 需要的体力
                        //体力判断
                        if (ResoultList[0] != 0)//体力足够
                        {


                        //调用对话功能模块
                        //指定进行哪段对话

                        _UIManager._DialoguePanel.gameObject.SetActive(true);

                        
                        yield return new WaitUntil(() => _WorkDialogManger.isDialogueFinished == true) ;//等待对话完成


                            #region//开始数值变化
                        
                            //返回主界面
                            _UIManager._homeBtn.onClick.Invoke();

                            //更新各UI进度条，数值等的变化
                            _UIManager.UpdateCoinsText(ResoultList[2]);//金币进度
                           
                            _UIManager.UpdatePowerPmgressbar(ResoultList[1]);//体力进度条

                        //加载特效  
                        _UIManager.StartEffect(_UIManager._effectPrefabs[3], "+" + ResoultList[4]);
                        _UIManager.StartEffect(_UIManager._effectPrefabs[5], "-" + ResoultList[3]);

                                       
                        _UIManager.InitWaitConunt();//初始化特效间隔

                        //_UIManager.CloseEffectCoro()；
                        #endregion

                      

                        }
                        else//体力不足
                        {
                            //当前工作页面弹出 体力不足提示
                            _UIManager.PopHintPower(true);

                            yield return new WaitForSeconds(1.5f);

                            _UIManager.PopHintPower(false);

                        }

                    #endregion

                    break;
                    case 1:
                    //更新金钱，心情，体力

                    #region Unity2.3版本
                    ResoultList = playerDataUtils.goToWork(index); //调用打工方法，返回变化后的数值

                    //进行体力判断，可工作则。。。不可则。。。。
                    //根据变化后的体力进行判断 当前体力 - 需要的体力
                    //体力判断
                    if (ResoultList[0] != 0)//体力足够
                    {


                        //调用对话功能模块
                        //指定进行哪段对话

                        _UIManager._DialoguePanel.gameObject.SetActive(true);


                        yield return new WaitUntil(() => _WorkDialogManger.isDialogueFinished == true);//等待对话完成


                        #region//开始数值变化

                        //返回主界面
                        _UIManager._homeBtn.onClick.Invoke();

                        //更新各UI进度条，数值等的变化
                        _UIManager.UpdateCoinsText(ResoultList[2]);//金币进度

                        _UIManager.UpdatePowerPmgressbar(ResoultList[1]);//体力进度条

                        //加载特效  
                        _UIManager.StartEffect(_UIManager._effectPrefabs[3], "+" + ResoultList[4]);
                        _UIManager.StartEffect(_UIManager._effectPrefabs[5], "-" + ResoultList[3]);


                        _UIManager.InitWaitConunt();//初始化特效间隔

                        //_UIManager.CloseEffectCoro()；
                        #endregion



                    }
                    else//体力不足
                    {
                        //当前工作页面弹出 体力不足提示
                        _UIManager.PopHintPower(true);

                        yield return new WaitForSeconds(1.5f);

                        _UIManager.PopHintPower(false);

                    }

                    #endregion




                    break;
                    case 2:
                    //更新金钱，心情，体力

                    #region Unity2.3版本
                    ResoultList = playerDataUtils.goToWork(index); //调用打工方法，返回变化后的数值

                    //进行体力判断，可工作则。。。不可则。。。。
                    //根据变化后的体力进行判断 当前体力 - 需要的体力
                    //体力判断
                    if (ResoultList[0] != 0)//体力足够
                    {


                        //调用对话功能模块
                        //指定进行哪段对话

                        _UIManager._DialoguePanel.gameObject.SetActive(true);


                        yield return new WaitUntil(() => _WorkDialogManger.isDialogueFinished == true);//等待对话完成


                        #region//开始数值变化

                        //返回主界面
                        _UIManager._homeBtn.onClick.Invoke();

                        //更新各UI进度条，数值等的变化
                        _UIManager.UpdateCoinsText(ResoultList[2]);//金币进度

                        _UIManager.UpdatePowerPmgressbar(ResoultList[1]);//体力进度条

                        //加载特效  
                        _UIManager.StartEffect(_UIManager._effectPrefabs[3], "+" + ResoultList[4]);
                        _UIManager.StartEffect(_UIManager._effectPrefabs[5], "-" + ResoultList[3]);


                        _UIManager.InitWaitConunt();//初始化特效间隔

                        //_UIManager.CloseEffectCoro()；
                        #endregion



                    }
                    else//体力不足
                    {
                        //当前工作页面弹出 体力不足提示
                        _UIManager.PopHintPower(true);

                        yield return new WaitForSeconds(1.5f);

                        _UIManager.PopHintPower(false);

                    }

                    #endregion

                    break;
                    case 3:
                    //更新金钱，心情，体力

                    #region Unity2.3版本
                    ResoultList = playerDataUtils.goToWork(index); //调用打工方法，返回变化后的数值

                    //进行体力判断，可工作则。。。不可则。。。。
                    //根据变化后的体力进行判断 当前体力 - 需要的体力
                    //体力判断
                    if (ResoultList[0] != 0)//体力足够
                    {


                        //调用对话功能模块
                        //指定进行哪段对话

                        _UIManager._DialoguePanel.gameObject.SetActive(true);


                        yield return new WaitUntil(() => _WorkDialogManger.isDialogueFinished == true);//等待对话完成


                        #region//开始数值变化

                        //返回主界面
                        _UIManager._homeBtn.onClick.Invoke();

                        //更新各UI进度条，数值等的变化
                        _UIManager.UpdateCoinsText(ResoultList[2]);//金币进度

                        _UIManager.UpdatePowerPmgressbar(ResoultList[1]);//体力进度条

                        //加载特效  
                        _UIManager.StartEffect(_UIManager._effectPrefabs[3], "+" + ResoultList[4]);
                        _UIManager.StartEffect(_UIManager._effectPrefabs[5], "-" + ResoultList[3]);


                        _UIManager.InitWaitConunt();//初始化特效间隔

                        //_UIManager.CloseEffectCoro()；
                        #endregion



                    }
                    else//体力不足
                    {
                        //当前工作页面弹出 体力不足提示
                        _UIManager.PopHintPower(true);

                        yield return new WaitForSeconds(1.5f);

                        _UIManager.PopHintPower(false);

                    }

                    #endregion

                    break;


                }

                //  Debug.Log("开始打工");


            }
        //喂食事件
        else if(string.Equals(str, "FeedContent"))
        {

            //如果喂食后当前饱食度大于33了，则心情值不在变化，且心情值最大值等要初始化

            //饱食增加，心情增加，金钱减少，钻石减少
            switch(index)
            {
                case 0:
                    //更新饱食，好感，金钱

                    

                    //_UIManager.UpdateUIAndEffect(index, playerDataUtils.giveFood(index));


                    #region Unity2.3版本
                    ResoultList = playerDataUtils.giveFood(index);

                    //状态码说明：/1：钻石足够 /-1:钻石不足 /2：金钱足够 /-2：金钱不足
                    if(ResoultList[0] > 0) // 金币或钻石足够
                    {
                        //返回主界面
                        _UIManager._homeBtn.onClick.Invoke();

                        if (ResoultList[0] == 1) //花费钻石
                        {
                            //返回 /0：状态码 /1：变化后的饱食 /2：变化后的钻石 / 3：变化后的好感 /4:当前等级最大好感 /5: 饱食差值 /6：钻石差值 /7：好感差值


                           

                            // isLevelUp 为true：表示当前正在升级，在升级事件里更新进度条，不在这里更新，所以要跳过。
                            if (isLevelUp == false) //默认为false，当前喂食后没有升级
                            {

                               // yield return new WaitUntil();//等到升级视频播放完毕后

                                //进度条更新
                                _UIManager.UpdateBellyPmgressbar(ResoultList[1]);//饱食进度条增加
                                _UIManager.UpdateDiamText(ResoultList[2]);//钻石进度减少
                                _UIManager.UpdateFavorPmgressbar(ResoultList[3], ResoultList[4], 0);//好感度增加

                                //加载特效  /0:belleyEffect /1: diamondEffect /2: favorEffect /3: moneyEffect /4: moodEffect /5: powerEffect /6: sleepEffect   
                                _UIManager.StartEffect(_UIManager._effectPrefabs[0], "+" + ResoultList[5]);//饱食特效
                                _UIManager.StartEffect(_UIManager._effectPrefabs[1], "-" + ResoultList[6]);//钻石特效
                                _UIManager.StartEffect(_UIManager._effectPrefabs[2], "+" + ResoultList[7]);//好感特效
                                _UIManager.InitWaitConunt();//初始化特效间隔

                            }
                            else //当前喂食后，达到升级状态，在升级时间里变化
                            {

                                //初始化升级状态
                                isLevelUp = false;

                            }  
                            



                        }
                        else //花费金币， 不增加好感度，所以没有升级状态的变化。
                        {
                            //返回 /0： 状态码 /1：变化后的饱食 /2：变化后的金币 /3： 饱食差值 /4： 金币差值

                            //更新各UI进度条，数值等的变化
                            _UIManager.UpdateBellyPmgressbar(ResoultList[1]);//饱食进度条增加
                            _UIManager.UpdateCoinsText(ResoultList[2]);//金币进度减少
                            //加载特效  /0:belleyEffect /1: diamondEffect /2: favorEffect /3: moneyEffect /4: moodEffect /5: powerEffect /6: sleepEffect   
                            _UIManager.StartEffect(_UIManager._effectPrefabs[0], "+" + ResoultList[3]);//饱食特效
                            _UIManager.StartEffect(_UIManager._effectPrefabs[3], "-" + ResoultList[4]);//金币特效
                            _UIManager.InitWaitConunt();//初始化特效间隔


                        }






                    }
                    else // 金币或钻石不足
                    {

                        if(ResoultList[0] == -1)
                        {
                            Debug.Log("钻石不足");

                        }
                        else // ResoultList[0] == -2
                        {
                            Debug.Log("金币不足");


                        }



                    }


                    #endregion





                    break;
                case 1:

                    #region Unity2.3版本
                    ResoultList = playerDataUtils.giveFood(index);

                    //状态码说明：/1：钻石足够 /-1:钻石不足 /2：金钱足够 /-2：金钱不足
                    if (ResoultList[0] > 0) // 金币或钻石足够
                    {
                        //返回主界面
                        _UIManager._homeBtn.onClick.Invoke();

                        if (ResoultList[0] == 1) //花费钻石
                        {
                            //返回 /0：状态码 /1：变化后的饱食 /2：变化后的钻石 / 3：变化后的好感 /4:当前等级最大好感 /5: 饱食差值 /6：钻石差值 /7：好感差值




                            // isLevelUp 为true：表示当前正在升级，在升级事件里更新进度条，不在这里更新，所以要跳过。
                            if (isLevelUp == false) //默认为false，当前喂食后没有升级
                            {

                                // yield return new WaitUntil();//等到升级视频播放完毕后

                                //进度条更新
                                _UIManager.UpdateBellyPmgressbar(ResoultList[1]);//饱食进度条增加
                                _UIManager.UpdateDiamText(ResoultList[2]);//钻石进度减少
                                _UIManager.UpdateFavorPmgressbar(ResoultList[3], ResoultList[4], 0);//好感度增加

                                //加载特效  /0:belleyEffect /1: diamondEffect /2: favorEffect /3: moneyEffect /4: moodEffect /5: powerEffect /6: sleepEffect   
                                _UIManager.StartEffect(_UIManager._effectPrefabs[0], "+" + ResoultList[5]);//饱食特效
                                _UIManager.StartEffect(_UIManager._effectPrefabs[1], "-" + ResoultList[6]);//钻石特效
                                _UIManager.StartEffect(_UIManager._effectPrefabs[2], "+" + ResoultList[7]);//好感特效
                                _UIManager.InitWaitConunt();//初始化特效间隔

                            }
                            else //当前喂食后，达到升级状态，在升级时间里变化
                            {

                                //初始化升级状态
                                isLevelUp = false;

                            }




                        }
                        else //花费金币， 不增加好感度，所以没有升级状态的变化。
                        {
                            //返回 /0： 状态码 /1：变化后的饱食 /2：变化后的金币 /3： 饱食差值 /4： 金币差值

                            //更新各UI进度条，数值等的变化
                            _UIManager.UpdateBellyPmgressbar(ResoultList[1]);//饱食进度条增加
                            _UIManager.UpdateCoinsText(ResoultList[2]);//金币进度减少
                            //加载特效  /0:belleyEffect /1: diamondEffect /2: favorEffect /3: moneyEffect /4: moodEffect /5: powerEffect /6: sleepEffect   
                            _UIManager.StartEffect(_UIManager._effectPrefabs[0], "+" + ResoultList[3]);//饱食特效
                            _UIManager.StartEffect(_UIManager._effectPrefabs[3], "-" + ResoultList[4]);//金币特效
                            _UIManager.InitWaitConunt();//初始化特效间隔


                        }






                    }
                    else // 金币或钻石不足
                    {

                        if (ResoultList[0] == -1)
                        {
                            Debug.Log("钻石不足");

                        }
                        else // ResoultList[0] == -2
                        {
                            Debug.Log("金币不足");


                        }



                    }


                    #endregion

                    break;
                case 2:

                    #region Unity2.3版本
                    ResoultList = playerDataUtils.giveFood(index);

                    //状态码说明：/1：钻石足够 /-1:钻石不足 /2：金钱足够 /-2：金钱不足
                    if (ResoultList[0] > 0) // 金币或钻石足够
                    {
                        //返回主界面
                        _UIManager._homeBtn.onClick.Invoke();

                        if (ResoultList[0] == 1) //花费钻石
                        {
                            //返回 /0：状态码 /1：变化后的饱食 /2：变化后的钻石 / 3：变化后的好感 /4:当前等级最大好感 /5: 饱食差值 /6：钻石差值 /7：好感差值




                            // isLevelUp 为true：表示当前正在升级，在升级事件里更新进度条，不在这里更新，所以要跳过。
                            if (isLevelUp == false) //默认为false，当前喂食后没有升级
                            {

                                // yield return new WaitUntil();//等到升级视频播放完毕后

                                //进度条更新
                                _UIManager.UpdateBellyPmgressbar(ResoultList[1]);//饱食进度条增加
                                _UIManager.UpdateDiamText(ResoultList[2]);//钻石进度减少
                                _UIManager.UpdateFavorPmgressbar(ResoultList[3], ResoultList[4], 0);//好感度增加

                                //加载特效  /0:belleyEffect /1: diamondEffect /2: favorEffect /3: moneyEffect /4: moodEffect /5: powerEffect /6: sleepEffect   
                                _UIManager.StartEffect(_UIManager._effectPrefabs[0], "+" + ResoultList[5]);//饱食特效
                                _UIManager.StartEffect(_UIManager._effectPrefabs[1], "-" + ResoultList[6]);//钻石特效
                                _UIManager.StartEffect(_UIManager._effectPrefabs[2], "+" + ResoultList[7]);//好感特效
                                _UIManager.InitWaitConunt();//初始化特效间隔

                            }
                            else //当前喂食后，达到升级状态，在升级时间里变化
                            {

                                //初始化升级状态
                                isLevelUp = false;

                            }




                        }
                        else //花费金币， 不增加好感度，所以没有升级状态的变化。
                        {
                            //返回 /0： 状态码 /1：变化后的饱食 /2：变化后的金币 /3： 饱食差值 /4： 金币差值

                            //更新各UI进度条，数值等的变化
                            _UIManager.UpdateBellyPmgressbar(ResoultList[1]);//饱食进度条增加
                            _UIManager.UpdateCoinsText(ResoultList[2]);//金币进度减少
                            //加载特效  /0:belleyEffect /1: diamondEffect /2: favorEffect /3: moneyEffect /4: moodEffect /5: powerEffect /6: sleepEffect   
                            _UIManager.StartEffect(_UIManager._effectPrefabs[0], "+" + ResoultList[3]);//饱食特效
                            _UIManager.StartEffect(_UIManager._effectPrefabs[3], "-" + ResoultList[4]);//金币特效
                            _UIManager.InitWaitConunt();//初始化特效间隔


                        }






                    }
                    else // 金币或钻石不足
                    {

                        if (ResoultList[0] == -1)
                        {
                            Debug.Log("钻石不足");

                        }
                        else // ResoultList[0] == -2
                        {
                            Debug.Log("金币不足");


                        }



                    }


                    #endregion

                    break;
                case 3:

                    #region Unity2.3版本
                    ResoultList = playerDataUtils.giveFood(index);

                    //状态码说明：/1：钻石足够 /-1:钻石不足 /2：金钱足够 /-2：金钱不足
                    if (ResoultList[0] > 0) // 金币或钻石足够
                    {
                        //返回主界面
                        _UIManager._homeBtn.onClick.Invoke();

                        if (ResoultList[0] == 1) //花费钻石
                        {
                            //返回 /0：状态码 /1：变化后的饱食 /2：变化后的钻石 / 3：变化后的好感 /4:当前等级最大好感 /5: 饱食差值 /6：钻石差值 /7：好感差值




                            // isLevelUp 为true：表示当前正在升级，在升级事件里更新进度条，不在这里更新，所以要跳过。
                            if (isLevelUp == false) //默认为false，当前喂食后没有升级
                            {

                                // yield return new WaitUntil();//等到升级视频播放完毕后

                                //进度条更新
                                _UIManager.UpdateBellyPmgressbar(ResoultList[1]);//饱食进度条增加
                                _UIManager.UpdateDiamText(ResoultList[2]);//钻石进度减少
                                _UIManager.UpdateFavorPmgressbar(ResoultList[3], ResoultList[4], 0);//好感度增加

                                //加载特效  /0:belleyEffect /1: diamondEffect /2: favorEffect /3: moneyEffect /4: moodEffect /5: powerEffect /6: sleepEffect   
                                _UIManager.StartEffect(_UIManager._effectPrefabs[0], "+" + ResoultList[5]);//饱食特效
                                _UIManager.StartEffect(_UIManager._effectPrefabs[1], "-" + ResoultList[6]);//钻石特效
                                _UIManager.StartEffect(_UIManager._effectPrefabs[2], "+" + ResoultList[7]);//好感特效
                                _UIManager.InitWaitConunt();//初始化特效间隔

                            }
                            else //当前喂食后，达到升级状态，在升级时间里变化
                            {

                                //初始化升级状态
                                isLevelUp = false;

                            }




                        }
                        else //花费金币， 不增加好感度，所以没有升级状态的变化。
                        {
                            //返回 /0： 状态码 /1：变化后的饱食 /2：变化后的金币 /3： 饱食差值 /4： 金币差值

                            //更新各UI进度条，数值等的变化
                            _UIManager.UpdateBellyPmgressbar(ResoultList[1]);//饱食进度条增加
                            _UIManager.UpdateCoinsText(ResoultList[2]);//金币进度减少
                            //加载特效  /0:belleyEffect /1: diamondEffect /2: favorEffect /3: moneyEffect /4: moodEffect /5: powerEffect /6: sleepEffect   
                            _UIManager.StartEffect(_UIManager._effectPrefabs[0], "+" + ResoultList[3]);//饱食特效
                            _UIManager.StartEffect(_UIManager._effectPrefabs[3], "-" + ResoultList[4]);//金币特效
                            _UIManager.InitWaitConunt();//初始化特效间隔


                        }






                    }
                    else // 金币或钻石不足
                    {

                        if (ResoultList[0] == -1)
                        {
                            Debug.Log("钻石不足");

                        }
                        else // ResoultList[0] == -2
                        {
                            Debug.Log("金币不足");


                        }



                    }


                    #endregion

                    break;
            }

        }
        
    }
    #endregion

    //每5分钟检查一次状态
    IEnumerator checkStatusCor()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkTimeSpan);
            if (playerDataUtils.getBellyPoint() < lowerLimitBellyPoint && subtractedMoodPoint == 0)//&&updateMoodStatusEnum == null)
            {
                //updateMoodStatusEnum = updateMoodStatusCor();
                //StartCoroutine(updateMoodStatusEnum);
                StartCoroutine("updateMoodStatusCor");
                //subtractedMoodPoint = 0;
            }
            else if (playerDataUtils.getBellyPoint() >= lowerLimitBellyPoint)
            {
                subtractedMoodPoint = 0;
                StopCoroutine("updateMoodStatusCor");
                /*oldUpdateMoodStatusEnum = updateMoodStatusEnum;
                if (oldUpdateMoodStatusEnum != null)
                {
                    StopCoroutine(oldUpdateMoodStatusEnum);
                    updateMoodStatusEnum = null; 
                }*/
            }
        }
    }

    //更新心情度
    IEnumerator updateMoodStatusCor()
    {
        float curMoodPoint;
        while (true)
        {
           //只要饱食度低于33点，一启动更新心情协程，先减一次心情然后一个小时后再次变化。。。

            //注意心情降低为0的情况。

            //subMoodPointPerHour = 1f:每小时降低的心情值
            //maxSubMoodPoint = 5f：最大降低心情值
            if (subMoodPointPerHour <= maxSubMoodPoint)
            {
                

                curMoodPoint = playerDataUtils.getMoodPoint(); //当前的心情值

                if (curMoodPoint != 0) // 如果变化前的值不为0，
                {
                   // playerDataUtils.ReduceMoodPoint(subMoodPointPerHour);//降低1点到5点的心情值

                    curMoodPoint = playerDataUtils.getMoodPoint(); //变化后的心情值

                    _UIManager.UpdateMoodPmgressbar(curMoodPoint); //进度条更新
                }
                else //如果变化前的 心情值已经变为0了
                {

                    Debug.Log("心情值已经降低为0");


                }
                
                subMoodPointPerHour++; //

                if (subMoodPointPerHour > 5)// 降低的心情值最大为5点
                {
                    subMoodPointPerHour = 5;

                }




                
            }
                      
             yield return new WaitForSeconds(oneHour);//饱食度低于33点时每小时变化
            
        }
    }
    // 更新饱食度
    IEnumerator updateBellyStatusCor()
    {
        bool isUpdateMood = true;
        float curBellyPoint;
        while (true)
        {
            StartCoroutine("HourTimer");//启动饱食计时

             yield return new WaitForSeconds(oneHour); //oneHour = 3600f;
                                                       // yield return new WaitForSeconds(playerDataUtils.GetBellyTime());

            StopCoroutine("HourTimer");//停止饱食计时

            // playerDataUtils.reduceBellyPoint(subBellyPointPerHour);//降低10点饱食度


            curBellyPoint = playerDataUtils.getBellyPoint();// 当前饱食度

            if(curBellyPoint != 0) // 当前饱食度不为0 ，变化前
            {
                playerDataUtils.subtractBellyPoint(subBellyPointPerHour);//降低5点饱食度 （数据文件变化）

                curBellyPoint = playerDataUtils.getBellyPoint();// 得到当前饱食度

                //更新饱食度进度条。。。。。subBellyPointPerHour = 5f
                _UIManager.UpdateBellyPmgressbar(curBellyPoint);


                if (curBellyPoint < 33 && isUpdateMood == true)
                {
                    isUpdateMood = false;
                    StartCoroutine("updateMoodStatusCor");
                    Debug.Log("开始降低心情值");
                }
                else if (curBellyPoint >= 33 && isUpdateMood == false)
                {
                    StopCoroutine("updateMoodStatusCor");
                    isUpdateMood = true;
                    Debug.Log("停止降低心情值");
                }

            }
            else //饱食度为0了
            {
                //更新饱食度进度条-----降到0

                Debug.Log("饱食度已经为0");

            }
 
        }
    }
    //更新睡眠度
    IEnumerator updateSleepingStatusCor()
    {
        float curSleepPoint;
        while (true)
        {
            
            yield return new WaitForSeconds(oneHour);//每小时更新一次

            playerDataUtils.subtractSleepingPoint(subSleepingPointPerHour); //降低3点睡眠度（变化写入数据文件）          
            curSleepPoint = playerDataUtils.getSleepingPoint();//当前睡眠值（变化后）
            _UIManager.UpdateSleepPmgressbar(curSleepPoint); //更新睡眠进度条

            if (curSleepPoint != 0)//变化后的睡眠值不为0 ，
            {

               
               // curSleepPoint = playerDataUtils.getSleepingPoint();//得到变化后的睡眠度, 可能为0
                 
               

                //睡眠分为两个等级
                //困倦：0~33
                //普通：34~100

                if (curSleepPoint <= 33)
                {
                    //进入困倦状态
                    _UIManager._sleepBtn.interactable = true; //哄睡按钮变为可用状态

                    _UIManager._sleepBtn.onClick.AddListener(() => {

                        _UIManager._EffectsPanel.GetChild(0).GetComponentInChildren<Text>().text = "可以哄我睡觉了";
                        // _UIManager._sleepBtn.onClick.Invoke();
                        _UIManager._EffectsPanel.GetChild(0).gameObject.SetActive(true);//显示睡眠提示

                    });// 显示睡眠提示



                    //_UIManager._sleepBtn.interactable = true;
                    //此时可进行哄睡小游戏

                    //如果不哄睡，则睡眠值持续下降，下降到0后会进入， 自动睡眠状态，
                    //睡眠状态，主界面播放少女睡眠视频，同时显示睡眠特效，
                    //8小时内禁止： 约会，打工，喂食，玩耍操作， 同时睡眠度，饱食度不降低。
                    //睡眠结束后，睡眠值恢复到60，心情值降低3点，弹窗角色对话框。





                    Debug.Log("当前状态为困倦状态。。。哄睡按钮可用");

                }
                else if (curSleepPoint >= 34)
                {
                    //普通状态
                   // _UIManager._sleepBtn.interactable = false; //哄睡按钮为不可用状态 
                    _UIManager._sleepBtn.onClick.AddListener(() => {

                        // _UIManager._sleepBtn.onClick.Invoke();
                        _UIManager._EffectsPanel.GetChild(0).gameObject.SetActive(true);//显示睡眠提示

                    });




                    Debug.Log("当前状态为普通状态。。。。。哄睡按钮不可用");


                }

            }
            else  //如果当前睡眠度为0
            {
                
                //进入自动睡眠状态，下次登陆后，睡眠值恢复到60，心情值降低3点
                //弹窗角色对话框“主人昨天没有哄我睡觉，不开心”。

                Debug.Log("睡眠值为0，进入睡眠状态！");//-0-----提示即将进入自动睡眠状态

                // StartCoroutine("AutoSleepingCor");//进入自动睡眠状态

                StartCoroutine("SleepLoad", false);


                //播放自动睡眠视频----这里可以用Invoke调用
                //_MovieManager.m_AutoSleepEvent.Invoke(true);




            }






        }
    }

    //自动睡眠
    IEnumerator AutoSleepingCor()
    {
       
    
        _UIManager.AllButtonInteractable(false);  //8小时内禁止： 约会，打工，喂食，玩耍操作， 同时睡眠度，饱食度不降低。

        StopCoroutine("updateSleepingStatusCor");//停止睡眠值降低---苏醒后恢复为60点
        StopCoroutine("updateMoodStatusCor");//停止心情值降低----苏醒后降低3点
        StopCoroutine("updateBellyStatusCor");//停止饱食度降低

        //播放自动睡眠视频
       // StartCoroutine("SleepLoad", false); Debug.Log("播放自动睡眠。。。。。");

        playerDataUtils.setAutoSleep(true); // 设置当前为自动睡眠状态（自动睡眠与哄睡的区别）
      

        while (true)
        {
            yield return new WaitForSeconds(oneSecond);//每1秒变化， oneSecond = 1f;

            playerDataUtils.subRemainingSleepingTime(Mathf.FloorToInt(oneSecond+0.5f));//减少剩余睡眠时间  //退出到再次登陆之间的时间差，只要改变剩余睡眠时间即可，如果睡眠时间有剩余，启动自动睡眠协程
           
            
            //如果自动睡眠结束-------
            if (playerDataUtils.isWakeUp())//自动睡眠是否结束
            {
                //自动睡眠结束
                //睡眠结束后，睡眠值恢复到60，心情值降低3点，弹窗角色对话框。
                //调用睡眠值更新UI到60，
                //调用心情值更新--降低3点

                //数值操作
                playerDataUtils.WakeUp();//睡眠度恢复到60  //降低3点睡眠度 //isAutoSleep 为false（睡醒了）

                #region UI进度条操作，在加载睡眠或苏醒视频方法中实现
                //UI进度条操作
                // _UIManager.UpdateSleepPmgressbar(recoverSleepingPoint);//更新睡眠进度条
                //_UIManager.UpdateMoodPmgressbar(-3);//跟新心情进度条


                // playerDataUtils.SetSleepPoint(60); //睡眠度恢复到60
                // playerDataUtils.subtractMoodPoint(3);//降低3点睡眠度
                #endregion

                //睡醒之后播放idle视频
                _MovieManager.IsSleepFinished = true;
                StartCoroutine("SleepLoad", true);
                //StartCoroutine("SleepLoad", true);
                //_MovieManager.m_PlayIdleEvent.Invoke();

                StartCoroutine("updateBellyStatusCor");
                StartCoroutine("updateSleepingStatusCor");
                StopCoroutine("AutoSleepingCor");//停止自动睡眠
                _UIManager.AllButtonInteractable(true);

            }
        }
    }
   
    
    
    
    
    
    //哄睡
    IEnumerator CoaxSleepingCor()
    {
        // playerDataUtils.setAutoSleep(false);
        //8小时内禁止： 约会，打工，喂食，玩耍操作， 同时睡眠度，饱食度不降低。
        _UIManager._dateBtn.interactable = false;
        _UIManager._workBtn.interactable = false;
        _UIManager._feedBtn.interactable = false;
        _UIManager._gameBtn.interactable = false;
        StopCoroutine("updateSleepingStatusCor");//停止睡眠值降低---苏醒后恢复为60点
        StopCoroutine("updateMoodStatusCor");//停止心情值降低----苏醒后降低3点
        StopCoroutine("updateBellyStatusCor");//停止饱食度降低


        while (true)
        {

           yield return new WaitForSeconds(1f);
           // playerDataUtils.subRemainingSleepingTime(Mathf.FloorToInt(oneSecond + 0.5f));
           // updateSleepTimerEvent.Invoke(playerDataUtils.getRemainSleepingTime());


            if (playerDataUtils.isWakeUp())
            {
                //更新玩家数据
                playerDataUtils.WakeUp();
                //哄睡后醒来触发事件
               // wakeUpEvent.Invoke(false);
                //数据更新事件
                //updateStatusEvent.Invoke();

               // StartCoroutine(updateBellyCor);
              //  StartCoroutine(updateSleepingCor);
               // StopCoroutine(coaxSleepingCor);
            }
        }
    }


    #region 计时器更新方法


    IEnumerator UpdateDateTimerCor()
    {
        while (true)
        {
            yield return new WaitForSeconds(oneSecond);
            playerDataUtils.UpdateDateTimer((int)Mathf.FloorToInt(oneSecond + 0.5f));
        }
    }
    IEnumerator UpdatePlayTimerCor()
    {
        while (true)
        {
            yield return new WaitForSeconds(oneSecond);
           // playerDataUtils.updatePlayTimer((int)Mathf.FloorToInt(oneSecond + 0.5f));
        }
    }

    //开始更新约会计时器
    private void StartUpdateDateTimerCor()
    {
        StartCoroutine("UpdateDateTimerCor");
        playerDataUtils.setIsUpdateDateTimerRuning(true);
    }
    //停止更新约会计时器
    private void StopUpdateDataTimerCor()
    {
        StopCoroutine("UpdateDataTimerCor");
        playerDataUtils.setIsUpdateDateTimerRuning(false);
    }
    //开始更新玩耍计时器
    private void StartUpdatePlayTimerCor()
    {
        StartCoroutine("UpdatePlayerTimerCor");
        //playerDataUtils.setIsUpdatePlayTimerRuning(true);
    }
    //停止更新玩耍计时器
    private void StopUpdatePlayTimerCor()
    {
        StopCoroutine("UpdatePlayerTimerCor");
       // playerDataUtils.setIsUpdatePlayTimerRuning(false);
    }




    #endregion

    #region 心情/喂食/睡眠等的计时方法

    IEnumerator HourTimer() //启动协程的时候开始运行
    {
        playerDataUtils.SetSleepTime(oneHour);

        while (true)
        {
         
                playerDataUtils.ReduceTime();//减少1秒
                yield return new WaitForSeconds(oneSecond);//等待一秒后     


        }



    }

    #endregion


    int escapeTimes = 1;
    // 退出功能
    private void Update()
    {

        if (Input.GetKey(KeyCode.Escape))
        {
            //这个地方可以写“再按一次退出”的提示

            escapeTimes++;

            StartCoroutine("resetTimes");
            if (escapeTimes > 1)
            {
                Application.Quit();
            }

        }

    }
    IEnumerator resetTimes()
    {
        yield return new WaitForSeconds(1);
        escapeTimes = 1;
    }

    private void OnDestroy()
    {
        //游戏结束时记录当前睡眠状态----由自动睡眠判断

        //游戏结束时需记录当前时间
        PlayerDataUtils.getInstance().setQuitTime(DateTime.Now.ToString());
        

    }


  

}
