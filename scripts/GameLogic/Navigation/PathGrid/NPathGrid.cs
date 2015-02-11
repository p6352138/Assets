using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameLogic.Navigation{
	public class NPathGrid : NSolidityGrid,NIPathTerrain {
		#region private field
		private NObstacleGrid m_obstacleGrid;
		#endregion

		#region public function
		public void InitObstacleData(List<Bounds> bounds){
			m_obstacleGrid = new NObstacleGrid();
			m_obstacleGrid.Init(bounds);
		}

		public void DrawObstacle(){
			m_obstacleGrid.DebugShowObstacleGrid();
		}
		#endregion

		#region interface function

		public int GetPathNodeIndex(Vector3 pos)
		{
			int result = 0;
			return result;
		}

		public Vector3 GetPathNodePos(int index)
		{
			// Use the center of the grid cell, as the position of the planning node.
			Vector3 nodePos = GetCellPosition(index);
			nodePos += new Vector3(m_cellSize / 2.0f, 0.0f, m_cellSize / 2.0f);
			nodePos.y = GetTerrainHeight(nodePos);
			return nodePos;
		}
		

		public void ComputePortalsForPathSmoothing(Vector3[] roughPath, out Vector3[] aPortalLeftEndPts, out Vector3[] aPortalRightEndPts)
		{
			aPortalLeftEndPts = null;
			aPortalRightEndPts = null;
		}

		public Vector3 GetValidPathFloorPos(Vector3 position)
		{
			Vector3 result = new Vector3(0,0,0);
			return result;
		}
		

		public float GetTerrainHeight(Vector3 position)
		{
			float result = 0;
			return result;
		}

		#endregion
	}
}
