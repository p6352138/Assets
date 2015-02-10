using UnityEngine;
using System.Collections;
using GameEntity;
using AppUtility;

namespace GameLogic.Navigation{ 
	class NavigationMgr : Singleton<NavigationMgr> {
		#region private Fields
		private NGrid m_grid;
		#endregion

		public void init(){
			m_grid = new NGrid();
		}

		public void showGrid(){
			NGrid.DebugDraw(CCearcueMgr.GetInstance().GetTerrainPosition());
		}

		public void showObstacleGrid(){
			NObstacleGrid.DebugShowObstacleGrid(CCearcueMgr.GetInstance().GetTerrainBounds());
		}

		public NGrid GetGrid(){
			return m_grid;
		}
	}
}
