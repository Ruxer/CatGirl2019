using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerData : ScriptableObject {

    //系统信息
    [Header("系统信息")]
    public int logTimes;//登陆次数


    //玩家信息
    [Header("玩家信息")]
    public string userName;
    public string password;
    public string registerTime;
    public float coins = 103;//金钱
    public float diamonds = 56;//钻石
    public float level = 1;//等级
    public float favorPoint = 60;//好感度
    public float healthPoint;//体力值
    //女主信息
    [Header("女主信息")]
    public float moodPoint = 60;//心情值
    public float bellyPoint = 50;//饱食度
    public float FatiguePoint = 67;//睡眠值

    public int gameVictory = 0;

    public float SleepTime;//睡眠变化时间
    public float remainSleepingTime ;//剩余的睡眠时间 单位：秒
    public bool isAutoSleep;//是否自动睡眠， 默认为false
    public bool isWorking;//是否正在打工, 默认为false
    public bool isDating;// 是否正在约会，默认false
    public string lastQuitTime;//上次的退出时间	

    [Header("对话")]
    public string[] female;
    public string[] male;
    public DialogueStruct workBegain;
    public DialogueStruct workResoult;
   // public DialogueStruct[] dialogues = new DialogueStruct[] { workBegain, workResoult };


    //冷却时间
    [Header("计时器")]
    public DateTimer[] dateTimers = new DateTimer[4]; //各项的计时器
    public PlayTimer[] playTimers = new PlayTimer[4];
    //统计信息
    [Header("统计信息")]
    public int[] workCount = new int[4];
    public float[] dateCount = new float[4];//约会次数统计
    public int[] foodCount = new int[3];
    public int[] playCount = new int[2];



    //变化数值数组
    [Header("约会数值")]
    public float[] dateAddFavorPoint;
    public float[] dateCostConins;
    [Header("打工数值")]
    public int workTimes;//打工次数
    public float[] workAddConins;
    public float[] workReduceMoods;
    public float[] workCostPowers;
    [Header("喂食数值")]
    public float[] feedCostConins;
    public float[] feedAddBellyPoint;
    public float[] feedAddFavorPoint;
    public float[] feedReduceDiamond;


   // public Dialogue diaStr;

   

}

[System.Serializable]
public struct DialogueStruct
{
   
    /*
    public void Dialogue(string[] _femaleStr, string[] _maleStr)
    {
        femaleStr = _femaleStr;

        maleStr = _maleStr;
    }
    */

    public string[] femaleStr;
    public string[] maleStr;


}
[System.Serializable]
public class Dialogue
{
    public string[] femaleStr;

    public string[] maleStr;




}
[System.Serializable]
public class DateTimer
{
    public int styleIndex; // 索引
    public string beginTime;//单位：S
    public float remainTime;
}

[System.Serializable]
public class PlayTimer
{
    public int styleIndex;
    public string beginTime;//单位：S
    public float remainTime;
}
