using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
//using UnityEngine.UI;
/*
 玩家数据存储工具类
 单例模式 调用例子：PlayerDataUtils.getInstance();

     */

//参数1：升级后玩家等级 参数2：钻石奖励
[Serializable]
public class LevelUpEvent : UnityEvent<float, float, float> { }
public class PlayerDataUtils  {

    #region//事件声明
    public LevelUpEvent levelUpEvent = new LevelUpEvent();


    #endregion

    //预定义数值
    private float recoverSleepingPointValue = 60;
    private float subMoodPointAutoSleeping = 3;//自动睡眠降低3点心情

    private static PlayerDataUtils instance;
    private static PlayerData playerData;
    private static DesignData designData;

    private bool isUpdateDateTimerRuning = false;
    private bool isUpdatePlayTimerRunig = false;

    //时间间隔：时 分 秒
    public float timeSpan;
    public float hours;
    public float minutes;
    public float seconds;

    public static PlayerDataUtils getInstance()
    {
        if (instance == null)
        {
            instance = new PlayerDataUtils();
            playerData = Resources.Load<PlayerData>("PlayerData");
            designData = Resources.Load<DesignData>("DesignData");
        }
        return instance;
    }

    //构造函数
    public  PlayerDataUtils()
    {
        
        /*
         timeSpan = Mathf.Floor(getTimeSpan());
        Debug.Log(timeSpan);
         hours = Mathf.Floor(timeSpan / 3600f); // 事件间隔的小时

         minutes = Mathf.Floor((timeSpan - hours * 3600f) / 60f); //时间间隔的分钟

         seconds = Mathf.Floor(timeSpan - (hours * 3600f) - (minutes * 60)); // 时间间隔的秒 
        */

    }



    public int LogTimes()
    {

        return playerData.logTimes;

    }
    public void SetLogTimes()
    {
        playerData.logTimes++;
    }


    #region /***玩家信息***/

    /// <summary>
    /// 注册玩家信息
    /// </summary>
    /// <param name="userName">玩家姓名</param>
    /// <param name="password">密码</param>
    /// <returns></returns>
    public bool Register(string userName,string password)
    {
        if (userName != null && password != null)
        {
            playerData.userName = userName;
            playerData.password = password;
            Debug.Log("注册成功！");
            return true; 
        }
  
            Debug.Log("用户名或密码不能为空！");
            return false;
        
    }


    /// <summary>
    /// 得到玩家姓名
    /// </summary>
    /// <returns></returns>
    public string getUserName()
    {
        if (playerData.userName != null)
        {
            return playerData.userName;
        }
        return "";
    }
    
    /// <summary>
    /// 玩家当前等级
    /// </summary>
    /// <returns></returns>
    public float getLevel()
    {
        return playerData.level;
    }

   
    /// <summary>
    /// 玩家上次退出到再次登录的时间间隔，单位为秒
    /// </summary>
    /// <returns></returns>
    public float getTimeSpan()
    {
        
        if (!string.IsNullOrEmpty(playerData.lastQuitTime))
        {
            return (float)(DateTime.Now - DateTime.Parse(playerData.lastQuitTime)).TotalSeconds;//.TotalHours;
        }
        
        return 0;
    }
    
    /// <summary>
    /// 设置退出时间
    /// </summary>
    /// <param name="quitTime"></param>
    public void setQuitTime(string quitTime)
    {
        if (!string.IsNullOrEmpty(quitTime))
        {
            playerData.lastQuitTime = quitTime;
        }
    }

    #endregion

    /*********************自定义************************/



    public void test()
    {
        //好感度
        playerData.favorPoint = 0f;
        //体力进度条
        playerData.healthPoint = 65f;
        //金钱
        playerData.coins = 1500f;
        //钻石
        playerData.diamonds = 15f;

        //心情值
        playerData.moodPoint = 100;
        //饱食度
        playerData.bellyPoint = 100;
        //睡眠值
        playerData.FatiguePoint = 100;


    }

    /***************************************************************/


    #region /***其他数据操作***/
    /***好感度操作***/

    //得到当前好感度值
    public float getFavorPoint()
    {
        return playerData.favorPoint;
    }
    //指定等级的最大好感度
    
    public float getMaxFavorPoint(float curlevel)
    {
        return designData.minLevelFavor + (curlevel - 1) * designData.addFavorPerLevel;
       // return designData.minLevelFavor + (getLevel() - 1) * designData.addFavorPerLevel;
    }
    public float getMaxFavorPoint()
    {

        return  designData.minLevelFavor + (getLevel() - 1) * designData.addFavorPerLevel;
    }
    //增加好感度
    public void addFavorPoint(float points)
    {
        if (points > 0)
        {
            playerData.favorPoint += points;
            if (playerData.favorPoint >= getMaxFavorPoint())
            {
                //通知，升级事件
                LevelUp();
            }
        }
    }

