using UnityEngine;
using System.Collections;
using GameEntity;

namespace GameLogic.Navigation{
	public class NSolidityGrid : NGrid {

		#region Fields
		private bool[,] m_solidList;
		#endregion

		public NSolidityGrid()
		{
		}

		public override void Awake ()
		{
			base.Awake ();
			m_solidList = new bool[GameDefine.NumberOfColumns,GameDefine.NumberOfRows];

			for (int i =0; i<GameDefine.NumberOfColumns; i++) {
				for (int j =0; j<GameDefine.NumberOfRows; j++) {
					m_solidList[i,j] = false;
				}
			}
		}


	}
}
