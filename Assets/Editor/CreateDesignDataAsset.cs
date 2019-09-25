using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class CreateDesignDataAsset  {
    [MenuItem("Assets/DesignDataAsset")]
    public static void Execute()
    {
        DesignData designData = ScriptableObject.CreateInstance<DesignData>();
        AssetDatabase.CreateAsset(designData,"Assets/Resources/DesignData.asset");
        AssetDatabase.Refresh();
    }

}
