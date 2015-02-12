using UnityEngine;
using System.Collections;

namespace GameLogic.Navigation
{
	public class NPathAgent : NIPathAgent {

		#region Editor fields
		public bool m_debugShowPath = false;
		#endregion

		#region Fields
		private NIPathRequestQuery m_query;
		private bool 			  m_bInitialized;
		#endregion

		#region interface function
		public Vector3 GetPathAgentFootPos()
		{
			NavigationMgr.GetInstance().
		}

		public void OnPathAgentRequestSucceeded(NIPathRequestQuery request)
		{

		}

		public void OnPathAgentRequestFailed()
		{

		}
		#endregion
	}
}
