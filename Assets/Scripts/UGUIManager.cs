using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;



public class UGUIManager : MonoBehaviour {


    public bool isInit = false;

    public Text tex;
    //public Camera _UICamera;
    private PlayerData playerData;
    private Canvas _canvas;

    /********-面板-*********/
    [HideInInspector]
    public RectTransform _movieTexture;//电影材质
    [HideInInspector]
    public Transform _TopPanel;//顶部图标面板
    [HideInInspector]
    public Transform _BottomPanel;//底部区域面板
    [HideInInspector]
    public Transform _Probars;//中间状态进度条面板
    [HideInInspector]
    public Transform _HomeBars;//底部页面切换面板
    [HideInInspector]
    public Transform _ScrollPanel;//滚动面板
    [HideInInspector]
    public Transform _DatePanel;//约会面板
    [HideInInspector]
    public Transform _WorkPanel;//工作面板
    [HideInInspector]
    public Transform _FeedPanel;//喂食面板
    [HideInInspector]
    public Transform _GamePanel;//游戏面板
    [HideInInspector]
    public Transform _EffectsPanel;//特效面板
    [HideInInspector]
    public Transform _TouchPanel;//触摸面板
    [HideInInspector]
    public Transform _PopPanel;//弹出面板
    [HideInInspector]
    public Transform _DialoguePanel;//对话面板
    [HideInInspector]
    public Transform _IniPanel;//初始化面板
    [HideInInspector]
    public Transform _loadingPanel;//加载面板
   

    /********--顶部面板子项--***************/
    [HideInInspector]
    public Image _levelExp; //好感度等级进度条
    [HideInInspector]
    public Image _LevelBottom;//好感度进度条底面
    [HideInInspector]
    public Text _level;//好感度等级
    [HideInInspector]
    public Image _headImage; //头像  
    [HideInInspector]
    public Image _powerbar;//体力进度条 
    [HideInInspector]
    public Text _powerText;//体力指示文本
    [HideInInspector]
    public Button _coinsBtn;//金钱 
    [HideInInspector]
    public Button _DmBtn;//钻石  
    [HideInInspector]
    public Button _setBtn;//设置

    //Probars
    /*********--底部状态进度条面板子项--************/
    [HideInInspector]
    public Image _moodbar;//玩耍进度条
    private Text _moodBottomText;//玩耍底部数值指示
    private Text _moodTopText;//玩耍上部数值指示
    [HideInInspector]
    public Image _feedbar;//喂食进度条
    private Text _feedBottomText;//喂食底部数值指示
    private Text _feedTopText;//喂食上部数值指示
    [HideInInspector]
    public Image _sleepbar;//睡眠进度条
    private Text _sleepBottomText;//睡眠底部数值指示
    private Text _sleepTopText;//睡眠上部数值指示
    [HideInInspector]
    public Button _gameBtn;//玩耍按钮
    [HideInInspector]
    public Button _feedBtn;//喂食按钮
    [HideInInspector]
    public Button _sleepBtn;//睡眠按钮

    /*************--列表区对象--*********************/
    //list
    private Transform _dateListContent;//约会父对象
    private Transform _workListContent;//打工父对象
    private Transform _feedListContent;//喂食父对象
    [HideInInspector]
    public List<Button> _dateList;//约会列表
    [HideInInspector]
    public List<Button> _workList;//打工列表
    [HideInInspector]
    public List<Button> _feedList;//喂食列表

    /**********--底部页面切换面板子项--*************/
    [HideInInspector]
    public Button _homeBtn;//家按钮
    [HideInInspector]
    public Button _dateBtn;//外出按钮
    [HideInInspector]
    public Button _workBtn;//工作按钮
    [HideInInspector]
    public Button _packBtn;//背包按钮
    [HideInInspector]
    public Button _shopBtn;//商店按钮
    [HideInInspector]
    public Button[] _btnArray; //home按钮组
    private RectTransform _whitepng;// 底条
    private Vector3 _origiPosition; //底条初始位置

    /************--游戏选项按钮--***********/
    [HideInInspector]
    public Button _gameJBtn;//剪刀按钮
    [HideInInspector]
    public Button _gameBBtn;//包袱按钮
    [HideInInspector]
    public Button _gameQBtn;//拳头按钮
    [HideInInspector]
    public Button _gameReturn;//游戏界面返回按钮 
    [HideInInspector]
    public Transform _RedPanel;//游戏红心面板
    private RawImage[] _redArray;//游戏红心图标数组

    /**********--特效Prefab对象--*************/
    [HideInInspector]
    public List<GameObject> _effectPrefabs = new List<GameObject>();//按索引分别为：/0:belleyEffect /1: diamondEffect /2: favorEffect /3: moneyEffect /4: moodEffect /5: powerEffect /6: sleepEffect 

    [HideInInspector]
    public GameObject _favorEffect;//好感特效图标
    [HideInInspector]
    public GameObject _powerEffect;//体力特效图标
    [HideInInspector]
    public GameObject _moneyEffect;//金钱特效图标
    [HideInInspector]
    public GameObject _diamondEffect;//钻石特效图标
    [HideInInspector]
    public GameObject _moodEffect;//心情特效图标
    [HideInInspector]
    public GameObject _bellyEffect;//饱食特效图标
    [HideInInspector]
    public GameObject _sleepEffect;//睡眠特效图标

    /************--触摸按钮--**************/
    [HideInInspector]
    public Button _touchBodyBtn;//触摸身体按钮
    [HideInInspector]
    public Button _touchHeadBtn;//触摸头部按钮

    /**************--升级调用按钮--*****************/
    [HideInInspector]
    public Button _levelUpBtn;//升级按钮

    /****************--对话相关--*******************/
    [HideInInspector]
    public Text _femaleText;//女主对话框
    [HideInInspector]
    public Text _maleText;//男主对话框
    [HideInInspector]
    public Button _dialogueBtn;//对话按钮

    /**************加载页面****************/

    private Image[] images; //面板片
    private Coroutine[] changes = null;

    /****************--弹出窗口--********************/
    [HideInInspector]
    public Button _PopBtn;//弹出窗口面板按钮
    [HideInInspector]
    public Transform _SetPanel;//设置弹窗
    [HideInInspector]
    public Button _set1;
    [HideInInspector]
    public Button _set2;
    [HideInInspector]
    public Button _set3;
    [HideInInspector]
    public Button _set4;

    [HideInInspector]
    public Transform _InputPanel;//输入面板
    [HideInInspector]
    public Text _inputText;//输入框
    [HideInInspector]
    public Button _confirmBtn;
   // private Button _

