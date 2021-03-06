﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameEntity;

namespace GameLogic.Navigation{
	public class NPathGrid : NSolidityGrid,NIPathTerrain {

		public enum eNeighborDirection
		{
			kNoNeighbor = -1,
			kLeft,
			kTop,
			kRight,
			kBottom,
			kNumNeighbors
		};

		#region private field
		private NObstacleGrid m_obstacleGrid;
		#endregion

		#region public function
        public void InitObstacleData(List<Bounds> bounds)
        {
			m_obstacleGrid = new NObstacleGrid();
			m_obstacleGrid.Init(bounds);
		}

        public void InitSolidityData()
        {
            //初始化SolidityGrid数据
            for (int i = 0; i < m_obstacleGrid.Obstaclelist.Count; i++)
            {
                int cellsize;
                int[] cells = m_obstacleGrid.Obstaclelist[i].GetObstructedCells(out cellsize);

                for (int j = 0; j < cellsize;j++ )
                {
                    SetSolidityObstacleCell(cells[j]);
                }
            }
        }

		public void DrawObstacle(){
			m_obstacleGrid.DebugShowObstacleGrid();
		}

        public int GetDistance(int index1,int index2)
        {
            int result;
            Vector2 point1 = new Vector2(GetRow(index1),GetColumn(index1));
            Vector2 point2 = new Vector2(GetRow(index2),GetColumn(index2));
            result = (int)Vector2.Distance(point1, point2);
            return result;
        }
		#endregion

		#region interface function
		
		public int GetNeighbors(int index, ref int[] neighbors)
		{
			//初始化节点数
			neighbors = new int[(int)eNeighborDirection.kNumNeighbors];

			for (int i = 0; i < (int)eNeighborDirection.kNumNeighbors; i++)
			{
				neighbors[i] = GetNeighbor(index, (eNeighborDirection)i);
			}
			
			return (int)eNeighborDirection.kNumNeighbors;
		}
		
		public int GetNumNodes()
		{
			return NumberOfCells;
		}

		public float GetHCost(int startIndex, int goalIndex)
		{
			Vector3 startPos = GetPathNodePos(startIndex);
			Vector3 goalPos = GetPathNodePos(goalIndex);
			float heuristicWeight = 2.0f;
			float cost = heuristicWeight * Vector3.Distance(startPos, goalPos);
			// Give extra cost to height difference
			cost = cost + Mathf.Abs(goalPos.y - startPos.y) * 1.0f;
			return cost;
		}

		//计算从起点到目标点寻路的消耗
		public float GetGCost(int startNodeIndex, int destNodeIndex)
		{
			Vector3 startPos = GetPathNodePos(startNodeIndex);
			Vector3 goalPos = GetPathNodePos(destNodeIndex);
			float cost = Vector3.Distance(startPos, goalPos);
			return cost;
		}

		public bool IsNodeBlocked(int index)
		{
			return IsBlocked(index);
		}

		public int GetPathNodeIndex(Vector3 pos)
		{
			int index = GetCellIndex(pos);

			if(!IsInBounds(index)){
				index = NNode.kInvalidIndex;
			}

			return index;
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
			// Save this value off, in case we need to use it to search for a valid location further along in this function.
			Vector3 originalPosition = position;
			
			// Find a position that is within the grid, at the same height as the grid.
			float padding = m_cellSize / 4.0f;
			Bounds gridBounds = GetGridBounds();
			position.x = Mathf.Clamp(position.x, gridBounds.min.x + padding, gridBounds.max.x - padding);
			position.y = Origin.y;
			position.z = Mathf.Clamp(position.z, gridBounds.min.z + padding, gridBounds.max.z - padding);
			
			// If this position is blocked, then look at all of the neighbors of this cell, and see if one of those cells is
			// unblocked. If one of those neighbors is unblocked, then we return the position of that neighbor, to ensure that 
			// the agent is always pathing to and from valid positions.
			int cellIndex = GetCellIndex(position);
			if ( IsBlocked(cellIndex) )
			{
				// Find the closest unblocked neighbor, if one exists.
				int[] neighbors = null;
				int numNeighbors = GetNeighbors(cellIndex, ref neighbors);
				float closestDistSq = float.MaxValue;
				for ( int i = 0; i < numNeighbors; i++ )
				{
					int neighborIndex = neighbors[i];
					if ( !IsBlocked(neighborIndex) )
					{
						Vector3 neighborPos = GetCellCenter(neighborIndex);
						float distToCellSq = (neighborPos - originalPosition).sqrMagnitude;
						if ( distToCellSq < closestDistSq )
						{
							closestDistSq = distToCellSq;
							position = neighborPos;	
						}
					}
				}
			}
			
			return position;
		}
		
