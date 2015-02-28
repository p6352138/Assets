using UnityEngine;
using System.Collections;
using GameEntity;
using AppUtility;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GameLogic.Navigation{ 
	class NavigationMgr : Singleton<NavigationMgr> {
		#region private Fields
		private NPathGrid m_grid;
		private NPathAgent m_agent;
		#endregion

		#region Init data
		public void init(Vector3 origin){
			m_grid = new NPathGrid();
            m_grid.Awake(origin);
		}

        public void InitPathData(List<Bounds> bounds)
		{
			m_grid.InitObstacleData(bounds);
		}
		#endregion

		#region showDebug
		public void showGrid(){
			m_grid.DebugDraw();
		}

		public void showObstacleGrid(){
			m_grid.DrawObstacle();
		}
		#endregion

		#region public function
		public NPathGrid GetGrid(){
			return m_grid;
		}

		public void ImportMapData(){
            m_grid.InitSolidityData();

			string txt = "";
			for(int i=0;i<GameDefine.NumberOfColumns;i++)
			{
				for(int j=0;j<GameDefine.NumberOfRows;j++)
				{
					int result = 0;
					if(m_grid.SolidList[i,j]){
						result = 1;
					}
					else{
						result = 0;
					}

					if(j == GameDefine.NumberOfRows - 1){

						txt += result.ToString()+"\n";
					}
					else{
						txt += result.ToString()+",";
					}
				}
			}

			if (!File.Exists (Application.dataPath + "/MapData.txt")) {
				FileStream fs = File.Create (Application.dataPath + "/MapData.txt");
			}

			StreamWriter sw = new StreamWriter (Application.dataPath + "/MapData.txt");
			sw.Write(txt);
			sw.Flush();
			sw.Close();

            Debug.LogWarning("map sucess!!!~~~~~~~");
		}
		#endregion
	}
}
