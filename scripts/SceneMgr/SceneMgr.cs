using UnityEngine;
using System.Collections;
using GameEntity;
using GameLogic.Navigation;
using NetWork.NetSession;

public class SceneMgr : MonoBehaviour {

	#region public Properties
	public bool isOutPutMap;
	public bool isDebug;
	public GameObject Terrian;
	public GameObject Player;
	public Vector3 MapOrigin;
	#endregion

	#region private Properties
	private int m_curSceneId;
	#endregion

	// Use this for initialization
	void Start () {
		m_curSceneId = 1;

		//NetSessionMgr.GetInstance().Init();
		NavigationMgr.GetInstance().init(MapOrigin);
		CCearcueMgr.GetInstance ().Init();

		if(m_curSceneId == GameDefine.FightSceneID){
			CCearcueMgr.GetInstance().setTerrian(Terrian);
			CCearcueMgr.GetInstance().CreateCearcue(1,CCearcueType.Terrian);
			
			CCearcueMgr.GetInstance().setPlayer(Player);
			CCearcueMgr.GetInstance().CreateCearcue(1,CCearcueType.Player);
		}

        if (isOutPutMap)
            NavigationMgr.GetInstance().ImportMapData();
	}
	
	// Update is called once per frame
	void Update () { 
		if(isDebug){
			NavigationMgr.GetInstance().showGrid();
			NavigationMgr.GetInstance().showObstacleGrid();
		}

		CCearcueMgr.GetInstance ().Update (Time.deltaTime);
	}

    void OnGUI()
    {
       
    }
}
