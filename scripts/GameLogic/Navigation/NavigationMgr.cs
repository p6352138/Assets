using UnityEngine;
using System.Collections;
using GameEntity;
using AppUtility;
using System.Collections.Generic;

namespace GameLogic.Navigation{ 
	class NavigationMgr : Singleton<NavigationMgr> {
		#region private Fields
		private NPathGrid m_grid;
		private NPathAgent m_agent;
		#endregion

		#region Init data
		public void init(){
			m_grid = new NPathGrid();
		}

		public void InitPathData(List<Bounds> bounds)
		{
			m_grid.InitObstacleData(bounds);
		}
		#endregion

		#region showDebug
		public void showGrid(){
			NGrid.DebugDraw(CCearcueMgr.GetInstance().GetTerrainPosition());
		}

		public void showObstacleGrid(){
			m_grid.DrawObstacle();
		}
		#endregion

		public NPathGrid GetGrid(){
			return m_grid;
		}
	}
}
