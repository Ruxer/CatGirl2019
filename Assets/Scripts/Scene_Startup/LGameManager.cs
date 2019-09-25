using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 测试GameManager的副本脚本
/// </summary>
public class LGameManager : MonoBehaviour {

   
    
    //加载游戏标题画面
    //确定
    //开始加载游戏画面
    //走进度条，异步加载另一个场景
    //提示 按任意键开始
    //进入游戏开场
    //一段时间后
    //加载渐隐画面

  
    /// <summary>
    /// 异步加载
    /// </summary>
    private AsyncOperation _asyncOperation;
    /// <summary>
    /// 加载UI
    /// </summary>
    private GameObject _loadingPanel;
    /// <summary>
    /// 加载进度条
    /// </summary>
    private Slider _loadingSlider;
    /// <summary>
    /// 文本
    /// </summary>
    private Text _loadingText;

    //替换图片-测试
    public Image handleImage;

    private void Start()
    {

       // LoadScene("CATGIRLMAIN3.0");

       // StartCoroutine("StateUpdateIE", "CATGIRLMAIN3.0");
        StartCoroutine("StateUpdateIE", "TestScene");

    }
    private void Update()
    {

      //  StateUpdate();

    }
 

    //协程
    IEnumerator StateUpdateIE(string loadSceneName)
    {

        if (string.IsNullOrEmpty(loadSceneName))//判断场景名是否为空
        {
            StopCoroutine("StateUpdateIE");
        }

       // _loadingPanel = GameObject.Find("Canvas").transform.Find("Loading").gameObject;
        _loadingPanel.SetActive(true);
        _loadingSlider = _loadingPanel.transform.Find("Slider").GetComponent<Slider>();
        _loadingText = _loadingPanel.transform.Find("Slider/Text").GetComponent<Text>();
        _asyncOperation = SceneManager.LoadSceneAsync(loadSceneName, LoadSceneMode.Single);
        _asyncOperation.allowSceneActivation = false;

        float displayProgress = 0; //当前进度
        float totalProgress = 0; //总进度

       

        while (!_asyncOperation.isDone)
        {
            
            _loadingText.text = "加载进度: " + (_asyncOperation.progress * 100) + "%";
            _loadingSlider.value = _asyncOperation.progress * 100;


            //让进度平滑

          //  while (totalProgress <= _asyncOperation.progress)
          //  {
                // displayProgress = Mathf.Lerp(totalProgress, _asyncOperation.progress, Time.deltaTime);

                // _loadingText.text = "加载进度: " + (displayProgress * 100) + "%";

              //  Debug.Log(_asyncOperation.progress);

          //  }
           
            if (_asyncOperation.progress >= 0.9f)
            {

                _loadingText.text = "按任意键继续。。。";


                _loadingSlider.value = 100;
                //替换图片
                handleImage.sprite = Resources.Load<Sprite>("进度猫1");


                if (Input.GetMouseButtonDown(0))
                {
                    
                    _asyncOperation.allowSceneActivation = true;

                    //进度将到达1.0，需要平滑显示。


                    //Screen.orientation = ScreenOrientation.Landscape;

                }

            }

            yield return null;
        }

      

    }



  





}