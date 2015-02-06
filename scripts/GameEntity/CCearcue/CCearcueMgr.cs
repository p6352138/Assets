using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AppUtility;

namespace GameEntity{
	class CCearcueMgr : Singleton<CCearcueMgr> {

		#region Fields
		private CTerrian m_curTerrian;
		private CPlayer m_curPlayer;
		#endregion

		#region public function
		/*
		protected CCearcueMgr(){
			m_curTerrian = new CTerrian();
			m_curPlayer = new CPlayer();
		}*/

		public static void CreateCearcue(CCearcueType type)
		{
			if(type == CCearcueType.Player){
			}
			else if(type == CCearcueType.Terrian){
			}
		}
		#endregion
	}
}
