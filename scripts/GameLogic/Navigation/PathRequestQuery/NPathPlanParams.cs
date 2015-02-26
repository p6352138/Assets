using UnityEngine;
using System.Collections;

namespace GameLogic.Navigation{

	public class NPathPlanParams {
		#region Fileds
		Vector3         m_startPos;
		Vector3         m_goalPos;
		NINavTarget      m_target;           // ex: a position, or a GameObject.
		float           m_replanInterval;   // number of seconds between each replan
		#endregion

		#region Properties
		public Vector3 StartPos 
		{
			get { return m_startPos; }
		}
		
		public Vector3 GoalPos
		{
			get { return m_goalPos; }
		}
		
		public float ReplanInterval
		{
			get { return m_replanInterval; }
		}
		#endregion

		public NPathPlanParams(Vector3 startPos, NINavTarget target, float replanInterval)
		{
			System.Diagnostics.Debug.Assert(replanInterval > 0.0f);
			m_startPos = startPos;
			m_target = target;
			m_goalPos = target.GetNavTargetPosition();
			m_replanInterval = replanInterval;
		}

		public void UpdateStartAndGoalPos(Vector3 newStartPos)
		{
			m_startPos = newStartPos;
			m_goalPos = m_target.GetNavTargetPosition();
		}
	}
}