    private void Awake()
    {

      


        //加载数据文件
        playerData = Resources.Load<PlayerData>("PlayerData");

        //限制FPS为60
        Application.targetFrameRate = 60;

        #region      /********--对象加载--********/

        #region /面板/组加载
        _canvas = this.GetComponent<Canvas>();//加载画布
        _movieTexture = this.transform.GetChild(0) as RectTransform;//加载电影材质
        _TopPanel = this.transform.GetChild(2);//加载顶部区域面板
        _BottomPanel = transform.GetChild(3);//加载底部区域面板
        _Probars = _BottomPanel.GetChild(0);//加载进度条面板
        _HomeBars = _BottomPanel.GetChild(1);//加载homebar面板
        _ScrollPanel = this.transform.GetChild(1);//加载滚动面板
        _DatePanel = _ScrollPanel.GetChild(0);//加载约会面板
        _WorkPanel = _ScrollPanel.GetChild(1);//加载打工面板
        _FeedPanel = _ScrollPanel.GetChild(2);//加载喂食面板
        _GamePanel = transform.GetChild(4);//加载游戏面板
        _EffectsPanel = transform.GetChild(5);//加载特效面板
        _TouchPanel = transform.GetChild(6);//加载触摸面板
        _PopPanel = transform.GetChild(7);//加载弹出面板
        _DialoguePanel = transform.GetChild(8);//加载对话面板
        _loadingPanel = transform.GetChild(10);//加载缓冲面板
        _IniPanel = transform.GetChild(9);//加载初始化面板
        #endregion
        //
        _movieTexture.sizeDelta = new Vector2(Screen.width + 200, Screen.height + 300);
        //
        Transform temp = _TopPanel.GetChild(0).GetChild(0);//
        //加载好感等级条图片
        _levelExp = _TopPanel.GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>();
        //加载好感等级条底部图片
        _LevelBottom = _TopPanel.GetChild(0).GetChild(0).GetComponent<Image>();
        //加载头像图片
        _headImage = _TopPanel.GetChild(0).GetChild(1).GetComponent<Image>();
        //加载好感度等级
        _level = _headImage.transform.GetComponentInChildren<Text>();

        //加载体力条
        _powerbar = _TopPanel.GetChild(1).GetChild(1).GetComponent<Image>();
        //加载体力指示文本
        _powerText = _powerbar.GetComponentInChildren<Text>();
        //加载金钱按钮
        _coinsBtn = _TopPanel.GetChild(2).GetComponentInChildren<Button>();
        //加载钻石按钮
        _DmBtn = _TopPanel.GetChild(3).GetComponentInChildren<Button>();
        //加载设置按钮
        _setBtn = _TopPanel.GetChild(4).GetComponentInChildren<Button>();
        //加载特效Prefab

         GameObject[] prefabsObjs =  Resources.LoadAll<GameObject>("Prefabs/Effects");

        foreach (var prefab in prefabsObjs)
        {

            _effectPrefabs.Add(prefab);

        }

        #region 触摸按钮相关
        //加载触摸头部按钮
        _touchHeadBtn = _TouchPanel.GetChild(0).GetComponent<Button>();
        //加载触摸身体按钮
        _touchBodyBtn = _TouchPanel.GetChild(1).GetComponent<Button>();
        //加载升级视频按钮
        _levelUpBtn = _TouchPanel.GetChild(2).GetComponent<Button>();
        #endregion

        #region 约会/打工/喂食 相关
        //加载列表
        _dateListContent = _DatePanel.GetChild(1).GetChild(0).GetChild(0);
        _workListContent = _WorkPanel.GetChild(1).GetChild(0).GetChild(0);
        _feedListContent = _FeedPanel.GetChild(1).GetChild(0).GetChild(0);
        //加载约会列表子项
        _dateList = new List<Button>();
        _dateList.Add(_dateListContent.GetChild(1).GetComponent<Button>());
        _dateList.Add(_dateListContent.GetChild(2).GetComponent<Button>());
        _dateList.Add(_dateListContent.GetChild(3).GetComponent<Button>());
        _dateList.Add(_dateListContent.GetChild(4).GetComponent<Button>());
        //加载打工列表子项
        _workList = new List<Button>();
        _workList.Add(_workListContent.GetChild(1).GetComponent<Button>());
        _workList.Add(_workListContent.GetChild(2).GetComponent<Button>());
        _workList.Add(_workListContent.GetChild(3).GetComponent<Button>());
        _workList.Add(_workListContent.GetChild(4).GetComponent<Button>());
        //加载喂食列表子项
        _feedList = new List<Button>();
        _feedList.Add(_feedListContent.GetChild(1).GetComponent<Button>());
        _feedList.Add(_feedListContent.GetChild(2).GetComponent<Button>());
        _feedList.Add(_feedListContent.GetChild(3).GetComponent<Button>());
        _feedList.Add(_feedListContent.GetChild(4).GetComponent<Button>());

        #endregion

        #region 进度条相关
        //
        _moodbar = _Probars.GetChild(0).GetChild(1).GetComponent<Image>();//加载心情进度条
        _moodBottomText = _moodbar.transform.parent.GetChild(0).GetComponent<Text>();//加载底部指示数值
        _moodTopText = _moodbar.GetComponentInChildren<Text>();//加载顶部指示数值
        _feedbar = _Probars.GetChild(1).GetChild(1).GetComponent<Image>();//加载饱食进度条
        _feedBottomText = _feedbar.transform.parent.GetChild(0).GetComponent<Text>();//加载底部指示数值
        _feedTopText = _feedbar.GetComponentInChildren<Text>();//加载顶部指示数值
        _sleepbar = _Probars.GetChild(2).GetChild(1).GetComponent<Image>();//加载睡眠进度条
        _sleepBottomText = _sleepbar.transform.parent.GetChild(0).GetComponent<Text>();//加载底部指示数值
        _sleepTopText = _sleepbar.GetComponentInChildren<Text>();//加载顶部指示数值
        _gameBtn = _Probars.GetChild(0).GetComponent<Button>();//加载玩耍按钮
        _feedBtn = _Probars.GetChild(1).GetComponent<Button>();//加载喂食按钮
        _sleepBtn = _Probars.GetChild(2).GetComponent<Button>(); //加载睡眠按钮
        #endregion

        #region 游戏相关
        _gameJBtn = _GamePanel.GetChild(0).GetChild(0).GetComponent<Button>();//加载游戏剪刀按钮
        _gameBBtn = _GamePanel.GetChild(0).GetChild(1).GetComponent<Button>();//加载游戏包袱按钮
        _gameQBtn = _GamePanel.GetChild(0).GetChild(2).GetComponent<Button>();//加载游戏拳头按钮
        _gameReturn = _GamePanel.GetChild(1).GetComponent<Button>();//加载游戏界面返回按钮
        _RedPanel = _GamePanel.GetChild(2);//加载游戏红心面板
        _redArray = new RawImage[] { _RedPanel.GetChild(0).GetComponent<RawImage>(), _RedPanel.GetChild(1).GetComponent<RawImage>(), _RedPanel.GetChild(2).GetComponent<RawImage>() };
        #endregion
        //
        _homeBtn = _HomeBars.GetChild(0).GetComponent<Button>();//加载家按钮
        _dateBtn = _HomeBars.GetChild(1).GetComponent<Button>();//加载外出按钮
        _workBtn = _HomeBars.GetChild(2).GetComponent<Button>();//加载工作按钮
        _packBtn = _HomeBars.GetChild(3).GetComponent<Button>();//加载背包按钮
        _shopBtn = _HomeBars.GetChild(4).GetComponent<Button>();//加载商店按钮
        _btnArray = new Button[] { _homeBtn, _dateBtn, _workBtn, _packBtn, _shopBtn };
        _whitepng = _HomeBars.GetChild(5) as RectTransform;//加载底部白条

        //弹窗加载
        _PopBtn = _PopPanel.GetComponent<Button>();
        _SetPanel = _PopPanel.GetChild(0);
        _set1 = _SetPanel.GetChild(0).GetComponent<Button>();
        _set2 = _SetPanel.GetChild(1).GetComponent<Button>();
        _set3 = _SetPanel.GetChild(2).GetComponent<Button>();
        _set4 = _SetPanel.GetChild(3).GetComponent<Button>();

        _InputPanel = _PopPanel.GetChild(2); //加载输入面板
        _inputText = _InputPanel.GetChild(1).GetComponentInChildren<Text>(); //加载输入框
        _confirmBtn = _InputPanel.GetChild(3).GetComponent<Button>(); //加载输入确定按钮
        // _femaleText = _DialoguePanel.GetChild(1).GetComponentInChildren<Text>();//加载女主对话
        //_maleText = _DialoguePanel.GetChild(2).GetComponentInChildren<Text>();//加载男主对话
        // _dialogueBtn = _DialoguePanel.GetChild(3).GetComponent<Button>();//加载对话按钮

        //
        #region /***********--加载页面--**********/

        images = _loadingPanel.GetComponentsInChildren<Image>();//加载面板片
        //去掉第一个

        int m = images.Length;
        ArrayList al = new ArrayList(images);
        al.RemoveAt(0);
        images = (Image[])al.ToArray(typeof(Image));

        changes = new Coroutine[8];

        //_IniPanel.gameObject.SetActive(true);


        #endregion
     



        #endregion


        #region  /***********--定义底部homebar按钮事件--********************/

        //home按钮事件
        _homeBtn.onClick.AddListener(() => {

            BtnManager(_homeBtn, 1);//按钮特效

            //_GamePanel.gameObject.SetActive(false);
            _Probars.gameObject.SetActive(true);
            _TouchPanel.gameObject.SetActive(true);
            //如果其他列表打开，则关闭
            _DatePanel.gameObject.SetActive(false);
            _WorkPanel.gameObject.SetActive(false);
            _FeedPanel.gameObject.SetActive(false);
            _GamePanel.gameObject.SetActive(false);

        });
        //约会按钮事件
        _dateBtn.onClick.AddListener(() => {

            BtnManager(_dateBtn, 2);//按钮特效

            // _GamePanel.gameObject.SetActive(false);
            _Probars.gameObject.SetActive(true);

            _DatePanel.gameObject.SetActive(true);//打开约会列表
            //关闭其他列表
            _WorkPanel.gameObject.SetActive(false);
            _FeedPanel.gameObject.SetActive(false);
            _Probars.gameObject.SetActive(false);
            _GamePanel.gameObject.SetActive(false);
            _TouchPanel.gameObject.SetActive(false);
        });
        //打工按钮事件
        _workBtn.onClick.AddListener(() => {

            BtnManager(_workBtn, 3);//按钮特效

            //_GamePanel.gameObject.SetActive(false);
            _Probars.gameObject.SetActive(true);

            _WorkPanel.gameObject.SetActive(true);//打开工作列表
            //关闭其他列表
            _DatePanel.gameObject.SetActive(false);
            _FeedPanel.gameObject.SetActive(false);
            _Probars.gameObject.SetActive(false);
            _GamePanel.gameObject.SetActive(false);
            _TouchPanel.gameObject.SetActive(false);
        });
        //背包按钮事件
        _packBtn.onClick.AddListener(() => {

            BtnManager(_packBtn, 4);//按钮特效

            //_GamePanel.gameObject.SetActive(false);
            _Probars.gameObject.SetActive(true);
            //如果其他列表打开，则关闭
            _DatePanel.gameObject.SetActive(false);
            _WorkPanel.gameObject.SetActive(false);
            _FeedPanel.gameObject.SetActive(false);
            _Probars.gameObject.SetActive(false);
            _GamePanel.gameObject.SetActive(false);
            _TouchPanel.gameObject.SetActive(false);
        });
        //商店按钮事件
        _shopBtn.onClick.AddListener(() => {

            BtnManager(_shopBtn, 5);//按钮特效

            //_GamePanel.gameObject.SetActive(false);
            _Probars.gameObject.SetActive(true);
            //如果其他列表打开，则关闭
            _DatePanel.gameObject.SetActive(false);
            _WorkPanel.gameObject.SetActive(false);
            _FeedPanel.gameObject.SetActive(false);
            //_Probars.gameObject.SetActive(false);
            _GamePanel.gameObject.SetActive(false);
           // _TouchPanel.gameObject.SetActive(false);
        });

        #endregion

        //玩耍事件
        _gameBtn.onClick.AddListener(() => {

            



        });
        //喂食事件
        _feedBtn.onClick.AddListener(() => {


            _Probars.gameObject.SetActive(false);//关闭状态进度条面板
            _FeedPanel.gameObject.SetActive(true);//打开喂食面板
            _TouchPanel.gameObject.SetActive(false);//关闭触摸面板

        });
        //睡眠事件
        _sleepBtn.onClick.AddListener(() => {

        

        });

        //游戏界面返回按钮事件
        _gameReturn.onClick.AddListener(() => {

        

        });

        #region//界面设置按钮事件
        _setBtn.onClick.AddListener(() => {

            _PopPanel.gameObject.SetActive(true);
            _SetPanel.gameObject.SetActive(true);

        });
        _PopBtn.onClick.AddListener(() => _PopPanel.gameObject.SetActive(false));

        _set1.onClick.AddListener(() => {



        });
        _set2.onClick.AddListener(() => {



        });
        _set3.onClick.AddListener(() => {



        });
        _set4.onClick.AddListener(() => {



        });
        #endregion
        //刷新一次UI
        RefreshGUI();


       // movie = MovieManager.GetInstance();

    }


