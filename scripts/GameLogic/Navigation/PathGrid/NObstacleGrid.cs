using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameLogic.Navigation{
	public class NObstacleGrid {

		#region private Properties
		private List<Bounds> m_boundslist;
		private List<NObstacle> m_Obstaclelist;
		#endregion

		#region public function
		public void Init(List<Bounds> boundslist)
		{
			m_boundslist = boundslist;
			m_Obstaclelist = new List<NObstacle>();

			foreach(Bounds bound in boundslist){
				NObstacle temp = new NObstacle();
				temp.InitObstacle(bound);
				m_Obstaclelist.Add(temp);
				temp = null;
			}
		}

		public void DebugShowObstacleGrid(){
			foreach(NObstacle ob in m_Obstaclelist){
				ob.Draw();
			}
		}
		#endregion
	}
}
