using UnityEngine;
using System.Collections;

namespace GameEntity{
	public class CPlayer : CCearcue {

		#region private Fields
		private int m_id;
		private GameObject m_go;
		#endregion

		public CPlayer(int id,GameObject go)
		{
			m_id = id;
			m_go = go;
		}
	}
}
