/*
  authro: wenster
 
 
 */

using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// 打包AssetBundle脚本
/// </summary>
public class VideoAssetBundles {

    //创建编辑器打包菜单
    [MenuItem("AssetBundle/Package With BuildMap")]
    private static void CreateAssetBundleMain()
    {
        Debug.Log("Package AssetBundle......");

        //输出路径为Assets下的StreamingAssets文件夹（要确定存在该路径）



        //创建资源数组
        AssetBundleBuild[] buildMap = new AssetBundleBuild[1];

        //指定资源打包名称
        buildMap[0].assetBundleName = "movieClipsbundle";

        //将多个文件打包到一个资源包中.
        string[] movieAssets = new string[3];
        movieAssets[0] = "Assets/Media/movies/movie1/idle.mp4";
        movieAssets[1] = "Assets/Media/movies/movie1/onBody.mp4";
        movieAssets[2] = "Assets/Media/movies/movie1/onHead.mp4";
        //movieAssets[3] = "Assets/Media/movies/movie3/idle.mp4";

        //在buildMap[0]下的包中打包多个资源.
        buildMap[0].assetNames = movieAssets;

       

        //创建文件窗口,根据选择返回路径.
        string packagePath = UnityEditor.EditorUtility.OpenFolderPanel("选择打包路径", "streamingAssets", "");

        if (packagePath.Length <= 0 || !Directory.Exists(packagePath)) //未选择路径或目录不存在
        {
            Debug.Log("文件目录不存在,或其他错误");
            //TODO  是否创建目录 Directory.CreateDirectory();
            return;
        }

        //调用方法打包
       // BuildPipeline.BuildAssetBundles(packagePath, BuildAssetBundleOptions.None, BuildTarget.Android);
        BuildPipeline.BuildAssetBundles(packagePath,buildMap, BuildAssetBundleOptions.None, BuildTarget.Android);

        AssetDatabase.Refresh();

        Debug.Log("Package Successful!");

      

    }


}
