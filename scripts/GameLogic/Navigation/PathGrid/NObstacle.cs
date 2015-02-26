using UnityEngine;
using System.Collections;
using GameEntity;

namespace GameLogic.Navigation{
	public class NObstacle {
		#region private Fields
		private int[] m_obstructedCellPool;
		private int m_numObstructedCellPoolRows;
		private int m_numObstructedCellPoolColumns;
		private Bounds m_bounds;

		private Vector3 upperLeftPos;
		private Vector3 upperRightPos;
		private Vector3 lowerLeftPos;
		private Vector3 lowerRightPos;
		
		private Vector3 horizDir;
		private Vector3 vertDir;
		private float horizLength;
		private float vertLength;

		private NPathGrid m_NGrid
		{
			get{return NavigationMgr.GetInstance().GetGrid();}
		}
		#endregion

		#region public function
		//Init obstacle data...
		public void InitObstacle(Bounds bounds){
			m_bounds = bounds;

			UpdateObstructedCellsPool();
		}

		public void Draw()
		{
			//calculation Obstacle bounds position in world coordtion
			Vector3 upperLeftPos = new Vector3(m_bounds.min.x,m_NGrid.Origin.y,m_bounds.max.z);
			Vector3 upperRightPos = new Vector3(m_bounds.max.x,m_NGrid.Origin.y,m_bounds.max.z);
			Vector3 lowerLeftPos = new Vector3(m_bounds.min.x,m_NGrid.Origin.y,m_bounds.min.z);
			Vector3 lowerRightPos = new Vector3(m_bounds.max.x,m_NGrid.Origin.y,m_bounds.min.z);
			
			Vector3 horizDir = (upperRightPos - upperLeftPos).normalized;
			Vector3 vertDir = (upperLeftPos - lowerLeftPos).normalized;
			float horizLength = m_bounds.size.x;
			float vertLength = m_bounds.size.z;
			
			//debug show Obstacle in pathgrid
			Color red = new Color(1,0,0);
			Debug.DrawLine(upperLeftPos, upperRightPos,red);
			Debug.DrawLine(upperRightPos, lowerRightPos,red);
			Debug.DrawLine(lowerRightPos, lowerLeftPos,red);
			Debug.DrawLine(lowerLeftPos, upperLeftPos,red);
		}

		public int[] GetObstructedCells(out int numObstructedCells)
		{
			numObstructedCells = 0;

			// Determine which cells are actually obstructed
			for ( int rowCount = 0; rowCount < m_numObstructedCellPoolRows; rowCount++ )
			{
				float currentVertLength = rowCount * GameDefine.CellSize;
				
				for ( int colCount = 0; colCount < m_numObstructedCellPoolColumns; colCount++ )
				{
					float currentHorizLength = colCount * GameDefine.CellSize;
					Vector3 testPos = lowerLeftPos + horizDir * currentHorizLength + vertDir * currentVertLength;
					testPos.x = Mathf.Clamp(testPos.x, m_bounds.min.x, m_bounds.max.x);
					testPos.z = Mathf.Clamp(testPos.z, m_bounds.min.z, m_bounds.max.z);
					if (NavigationMgr.GetInstance().GetGrid().IsInBounds(testPos) )
					{
						int obstructedCellIndex = NavigationMgr.GetInstance().GetGrid().GetCellIndex(testPos);
						m_obstructedCellPool[numObstructedCells] = obstructedCellIndex;
						numObstructedCells++;
					}
					
					if ( currentHorizLength > horizLength )
					{
						break;
					}
				}
				
				if ( currentVertLength > vertLength )
				{
					break;
				}
			}

			return m_obstructedCellPool;
		}
		#endregion

		#region private function
		//compute the max num of obstruct cells on object's collider
		private void UpdateObstructedCellsPool(){
			//save bounds's position data in coordtion
			upperLeftPos = new Vector3(m_bounds.min.x,m_NGrid.Origin.y,m_bounds.max.z);
			upperRightPos = new Vector3(m_bounds.max.x,m_NGrid.Origin.y,m_bounds.max.z);
			lowerLeftPos = new Vector3(m_bounds.min.x,m_NGrid.Origin.y,m_bounds.min.z);
			lowerRightPos = new Vector3(m_bounds.max.x,m_NGrid.Origin.y,m_bounds.min.z);
			
			horizDir = (upperRightPos - upperLeftPos).normalized;
			vertDir = (upperLeftPos - lowerLeftPos).normalized;
			horizLength = m_bounds.size.x;
			vertLength = m_bounds.size.z;

			//compute the max num of obstruct cells on object's collider
			int maxNumObstructedRows = (int)( m_bounds.size.z / GameDefine.CellSize ) + 2;
			int maxNumObstructedCols = (int)( m_bounds.size.x / GameDefine.CellSize ) + 2;
			int maxNumObstructedCells = maxNumObstructedRows * maxNumObstructedCols;
			if ( m_obstructedCellPool == null || (maxNumObstructedCells != m_obstructedCellPool.Length) )
			{
				m_obstructedCellPool = new int[maxNumObstructedCells];	
				m_numObstructedCellPoolRows = maxNumObstructedRows;
				m_numObstructedCellPoolColumns = maxNumObstructedCols;
			}
			
			// Clear the contents of the pool
			for ( int i = 0; i < m_obstructedCellPool.Length; i++ )
			{
				m_obstructedCellPool[i] = -1;	
			}
		}
		#endregion
	}
}