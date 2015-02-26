using UnityEngine;
using System.Collections;

namespace GameLogic.Navigation{
	public abstract class NPlanner {
		/// <summary>
		/// 寻路状态
		/// </summary>
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
		private NIPlanningWorld 	m_world;
		#endregion

		#region Properties
		protected NIPlanningWorld World
		{
			get { return m_world; }
		}
		#endregion

		public NPlanner()
		{
			m_planStatus = ePlanStatus.kInvalid;
			m_maxNumberOfNodes = 0;
		}

		public virtual void Awake(int maxNumberOfNodes)
		{
			m_maxNumberOfNodes = maxNumberOfNodes;
		}
		
		public virtual void Start(NIPlanningWorld world)
		{
			m_world = world;
		}

		/// <summary>
		/// Update the planner by one step
		/// </summary>
		/// <param name="numCyclesToConsume">Maximum number of planning cycles the planner can consume</param>
		/// <returns>The number of planning cycles that were actually consumed</returns>
		public virtual int Update(int numCyclesToConsume)
		{
			return 0;
		}
		
		public virtual void OnDrawGizmos()
		{
			
		}
		
		public bool HasPlanSucceeded()
		{
			return (m_planStatus == ePlanStatus.kPlanSucceeded);
		}
		
		public bool HasPlanFailed()
		{
			return (m_planStatus == ePlanStatus.kPlanFailed);
		}
		
		public bool HasPlanCompleted()
		{
			return (m_planStatus == ePlanStatus.kPlanFailed || m_planStatus == ePlanStatus.kPlanSucceeded);
		}
	}
}