    void Start() {
     
        Invoke("UpdateIndicate", 0.0001f);//底部home按钮的白条指示初始化

      

        // IniManger();

        // StartCoroutine("Execute");

        // UpdateBellyPmgressbar(-5f);


    }

 


    //主页面与游戏页面切换时的面板显示
    public void SwitchScene(bool sw) // true 为进入游戏  false为进入主页面
    {



        //调整面板的显示
        _ScrollPanel.gameObject.SetActive(!sw);//关闭滚动面板
        _TopPanel.gameObject.SetActive(!sw);//关闭顶部面板
        _Probars.gameObject.SetActive(!sw);//关闭状态进度条面板
        _BottomPanel.gameObject.SetActive(!sw);//关闭底部面板
        _GamePanel.gameObject.SetActive(sw);//打开游戏选项面板
        _EffectsPanel.gameObject.SetActive(!sw);//关闭特效面版
        _TouchPanel.gameObject.SetActive(!sw);//关闭触摸面板


    }






    #region/*********************--更新数值/进度条 相关--*****************************/

    public int redCount = 0;
    //游戏红心状态转变方法
   public void RedPngState(bool isGameing)
    {
        Color tempColor = new Color(0, 0, 1, 0.5f) ;
        if (isGameing)
        {
            _redArray[redCount].color = tempColor;

            redCount++;

        }
        else
        {
            tempColor.a = 1f;
            foreach (var item in _redArray)
            {
                item.color = tempColor;
            }
            redCount = 0;
        }
        
    }
    //游戏胜利一次方法
    public void GameVictoryMethod()
    {

        playerData.gameVictory++;


    }


