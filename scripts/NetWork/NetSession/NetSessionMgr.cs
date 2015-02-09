using UnityEngine;
using System.Collections;
using AppUtility;
using NetWork.NetModule;

namespace NetWork.NetSession
{
	class NetSessionMgr : Singleton<NetSessionMgr>
	{
		#region netImpl
		private NetSessionImpl m_NetImpl;
		#endregion

		public void Init(){
			NetModuleMgr.GetInstance().Init();
			m_NetImpl.Init();
		}
	}
}
