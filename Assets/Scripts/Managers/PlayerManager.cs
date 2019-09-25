using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{

  
    //标签按钮列表
    public List<Button> btns;
    //标签按钮标题
    private List<Text> titles;

    //外出列表
    public List<Image> images;


    private void Awake()
    {
        titles = new List<Text>();

        foreach (var item in btns)
        {
            Text txt = item.GetComponentInChildren<Text>();
            titles.Add(txt);

            item.onClick.AddListener(()=> {

                SelectionMethod(item);


            });


        }

        foreach (var item in images)
        {

          


        }





    }

    private void Start()
    {
      


        //SelectionMethod();

    }

    /// <summary>
    /// 标签选择
    /// 除当前标签外,其他呈灰色状态
    /// </summary>
    private void SelectionMethod(Button btn)
    {

        //所有按钮恢复到灰色状态,btn除外
        foreach (var item in btns)
        {
            Color tempColor = new Color(1, 1, 1, 0.5f);
            
            item.image.color = tempColor;
            titles[(btns.IndexOf(item))].color = tempColor;

            tempColor.a = 1f;
            btn.image.color = tempColor;
            titles[(btns.IndexOf(btn))].color = tempColor;
        }



    }

    /// <summary>
    /// 约会事件
    /// </summary>
    private void dataMethod()
    {




    }





}