		public float GetTerrainHeight(Vector3 position)
		{
			/*
			if ( m_heightmap == null )
			{
				return Origin.y;	
			}
			else
			{
				return m_heightmap.SampleHeight(position);	
			}*/
			return 1.0f;
		}
		#endregion

        #region private function
        private void SetSolidityObstacleCell(int index)
        {
            if(index <= 0)
            {
                Debug.LogError("the index is faild data!");
                return;
            }

            int col = index / GameDefine.NumberOfColumns;
            int row = index % GameDefine.NumberOfRows;

            SolidList[col, row] = true;
        }
        #endregion
        #region public function
        public eNeighborDirection GetNeighborDirection(int index, int neighborIndex)
		{
			for (int i = 0; i < (int)eNeighborDirection.kNumNeighbors; i++)
			{
				int testNeighborIndex = GetNeighbor(index, (eNeighborDirection)i);
				if ( testNeighborIndex	== neighborIndex )
				{
					return (eNeighborDirection)i;
				}
			}
			
			return eNeighborDirection.kNoNeighbor;
		}
		
		private int GetNeighbor(int index, eNeighborDirection neighborDirection)
		{
			Vector3 neighborPos = GetCellCenter(index);
			
			switch (neighborDirection)
			{
			case eNeighborDirection.kLeft:
				neighborPos.x -= m_cellSize;
				break;
			case eNeighborDirection.kTop:
				neighborPos.z += m_cellSize;
				break;
			case eNeighborDirection.kRight:
				neighborPos.x += m_cellSize;
				break;
			case eNeighborDirection.kBottom:
				neighborPos.z -= m_cellSize;
				break;
			default:
				System.Diagnostics.Debug.Assert(false);
				break;
			};
			
			int neighborIndex = GetCellIndex(neighborPos);
			if ( !IsInBounds(neighborIndex) )
			{
				neighborIndex = (int)eNeighborDirection.kNoNeighbor;
			}
			
			return neighborIndex;
		}
		#endregion
	}

	/// <summary>
	/// Given a list of points, this class computes portals on a grid terrain. Portals are used for path smoothing. 
	/// </summary>
	public static class GridPortalComputer
	{
		public static void ComputePortals(Vector3[] roughPath, NPathGrid grid, out Vector3[] aLeftEndPts, out Vector3[] aRightEndPts)
		{
			aLeftEndPts = null;
			aRightEndPts = null;
			
			if ( roughPath.Length < 2 )
			{
				return;
			}
			
			aLeftEndPts = new Vector3[roughPath.Length-1];
			aRightEndPts = new Vector3[roughPath.Length-1];
			
			for ( int i = 0; i < roughPath.Length - 1; i++ )
			{
				Vector3 currentPos = roughPath[i];
				Vector3 nextPos = roughPath[i+1];
				Vector3 leftPoint, rightPoint;
				ComputePortal(currentPos, nextPos, grid, out leftPoint, out rightPoint);
				aLeftEndPts[i] = leftPoint;
				aRightEndPts[i] = rightPoint;
			}
		}
		
		private static void ComputePortal(Vector3 startPos, Vector3 endPos, NPathGrid grid, out Vector3 leftPoint, out Vector3 rightPoint)
		{
			leftPoint = Vector3.zero;
			rightPoint = Vector3.zero;
			
			int startCellIndex = grid.GetCellIndex(startPos);
			int endCellIndex = grid.GetCellIndex(endPos);
			
			Bounds startCellBounds = grid.GetCellBounds(startCellIndex);
			NPathGrid.eNeighborDirection neighborDirection = grid.GetNeighborDirection(startCellIndex, endCellIndex);
			switch (neighborDirection)
			{
			case NPathGrid.eNeighborDirection.kTop:
				leftPoint = new Vector3(startCellBounds.min.x, grid.Origin.y, startCellBounds.max.z);
				rightPoint = new Vector3(startCellBounds.max.x, grid.Origin.y, startCellBounds.max.z);
				break;
			case NPathGrid.eNeighborDirection.kRight:
				leftPoint = new Vector3(startCellBounds.max.x, grid.Origin.y, startCellBounds.max.z);
				rightPoint = new Vector3(startCellBounds.max.x, grid.Origin.y, startCellBounds.min.z);
				break;
			case NPathGrid.eNeighborDirection.kBottom:
				leftPoint = new Vector3(startCellBounds.max.x, grid.Origin.y, startCellBounds.min.z);
				rightPoint = new Vector3(startCellBounds.min.x, grid.Origin.y, startCellBounds.min.z);
				break;
			case NPathGrid.eNeighborDirection.kLeft:
				leftPoint = new Vector3(startCellBounds.min.x, grid.Origin.y, startCellBounds.min.z);
				rightPoint = new Vector3(startCellBounds.min.x, grid.Origin.y, startCellBounds.max.z);
				break;
			default:
				UnityEngine.Debug.LogError("ComputePortal failed to find a neighbor");
				break;
			};
		}
	};
}
