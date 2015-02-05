using UnityEngine;
using UnityEditor;
using System.Collections;

public class CreateAssetBundle : MonoBehaviour {
	[MenuItem("Custom Editor/Create AssetBunldes ALL")]
	static void CreateAssetBunldesALL ()
	{
	 
	    Caching.CleanCache ();
	 
	    string Path = Application.dataPath + "/StreamingAssets/ALL.assetbundle";
	 
	    Object[] SelectedAsset = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);
	 	
		//Object[] SelectedAsset = Resources.LoadAll("resourceObject");
		
		BuildTarget buildTarget = BuildTarget.Android ;
	 	#if UNITY_ANDROID
       	   buildTarget = BuildTarget.Android;
		#elif UNITY_IPHONE
		   buildTarget = BuildTarget.iPhone;
		#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
		   buildTarget = BuildTarget.StandaloneWindows;
		#else
		   buildTarget = BuildTarget.WebPlayer;
		#endif
		
	    if (BuildPipeline.BuildAssetBundle (null, SelectedAsset, Path, BuildAssetBundleOptions.CollectDependencies,buildTarget)) {
	        AssetDatabase.Refresh ();
			Debug.Log("all resource pack success");
	    }
		else {
	 		Debug.Log("all resource pack fail");
	    }
	    
	}
	
	[MenuItem("Custom Editor/Create AssetBunldes Main")]
 	static void CreateAssetBunldesMain ()
 	{
 	    Object[] SelectedAsset = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);
 	 
		BuildTarget buildTarget = BuildTarget.Android ;
	 	#if UNITY_ANDROID
       	   buildTarget = BuildTarget.Android;
		#elif UNITY_IPHONE
		   buildTarget = BuildTarget.iPhone;
		#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
		   buildTarget = BuildTarget.StandaloneWindows;
		#else
		   buildTarget = BuildTarget.WebPlayer;
		#endif
		
 	    foreach (Object obj in SelectedAsset)
 	    {
 	        string sourcePath = AssetDatabase.GetAssetPath (obj);
 	        string targetPath = Application.dataPath + "/StreamingAssets/" + obj.name + ".assetbundle";
 	        if (BuildPipeline.BuildAssetBundle (obj, null, targetPath, BuildAssetBundleOptions.CollectDependencies,buildTarget)) {
 	            Debug.Log(obj.name +"resource pack success");
				AssetDatabase.Refresh ();  
 	        }
 	        else
 	        {
 	            Debug.Log(obj.name +"resource pack fail");
 	        }
 	    }
 	}
}
