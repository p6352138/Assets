using UnityEngine;
using System.Collections;
using GameEntity;

namespace GameLogic.Navigation{
	public class NSolidityGrid : NGrid {

		#region Fields
		private bool[,] m_solidList;
		#endregion

		public override void Awake ()
		{
			base.Awake ();
			m_solidList = new bool[GameDefine.NumberOfColumns,GameDefine.NumberOfRows];
		}
	}
}
