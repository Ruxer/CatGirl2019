using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    //加载数据文件
    private PlayerData playerData;
    
    //饱食度UI
    private Image bellyPointPrograssbar;//饱食度进度条
    //心情值UI
    private Image moodPointPrograssbar;//心情值进度条
    //疲劳度UI
    private Image fatiguePointPrograssbar;//疲劳度进度条

    //三大进度条组
    private  List<Image> prograssbars;

    //进度条文本
    public Transform bellyText;
    public Transform moodText;
    public Transform fatigueText;

    //


    //对饱食度变换事件的监听
    //对心情值变换事件的监听
    //对睡眠值变换事件的监听

    /*
    [HideInInspector]
        public Image _feedbar;//喂食进度条
        private Text _feedBottomText;//喂食底部数值指示
        private Text _feedTopText;//喂食上部数值指示

        */

    //标签页图标
    private Image home;


    private void Awake()
    {
        prograssbars = new List<Image>();
        //遍历进度条子对象,将符合条件的放入列表中.
        foreach (var image in GetComponentsInChildren<Image>(true))
        {
            string[] s = image.name.Split('_');
            
            if (s[1] == "top")
            {
                prograssbars.Add(image);
               // Debug.Log(image.name);
            }

        }

       
    }

    // Use this for initialization
    void Start () {



      

    }

    //****比较fillAmount与localScale的性能问题*****//

    //饱食度UI更新方法
    public void BellyUIUpdeta(float BellyValue)//10
    {
        prograssbars[0].fillAmount -= 0.1f;
        //Debug.Log("Belly" + BellyValue);
        bellyText.GetComponent<Text>().text = "" + "" + BellyValue + "%";

    }
    //心情值UI更新方法
    public  void MoodUIUpdate(float changValue,float MoodPoint)//1,2,3,4,5,5...
    {
        
        switch (changValue)
        {
            case 0:
                prograssbars[1].fillAmount = 0f;
                break;
            case 1:
                prograssbars[1].fillAmount -= 0.01f;
                break;
            case 2:
                prograssbars[1].fillAmount -= 0.02f;
                break;
            case 3:
                prograssbars[1].fillAmount -= 0.03f;
                break;
            case 4:
                prograssbars[1].fillAmount -= 0.04f;
                break;
            case 5:
                prograssbars[1].fillAmount -= 0.05f;
                break;
            default:
                Debug.Log("心情值UI更新逻辑错误.");
                break;
        }

        if (MoodPoint >= 0 )
        {
            //Debug.Log("Mood" + changedValue);
            moodText.GetComponent<Text>().text = "" + "" + MoodPoint + "%";
        }
        

    }
    //疲劳度UI更新方法
    public void FatigueUIUpdate(float FatigueValue)//3
    {

        prograssbars[2].fillAmount -= 0.03f;
        //Debug.Log("Fatigue" + currentValue);
        fatigueText.GetComponent<Text>().text = "" + "" + FatigueValue + "%";

    }

}
