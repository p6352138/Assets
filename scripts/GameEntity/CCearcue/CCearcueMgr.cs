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
        private List<CCreature> m_monsterEntity;
		#endregion

        #region public Fields
        public GameObject testTerrian;
        public GameObject testPlayer;
        public GameObject testMonster1;
        public GameObject testMonster2;
        public GameObject testMonster3;
        public GameObject testMonster4;

        public CPlayer player
        {
            get { return m_curPlayer; }
        }
        #endregion

        #region public function
		public void Init(){
			m_allEntity = new List<CCreature> ();
            m_monsterEntity = new List<CCreature>();
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
                m_curPlayer = new CPlayer(id, testPlayer);
                m_allEntity.Add(m_curPlayer);
			}
			else if(type == CCearcueType.Terrian){
				m_curTerrian = new CTerrian(id,go);
            }
            else if (type == CCearcueType.Monster)
            {
                CMonster monster = new CMonster(id, testMonster1);
                if (id == 1001)
                {
                    monster = new CMonster(id, testMonster1);
                }
                else if (id == 1002)
                {
                    monster = new CMonster(id, testMonster2);
                }
                else if (id == 1003)
                {
                    monster = new CMonster(id, testMonster3);
                }
                else if (id == 1004)
                {
                    monster = new CMonster(id, testMonster4);
                }
               
                m_monsterEntity.Add(monster);
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

            foreach (CCreature item in m_monsterEntity)
                item.Update(deltaTime);
		}
		#endregion
	}
}
