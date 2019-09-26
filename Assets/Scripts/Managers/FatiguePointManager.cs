using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatiguePointManager : MonoBehaviour {

    private readonly float timePerFatigueCoroutinesChange = 3600f;//疲劳度的变化阈值


    private PlayerDataUtils playerDataUtils;//定义工具类

    //定义UI模块监听委托类型
    public delegate void UpdateFatiguePointHandle(float currentValue);

    //定义UI模块监听睡眠值的事件成员
    public event UpdateFatiguePointHandle OnReduceFatiguePointEvent;

    private void Awake()
    {

        //加载工具类
        playerDataUtils = PlayerDataUtils.getInstance();



    }



    // Use this for initialization
    void Start () {

       // StartCoroutine("FatiguePointChangeCoroutine");


	}
	

    //睡眠值变化协程
    IEnumerator FatiguePointChangeCoroutine()
    {
        float currentFatigueState;//当前睡眠值状态

        while (true)
        {

            yield return new WaitForSeconds(timePerFatigueCoroutinesChange);


            currentFatigueState = playerDataUtils.ReduceFatiguePoint();

            if (currentFatigueState != 0)//未进入睡眠状态
            {

                if (currentFatigueState > 0)//正常状态
                {
                   // Debug.Log("当前疲劳度：" + currentFatigueState);
                    //更新UI

                    if (OnReduceFatiguePointEvent != null)
                    {
                        OnReduceFatiguePointEvent.Invoke(currentFatigueState);
                    }


                }
                else //进入了困倦状态
                {
                    
                    //更新UI

                   // Debug.Log("当前疲劳度：" + -currentFatigueState );

                    if (OnReduceFatiguePointEvent != null)
                    {
                        OnReduceFatiguePointEvent.Invoke(-currentFatigueState);
                    }


                }

            }
            else  //睡眠度降为0,进入自动睡眠状态.
            {
                //更新UI

                if (OnReduceFatiguePointEvent != null)
                {
                    OnReduceFatiguePointEvent.Invoke(0);
                }

              //  Debug.Log("当前疲劳度：" + currentFatigueState + "协程停止");
                
                //停止协程
                StopCoroutine("FatiguePointChangeCoroutine");


                //当前睡眠值为0，进入自动睡眠状态
                //直到6小时后,睡眠度恢复到60时,在开始协程事件.
                playerDataUtils.setAutoSleep(true);


            }






        } 





    }




}
