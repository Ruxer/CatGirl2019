using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Video;

/// <summary>
/// 加载AssetBundle资源脚本
/// </summary>
public class AssetBundleManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        AssetBundle myAssetsBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "movieclipsbundle"));

        if (myAssetsBundle == null)
        {
            Debug.Log("load Assets Failed!");

            return;

        }
        else
        {

            Debug.Log("load Successful!");

            Object[] obj = myAssetsBundle.LoadAllAssets(typeof(VideoClip));


            VideoPlayer vd =  GameObject.Find("Coax sleep").GetComponent<VideoPlayer>();

            vd.clip = (VideoClip)obj[2];
            vd.isLooping = true;
            vd.Play();

        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
