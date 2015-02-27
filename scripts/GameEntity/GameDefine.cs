using UnityEngine;
using System.Collections;
using AppUtility;

namespace GameEntity{
	class GameDefine : Singleton<GameDefine> {
		#region sceneid
		public static readonly int 			FightSceneID = 1;
		#endregion

		#region pathGridConfig
		public static readonly int 			NumberOfRows = 100;
		public static readonly int 			NumberOfColumns = 100;
		public static readonly float 		CellSize = 0.5f;
		#endregion
	}
}