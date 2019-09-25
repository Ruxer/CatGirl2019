using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSingleObject : MonoBehaviour {


    private static GlobalSingleObject instance;

    public static GlobalSingleObject GetInstance()
    {
        if (instance == null)
        {
            instance = new GlobalSingleObject();
        }

        return instance;

    }

    //这里是全局会用到的变量与数据。





}
