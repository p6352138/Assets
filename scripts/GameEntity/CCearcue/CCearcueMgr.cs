﻿using UnityEngine;
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

		public GameObject testTerrian;
		public GameObject testPlayer;

		public void setTerrian(GameObject ob)
		{
			testTerrian = ob;
		}

		public void CreateCearcue(int id,CCearcueType type)
		{
			//load prefab resoure
			GameObject go = testTerrian;

			if(type == CCearcueType.Player){
				m_curPlayer = new CPlayer(id,testPlayer);
			}
			else if(type == CCearcueType.Terrian){
				m_curTerrian = new CTerrian(id,go);
			}
		}

		public Vector3 GetTerrainPosition(){
			return m_curTerrian.GetTerrianPosion();
		}

		public List<Bounds> GetTerrainBounds(){
			return m_curTerrian.GetTerrianBounds();
		}
		#endregion
	}
}