    //睡眠对话框
    void SetFalse()
    {

        _EffectsPanel.GetChild(0).gameObject.SetActive(false);

    }

    //UI初始化
    public  void RefreshGUI()
    {
        //好感度进度条
        _levelExp.fillAmount = 44 / 50f;
        //好感度底部进度条
        _LevelBottom.fillAmount = 1f - (44 / 50f);
        //好感度等级
        _level.text = playerData.level.ToString();
        //体力条
        _powerbar.fillAmount = playerData.healthPoint / 100f;
        //体力进度显示
        _powerbar.GetComponentInChildren<Text>().text = playerData.healthPoint.ToString() + "/" +"100";
        //金钱
        _coinsBtn.GetComponentInChildren<Text>().text = playerData.coins.ToString();
        //钻石
        _DmBtn.GetComponentInChildren<Text>().text = playerData.diamonds.ToString();

        //心情值进度条
        _moodbar.fillAmount = playerData.moodPoint / 100f;
        //底部心情进度字符串
        _moodbar.transform.parent.GetChild(0).GetComponent<Text>().text = playerData.moodPoint.ToString() ;
        //顶部心情进度字符串
        _moodbar.GetComponentInChildren<Text>().text = playerData.moodPoint.ToString();

        //饱食度进度条
        _feedbar.fillAmount = Mathf.Floor(playerData.bellyPoint) / 100f;
        //底部饱食进度字符串
        _feedbar.transform.parent.GetChild(0).GetComponent<Text>().text = playerData.bellyPoint.ToString();
        //顶部饱食进度字符串
        _feedbar.GetComponentInChildren<Text>().text = playerData.bellyPoint.ToString();

        //睡眠值进度条
        _sleepbar.fillAmount = playerData.FatiguePoint / 100f;
        //底部睡眠进度字符串
        _sleepbar.transform.parent.GetChild(0).GetComponent<Text>().text = playerData.FatiguePoint.ToString();
        //顶部睡眠进度字符串
        _sleepbar.GetComponentInChildren<Text>().text = playerData.FatiguePoint.ToString();


    }
    //更新进度条与加载特效方法----根据索引确定更新那些UI，根据数组来相应的赋值
    public void UpdateUIAndEffect(int index, float[] ResoultList)
    {

        switch (index)
        {
            case 0://约会

                break;
            case 1: // 打工

                break;
            case 2: // 喂食

                /*
                //进度条更新
                _UIManager.UpdateBellyPmgressbar(ResoultList[1]);//饱食进度条增加
                _UIManager.UpdateDiamText(ResoultList[2]);//钻石进度减少
                _UIManager.UpdateFavorPmgressbar(ResoultList[3], ResoultList[4], 0);//好感度增加

                //加载特效  /0:belleyEffect /1: diamondEffect /2: favorEffect /3: moneyEffect /4: moodEffect /5: powerEffect /6: sleepEffect   
                _UIManager.StartEffect(_UIManager._effectPrefabs[0], "+" + ResoultList[5]);//饱食特效
                _UIManager.StartEffect(_UIManager._effectPrefabs[1], "-" + ResoultList[6]);//钻石特效
                _UIManager.StartEffect(_UIManager._effectPrefabs[2], "+" + ResoultList[7]);//好感特效
                _UIManager.InitWaitConunt();//初始化特效间隔

                */

                break;
            case 3:

                break;
            case 4:

                break;
        }



       


    }

    #endregion

    #region 更新数值计算方法

    //更新好感度等级
    public void UpdateFavorLevelText(float laterValue)
    {

        _level.text = laterValue.ToString();

    }

    //更新钻石
    private Coroutine DiamTextCoro;
    public void UpdateDiamText(float laterValue)
    {
        Text dmText = _DmBtn.GetComponentInChildren<Text>();
        //变化前的钻石数值       
        float curDiam = float.Parse(dmText.text); 
        //变化后的钻石数值
        float laterDiam = laterValue;

        //启动文本指示动画
        
        DiamTextCoro = StartCoroutine(TweenText(new Text[] { dmText }, curDiam, laterDiam,6));

        Textcoros[6] = DiamTextCoro;

    }

    //更新金钱
    private Coroutine coinsTextCoro;
    public void UpdateCoinsText(float laterValue)
    {
        Text coinsText = _coinsBtn.GetComponentInChildren<Text>();
        //变化前金钱数量
        float curCoins = float.Parse(coinsText.text);
        //变化后的数值
        float laterCoins = laterValue;
        
        //启动指示文本动画
        coinsTextCoro =  StartCoroutine(TweenText(new Text[] { coinsText }, curCoins, laterCoins, 7));
        Textcoros[7] = coinsTextCoro;
    }

