using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AppUtility;

namespace GameEntity{
	class CCearcueMgr : Singleton<CCearcueMgr> {

		#region Fields
		private CTerrian m_curTerrian;
		private CPlayer m_curPlayer;

		private List<CCreature> m_allEntity;
		#endregion

		#region public function

		public GameObject testTerrian;
		public GameObject testPlayer;

		public void Init(){
			m_allEntity = new List<CCreature> ();
		}

		public void setTerrian(GameObject ob)
		{
			testTerrian = ob;
		}

		public void setPlayer(GameObject ob)
		{
			testPlayer = ob;
		}

		public void CreateCearcue(int id,CCearcueType type)
		{
			//load prefab resoure
			GameObject go = testTerrian;

			if(type == CCearcueType.Player){
				m_curPlayer = new CPlayer(id,testPlayer);
				m_allEntity.Add(m_curPlayer);
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

		public void Update(float deltaTime)
		{
			foreach (CCreature item in m_allEntity)
				item.Update (deltaTime);
		}
		#endregion
	}
}
