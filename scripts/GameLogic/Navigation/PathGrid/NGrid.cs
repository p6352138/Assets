using UnityEngine;
using System.Collections;

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
		#endregion

		static NGrid(){
			XAxis = new Vector3(1,0,0);
			ZAxis = new Vector3(0,0,1);
			Depth = 1.0f;
		}

		public static void DebugDraw()
		{
			Vector3 startPos,endPos;

			for(int i=0;i<m_Rows+1;i++){
				startPos = m_origin + i * XAxis * m_cellSize;
				endPos = startPos + i * ZAxis * Width;
				Debug.DrawLine(startPos,endPos);
			}

			for(int i=0;i<m_Columns+1;i++){
				startPos = m_origin + i * ZAxis * m_cellSize;
				endPos = startPos + i * XAxis * Height;
				Debug.DrawLine(startPos,endPos);
			}
		}
	}
}
