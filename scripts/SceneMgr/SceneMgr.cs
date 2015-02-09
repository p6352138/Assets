using UnityEngine;
using System.Collections;
using GameEntity;
using GameLogic.Navigation;

public class SceneMgr : MonoBehaviour {

	#region public Properties
	public bool isDebug;
	public GameObject Terrian;
	#endregion

	#region private Properties
	private int m_curSceneId;
	#endregion

	// Use this for initialization
	void Start () {
		m_curSceneId = 1;

		if(m_curSceneId == GameDefine.FightSceneID){
			CCearcueMgr.GetInstance().setTerrian(Terrian);
			CCearcueMgr.GetInstance().CreateCearcue(1,CCearcueType.Terrian);

		}
	}
	
	// Update is called once per frame
	void Update () { 
		if(isDebug)
			NavigationMgr.GetInstance().showGrid();
	}
}
