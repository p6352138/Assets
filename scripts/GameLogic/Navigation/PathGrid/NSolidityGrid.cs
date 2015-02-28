using UnityEngine;
using System.Collections;
using GameEntity;
using System;

namespace GameLogic.Navigation{
	public class NSolidityGrid : NGrid {

		#region Fields
		private bool[,] m_solidList;
		#endregion

		#region Properties
		public bool[,] SolidList{get{return m_solidList;}}
		#endregion

		public NSolidityGrid()
		{
		}

		public override void Awake (Vector3 origin)
		{
            base.Awake(origin);
			m_solidList = new bool[GameDefine.NumberOfColumns,GameDefine.NumberOfRows];

			for (int i =0; i<GameDefine.NumberOfColumns; i++) {
				for (int j =0; j<GameDefine.NumberOfRows; j++) {
					m_solidList[i,j] = false;
				}
			}
		}

		public void SetSolidity(bool[,] solidityList)
		{
			m_solidList = (bool[,])solidityList.Clone();
		}
		
		public void SetSolidity(int cellIndex, bool bSolid)
		{
			if ( !IsInBounds(cellIndex) )
			{
				return;
			}
			
			int col = GetColumn(cellIndex);
			int row = GetRow(cellIndex);
			m_solidList[col, row] = bSolid;
		}
		
		public void SetSolidity(Vector3 cellPos, bool bSolid)
		{
			int cellIndex = GetCellIndex(cellPos);	
			SetSolidity(cellIndex, bSolid);
		}
		
		/// <summary>
		///Fast 2D Raycast traversal on a grid.
		///Algorithm:   http://www.cse.yorku.ca/~amana/research/grid.pdf
		/// </summary>
		/// <param name="ray">
		///The ray being cast
		/// </param>
		/// <param name="isectPt">
		///Returns the position where the ray intersets with an obstruction, or goes off the grid
		/// </param>
		public void Raycast2D(Ray ray, out Vector3 isectPt)
		{
			//init: gather starting location information.
			isectPt = new Vector3(0.0f, 0.0f, 0.0f);
			int startCell = GetCellIndex(ray.origin);
			bool bInBounds = ( startCell >= 0 && startCell < NumberOfCells );
			System.Diagnostics.Debug.Assert(bInBounds, "starting position of the ray is not in bounds in call to Raycast2D" +
			                                "Add logic to find the starting cell when the ray position starts out of bounds");
			if (!bInBounds)
			{
				return;
			}
			int X = GetColumn(startCell);
			int Y = GetRow(startCell);
			int stepX = Math.Sign(ray.direction.x);
			int stepY = Math.Sign(ray.direction.y);
			Vector3 startCellPos = GetCellPosition(startCell);
			float nearestGridX = (stepX < 0) ? (startCellPos.x) : (startCellPos.x + m_cellSize);
			float nearestGridY = (stepY < 0) ? (startCellPos.z - m_cellSize) : (startCellPos.z);
			float thetaInDegrees = Vector3.Angle( XAxis, ray.direction );
			float thetaInRadians = thetaInDegrees * Mathf.Deg2Rad;
			float cosTheta = Mathf.Cos( thetaInRadians );
			float sinTheta = Mathf.Sin( thetaInRadians );
			//parametric form requires taking ray.Position as the origin, hence the " - ray.Position" 
			float tMaxX = Math.Abs((nearestGridX - ray.origin.x) / cosTheta);
			float tMaxY = Math.Abs((nearestGridY - ray.origin.y) / sinTheta);
			float tDeltaX = Math.Abs(m_cellSize / cosTheta);
			float tDeltaY = Math.Abs(m_cellSize / sinTheta);
			
			//loop: traverse the cells until there is a collision or ray is out of bounds.
			bool bCollided = false;
			bool bHitMapEdge = false;
			int prevX = X;
			int prevY = Y;
			int endCell = -1;
			while (!bCollided)
			{
				if (tMaxX < tMaxY)
				{
					prevX = X;
					tMaxX += tDeltaX;
					X += stepX;
				}
				else
				{
					prevY = Y;
					tMaxY += tDeltaY;
					Y += stepY;
				}
				
				if (!IsInBounds(X, Y))
				{
					endCell = GetCellIndex( new Vector3(prevY, prevX, 0.0f) );
					bCollided = true;
					bHitMapEdge = true;
				}
				else if (m_solidList[X, Y])
				{
					endCell = GetCellIndex( new Vector3(Y, X, 0.0f) );
					bCollided = true;
				}
			}
			
			bool bIntersectionFound = false;
			if (bHitMapEdge)
			{
				Bounds gridBounds = GetGridBounds();
				float isectDist = 0.0f;
				bIntersectionFound = gridBounds.IntersectRay(ray, out isectDist);
				if (bIntersectionFound)
				{
					isectPt = ray.GetPoint(isectDist);	
				}
			}
			else
			{
				Bounds cellBounds = GetCellBounds(endCell);
				float isectDist = 0.0f;
				bIntersectionFound = cellBounds.IntersectRay(ray, out isectDist);
				if (bIntersectionFound)
				{
					isectPt = ray.GetPoint(isectDist);	
				}
			}
			System.Diagnostics.Debug.Assert(bIntersectionFound, "Raycast2D Intersection should always be found");
		}
		
		// Determine if the position is blocked by collision
		public bool IsBlocked(Vector3 pos)
		{
			int cellIndex = GetCellIndex(pos);
			bool bInBounds = IsInBounds(cellIndex);
			if (!bInBounds)
			{
				return true;
			}
			
			int col = GetColumn(cellIndex);
			int row = GetRow(cellIndex);
			return m_solidList[col, row];
		}
		
		public bool IsBlocked(int index)
		{
			int row = GetRow(index);
			int col = GetColumn(index);
			if ( !IsInBounds(col, row) )
			{
				return true;
			}
			
			return m_solidList[col, row];
		}
	}
}