    //更新好感度进度条
    private Coroutine FavorCoro;
    private Coroutine FavorCoro2;
    public void UpdateFavorPmgressbar(float laterValue, float MaxPoint, float preMaxPoint, bool isInit = false)//10, 50
    {

        // 100 * x = 50;x = 50 / 100 getmaxpoint / 100

        if (isInit) // 升级进度更新
        {
            float curFavorPoint = _levelExp.fillAmount * 100f;

            float laterFavorPoint = Mathf.Floor((laterValue / MaxPoint) * 100);

          
            FavorCoro = StartCoroutine(TweenPrograssBar(_levelExp, curFavorPoint, preMaxPoint, 0, new float[] { laterFavorPoint, 0 }));

            coros[0] = FavorCoro;
            //
            FavorCoro2 = StartCoroutine(TweenPrograssBar(_LevelBottom, 100 - curFavorPoint, 100 - preMaxPoint, 1, new float[] { laterFavorPoint, 1}));//底部白条变化

            coros[1] = FavorCoro2;

        }
        else
        {
            float curFavorPoint = _levelExp.fillAmount * 100f;

            float laterFavorPoint = Mathf.Floor((laterValue / MaxPoint) * 100);

            FavorCoro = StartCoroutine(TweenPrograssBar(_levelExp, curFavorPoint, laterFavorPoint, 0));

            coros[0] = FavorCoro;
            //
            FavorCoro2 = StartCoroutine(TweenPrograssBar(_LevelBottom, 100 - curFavorPoint, 100 - laterFavorPoint, 1));//底部白条变化

            coros[1] = FavorCoro2;

        }


       

    }

    IEnumerator UpdateProgressCoro(Image image, float curFavorPoint, float laterFavorPoint, int index)
    {

        if(curFavorPoint > laterFavorPoint)
        {
            curFavorPoint = 0;

        }

        float a = 0f;

        while (true)
        {
            a += 0.5f * 0.04f;

            if(a > 1f)
            {
                StopCoroutine(coros[index]);


            }
            image.fillAmount = Mathf.Lerp(curFavorPoint, laterFavorPoint ,a);

            yield return new WaitForSeconds(0.04f);
        }



    }

    //更新体力进度条
    private Coroutine powerCoro;
    private Coroutine powerTextCoro;
    public void UpdatePowerPmgressbar(float laterValue)
    {
        //当前体力值
        float curPowerPoint = _powerbar.fillAmount * 100f;
        //变化后的体力值
        float laterPowerPoint =laterValue;
        //启动进度条动画



        powerCoro =  StartCoroutine(TweenPrograssBar(_powerbar, curPowerPoint, laterPowerPoint, 5));
        coros[5] = powerCoro;
        //启动文本指示动画
        powerTextCoro =  StartCoroutine(TweenText(new Text[] { _powerText }, curPowerPoint, laterPowerPoint, 5, true));
        Textcoros[5] = powerTextCoro;

    }
    

    private Coroutine moodCoro;
    private Coroutine moodTextCoro;
    //更新心情进度条
    public void UpdateMoodPmgressbar(float latervalue)
    {
        //当前心情值（变化前）
        float curMoodPoint = _moodbar.fillAmount * 100f; //Debug.Log(curMoodPoint);
        //变化后的心情值
        // float laterMoodPoint = changeValue == 0 ? 0 : curMoodPoint + changeValue;
        float laterMoodPoint = latervalue + (curMoodPoint - Mathf.Floor(curMoodPoint));

       
            //启动进度条动画

            moodCoro = StartCoroutine(TweenPrograssBar(_moodbar, curMoodPoint, laterMoodPoint, 2));

            coros[2] = moodCoro;

            // moodCoro =  StartCoroutine(TweenPrograssBar(_moodbar, curMoodPoint, laterMoodPoint));
            //启动文本指示动画

            moodTextCoro = StartCoroutine(TweenText(new Text[] { _moodBottomText, _moodTopText }, curMoodPoint, laterMoodPoint, 2));

            Textcoros[2] = moodTextCoro;

            // StartCoroutine(TweenText(_moodBottomText, curMoodPoint, laterMoodPoint));
            // StartCoroutine(TweenText( _moodTopText, curMoodPoint, laterMoodPoint));


        



    }
    private Coroutine bellyCoro;
    private Coroutine bellyTextCoro;

    Coroutine[] coros = new Coroutine[10];
    Coroutine[] Textcoros = new Coroutine[10];

    //更新饱食度进度条 
    public void UpdateBellyPmgressbar(float laterValue)// 变化后的饱食度
    {
        /*
        //当前饱食度（变化前）
        float curBellyPoint = _feedbar.fillAmount * 100f;
        //变化后的饱食度      
        float laterBellyPoint =  changeValue == 0 ?  0 :  curBellyPoint + changeValue;
        */
        //当前饱食度（变化前）
        float curBellyPoint =  _feedbar.fillAmount * 100f; //Debug.Log(curBellyPoint);     
        //变化后的饱食度   
       // float laterBellyPoint = changeValue == 0 ? 0 : curBellyPoint + changeValue; //Debug.Log(laterBellyPoint);
        float laterBellyPoint = laterValue + (curBellyPoint - Mathf.Floor(curBellyPoint));

        //float laterBellyPoint = curBellyPoint + changeValue;

      

        //coros.Add(bellyCoro);

        //启动进度条动画
            bellyCoro = StartCoroutine(TweenPrograssBar(_feedbar, curBellyPoint, laterBellyPoint, 3));

            coros[3] = bellyCoro;


            //启动文本指示动画
            
            bellyTextCoro = StartCoroutine(TweenText(new Text[] { _feedBottomText, _feedTopText }, curBellyPoint, laterBellyPoint, 3));

            Textcoros[3] = bellyTextCoro;


            // StartCoroutine(TweenText(_feedBottomText, curBellyPoint, laterBellyPoint));
            // StartCoroutine(TweenText( _feedTopText, curBellyPoint, laterBellyPoint));

        
        

    }

    private Coroutine sleepCoro;
    private Coroutine sleepTextCoro;

    //更新睡眠进度条   
    public void UpdateSleepPmgressbar(float laterValue)
    {

        //当前睡眠值（变化前）
        float curSleepPoint = _sleepbar.fillAmount * 100f; //Debug.Log(curSleepPoint);

       

        //变化后的睡眠值
        // float laterSleepPoint = changeValue == 0 ? 0 : curSleepPoint + changeValue;
        float laterSleepPoint = laterValue + (curSleepPoint - Mathf.Floor(curSleepPoint));

      
            //启动进度条动画

            sleepCoro = StartCoroutine(TweenPrograssBar(_sleepbar, curSleepPoint, laterSleepPoint, 4));

            coros[4] = sleepCoro;

            // StartCoroutine(TweenPrograssBar(_sleepbar, curSleepPoint, laterSleepPoint));

            sleepTextCoro = StartCoroutine(TweenText(new Text[] { _sleepBottomText, _sleepTopText }, curSleepPoint, laterSleepPoint, 4));

            Textcoros[4] = sleepTextCoro;

            // StartCoroutine(TweenText(_sleepBottomText, curSleepPoint, laterSleepPoint));
            // StartCoroutine(TweenText(_sleepTopText, curSleepPoint, laterSleepPoint));


      


    }

