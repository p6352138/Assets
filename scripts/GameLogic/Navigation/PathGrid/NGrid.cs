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
		public static float Width
		{
			get{return m_Rows*m_cellSize;}
		}

		public static float Height
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

		public virtual void Awake(){
			m_Rows = GameDefine.NumberOfRows;
			m_Columns = GameDefine.NumberOfColumns;
			m_cellSize = GameDefine.CellSize;
		}

		public static void DebugDraw(Vector3 origin)
		{
			m_origin = origin;
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
