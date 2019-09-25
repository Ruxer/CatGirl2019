using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class WorkDialog : MonoBehaviour,IPointerClickHandler {

    private string[] workDialogStr =
    {
        "小唯：主人，我要出门打工啦！",
        "我：加油啊，小唯！",
        "Wait：过了半天。。。",
        "小唯：主人，我回来了。",
        "我：怎么样，打工顺利吗？"
    };

    private string[] workPerfectDialogStr =
    {
        "小唯：店长说我做的超棒，付了双倍工资！",
        "我：太好了！"
    };
    private string[] workSucessDialogStr =
    {
        "小唯：一切顺利！",
        "我：辛苦了，小唯！"
    };
    private string[] workFailDialogStr =
    {
        "小唯：不太好，被店长骂了..还扣了工资！",
        "我：别灰心，小唯！"
    };
    private PlayerDataUtils playerDataUtils;
    private Dictionary<string, int> workAwardInfoDict;
    private string[] workResultStr;
    private string actressName = "小唯";
    private string playerName = "我";
    private string waitSign = "Wait";
    private char splitSign ='：';
    private Sprite actressSprite;//女主头像
    private int workDialogIndex=0;
    private int workResultDialogIndex=0;
    private Image actressImage;
    private Text characterNameText;
    private Text dialogText;
    private string showDialogCor="ShowDialogCor";
    private bool canClick = false;
    private GameObject activityResultPanel;

    [HideInInspector]
    public bool isDialogueFinished = false;
    private void Awake()
    {
        playerDataUtils = PlayerDataUtils.getInstance();
       // activityResultPanel = transform.parent.Find("ActivityResult_Panel").gameObject;
        actressImage = transform.Find("Actress_Image").GetComponent<Image>();
        characterNameText = transform.Find("Dialog_Image/Name_Text").GetComponent<Text>();
        dialogText = transform.Find("Dialog_Image/Dialog_Text").GetComponent<Text>();
        actressSprite = Resources.Load<Sprite>("UIImage/Actress/Actress_1");
        //this.gameObject.SetActive(false);
    }
    private void Start()
    {

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!canClick) return;
        string characterName = "";
        string dialogContent = "";
        workDialogIndex += 1;
        if (workDialogIndex >= workDialogStr.Length)
        {
            if (workResultStr == null)
            {
                //根据熟练度计算打工结果，这里先随机产生
                Random.InitState(System.DateTime.Now.Millisecond);
                int randomNum = Random.Range(1, 4);
                switch (randomNum)
                {
                    case 1:
                        workResultStr = workPerfectDialogStr;
                        break;
                    case 2:
                        workResultStr = workSucessDialogStr;
                        break;
                    case 3:
                        workResultStr = workFailDialogStr;
                        break;
                    default:
                        break;
                }
                characterName = workResultStr[workResultDialogIndex].Split(splitSign)[0];
                dialogContent = workResultStr[workResultDialogIndex].Split(splitSign)[1];
                if (characterName.Equals(actressName))
                {
                    EnableAtressSay();
                    StartCoroutine(showDialogCor, dialogContent);
                    return;
                }
                else if(characterName.Equals(playerName))
                {
                    EnablePlayerSay();
                    StartCoroutine(showDialogCor,dialogContent);
                    return;
                }
                
            }
            workResultDialogIndex += 1;
            if (workResultDialogIndex >= workResultStr.Length)
            {
                //结果对话完成
                // activityResultPanel.SetActive(true);
                // this.gameObject.SetActive(false);
                isDialogueFinished = true;
                this.transform.parent.gameObject.SetActive(false);
                return;
            }
            characterName = workResultStr[workResultDialogIndex].Split(splitSign)[0];
            dialogContent = workResultStr[workResultDialogIndex].Split(splitSign)[1];
            if (characterName.Equals(actressName))
            {
                EnableAtressSay();
                StartCoroutine(showDialogCor,dialogContent);
                return;
            }else if (characterName.Equals(playerName))
            {
                EnablePlayerSay();
                StartCoroutine(showDialogCor,dialogContent);
                return;
            }

        }
        characterName = workDialogStr[workDialogIndex].Split(splitSign)[0];
        dialogContent = workDialogStr[workDialogIndex].Split(splitSign)[1];
        if (characterName.Equals(waitSign))
        {
            actressImage.color = Color.clear;
            characterNameText.text = "";
            StartCoroutine(showDialogCor,dialogContent);
            return;
        }
        if (!string.IsNullOrEmpty(characterName))
        {
            if (characterName.Equals(actressName))
            {
                EnableAtressSay();
            }
            else if(characterName.Equals(playerName))
            {
                EnablePlayerSay();
            }
        }
        if (!string.IsNullOrEmpty(dialogContent))
        {
            StartCoroutine(showDialogCor,dialogContent);
        }

    }
    
    public void StartDialog(int styleIndex=1)
    {
        //workAwardInfoDict = playerDataUtils.getWorkAwardInfo(styleIndex);
        string characterName = workDialogStr[workDialogIndex].Split(splitSign)[0];
        string dialogContent = workDialogStr[workDialogIndex].Split(splitSign)[1];
        if (characterName.Equals(actressName))
        {
            EnableAtressSay();
        }
        else if(characterName.Equals(playerName))
        {
            EnablePlayerSay();
        }
        StartCoroutine(showDialogCor, dialogContent);
    }

    private void OnEnable()
    {


        StartDialog();

    }

    private void OnDisable()
    {
        workDialogIndex = 0;
        workResultDialogIndex = 0;
        StopCoroutine(showDialogCor);
    }

    IEnumerator ShowDialogCor(string dialog)
    {
        canClick = false;
        int index = 0;
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            index += 1;
            if (index > dialog.Length)
            {
                canClick = true;
                StopCoroutine(showDialogCor);
                yield return null;
            }
            dialogText.text = dialog.Substring(0,index);
                
        }
    }

    private void EnableAtressSay()
    {
        actressImage.color = Color.white;

        characterNameText.text = actressName+splitSign;
    }
    private void EnablePlayerSay()
    {
        actressImage.color = Color.clear;
        characterNameText.text = playerName+splitSign;
    }
}