    //进度条动画
    IEnumerator TweenPrograssBar(Image image , float curValue, float laterValue, int coroIndex, float[] levelValue = null)
    {


        if(levelValue != null) // curValue为当前进度值， laterValue为100，为一个满值， levelValue[0]为升级后变化的值
        {
            float curPoint = curValue;
            float laterPoint = laterValue;


            float Point  = curValue;

            float Point2 = 0;
            float laterPoint2 = levelValue[0];

            bool isChange = true;

            while (true)   // 两个变化的值不等 
            {

                //没有变化到满值
                if (Point != laterValue)
                {
                    Point++;

                    image.fillAmount = Mathf.Floor(Point) / 100;

                }
                else //进度满值的时候,初始化
                {

                    if (isChange)
                    {
                        isChange = false;
                        if (levelValue[1] == 0)
                        {
                            image.fillAmount = 0f;
                        }
                        else
                        {
                            image.fillAmount = 1f;
                        }


                    }
                    else
                    {
                        //升级后没有到相应数值时
                        if(Point2 != laterPoint2)
                        {
                            Point2++;

                            image.fillAmount = Mathf.Floor(Point2) / 100;
                        }
                        else
                        {

                            break;//跳出循环
                        }




                    }



                   

                }

               


                yield return new WaitForSeconds(0.005f);

            }




        }
        else
        {

            float _curValue = curValue;
            float _laterValue = laterValue;


            float change = _curValue;

            while (change != _laterValue)   // 两个变化的值不等 
            {

                if (_laterValue == 0)
                {

                    change -= 1f;

                }
                else
                {


                    if ((_laterValue - _curValue) < 0) //减少
                    {
                        change -= 1f;

                    }
                    else //增加
                    {
                        change += 1f;

                    }


                }


                image.fillAmount = Mathf.Floor(change) / 100;


                yield return new WaitForSeconds(0.005f);

            }



         



        }

        StopCoroutine(coros[coroIndex]);
        coros[coroIndex] = null;


        /*
        Image image = (Image)args[0];

        float _curValue = (float)args[1]; //Debug.Log(curValue);
        float _laterValue = (float)args[2];
        */





    }

    ArrayList CoroTextArgs = new ArrayList();

    //文本指示数值动画      Text changeText1, Text changeText2, float curValue, float laterValue, bool isDouble
    IEnumerator TweenText(Text[] changeText, float curValue, float laterValue, int coroIndex, bool isDouble = false ) 
    {
        #region

         float _curValue = curValue ;      
         float _laterValue = laterValue;
         float change = _curValue;

        /*
        float _curValue = (float)args[2];
        float _laterValue = (float)args[3];
        float change = _curValue;

        Text text1 = (Text)args[0];
        Text text2 = (Text)args[1];

        bool isDouble = (bool)args[4];
       */

        while (change != _laterValue) //变化没有停止时
        {


            if (_laterValue == 0) // 变化到0
            {

                change -= 1f;

            }
            else
            {
                if ((_laterValue - _curValue) < 0) //当前变化是往小的变
                {

                    change--;

                }
                else //当前是在增加
                {



                    change++;


                }


            }
            
            if(changeText.Length == 2)
            {
                changeText[0].text = change.ToString();
                changeText[1].text = change.ToString();


            }
            else
            {
                changeText[0].text = ((int)change).ToString();


            }




            if (isDouble)
            {
             
                changeText[0].text = ((int)change).ToString() + "/" + "100";
            }

            #endregion

            yield return new WaitForSeconds(0.01f);

        }

   
       

        StopCoroutine(Textcoros[coroIndex]);
        Textcoros[coroIndex] = null;
    }

    #endregion

    #region//禁止/启用 

    //禁止所有按钮使用
    public void AllButtonInteractable(bool state)
    {

        foreach (var homeBtn in _HomeBars.GetComponentsInChildren<Button>())
        {

            homeBtn.interactable = state;


        }

        foreach (var gressBarBtn in _Probars.GetComponentsInChildren<Button>())
        {
            gressBarBtn.interactable = state;
        }



    }
    //隐藏/显示UI
    public void HideUI(bool state)
    {

        _TopPanel.gameObject.SetActive(state);
        _BottomPanel.gameObject.SetActive(state);


    }


    //游戏按钮点击
    public void GameButtonInteractable(bool state)
    {
        foreach (var Btn in _GamePanel.GetComponentsInChildren<Button>())
        {
            //让游戏面板禁止响应/接受响应。遍历游戏选项按钮并设为禁用/启用
            Btn.interactable = state;

        }



    }

    #endregion

    #region 底部按钮特效
    //若底部按钮个数变化，指示条更新方法
    public void UpdateIndicate()
    {
        //_homeBtn.onClick.Invoke();
        Vector2 vec = new Vector2((_btnArray[0].transform as RectTransform).anchoredPosition.x, -258.8f);
        _whitepng.anchoredPosition = vec;


    }

    //底部按钮特效
    void BtnManager(Button Btn, int index)
    {
        
        //所有按钮恢复到变暗状态
        foreach (var item in _btnArray)
        {
            //图片
            Image image = item.GetComponent<Image>();
            Color tempColor = image.color;
            tempColor.a = 0.4f;
            image.color = tempColor;
            //字体
            item.GetComponentInChildren<Text>().color = tempColor;


        }

        //移动底部白条
        MoveWhite(index, _btnArray);
       // _whitepng.Translate(new Vector2(0.1f, 0));

        //加载当前按钮特效-（协程处理）
        StartCoroutine("BtnEffect", Btn);

    }
    //移动白条
    void MoveWhite(int index, Button[] BtnArray)
    {
        //判断当前激活的按钮数
        int btnCount = 0;
        //按钮位置数组
        List<Vector2> RectList = new List<Vector2>();
        
        foreach (Button Btn in BtnArray)
        {
           if(Btn.IsActive())
            {                

                RectList.Add(new Vector2((Btn.transform as RectTransform).anchoredPosition.x, -258.8f));
              
                btnCount++;
            }

           
          

        }

        //初始化按钮位置

       //计算每个按钮的位置

        //索引与当前激活的按钮的对应
        switch (index)
        {
            case 1:
                //获得当前的应该显示的位置
                // _whitepng.anchoredPosition = RectList[0];
                //
                //_whitepng.anchoredPosition = RectList[0];

                //调用缓动动画
                StartCoroutine(IndicateBarMove(RectList[0]));
                break;
            case 2:
                //获得当前的应该显示的位置
                //_whitepng.anchoredPosition = RectList[1];
                //调用缓动动画
                StartCoroutine(IndicateBarMove(RectList[1]));
                break;
            case 3:
                //获得当前的应该显示的位置
                //_whitepng.anchoredPosition = RectList[2];
                //调用缓动动画
                StartCoroutine(IndicateBarMove(RectList[2]));
                break;
            case 4:
                //获得当前的应该显示的位置
                //_whitepng.anchoredPosition = RectList[3];
                //调用缓动动画
                StartCoroutine(IndicateBarMove(RectList[3]));
                break;
            case 5:
                //获得当前的应该显示的位置
               // _whitepng.anchoredPosition = RectList[4];
                //调用缓动动画
                StartCoroutine(IndicateBarMove(RectList[4]));
                break;




        }


    }
    //指示条移动
    IEnumerator IndicateBarMove(Vector2 aimPositon)
    {
        


        while ( Mathf.Abs( aimPositon.x -  _whitepng.anchoredPosition.x) > 0.3f)
        {

            _whitepng.anchoredPosition =  Vector2.Lerp(_whitepng.anchoredPosition, aimPositon, 0.5f );

           

            //范围性判断


            yield return new WaitForSeconds(0.01f);

        }

       
    }

