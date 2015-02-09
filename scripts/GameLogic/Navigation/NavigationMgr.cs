using UnityEngine;
using System.Collections;
using GameEntity;
using AppUtility;

namespace GameLogic.Navigation{ 
	class NavigationMgr : Singleton<NavigationMgr> {

		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		public void showGrid(){
			NGrid.DebugDraw(CCearcueMgr.GetInstance().GetTerrainPosition());
		}

		public void showObstacleGrid(){
			NObstacleGrid.DebugShowObstacleGrid(CCearcueMgr.GetInstance().GetTerrainBounds());
		}
	}
}
