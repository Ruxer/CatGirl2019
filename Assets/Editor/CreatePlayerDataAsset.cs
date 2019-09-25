using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class CreatePlayerDataAsset  {
    [MenuItem("Assets/PlayerDataAsset")]
	public static void Execute()
    {
        PlayerData playerData = ScriptableObject.CreateInstance<PlayerData>();
        AssetDatabase.CreateAsset(playerData, "Assets/Resources/PlayerData.asset");
        AssetDatabase.Refresh();
    }
}
