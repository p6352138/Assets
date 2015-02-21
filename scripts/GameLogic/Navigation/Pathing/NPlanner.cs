using UnityEngine;
using System.Collections;

namespace GameLogic.Navigation{
	public abstract class NPlanner {
		public enum ePlanStatus
		{
			kInvalid = -1,
			kPlanning,
			kPlanSucceeded,
			kPlanFailed
		};

		#region Fields
		protected ePlanStatus 		m_planStatus;
		protected int 				m_maxNumberOfNodes;
		private NIPlanningWorld 		m_world;
		#endregion
	}
}