    #endregion

    //点击页面切换按钮后的特效
    IEnumerator BtnEffect(Button Btn)
    {
        Color tempColor;
        Image image = Btn.GetComponent<Image>();//图片
        Text tex = image.GetComponentInChildren<Text>();//字体
        tempColor = image.color;
        while (image.color.a < 1f)
        {
           //image.color= Color.Lerp(image.color, new Color(image.color.r, image.color.g, image.color.b, 1f),1.5f);// Time.deltaTime);
           tempColor = Color.Lerp(image.color, new Color(image.color.r, image.color.g, image.color.b, 1f), 1.5f);// Time.deltaTime);
            image.color = tempColor;
            tex.color = tempColor;
            yield return new WaitForSeconds(0.01f);

        }
       
        yield return null;

    }

    #region /***********--增加好感度等的特效--**************/

    [HideInInspector]
    public bool isDestrooy = false;//特效释放播放完毕
    [HideInInspector]
    public static float waitCount = 0;
    //状态改变特效
   public void StartEffect(GameObject effectObj, string text)
    {
        waitCount++;
        
        //实例化特效对象
        GameObject insObj = Instantiate(effectObj) as GameObject;
        // RectTransform insObj =  insObj.transform as RectTransform;
        insObj.GetComponentInChildren<Text>().text =text;

        isDestrooy = false;

        //等待一段事件后调用
        //InvokeRepeating
        

        StartCoroutine("MoveToEffect", insObj);


    }
    //向上移动并逐渐透明且消失
    IEnumerator MoveToEffect(GameObject effectObj)
    {

       

        yield return new WaitForSeconds(0.6f * (waitCount - 1f));

   
        RectTransform effectObjRect = effectObj.transform as RectTransform;

        effectObj.transform.parent = _EffectsPanel;//显示特效

        effectObjRect.localPosition = new Vector3(0, 540f, 0);
        effectObjRect.localScale = new Vector3(1f, 1f, 1f);
              
        Vector2 effectVec = effectObjRect.anchoredPosition;
        Color imageColor = effectObjRect.GetChild(0).GetComponent<Image>().color;
        Color textColor = effectObjRect.GetComponentInChildren<Text>().color;
       
        //开始移动
        while (imageColor.a > 0)
         // while(effectVec.y < 680f)
        {
                     
            effectVec.y += 2f;
            
            if(effectVec.y > 680)
            {
                imageColor.a -= 0.02f;
                textColor.a -= 0.02f;
                effectObjRect.GetChild(0).GetComponent<Image>().color = imageColor;
                effectObjRect.GetComponentInChildren<Text>().color = textColor;

            }
                       
            effectObjRect.anchoredPosition = effectVec;

            yield return new WaitForSeconds(0.005f);
           
        }


        Destroy(effectObj);
        isDestrooy = true;


    }
    //初始化特效间隔
    public void InitWaitConunt()
    {

        waitCount = 0;

    }
    //关闭特效协程
    public void CloseEffectCoro()
    {

        StopCoroutine("MoveToEffect");

    }
    #endregion

    #region/*****************--打工相关--***********************/
   
    [HideInInspector]
    public bool isWorkFinished = false;
    public void TimeCDMehod(Button itemBtn, float cdTime)
    {
       // isWorkFinished = false;

        //当前子项的最大工作次数
       // int maxWorkTiems = 30;

        //Text timeText = itemBtn.transform.GetChild(0).GetComponent<Text>();//冷却计时text
       // Text proficiencyText = itemBtn.transform.GetChild(1).GetComponent<Text>();//熟练度text



        //当前item不可在交互
        itemBtn.interactable = false;

        //显示时间Text
        //timeText.gameObject.SetActive(true);

        //熟练度更新
       // proficiencyText.text = "%" + (workTimes * 100) / maxWorkTiems;

        //显示打工倒计时
        timerCoro = StartCoroutine(TimerMethod(itemBtn, cdTime));//等倒计时为0的时候，通知isWorkFinished为true，监听即可。
        

    }
    
    //冷却时间计时
    private Coroutine timerCoro;
    [HideInInspector]
    public float CacheTime;
    IEnumerator TimerMethod(Button itemBtn, float time)
    {
        //分别计算出，小时位，分位，秒位的数值------1:20:50----(60+20)*60 +50 = time = 4850
        // float minutes = Mathf.Floor(time / 60);//

        CacheTime = time;

        float hour = 0;//小时
        float minutes = 0;//分钟
        float seconds = 0;//秒

        string hoursStr;//小时字符串
        string minutesStr;//分钟字符串
        string secondsStr;//秒字符串

        Text timeText = itemBtn.transform.GetChild(0).GetComponent<Text>();//冷却计时text
        timeText.gameObject.SetActive(true);//显示计时

        if (time >= 60)
        {

            if ((time / 3600) > 0)
            {
                hour = Mathf.Floor(time / 3600);//小时数

                minutes = Mathf.Floor((time - hour * 3600) / 60);//分钟数

                seconds = time - (hour * 3600) - (minutes * 60);

            }
            else
            {

                minutes = Mathf.Floor(time / 60);//分钟数

                seconds = time - (minutes * 60);//秒数

            }
            
        }
        else
        {

            seconds = time;//秒数

        }
       
     
        while (true)
        {
            
            yield return new WaitForSeconds(1f);//等待1秒

            CacheTime--;
            
            if(seconds != 0)// 秒位 不为0
            {
                seconds--;// 秒位 减一

            }
            else // 秒位 为0
            {
                if(minutes != 0)//分位不为0
                {
                    minutes--;
                    seconds = 59;
                }
                else// 分位 为0
                {

                    if (hour != 0)//小时位 不为0
                    {
                        hour--;
                        minutes = 59;
                        seconds = 59;
                    }
                    else // 小时位 为0
                    {
                        CacheTime = 0;
                        //隐藏时间Text
                        timeText.gameObject.SetActive(false);

                        itemBtn.interactable = true;//交互开启

                        //调用打工完成通知
                        isWorkFinished = true;


                        //停止计时
                        StopCoroutine(timerCoro);
                       
                    }

                }
                


            }

            //显示字符串

            if(hour < 10)
            {

                hoursStr = "0" + hour.ToString();

            }
            else
            {

                hoursStr = hour.ToString();

            }
           
            if(seconds < 10)
            {
               secondsStr = "0" + seconds.ToString();
            }
            else
            {
                secondsStr = seconds.ToString();
            }
           
            if(minutes < 10)
            {
                minutesStr = "0" + minutes.ToString();
            }
            else
            {
                minutesStr = minutes.ToString();
            }

            timeText.text = hoursStr + ":" + minutesStr + ":" + secondsStr;//显示
            
        }
        
    }
    
