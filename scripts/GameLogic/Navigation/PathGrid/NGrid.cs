using UnityEngine;
using System.Collections;
using GameEntity;

namespace GameLogic.Navigation{
	public class NGrid  {
		#region Contants
		protected static readonly Vector3 XAxis;
		protected static readonly Vector3 ZAxis;
		private static readonly float Depth;
		#endregion

		#region Fields
		protected static int m_Rows;
		protected static int m_Columns;
		protected static float m_cellSize;
		private static Vector3 m_origin;
		#endregion

		#region Properties
		public float Width
		{
			get{return m_Rows*m_cellSize;}
		}

		public float Height
		{
			get{return m_Columns*m_cellSize;}
		}

		public int NumberOfCells
		{
			get { return m_Rows * m_Columns; }
		}

		public Vector3 Origin
		{
			get{return m_origin;}
		}

		public float Left
		{
			get{return Origin.x;}
		}

		public float Right
		{
			get{return Origin.x + Width;}
		}

		public float Top
		{
			get{return Origin.z + Height;}
		}

		public float Bottom
		{
			get{return Origin.z;}
		}
		#endregion

		static NGrid(){
			XAxis = new Vector3(1,0,0);
			ZAxis = new Vector3(0,0,1);
			Depth = 1.0f;
		}

		public virtual void Awake(Vector3 origin){
            m_origin = origin;
			m_Rows = GameDefine.NumberOfRows;
			m_Columns = GameDefine.NumberOfColumns;
			m_cellSize = GameDefine.CellSize;
		}

		// Update is called once per frame
		public virtual void Update () 
		{
			;
		}
		
		public virtual void OnDrawGizmos()
		{
			;
		}

		public void DebugDraw()
		{
			Vector3 startPos,endPos;

			for(int i=0;i<m_Rows+1;i++){
				startPos = m_origin + i * XAxis * m_cellSize;
				endPos = startPos + ZAxis * Width;
				Debug.DrawLine(startPos,endPos);
			}

			for(int i=0;i<m_Columns+1;i++){
				startPos = m_origin + i * ZAxis * m_cellSize;
				endPos = startPos + XAxis * Height;
				Debug.DrawLine(startPos,endPos);
			}
		}

		/// <summary>
		/// 获取当前位置位于哪一个网格
		/// </summary>
		/// <returns>The nearest cell center.</returns>
		/// <param name="pos">Position.</param>
		public Vector3 GetNearestCellCenter(Vector3 pos)
		{
			int index = GetCellIndex(pos);
			Vector3 cellPos = GetCellPosition( index );
			cellPos.x += ( m_cellSize / 2.0f );
			cellPos.z += ( m_cellSize / 2.0f );
			return cellPos;
		}

		// 获得某一个网格的中心点位置
		public Vector3 GetCellCenter(int index)
		{
			Vector3 cellPosition = GetCellPosition(index);	
			cellPosition.x += ( m_cellSize / 2.0f );
			cellPosition.z += ( m_cellSize / 2.0f );
			return cellPosition;
		}

		// pass in world space coords. Get the tile index at the passed position
		public int GetCellIndex(Vector3 pos)
		{
			if ( !IsInBounds(pos) )
			{
				return SimpleAI.Planning.Node.kInvalidIndex;	
			}
			
			pos -= Origin;
			
			int col = (int)(pos.x / m_cellSize);
			int row = (int)(pos.z / m_cellSize);
			
			return (row * m_Columns + col);
		}

		public int GetCellIndex(int col,int row)
		{
			return (row * m_Columns + col);
		}
		/// <summary>
		/// Returns the lower left position of the grid cell at the passed tile index. The origin of the grid is at the lower left,
		/// so it uses a cartesian coordinate system.
		/// </summary>
		/// <param name="index">index to the grid cell to consider</param>
		/// <returns>Lower left position of the grid cell (origin position of the grid cell), in world space coordinates</returns>
		public Vector3 GetCellPosition(int index)
		{
			int row = GetRow(index);
			int col = GetColumn(index);
			float x = col * m_cellSize;
			float z = row * m_cellSize;
			Vector3 cellPosition = Origin + new Vector3(x, 0.0f, z);
			return cellPosition;
		}

		// pass in world space coords. Get the tile index at the passed position, clamped to be within the grid.
		public int GetCellIndexClamped(Vector3 pos)
		{
			pos -= Origin;
			
			int col = (int)(pos.x / m_cellSize);
			int row = (int)(pos.z / m_cellSize);
			
			//make sure the position is in range.
			col = (int)Mathf.Clamp(col, 0, m_Columns - 1);
			row = (int)Mathf.Clamp(row, 0, m_Rows - 1);
			
			return (row * m_Columns + col);
		}
		
		public Bounds GetCellBounds(int index)
		{
			Vector3 cellCenterPos = GetCellPosition(index);
			cellCenterPos.x += ( m_cellSize / 2.0f );
			cellCenterPos.z += ( m_cellSize / 2.0f );
			Bounds cellBounds = new Bounds(cellCenterPos, new Vector3(m_cellSize, Depth, m_cellSize));
			return cellBounds;
		}
		
		public Bounds GetGridBounds()
		{
			Vector3 gridCenter = Origin + (Width / 2.0f) * XAxis + (Height / 2.0f) * ZAxis;
			Bounds gridBounds = new Bounds(gridCenter, new Vector3(Width, Depth, Height));
			return gridBounds;
		}
		
		public int GetRow(int index)
		{
			int row = index / m_Columns;
			return row;
		}
		
		public int GetColumn(int index)
		{
			int col = index % m_Columns;
			return col;
		}

		#region 判断是否在路点网格内
		public bool IsInBounds(int col,int row){
			if(col < 0 || col >= m_Columns){
				return false;
			}
			else if(row < 0 || row >= m_Rows){
				return false;
			}
			else{
				return true;
			}
		}

		public bool IsInBounds(int index){
			return ( index >= 0 && index < NumberOfCells );
		}

		// pass in world space coords
		public bool IsInBounds(Vector3 pos)
		{
			return ( pos.x >= Left &&
			        pos.x <= Right &&
			        pos.z <= Top &&
			        pos.z >= Bottom );
		}
		#endregion
	}
}