    /***体力值操作***/

    //得到当前等级下的最大体力值
    public float getMaxHealthPoint()
    {
        float point = designData.maxHealthPoint;
        return point;
    }
    //得到当前体力值
    public float getHealthPoint()
    {
        if(playerData.healthPoint > getMaxHealthPoint())
        {

            playerData.healthPoint = getMaxHealthPoint();
        }

        return playerData.healthPoint;
    }
    //减少体力值
    public bool subHealthPoint(float points)
    {
        if ((getHealthPoint() - points) >= 0)
        {
            playerData.healthPoint -= points;
            return true;
        }
        return false;
    }
    //增加体力值
    public void addHealthPoint(float points)
    {
        if (points > 0)
        {
            playerData.healthPoint += points;
            if (playerData.healthPoint > getMaxHealthPoint())
            {
                playerData.healthPoint = getMaxHealthPoint();
            }
        }
    }
      
   
    /***金币相关操作***/

    /// <summary>
    /// 当前金币
    /// </summary>
    /// <returns>当前金币数</returns>
    public float getCoinCount()
    {
        return playerData.coins;
    }

    /// <summary>
    /// 减少金币
    /// </summary>
    /// <param name="count">减少数目</param>
    /// <returns>操作后数目是否不小于0</returns>
    public bool subCoins(float count)
    {
        if ((getCoinCount() - count) >= 0)
        {
            playerData.coins -= count;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 增加金币
    /// </summary>
    /// <param name="count">增加数目</param>
    public void addCoins(float count)
    {
        if (count > 0)
        {
            playerData.coins += count;
        }
    }

    /***钻石相关操作***/
    /// <summary>
    /// 当前钻石数目
    /// </summary>
    /// <returns>当前钻石数目</returns>
    public float getDiamondCount()
    {
        return playerData.diamonds;
    }
    /// <summary>
    /// 减少钻石
    /// </summary>
    /// <param name="count">减少数目</param>
    public void subDiamonds(float count)
    {
        if (count > 0)
        {
            playerData.diamonds -= count;
            if (playerData.diamonds < 0)
            {
                playerData.diamonds = 0;
            }
        }
    }

    #endregion

    #region /***心情值操作***/

    //得到当前等级下的最大心情值
    public int getMaxMoodPoint()
    {
        int point = designData.maxMoodPoint;
        return point;
    }
    //得到当前的心情值
    public float getMoodPoint()
    {
        return playerData.moodPoint;
    }
    //设置当前心情值
    public void SetMoodPoint(float point)
    {
        if(point <= 0)
        {
            playerData.moodPoint = 0;

        }
        else
        {
            playerData.moodPoint = point;
        }
    }
    //降低心情值:从1变化到5
    public static float tempPoint = 1f;

    /// <summary>
    /// 降低心情值（一次降低1点、2点、3点、4点、5点，最高为5点）
    /// </summary>
    /// <returns>返回0：停止心情协程（UI层为0）；返回正数：1-4的变化数值；返回负数：最高值5的变化数值</returns>
    public float ReduceMoodPoint()
    {
        if ((playerData.moodPoint - 1) <= 0)//心情值降为0
        {

            playerData.moodPoint = 0;//置零
            tempPoint = 1f;//初始化
            return 0;//返回0状态 

        }
        else
        {

            if (tempPoint == 1)//第一次调用
            {
                playerData.moodPoint -= 1f;
                tempPoint++;
                return tempPoint - 1;

            }
            else if (tempPoint <= 4)//依次第2,3,4,次调用
            {
                playerData.moodPoint -= tempPoint;//判断如果此时moodPoint<=0
                tempPoint++;
                return tempPoint - 1;


            }
            else //第5次以及n次调用
            {
                playerData.moodPoint -= tempPoint;//判断如果此时<=0
                return tempPoint;


            }



        }












    }
    //增加心情值
    public void addMoodPoint(int points)
    {
        if (points >= 0)
        {
            playerData.moodPoint += points;
            if (playerData.moodPoint > getMaxMoodPoint())
            {
                playerData.moodPoint = getMaxMoodPoint();
            }
        }
    }

    #endregion

    #region /***饱食度操作***/

    //得到当前等级下的最大饱腹度值
    public float getMaxBellyPoint()
    {
        float point = designData.maxBellyPoint;
        return point;
    }
    //得到当前的饱腹度值
    public float getBellyPoint()
    {
        return playerData.bellyPoint;
    }
    //设置当前饱食度
    public void SetBellyPoint(float point)
    {
        if(point <= 0)
        {
            playerData.bellyPoint = 0;

        }else
        {
            playerData.bellyPoint = point;
        }

       

    }
    //降低饱腹度
    public void subtractBellyPoint(float point)
    {
        playerData.bellyPoint -= point;
        if (playerData.bellyPoint <= 0)
        {
            playerData.bellyPoint = 0;
        }
    }

    //带有返回值的降低饱食度
    /// <summary>
    /// 降低饱食度
    /// </summary>
    /// <param name="point">降低的点数</param>
    /// <returns> 0表示停止循环(UI层上表示为0)；负值表示小于33的数值，同时心情值下降；正数表示正常数值</returns>
    public float ReduceBellyPoint(float point)
    {
        if ((playerData.bellyPoint -= point) <= 0)
        {
           // Debug.Log(playerData.bellyPoint + "已经小于0了。。。");
            playerData.bellyPoint = 0;
            //反馈，停止循环
            //Debug.Log(playerData.bellyPoint);
            return 0;
        }
        else if(playerData.bellyPoint <= 33)
        {
           // Debug.Log(playerData.bellyPoint + "已经低于33了。。。");
            return -(playerData.bellyPoint);
        }
        else
        {
           // Debug.Log(playerData.bellyPoint + "不低于33的时候");
            return playerData.bellyPoint;
        }

      

    }

    //增加饱食度
    public void addBellyPoint(float points)
    {
        if (points > 0)
        {
            playerData.bellyPoint += points;
            if (playerData.bellyPoint > getMaxBellyPoint())
            {
                playerData.bellyPoint = getMaxBellyPoint();
            }
        }
    }

    #endregion

    #region /***睡眠值操作***/

    //得到当前等级下最大睡眠度值
    public int getMaxSleepingPoint()
    {
        // int point = designData.maxSleepingPoint;
        // return point;
        return 0;

    }
    //得到当前睡眠度值
    public float getSleepingPoint()
    {
        return playerData.FatiguePoint;
    }
    //设置睡眠值
    public void SetSleepPoint(float Point)
    {
        if(Point <= 0)
        {
            playerData.FatiguePoint = 0;
        }else
        {

            playerData.FatiguePoint = Point;

        }
    }
    //降低睡眠度
    public void subtractSleepingPoint(float point)
    {
        playerData.FatiguePoint -= point;
        if (playerData.FatiguePoint <= 0)
        {
            playerData.FatiguePoint = 0;
        }
    }
    //降低睡眠值,每次降低3点
    //睡眠值有两个阈值：1.低于33点的困倦状态。2.高于33点的正常状态
    /// <summary>
    /// 降低疲劳度
    /// </summary>
    /// <returns>返回值：0表示停止变化，进入自动睡眠状态,UI层显示为0；正数：表示正常疲劳度；负数：表示进入了困倦状态</returns>
    public float ReduceFatiguePoint()
    {

        if((playerData.FatiguePoint -= 3f) <= 0)
        {
            playerData.FatiguePoint = 0;

            return 0;
            
        }
        else if(playerData.FatiguePoint <= 33)
        {           
            return -playerData.FatiguePoint;

        }
        else
        {
            return playerData.FatiguePoint;
            
        }





    }

    #endregion

    #region /**********--自动睡眠--***************/

    //一次睡眠需要的时长:8小时或28800秒
    public float getSleepingDuration()
    {
        return designData.sleepingDuration;
    }
    //剩余睡眠时间，返回0代表睡眠结束s
    public float getRemainSleepingTime()
    {
        return playerData.remainSleepingTime;
    }
    //设置剩余睡眠时间
    //timeValue:剩余睡眠时间 为28800秒
    public void setRemainSleepingTime(float timeValue)
    {
        if (timeValue >= 0)
        {
            playerData.remainSleepingTime = timeValue;
        }
    }
    //减少剩余睡眠时间
    public void subRemainingSleepingTime(float subValue)
    {
        playerData.remainSleepingTime -= subValue;

        if (playerData.remainSleepingTime <= 0)
        {
            playerData.remainSleepingTime = 0;
        }
    }
    //是否苏醒
    public bool isWakeUp()
    {
        if (playerData.remainSleepingTime == 0)
        {
           
            return true;
        }
        return false;
    }
    //是否自动睡眠(睡眠状态)
    public bool isAutoSleep()
    {
        return playerData.isAutoSleep;
    }
    //设置自动睡眠
    public void setAutoSleep(bool isAuto)
    {
        playerData.isAutoSleep = isAuto;
    }
    //睡眠恢复睡眠度
    public void recoverSleepingPoint()
    {
        playerData.FatiguePoint = recoverSleepingPointValue;// recoverSleepingPointValue = 60
    
       
       // ReduceMoodPoint(subMoodPointAutoSleeping);//降低3点睡眠度 // subMoodPointAutoSleeping = 3


    }
    //睡醒后数据更新
    public void WakeUp()
    {
        recoverSleepingPoint();//恢复睡眠度到60  //心情值减少3点

        //setRemainSleepingTime(getSleepingDuration());//初始剩余睡眠时间为28800秒
        //setRemainSleepingTime(7200s);
        setAutoSleep(false);//修改自动睡眠为false
    }

    //当前时间
    public float GetSleepTime()
    {

        return playerData.SleepTime;

    }
    //设置时间
    public void SetSleepTime(float curTime)
    {

        playerData.SleepTime = curTime;

    }
    //减少时间
    public void ReduceTime()
    {
        if(playerData.SleepTime == 0)
        {
            playerData.SleepTime = 20f;//初始化为一个小时
            playerData.SleepTime--;

        }
        else
        {
            playerData.SleepTime--;//每秒减少1


        }


    }

    #endregion


    #region /**********************--读取数据表--**********************/

    /// <summary>
    /// 根据index索引读取约会数据表
    /// </summary>
    /// <param name="index"></param>
    /// <returns>返回读取到的数据组</returns>
    public float[] getDateChangeValue(int index)
    {
        float[] changeValues;

        changeValues = new float[] { playerData.dateAddFavorPoint[index], playerData.dateCostConins[index] };

        return changeValues;
    }
    /// <summary>
    /// 根据index索引读取打工数据表
    /// </summary>
    /// <param name="index"></param>
    /// <returns>返回读取到的数据组</returns>
    public float[] getWorkChnageValue(int index)
    {
        float[] changeValues;

        changeValues = new float[] { playerData.workAddConins[index], playerData.workReduceMoods[index], playerData.workCostPowers[index] };

        return changeValues;
    }
    /// <summary>
    /// 根据index索引读取喂食数据表
    /// </summary>
    /// <param name="index"></param>
    /// <returns>返回读取到的数据组</returns>
    public float[] getFeedChangeValue(int index)
    {

        float[] changeValues;

        changeValues = new float[] { playerData.feedAddBellyPoint[index], playerData.feedAddFavorPoint[index], playerData.feedCostConins[index], playerData.feedReduceDiamond[index] };

        return changeValues;
    }

    #endregion

    #region 约会事件

    /// <summary>
    /// 约会
    /// </summary>
    /// <param name="styleIndex">约会类型(从0开始计数)</param>
    /// <returns>0:代表金钱不足 -1：正在约会，不能再次执行  </returns>
    /// (更新玩家数据、统计数据、设置计时器)
    /// 
    public float[] GoToDate(int styleIndex)
    {
       // int index = styleIndex - 1;
        DateStyle dateStyle = designData.dateStyleArray[styleIndex];// dateSyele为约会列表单曲的子项数据。（包括花费金币/冷却时间/增加好感）

        //如果是第一次开始约会

        if (IsDating()) // 如果isWorking false： 代表都没有在约会，true：当前有在约会
        {

            //当前有在约会，请等待冷却时间结束后再进行其他约会
            //return new float[] {-1 };
            return null;

        }
        else //没有在约会
        {
            // 当前项开始约会
            if (getCoinCount() - dateStyle.consumeCoins >= 0) // getCoinCount()当前金币数 - 将要花费的金币数 足够花费
            {
                subCoins(dateStyle.consumeCoins); // 减去花费的金币
                addFavorPoint(dateStyle.addFavorPoint); // 增加好感------应该判断变化后是否会升级,如果升级应该获得升级通知

                playerData.dateCount[styleIndex] += 1; // 当前子项的约会次数加一

                playerData.dateTimers[styleIndex].beginTime = System.DateTime.Now.ToString(); // 当前项的计时器记录当前系统时间
                playerData.dateTimers[styleIndex].remainTime = dateStyle.coldDownTime; // 当前项的冷却时间


               // SetIsDating(true);//当前项在工作

                return new float[]{1, getCoinCount(), getFavorPoint(), playerData.dateCount[styleIndex], dateStyle.coldDownTime, getMaxFavorPoint(),dateStyle.consumeCoins, dateStyle.addFavorPoint  }; //状态码 / 返回变化后的金币，变化后的好感度，当前的约会次数, 当前项的冷却时间， 当前等级最大的好感度 / 金币差值 / 好感度差值
            }
            else
            {
               
                return new float[]{0 };//如果第一位为0，表示金币不足


            }




        }

        #region 升级操作
        /*   
      public float[] LevelUp()
     {
         float cur = getFavorPoint();
         if (getFavorPoint() >= getMaxFavorPoint())
         {
             playerData.favorPoint -= getMaxFavorPoint();//当前变化后的好感度 - 当前等级最大的好感度
             playerData.level += 1; //等级加1
                                    // Debug.Log("升级成功&获得钻石奖励-10！");
                                    //钻石加10
             playerData.diamonds += 10f;
             return new float[]{ 1,  playerData.favorPoint, playerData.diamonds, playerData.level} ;//返回变化后的好感度和钻石


         }

         return new float[] {0 };
     }
       */

        #endregion 

    }

    #region
    public void SetIsDating(bool isDate)
    {
        playerData.isDating = isDate;

    }
    public bool IsDating()
    {
        return playerData.isDating;

    }
    
    public void GetDateTimers()
    {




    }
    #endregion

    //获取计时器数组
    public DateTimer[] getDateTimers()
     {
         return playerData.dateTimers;
     }
    //返回时间间隔
     public int getDateTimeSpan(int index)
     {
         DateTimer dateTimer = getDateTimers()[index];
         string beginTime = dateTimer.beginTime;
         int seconds = 0;
         if (!string.IsNullOrEmpty(beginTime))
         {
             seconds = (int)((DateTime.Now - DateTime.Parse(beginTime)).TotalSeconds);
         }
         return seconds;
     }
    //初始化时间
     public void initDateRemainTime()
     {
         for (int i = 0; i < playerData.dateTimers.Length; i++)
         {
             DateTimer dateTimer = playerData.dateTimers[i];
             if (dateTimer.remainTime != 0)
             {
                 if (getDateTimeSpan(i) - dateTimer.remainTime >= 0)
                 {
                     dateTimer.remainTime = 0;
                 }
                 else
                 {
                     dateTimer.remainTime -= getDateTimeSpan(i);
                 }



                 if (!isUpdateDateTimerRuning)
                 {
                     //startDateTimerUpdateEvent.Invoke();
                 }
                 Debug.Log("Date-" + i + "剩余冷却时间：" + dateTimer.remainTime + "s");
             }
         }
     }

     public void setIsUpdateDateTimerRuning(bool isRuning)
     {
         isUpdateDateTimerRuning = isRuning;
     }
     public void setDateRemainTime(int seconds)
     {

     }
     public void UpdateDateTimer(int seconds)
     {
         for (int i = 0; i < playerData.dateTimers.Length; i++)
         {
             DateTimer dateTimer = playerData.dateTimers[i];
             if (dateTimer.remainTime != 0)
             {
                 dateTimer.remainTime -= seconds;
                 if (dateTimer.remainTime < 0) dateTimer.remainTime = 0;
             }
         }
         bool shouldStop = true;
         for (int i = 0; i < playerData.dateTimers.Length; i++)
         {
             if (playerData.dateTimers[i].remainTime != 0)
             {
                 shouldStop = false;
             }
         }
         if (shouldStop)
         {
            // stopDataTimerUpdateEvent.Invoke();
         }
     }








    #endregion


    #region 工作事件

    /// <summary>
    /// 工作
    /// </summary>
    /// <param name="styleIndex"></param>
    /// <returns></returns>
    public float[] goToWork(int styleIndex)
    {
        WorkStyle workStyle = designData.workStyleArray[styleIndex]; // 根据索引获取数据

       
        if (subHealthPoint(workStyle.consumeHP))// subHealthPoint：减少体力 返回值true：变化后的体力值>=0, false: 小于0
        {
            playerData.workCount[styleIndex] += 1; // 相对应的打工次数加一

            float coins = workStyle.minCoins + Mathf.Floor( getWorkLevel(styleIndex) * (workStyle.maxCoins - workStyle.minCoins + 0f) / workStyle.maxLevel); //根据打工次数计算出当前增加的金币
           // Debug.Log(coins);
            if (coins > workStyle.maxCoins)
            {
                coins = workStyle.maxCoins;
            }
            addCoins(coins); // 增加金币
                             //Debug.Log("---"+coins);  


            return new float[] {1f, getHealthPoint(), getCoinCount(),workStyle.consumeHP, coins  };//打工返回，状态码 / 变化后的体力 / 变化后的金币数 / 体力差值 / 金币差值/ 
        }


        return new float[] {0f };

    }
    /// <summary>
    ///获取工作等级
    /// </summary>
    /// <param name="styleIndex"></param>
    public float getWorkLevel(int styleIndex)
    {
        float workLevel = 1;
        workLevel = playerData.workCount[styleIndex];//打工次数
        if (workLevel == 0)
        {
            workLevel = 1;
        }
        else if (workLevel > designData.workStyleArray[styleIndex].maxLevel)
        {
            workLevel = designData.workStyleArray[styleIndex].maxLevel;
        }
        return workLevel;
    }



    #endregion

    #region 喂食事件

    /// <summary>
    /// 喂食
    /// </summary>
    /// <param name="styleIndex"></param>
    /// <returns>0:喂食成功 1：代表缺少金钱 -1：代表缺少钻石</returns>
    public float[] giveFood(int styleIndex)
    {
       
        FeedStyle feedStyle = designData.feedStyleArray[styleIndex];//读取喂食数据
        if (feedStyle.isConsumeDiamonds)//当前喂食是否花费钻石 / true：花费钻石 / false：花费金币  ，默认为false
        {
            if ((getDiamondCount() - feedStyle.consumeDiamonds) >= 0) //钻石足够
            {
                subDiamonds(feedStyle.consumeDiamonds);//减少钻石
                addBellyPoint(feedStyle.addBellyPoint);//增加饱食
                addFavorPoint(feedStyle.addFavorPoint);//增加好感

                return new float[] {1,getBellyPoint(), getDiamondCount(), getFavorPoint(), getMaxFavorPoint(), feedStyle.addBellyPoint, feedStyle.consumeDiamonds, feedStyle.addFavorPoint  };//返回 /0：状态码 /1：变化后的饱食 /2：变化后的钻石 / 3：变化后的好感 /4:当前等级最大好感 /5: 饱食差值 /6：钻石差值 /7：好感差值
            }
            else //钻石不足
            {
                return new float[] {-1 };
            }
        }
        else // 花费金币喂食
        {
            if (getCoinCount() - feedStyle.consumeCoins >= 0) // 金币足够
            {
                subCoins(feedStyle.consumeCoins);//金钱减少
                addBellyPoint(feedStyle.addBellyPoint);//饱食增加

                return new float[] {2, getBellyPoint(), getCoinCount(), feedStyle.addBellyPoint, feedStyle.consumeCoins };//返回 /0： 状态码 /1：变化后的饱食 /2：变化后的金币 /3： 饱食差值 /4： 金币差值
            }
            else //金币不足
            {
                return new float[] {-2 };
            }
        }
    }


    #endregion




    #region /***************--数据操作--***********************/
    /// <summary>
    /// 约会
    /// </summary>
    /// <param name="coins">需要的金钱</param>
    /// <param name="favor">增加的好感度</param>
    /// <returns></returns>
    public bool goToDate(float addFavor, float needCoins)
    {
        if (playerData.coins >= needCoins)
        {
            playerData.coins -= needCoins;
            playerData.favorPoint += addFavor;
            Debug.Log("约会成功！");
           
            return true;
        }
        Debug.Log("金钱不足！");
        return false;
    }
    /// <summary>
    /// 工作（增加金钱和心情）
    /// </summary>
    /// <param name="reducePower"></param>
    /// <param name="addCoins"></param>
    /// <param name="reduceMood"></param>
    /// <returns></returns>
    public bool goToWork(float addCoins, float reduceMood, float reducePower)
    {
        if (playerData.healthPoint >= reducePower)
        {
            playerData.healthPoint -= reducePower;//减少体力
            playerData.coins += addCoins;//增加金钱
            playerData.moodPoint -= reduceMood;//减少心情
            Debug.Log("工作成功：获得金钱-"+addCoins+"&获得心情："+reduceMood);
           
            return true;
        }
        Debug.Log("体力不足！");
        return false;
    }
   
    /// <summary>
    /// 喂食
    /// </summary>
    /// <param name="needCoins"></param>
    /// <param name="addBellyPoint"></param>
    /// <returns></returns>
    public bool giveFood(float addBellyPoint,float addFavor, float needCoins, float needDiamond)
    {
        if (playerData.coins >= needCoins)
        {
            playerData.coins -= needCoins;
            playerData.bellyPoint += addBellyPoint;
            playerData.favorPoint += addFavor;
            /*
            if(playerData.bellyPoint> getMaxBellyPoint())
            {
                playerData.bellyPoint = getMaxBellyPoint();
            }
            */
            Debug.Log("喂食成功！");
            return true;
        }
        Debug.Log("金钱不足！");
        return false;
    }
    /// <summary>
    /// 玩耍
    /// </summary>
    /// <param name="needHP"></param>
    /// <param name="addFavor"></param>
    /// <returns></returns>
    public bool goToPlay(int needHP, int addFavor)
    {
        if (playerData.healthPoint >= needHP)
        {
            playerData.healthPoint -= needHP;
            playerData.favorPoint += addFavor;
            Debug.Log("玩耍成功！");
            LevelUp();
            return true;
        }
        Debug.Log("体力不足！");
        return false;
    }
    /// <summary>
    /// 完成游戏 根据游戏结果获得奖励
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    //public bool sleepResult(EGameResult result)
    public bool completeGame(EGameResult result)
    {
       // playerData.healthPoint = getMaxHealthPoint();
       // playerData.sleepingPoint = getMaxSleepingPoint();
        Debug.Log("体力&睡眠回满！");
        //根据结果给予体力奖励
        switch (result)
        {
            case EGameResult.average:
                //TODO
                Debug.Log("获得体力奖励---20%");
                break;
            case EGameResult.good:
                Debug.Log("获得体力奖励---40%");
                break;
            case EGameResult.excellent:
                Debug.Log("获得体力奖励---60%");
                break;
            default:
                break;
                
        }

        return true;
    }
    public float completeGame(int victimes)
    {
        switch (victimes)
        {
            case 0:
                return getMaxHealthPoint() * 1.2f;
                Debug.Log("获得体力奖励---20%");
                break;
            case 1:
                return getMaxHealthPoint() * 1.4f;
                Debug.Log("获得体力奖励---40%");
                break;
            case 2:
                return getMaxHealthPoint() * 1.6f;
                Debug.Log("获得体力奖励---60%");
                break;
            default:
                return getMaxHealthPoint() * 1.6f;

                break;

        }
        return getMaxHealthPoint();


    }
    public int GameVictoryTimes()
    {

        return playerData.gameVictory;

    }

    /// <summary>
    /// 升级
    /// </summary>
    /// <returns>返回值为获得的钻石奖励数量</returns>
    public void LevelUp()
    {
        float cur = getFavorPoint();
        if (getFavorPoint() >= getMaxFavorPoint())
        {
            playerData.favorPoint -= getMaxFavorPoint();//当前变化后的好感度 - 当前等级最大的好感度 ， 可能为0 
            playerData.level += 1; //等级加1             
            playerData.diamonds += 10f;//钻石加10

            levelUpEvent.Invoke(getFavorPoint(), getDiamondCount(), getLevel()); //通知升级 并且传递 变化后的好感度, 钻石数目， 等级

          


        }
        
       
    }
    /// <summary>
    /// 增加体力值
    /// </summary>
    /// <param name="addPoint"></param>
    public void addHP(int addPoint)
    {
        playerData.healthPoint += addPoint;
        if (playerData.healthPoint > getMaxHealthPoint())
        {
            playerData.healthPoint = getMaxHealthPoint();
        }
    }
    /// <summary>
    /// 用钻石兑换金币
    /// </summary>
    /// <param name="needDiamonds"></param>
    /// <returns></returns>
    public bool exchangeCoins(int consumedDiamonds)
    {
        if (playerData.diamonds >= consumedDiamonds)
        {
            playerData.diamonds -= consumedDiamonds;
            playerData.coins += consumedDiamonds * 100;
            Debug.Log(consumedDiamonds+"钻石兑换得到金币"+consumedDiamonds*100);
            return true;
        }
        return false;
    }

    #endregion

    #region]/**************--对话数据加载--****************/
    //对话读取
    //根据参数，加载相应场景的全部资源

    public string[] GetDialogueData()
    {
        //加载数据
       var data = playerData.workResoult;
        //解析数据
        string[] ReData = data.femaleStr;
        //返回数据

        return ReData;
    }


    public string[] XmlHandleMethod(int index)
    {
        #region
        /*
        Debug.Log(Application.streamingAssetsPath);


        string url = Application.streamingAssetsPath + "/Dialogue.xml";

        XmlDocument xmlDocument = new XmlDocument();

        xmlDocument.Load(url);

        XmlNodeList xmlNodeList = xmlDocument.SelectSingleNode("dialogues").ChildNodes;

        foreach (XmlNode xmlNode in xmlNodeList)
        {

            Debug.Log(xmlNode);


        }
        */
        #endregion

        //读取对话数据

        string[] femaleStr = playerData.female;
        string[] maleStr = playerData.male;

       

        return new string[] {femaleStr[index], maleStr[index] };

        

    }

    #endregion


    #region 根据提供的时间数值返回时，分，秒
    /*
    hours = Mathf.Floor(timeSpan / 3600f); // 事件间隔的小时

         minutes = Mathf.Floor((timeSpan - hours* 3600f) / 60f); //时间间隔的分钟

         seconds = Mathf.Floor(timeSpan - (hours* 3600f) - (minutes* 60)); // 时间间隔的秒
    
    */
    public float Hours(float TotalTime) {
        
        return Mathf.Floor(TotalTime / 3600f); 
    }
    public float Minutes(float TotalTime)
    {
        return Mathf.Floor((TotalTime - (Hours(TotalTime)) * 3600f) / 60f);
    }
    public float Seconds(float TotalTime)
    {
        return Mathf.Floor(TotalTime - (Hours(TotalTime) * 3600f) - (Minutes(TotalTime) * 60));
    }

    #endregion


    //调试用
    public void DataInit()
    {

        playerData.logTimes = 0;


        playerData.coins = 1030;//默认金钱
        playerData.diamonds = 245;//默认钻石
        playerData.level = 1;//默认等级
        playerData.favorPoint = 44;//默认好感度
        playerData.healthPoint = 78;//默认体力
        playerData.moodPoint = 56;//默认心情值
        playerData.bellyPoint = 48;//默认饱食度
        playerData.FatiguePoint = 2;//默认睡眠值
        playerData.remainSleepingTime = 120;// 28800;//默认剩余睡眠时间
        
        playerData.isAutoSleep = true;//自动睡眠默认为false
        playerData.isWorking = false;//工作状态为false
        playerData.isDating = false;//约会状态为false



        designData.minLevelFavor = 50;
        designData.addFavorPerLevel = 5;
        designData.maxHealthPoint = 120;
        designData.maxMoodPoint = 100;
        designData.maxBellyPoint = 100;
        designData.maxSleepingPoint = 100;
        designData.sleepingDuration = 28800;

        //工作相关

        designData.workStyleArray[0].consumeHP = 10;
        designData.workStyleArray[0].minCoins = 50;
        designData.workStyleArray[0].maxCoins = 90;

        designData.workStyleArray[1].consumeHP = 15;
        designData.workStyleArray[1].minCoins = 120;
        designData.workStyleArray[1].maxCoins = 180;

        designData.workStyleArray[2].consumeHP = 20;
        designData.workStyleArray[2].minCoins = 200;
        designData.workStyleArray[2].maxCoins = 300;

        designData.workStyleArray[3].consumeHP = 30;
        designData.workStyleArray[3].minCoins = 345;
        designData.workStyleArray[3].maxCoins = 570;

        //约会相关
        designData.dateStyleArray[0].consumeCoins = 20;
        designData.dateStyleArray[0].coldDownTime = 60;
        designData.dateStyleArray[0].addFavorPoint = 4;

        designData.dateStyleArray[1].consumeCoins = 150;
        designData.dateStyleArray[1].coldDownTime = 1800;
        designData.dateStyleArray[1].addFavorPoint = 18;

        designData.dateStyleArray[2].consumeCoins = 800;
        designData.dateStyleArray[2].coldDownTime = 7200;
        designData.dateStyleArray[2].addFavorPoint = 64;

        designData.dateStyleArray[3].consumeCoins = 1800;
        designData.dateStyleArray[3].coldDownTime = 28800;
        designData.dateStyleArray[3].addFavorPoint = 90;

        //玩耍相关
        designData.playStyleArray[0].consumeHP = 10;
        designData.playStyleArray[0].coldDownTime = 900;
        designData.playStyleArray[0].addMoodPoint = 2;

        designData.playStyleArray[1].consumeHP = 20;
        designData.playStyleArray[1].coldDownTime = 3600;
        designData.playStyleArray[1].addMoodPoint = 6;


        //喂食相关
        designData.feedStyleArray[0].consumeCoins = 50;
        designData.feedStyleArray[0].addBellyPoint = 10;

        designData.feedStyleArray[1].consumeCoins = 150;
        designData.feedStyleArray[1].addBellyPoint = 60;

        designData.feedStyleArray[2].isConsumeDiamonds = true;
        designData.feedStyleArray[2].consumeDiamonds = 10;
        designData.feedStyleArray[2].addBellyPoint = 20;
        designData.feedStyleArray[2].addFavorPoint = 5;

    }



}


//游戏结果判定
public enum EGameResult
{
    average,//一般
    good,//好
    excellent//优秀
}




