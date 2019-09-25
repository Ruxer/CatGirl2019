using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodPointManager : MonoBehaviour {



    //定义工具类
    private PlayerDataUtils playerDataUtils;
    //饱食管理器对象
    private BellyPointManager BellyManager;

    private readonly float timePerMoodCoroutineChange = 5f;//每次心情协程变化的时间

   // private float reduceMoodPointPerHour = 1f;//心情低于33时，每小时降低1点心情
    private readonly float maxReduceMoodPoint = 5f;//心情低于33时，最多降低5点心情

    //观察者模式
    //消息系统
    //委托与事件
    //目的:当饱食度高于33时,停止心情协程.
    //定义UI监听委托类型
    public delegate void UpdateMoodPointUIHandle(float changedValue);//更新心情值UI委托
    //定义委托事件
    public event UpdateMoodPointUIHandle OnReduceMoodPointEvent;

    //心情值
    public float currentMoodPoint;

    private void Awake()
    {
        //获取全局工具类对象.
        playerDataUtils = PlayerDataUtils.getInstance();
        

    }

    // Use this for initialization
    void Start () {

        this.BellyManager = gameObject.GetComponent<BellyPointManager>();

        this.AddBellyListener();//订阅饱食管理器模块的心情变换事件



    }
	
    /// <summary>
    /// 订阅停止心情协成事件,并处理逻辑
    /// </summary>
    private void AddStopMoodCoro()
    {


    }


    //定义回调方法
    //参数模型一致
    //这里用Lambda表达式
    /// <summary>
    /// 订阅心情变换事件,并处理逻辑.
    /// </summary>
    private void AddBellyListener()
    {
        //开始心情协程
        this.BellyManager.OnReduceMoodPointEvent += () => {

           // Debug.Log("这里是触发心情降低的协程》》》》");

            StartCoroutine("MoodPointChangeCoroutine");

          

        };



    }
    /// <summary>
    /// 心情变换协程
    /// </summary>
    /// <returns></returns>
    IEnumerator MoodPointChangeCoroutine()
    {
        
        float curentChangedValue;//当前变换的值
        
        while (true)
        {



            //1.心情：饱食度低于33，心情每小时依次变化：1，2，3，4，5，5，5等,最多到5.          
            //注意心情降低为0的情况。登录间隔时间太常,在登录时的逻辑判断后.
            //当前心情值
            currentMoodPoint = playerDataUtils.getMoodPoint();
            //当前心情状态
            //MoodState[0]:变换的值, MoodState[1]:状态值
            curentChangedValue = playerDataUtils.ReduceMoodPoint();
            //心情值
            currentMoodPoint = playerDataUtils.getMoodPoint();

            if (currentMoodPoint <= 0)
            {

                //通知UI更新
                if (OnReduceMoodPointEvent != null)
                {
                    OnReduceMoodPointEvent.Invoke(0);

                }
                //停止协程.直到接到再次启动的通知.

                PlayerDataUtils.tempPoint = 1f;
                StopCoroutine("MoodPointChangeCoroutine");

            }

            if (curentChangedValue != 0)
            {
               
                   // Debug.Log("当前心情值：" + currentMoodState);

                    //更新UI
                    if (OnReduceMoodPointEvent != null)
                    {
                        OnReduceMoodPointEvent.Invoke(curentChangedValue);
                    
                    }

            }
            else
            {
                Debug.Log("当前心情值：" + curentChangedValue);//心情值为0
                //更新UI
                if (OnReduceMoodPointEvent != null)
                {
                    OnReduceMoodPointEvent.Invoke(curentChangedValue);

                }

                //停止协程
                Debug.Log("停止协程");
                //重置工具类中的心情值累加器.
                PlayerDataUtils.tempPoint = 1f;
                StopCoroutine("MoodPointChangeCoroutine");


            }

          

            yield return new WaitForSeconds(timePerMoodCoroutineChange);


            /*

            //subMoodPointPerHour = 1f:每小时降低的心情值
            //maxSubMoodPoint = 5f：最大降低心情值
            if (reduceMoodPointPerHour <= maxReduceMoodPoint)
            {


                curMoodPoint = playerDataUtils.getMoodPoint(); //当前的心情值

                if (curMoodPoint != 0) // 如果变化前的值不为0，
                {
                    Debug.Log("当前心情值：" + curMoodPoint);

                    playerDataUtils.ReduceMoodPoint(reduceMoodPointPerHour);//降低1点到5点的心情值

                    curMoodPoint = playerDataUtils.getMoodPoint(); //变化后的心情值

                   // _UIManager.UpdateMoodPmgressbar(curMoodPoint); //进度条更新
                }
                else //如果变化前的 心情值已经变为0了
                {

                    Debug.Log("心情值已经降低为0, 停止心情管理器");

                    StopCoroutine("MoodPointChangeIE");


                }

                reduceMoodPointPerHour++; //

                if (reduceMoodPointPerHour > 5)// 降低的心情值最大为5点
                {
                    reduceMoodPointPerHour = 5;

                }





            }




           

            yield return new WaitForSeconds(5f);

        */

        }






    }





}