    #endregion



    #region /****************-对话相关-*******************/
    private bool isFemaleTalking = true;//当前女主讲话是否完毕,false则是讲话完毕
    private bool isMaleTalking = true; //当前男主讲话是否完毕， false讲话完毕
    private int  diaCount = 0; // 讲话索引
    private bool isStart = true;//当前开始第一次对话。
    private Coroutine dialogueCoro;
    private string[] FemaleStrs;//女主讲话数据
    private string[] MaleStrs;//男主讲话数据

    //对话功能模块
    public void Dialogue(string[] _femaleStrs, string[] _maleStrs)
    {
        //数据传递
        FemaleStrs = _femaleStrs;
        MaleStrs = _maleStrs;

        _DialoguePanel.gameObject.SetActive(true);//显示对话面板

        //启动对话方法
       // DialogueMethod(-1);

    }


    //无选择的对话
    public bool DialogueMethod(int index) //传递过来的数据为两个数组，一个为女主所讲的话，一个为男主所讲的话。
    {
       



        //参数将接受两段数据
        //一段是女主讲话数据
        //一段是男主讲话数据

        //如果女主将要讲多段对话，则，讲一段，用户按下屏幕，在讲一段，直到全都讲完后，开始男主讲话

        //男主讲话，如果没有选项选择的话， 则跟女主讲话一个套路。

        //dialogueStrs[0],
        //dialogueStrs[1]

        //根据点击移动索引
        diaCount += index;


        if(diaCount < FemaleStrs.Length) // 当前索引小于女主讲话段数
        {



        }


        if (isStart) // 当前是第一次讲话
        {

            isStart = false;//改变第一次讲话状态
            dialogueCoro =   StartCoroutine(TypeText(_femaleText, FemaleStrs[diaCount], isFemaleTalking));//女主开始第一次讲话
            
        }
        else //
        {
            //判断
            //女主说话完毕
            //男主说话完毕                   
           
            if (!isFemaleTalking) //女主说话是否完毕
            {
                //关闭上一次讲话后的协程
                StopCoroutine(dialogueCoro);
                //恢复女主未讲话状态
                isFemaleTalking = true;
                //男主文本
                dialogueCoro = StartCoroutine(TypeText(_maleText, MaleStrs[1], isMaleTalking));
            }
            else if(FemaleStrs.Length > 0)// 女主有多段对话要讲
            {

                if (!isFemaleTalking) // 已经讲话完毕
                {
                    //关闭上一次讲话的协程
                    StopCoroutine(dialogueCoro);

                    dialogueCoro = StartCoroutine(TypeText(_femaleText, FemaleStrs[0], isFemaleTalking));
                }

               


            }

            //如果男主讲话完毕，或当前是女主第一个讲话
            if (!isMaleTalking)
            {
                //关闭上一次的协程
                StopCoroutine(dialogueCoro);
                //恢复男主未讲话状态
                isMaleTalking = true;
                //女主开始讲话
                dialogueCoro =   StartCoroutine(TypeText(_femaleText, FemaleStrs[0], isFemaleTalking));

            }
            else if(MaleStrs.Length > 0)  //男主有多段话要讲
            {

                if (!isMaleTalking) // 已经讲话完毕
                {
                    //关闭上次协程
                    StopCoroutine(dialogueCoro);

                    dialogueCoro = StartCoroutine(TypeText(_maleText, MaleStrs[1], isMaleTalking));
                }

               

            }


            


        }

        // 全都讲话完毕
        StopCoroutine(dialogueCoro);


        

        return false;


    }

    //对于协程的关闭函数。
    private IEnumerator TypeText(Text dialoguText, string word, bool isTalking)
    {
        
        foreach (char letter in word.ToCharArray())
        {
            dialoguText.text += letter;

            yield return new WaitForSeconds(0.15f);
        }


    
        isTalking = false;//讲话完毕
        
        yield return new WaitForSeconds(5f);

    }

    #endregion


    #region/******************--弹出相关--**********************/
    //设置弹窗
    public void PopSetting()
    {


    }

    public void PopHintPower(bool PopOrhide)
    {

        _PopPanel.gameObject.SetActive(PopOrhide);

        _PopPanel.GetChild(1).gameObject.SetActive(PopOrhide);



    }


    #endregion
          

    #region 上隐下现动画效果
    public bool UpFinished = false;
    //上隐
    IEnumerator ExecuteUp()
    {
        UpFinished = false;

        for (int i = images.Length - 1; i >= 0; i--)
        {


            changes[i] = StartCoroutine(ChangeMethod(images[i], 1f, i));


            yield return new WaitForSeconds(0.1f);

        }

        yield return new WaitForSeconds(0.2f);

        UpFinished = true;

        StopCoroutine("ExecuteUp");

       

    }
    public bool DownFinished = false;
    //下现
    IEnumerator ExecuteDown()
    {

        DownFinished = false;

        for (int i = images.Length - 1; i >= 0; i--)
        {
            
            changes[i] = StartCoroutine(ChangeMethod(images[i], 0f, i));
            
            yield return new WaitForSeconds(0.1f);

        }

        yield return new WaitForSeconds(0.2f);

        DownFinished = true;

        StopCoroutine("ExecuteDown");
       
      

    }

   

    IEnumerator ChangeMethod(Image image, float alpha, int index)
    {
       
        Color stateColor1 = image.color;
        Color stateColor2 = image.color;
        Color newColor;
        stateColor2.a = alpha;
        float ap = 0;

        while (Mathf.Abs(image.color.a - alpha) > 0.01f)
        {

            ap += 0.08f;

            newColor = Color.Lerp(stateColor1, stateColor2, ap);

            image.color = newColor;

            yield return new WaitForSeconds(0.04f);
        }

        StopCoroutine(changes[index]);



    }

    #endregion




}
