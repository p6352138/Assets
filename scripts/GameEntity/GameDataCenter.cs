using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AppUtility;

namespace GameLogic{
	class GameDataCenter : Singleton<GameDataCenter> {
		public int SocketReset = 0;
		public long heartTime = 5000;
	}
}

