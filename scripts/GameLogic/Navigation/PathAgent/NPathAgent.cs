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

		#region public function
		public Vector3 GetPositon()
		{
			Vector3 result = new Vector3(0,0,0);
			return result;
		}

		#endregion

		#region interface function
		public Vector3 GetPathAgentFootPos()
		{
			return NavigationMgr.GetInstance().GetGrid().GetValidPathFloorPos(GetPositon());
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
