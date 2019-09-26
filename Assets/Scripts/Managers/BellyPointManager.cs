using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 饱食度管理类
/// </summary>
public class BellyPointManager : MonoBehaviour {


    private PlayerDataUtils playerDataUtils;//定义工具类

    private readonly float timePerBellyCoroutinesChange = 3600f;//饱食度的变化阈值
   

    //定义委托类型，确定回调方法原型
    public delegate void ReduceBellyPointHandler(float changedValue);//饱食度减少委托
    public delegate void ReduceMoodPointHandle(); //心情度减少委托
    public delegate void UpdateBellyPointUIHandle(float changedValue);//跟新饱食度UI委托

    //定义事件成员，让其他类型对象能够订阅该事件，只有该事件成员才有状态和数据。
    //委托方法原型只是抽象,事件成员才是实体.


    //通知饱食降低事件成员
    public event ReduceBellyPointHandler OnReduceBellyPointEvent;

    //通知心情降低事件成员，
    public event ReduceMoodPointHandle OnReduceMoodPointEvent;

    //通知饱食度UI更新事件
    public event UpdateBellyPointUIHandle UpdateBellyPointUIEvent;

    //心情变化事件,是否触发
    private bool isMoodEventNoticed = false;



    /*
    #region 实例化UI布局管理器与视频播放管理器
    CanvasManagerObject = GameObject.FindGameObjectWithTag("UIManager");
            movieManagerObject = GameObject.FindGameObjectWithTag("MovieManager");
           // DialogueManagerObject = GameObject.FindGameObjectWithTag("DialogueManager");

            _UIManager = CanvasManagerObject.AddComponent<UGUIManager>();
            _MovieManager = movieManagerObject.AddComponent<MovieManager>();
            _WorkDialogManger = CanvasManagerObject.transform.GetChild(8).GetComponentInChildren<WorkDialog>();

            #endregion

    */

    //定义触发事件的方法。
    public void OnBellySub()
    {

        //判断是否有方法订阅了该事件，如果有则，通知
        if(OnReduceBellyPointEvent != null)
        {

            //根据情况触发通知。 

            //OnSubBelly(10f);

            OnReduceBellyPointEvent.Invoke(10f);

        }

          
    }

    private void Awake()        
    {
       
        //Debug.Log("脚本本身的启动与关闭。。。。");

        //实例化工具类(单例)
        playerDataUtils = PlayerDataUtils.getInstance();

        


    }

    // Use this for initialization
    void Start () {
        
        //注意：要由GameManager统一管理开启协程

       // StartCoroutine("BellyPointChangeIE");


	}
	


    //三大数据基础变化
    //1.心情：饱食度低于33，心情每小时变化：1，2，3，4，5，5，5最多到5.
    //2.饱食：每小时降低10点。睡眠不降低。固定不受其他条件改变.
    //3.睡眠：每小时降低3点，哄睡进入睡眠状态，并慢慢回满睡眠值，8小时静止操作。达到0会自动睡眠，恢复到60，心情降低3.


    private void OnEnable()//脚本启动时
    {

       // float curBellyPoint = playerDataUtils.getBellyPoint();//当前饱食度

        //每小时变化
        
        //注意：要确定这一个小时变化的时间变化，
        //默认在第一次启动的时候是一小时开始时计时，然后其他时候得 计算小时时间变化段。

        //启动饱食度变化函数




    }


    private void OnDisable()//脚本关闭时   
    {
        



    }


    //饱食度变化函数
    private void BellyPointChange()
    {





    }
    //饱食度变化协程
    IEnumerator BellyPointChangeCoroutine()
    {
        float currentBellyState;//当前饱食度状态

       // float currentBellyPoint = playerDataUtils.getBellyPoint();

        //第一次启动游戏，将会在一小时后，数值变化。

        while (true)
        {

           // Debug.Log(timePerBellyCoroutinesChange + " 秒后开始更新。。。");

            yield return new WaitForSeconds(timePerBellyCoroutinesChange);//间隔多长时间,调用一次

            //this.OnBellySub();

            //判断当前睡眠状态,自动睡眠状态下,饱食不会降低.
            //如果为睡眠状态不调用
            //如果为清醒状态则调用

            //睡眠状态
            bool sleepState = playerDataUtils.isAutoSleep();

            if (sleepState)//当前状态为睡眠
            {
                Debug.Log("当前女主为睡眠状态,饱食管理器退出此次循环逻辑,等待下次再进行判断");
                continue; //退出此次循环,进行下一次循环
            }

            #region //数据变化逻辑


            #endregion

            //降低10点饱食度           
            //根据返回的饱食度状态进行判断
            currentBellyState = playerDataUtils.ReduceBellyPoint(10f);//降低10点饱食度,返回当前饱食度数值
            //对currentBellyState进行判断
            if (currentBellyState == 0) /***为0,降到底,停止此次循环逻辑判断***/
            {

               // Debug.Log("饱食降到0，停止饱食管理器");

                //通知UI更新               
                if (OnReduceBellyPointEvent != null)
                {
                    
                    OnReduceBellyPointEvent.Invoke(0);
                    
                }

                //饱食为0,考虑下次循环时饱食依旧为0时,会多调用UI更新.
                //玩家操作,可增加饱食度,
                isMoodEventNoticed = false;//重置
                //停止此次循环
                continue;
               


            }
            else
            {   //饱食为0后,再次增加饱食度,后的变换情况---判断??心情协成又调用了一次.
                //可用bool值判断,是否进行.
                if (currentBellyState < 0)//当前的饱食度低于33, 心情开始降低
                {

                    //toDO
                    //通知UI更新
                    //更新饱食度进度条。。。。。subBellyPointPerHour = 5f

                    if (OnReduceBellyPointEvent != null)
                    {


                        OnReduceBellyPointEvent.Invoke(-currentBellyState);


                    }

                    // _UIManager.UpdateBellyPmgressbar(curBellyPoint);

                    //当前饱食度小于33
                    //开始降低心情值
                    //这里是用C#的事件机制来实现
                    //定义委托类型，让心情管理器模块订阅该事件，以便在饱食度低于33时通知心情管理模块

                    //注意：在下一次生命周期内，本通知只执行一次
                    if (OnReduceMoodPointEvent != null)//说明心情管理模块已经订阅该事件,
                    {
                        if (!isMoodEventNoticed)//心情变化事件未触发
                        {
                            Debug.Log("饱食度低于33,进入饥饿状态,心情开始降低...");
                            // Debug.Log("当前饱食度:" + -currentBellyState);
                            
                            OnReduceMoodPointEvent.Invoke();//触发通知
                            isMoodEventNoticed = true;//心情变换事件已经触发,在心情值高于33时,其值为false.需要在心情管理模块判断
                        }


                    }
                    else
                    {
                        //心情管理模块未订阅心情变化事件
                        Debug.Log("心情管理模块未订阅心情变化事件,请查看心情管理模块订阅");

                    }


                  

                }
                else
                {
                    //正常变化
                    
                    //修改心情变换事件状态为未触发.
                    isMoodEventNoticed = false;

                    //UI变化
                    //通知UI更新

                    //更新饱食度进度条。。。。。subBellyPointPerHour = 5f
                   // _UIManager.UpdateBellyPmgressbar(curBellyPoint);

                  //  Debug.Log("当前饱食度:" + currentBellyState);
                    if (OnReduceBellyPointEvent != null)
                    {


                        OnReduceBellyPointEvent.Invoke(currentBellyState);


                    }

                }


            }





        }









    }

    private void OnDestroy()//脚本销毁时
    {
        


    }

}
