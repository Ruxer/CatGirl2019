using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class DesignData : ScriptableObject {

    [SerializeField]
    public List<LevelData> levelData;
   

    public int minLevelFavor = 50;
    public int addFavorPerLevel = 5;//每次升级增加所需的好感值

    public int maxHealthPoint = 120;
    public int maxMoodPoint = 100;
    public int maxBellyPoint = 100;
    public int maxSleepingPoint = 100;

    public float sleepingDuration;//睡眠一次的时长 单位：秒

    public WorkStyle[] workStyleArray = new WorkStyle[4];
    public DateStyle[] dateStyleArray = new DateStyle[4];//各项约会数据
    public PlayStyle[] playStyleArray = new PlayStyle[2];
    public FeedStyle[] feedStyleArray = new FeedStyle[3];

}
[System.Serializable]
public class WorkStyle
{
    public int workStyleNum;
    public float consumeHP;
    public float minCoins;
    public float maxCoins;
    public float maxLevel;
}
[System.Serializable]
public class DateStyle
{
    public int dateStyleNum; // 索引
    public float consumeCoins; //减少金币
    public float coldDownTime; // 冷却时间
    public float addFavorPoint;//增加好感
}
[System.Serializable]
public class PlayStyle
{
    public int playStyleNum;
    public int consumeHP; //减少体力
    public int coldDownTime;
    public int addMoodPoint;//增加心情
}
[System.Serializable]
public class FeedStyle
{
    public int feedStyleNum;
    public bool isConsumeDiamonds;
    public int consumeDiamonds;
    public int consumeCoins;
    public int addBellyPoint;
    public int addFavorPoint;
}
public class LevelData
{
    public float maxFavorPoint;
    public float maxHealthPoint;
    public float maxMoodPoint;
    public float maxBellyPoint;
    public float maxSleepingPoint;
}
