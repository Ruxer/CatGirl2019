using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

/// <summary>
/// 播放视频，动态创建视频播放器，动态加载视频，动态释放视频。
/// 视频将用assetBundle打包,在游戏运行时,动态加载assetbundle,获取视频资源.
/// </summary>
public class PlayClipManager : MonoBehaviour {


    //播放器
    private VideoPlayer mainPlayer;

    //视频资源
    private VideoClip idle;

    public RawImage rawIamge;

    AssetBundle ab;
    RenderTexture renderTexture;
    private Camera movieCamera;
    //
    private void Awake()
    {

        ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/movieclipsbundle");

        mainPlayer = GetComponent<VideoPlayer>();
        movieCamera = GetComponent<Camera>();

        renderTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);

        renderTexture.name = "RenderTexture001";

       

       

       

       


    }





    // Use this for initialization
    void Start () {

        

        mainPlayer.clip = ab.LoadAsset<VideoClip>("idle.mp4");

        mainPlayer.isLooping = true;

        //mainPlayer.renderMode = VideoRenderMode.APIOnly;

        //mainPlayer.targetCamera = GetComponent<Camera>();
        movieCamera.targetTexture = renderTexture;
        rawIamge.texture = renderTexture;

        //image.texture = mainPlayer.texture;

        mainPlayer.Play();
       
    }
	
	// Update is called once per frame
	void Update () {
		



	}


    #region 播放打招呼视频

    //创建播放器
    public void CreateVideoPlayer()
    {

        //确定视频播放器的父对象
        //playClips





    }



    #endregion


    private IEnumerator LoadMovie()
    {

        



        yield return null;

    }


    private void OnDestroy()
    {

        rawIamge.texture = null;


    }






}
