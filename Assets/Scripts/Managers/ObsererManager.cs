using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 管理饱食、心情、疲劳三大管理器的运行。
/// 三大管理器为全局事件.
/// 三大数据基础变化
///    1.心情：饱食度低于33，心情每小时变化：1，2，3，4，5，5，5最多到5.
///    2.饱食：每小时降低10点。睡眠不降低。固定不受其他条件改变.
///    3.睡眠：每小时降低3点，哄睡进入睡眠状态，并慢慢回满睡眠值，8小时静止操作。达到0会自动睡眠，恢复到60，心情降低3.
///
/// </summary>
[RequireComponent(typeof(BellyPointManager))]
[RequireComponent(typeof(MoodPointManager))]
[RequireComponent(typeof(FatiguePointManager))]
public class ObsererManager : MonoBehaviour {

    //定义三大管理器
    private  BellyPointManager _BellyPointManager;
    private  MoodPointManager _MoodPointManager;
    private  FatiguePointManager _FatiguePointManager;

    //视频播放管理器
    //private PlayClipManager _MovieManager; 

    //UI管理器
    private UIManager _UIManager;

    private PlayerDataUtils playerDataUtils;

    private void Awake()
    {
        //加载三大管理器
        this._BellyPointManager = gameObject.GetComponent<BellyPointManager>();
        this._MoodPointManager = gameObject.GetComponent<MoodPointManager>();
        this._FatiguePointManager = gameObject.GetComponent<FatiguePointManager>();

        //加载视频管理器
        //_MovieManager = GameObject.Find("MovieCamera").GetComponent<PlayClipManager>();

        //加载UI管理器
        _UIManager = GameObject.Find("Prograss_Panel").GetComponent<UIManager>();

        //管理自动事件处理

        playerDataUtils = PlayerDataUtils.getInstance();

        playerDataUtils.test();

    }
    
    // Use this for initialization
    void Start () {

        //订阅饱食/心情/疲劳 变化事件
        AddBellyEventListener();
        AddMoodEventListener();
        AddFatigueEventListener();

        //启动饱食与疲劳协程
        _BellyPointManager.StartCoroutine("BellyPointChangeCoroutine");
        // moodPointManager.StartCoroutine("MoodPointChangeCoroutine");
        _FatiguePointManager.StartCoroutine("FatiguePointChangeCoroutine");

        

    }

    //在什么样的情况下触发调用饱食、心情、疲劳的管理协程。
    //TODO


    /// <summary>
    /// 订阅饱食变换事件
    /// </summary>
    private void AddBellyEventListener()
    {

        _BellyPointManager.OnReduceBellyPointEvent += BellyCallBackMethod;




    }
    /// <summary>
    /// 订阅心情变换事件
    /// </summary>
    private void AddMoodEventListener()
    {

        _MoodPointManager.OnReduceMoodPointEvent += MoodCallBackMethod;

    }
    /// <summary>
    /// 订阅疲劳变换事件
    /// </summary>
    private void AddFatigueEventListener()
    {

        _FatiguePointManager.OnReduceFatiguePointEvent += FatigueCallBackMethod;

    }
    /// <summary>
    /// 饱食回调函数
    /// </summary>
    private void BellyCallBackMethod(float BellyValue)
    {
        //通知UI更新
        Debug.Log("饱食度UI更新...");
        _UIManager.BellyUIUpdeta(Mathf.Abs(BellyValue));
        

    }
    /// <summary>
    /// 心情回调函数
    /// </summary>
    private void MoodCallBackMethod(float changValue)
    {
        //通知UI更新
        Debug.Log("心情值UI更新...");
        _UIManager.MoodUIUpdate(changValue,_MoodPointManager.currentMoodPoint);



    }
    /// <summary>
    /// 疲劳回调函数
    /// </summary>
    private void FatigueCallBackMethod(float FatigueValue)
    {
        //通知UI更新
        Debug.Log("疲劳度UI更新...");
        _UIManager.FatigueUIUpdate(Mathf.Abs(FatigueValue));

    }


    #region   /******事件的订阅示例******/

    /*
    //订阅饱食事件
    private void AddBellyListener()
    {
        //像this.bellyManager的OnSubBelly事件，登记方法OnSubBellyMethod方法的地址。
        //this.bellyManager.OnSubBelly += this.OnSubBellyMethod;

        //用匿名方法订阅
        //this.bellyManager.OnSubBelly += delegate (float a) { Debug.Log(a); };

        //用Lambda表达式订阅
        this._BellyPointManager.OnReduceBellyPointEvent += (float a) => Debug.Log(a);

       

    }
    
    //取消订阅饱食事件
    private void RemoveBellyListener()
    {

        this._BellyPointManager.OnReduceBellyPointEvent -= this.OnSubBellyMethod;//


    }

    //饱食的回调函数
    private void OnSubBellyMethod(float a)
    {


        Debug.Log(a);


    }
    */
    #endregion



}
