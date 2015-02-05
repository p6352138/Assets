using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using AppUtility ;

public class RescourseMgr : MonoBehaviour {
	private Dictionary<string,GameObject> m_resourceList ;
	private 	AssetBundle			m_UIResource   		;
	private 	AssetBundle			m_IconResource 		;
	private 	AssetBundle     	m_EffectResource	;
	private		AssetBundle			m_RoleResource		;
	private		Object[]			m_allAssert			;
	
	private 	static RescourseMgr instance			;
	public 		string 				textStr = "" 		;
	
	public 		string PathURL ;
	
	public static RescourseMgr GetInstance()
    {
        if (instance == null)
        {
            instance = GameObject.Find("GameMain").GetComponent("RescourseMgr") as RescourseMgr;
        }

        return instance;
    }
	
	void Start(){
		//StartCoroutine(LoadAllGameObject(PathURL + "ALL.assetbundle"));
	}
	
	//init
	public void Init(){
		LoadAssertPC();
		//gameGlobal.InitUI();
	}
	
	// Update is called once per frame
	public void Update () {
		
	}
	
	/* void OnGUI()
    {
        if(GUI.Button(new Rect(0.0f,0.0f,100.0f,100.0f),"Main Assetbundle"))
        {
            StartCoroutine(LoadAssertAndroid());
            //StartCoroutine(LoadMainGameObject(PathURL +  "Prefab1.assetbundle"));
        }
 
        if(GUI.Button(new Rect(0.0f,110.0f,100.0f,100.0f),"ALL Assetbundle"))
        {
            //StartCoroutine(LoadAllGameObject(PathURL + "ALL.assetbundle"));
			gameGlobal.g_rescoureMgr.Init();
			//StartCoroutine(LoadAllGameObject(PathURL + "ALL.assetbundle"));
        }
		
		GUI.TextArea(new Rect(0.0f,210.0f,200.0f,200.0f),textStr) ;
 
    }*/
	
	//load assert
	public IEnumerator LoadAssertAndroid(){
		string path = "jar:file://" + Application.dataPath + "!/assets/" + "ALL.assetbundle";
		WWW bundle = new WWW(path);
		textStr += "LoadMainGameObject:" + bundle.progress + "\n";
		yield return bundle ;
		m_UIResource = bundle.assetBundle ;
		m_allAssert  = bundle.assetBundle.LoadAll();
		foreach(Object temp in m_allAssert){
			System.Type type = temp.GetType();
			if(type == typeof(GameObject))
				textStr += temp.name + "\n";
		}
	}
	
	public void LoadUIResource(WWW assert){
		textStr += assert.assetBundle + "\n";
		textStr += assert.assetBundle.name + "\n";
		m_UIResource = assert.assetBundle ;
		//common.debug.GetInstance().Log("LoadResource:" + assert.assetBundle.name);
		/*m_allAssert  = assert.assetBundle.LoadAll();
		textStr += "assert.assetBundle.LoadAll success" + "\n";
		foreach(Object temp in m_allAssert){
			System.Type type = temp.GetType();
			if(type == typeof(GameObject)){
				m_resourceList.Add(temp.name,(GameObject)temp);
			}
			textStr += temp.name + "\n";	
		}*/
		int pro = 0;
		//gameGlobal.g_rescoureLoader.LoadRescoure(PathURL + "ALL.assetbundle",LoadResource,ref pro);	
	}
	
	public void LoadResource(WWW assert){
		textStr += assert.assetBundle + "\n";
		textStr += assert.assetBundle.name + "\n";
		m_RoleResource = assert.assetBundle ;
		//common.debug.GetInstance().Log("LoadResource:" + assert.assetBundle.name);
		/*m_allAssert  = assert.assetBundle.LoadAll();
		textStr += "assert.assetBundle.LoadAll success" + "\n";
		foreach(Object temp in m_allAssert){
			System.Type type = temp.GetType();
			if(type == typeof(GameObject)){
				m_resourceList.Add(temp.name,(GameObject)temp);
			}
			textStr += temp.name + "\n";	
		}*/
		//gameGlobal.InitUI();
	}
	
	public IEnumerator LoadAssertIphone(){
		string path = Application.dataPath + "/Raw/" + "ALL.assetbundle";
		WWW bundle = new WWW(path);
		yield return bundle ;
		m_UIResource = bundle.assetBundle ;
		m_allAssert  = bundle.assetBundle.LoadAll();
	}
	
	public void LoadAssertPC(){
		m_resourceList = new Dictionary<string, GameObject>();
		Object[] resList = Resources.LoadAll("object");
		foreach(Object obj in resList){
			System.Type type = obj.GetType();
			if(type == typeof(GameObject)){
				if(!m_resourceList.ContainsKey(obj.name))
					m_resourceList.Add(obj.name,(GameObject)obj) ;
				else{
					//common.debug.GetInstance().Error("same resource name:" + obj.name);
				}
			}
		}
		
		resList = Resources.LoadAll("UI");
		foreach(Object obj in resList){
			System.Type type = obj.GetType();
			if(type == typeof(GameObject)){
				if(!m_resourceList.ContainsKey(obj.name))
					m_resourceList.Add(obj.name,(GameObject)obj) ;
				else{
					//common.debug.GetInstance().Error("same resource name:" + obj.name);
				}
			}
		}
		//gameGlobal.InitUI();
	}
	
	private IEnumerator LoadAllGameObject(string path){
		WWW bundle = new WWW(path);
		//print("LoadALLGameObject:" + bundle.progress);
		textStr += "LoadALLGameObject:" + path + "\n";
		yield return bundle;
		if(bundle.error != null){
			textStr += "error:" + bundle.error + "\n";
		}
		else{
			textStr += bundle.ToString() + "\n";
			textStr += bundle.assetBundle.name ;
			//print (bundle.assetBundle);
			if(bundle.assetBundle != null){
				Object[] allAssert = bundle.assetBundle.LoadAll();
				foreach(Object temp in allAssert){
				System.Type type = temp.GetType();
				if(type == typeof(GameObject))
					textStr += temp.name;
				}
			}
			bundle.assetBundle.Unload(false);
		}
	}
	
	//////////////////////////////////////////////// interface /////////////////////////////////////////////
	/// <summary>
	/// Gets the game object resource.
	/// </summary>
	/// <returns>
	/// The game object resource.
	/// </returns>
	/// <param name='id'>
	/// Identifier.
	/// </param>
	public GameObject GetGameObjectResource(string name){
#if UNITY_EDITOR || UNITY_STANDALONE_WIN 
		if(m_resourceList.ContainsKey(name)){
			//common.debug.GetInstance().Log("Load resource :" + name);
			return m_resourceList[name] ;
		}
		else{
			//common.debug.GetInstance().Warmming("Load resource error:" + name);
			return null ;
		}
#else
		GameObject obj = null ;
		obj = Resources.Load("object/" + name) as GameObject;
		if(obj == null){
			obj = Resources.Load("UI/" + name) as GameObject;
			if(obj == null){
				common.debug.GetInstance().Warmming("Load resource error:" + name);
				return null ;
			}
			else{
				return obj ;
			}
		}
		else{
			return obj ;
		}
#endif
	}
	
	public AudioClip GetAudioClip(string name){
		string path = "Music/" + name ;
		return Resources.Load(path) as AudioClip;
	}
}
