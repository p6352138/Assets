using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace GameLogic.Navigation{
	/// <summary>
	///The client making the request can ask for information about the request through this interface.
	/// </summary>
	public interface NIPathRequestQuery  {
		/// <summary>
		///Retrieve the path solution of the request
		/// </summary>
		/// <returns></returns>
		LinkedList<NNode> GetSolutionPath();
		
		/// <summary>
		///Retrieve the path solution of the request, as a list of points
		/// </summary>
		/// <param name="world">
		///PathTerrain where the request was made
		/// </param>
		/// <returns>
		/// A <see cref="Vector3[]"/>
		/// </returns>
		Vector3[] GetSolutionPath(NIPathTerrain world);
		
		/// <summary>
		///Get the start position of the path request 
		/// </summary>
		/// <returns>
		/// A <see cref="Vector3"/>
		/// </returns>
		Vector3 GetStartPos();
		
		/// <summary>
		///Get the goal position of the path request
		/// </summary>
		/// <returns>
		/// A <see cref="Vector3"/>
		/// </returns>
		Vector3 GetGoalPos();
		
		/// <summary>
		///Get the IPathAgent that originally made the path request 
		/// </summary>
		/// <returns>
		/// A <see cref="IPathAgent"/>
		/// </returns>
		NIPathAgent GetPathAgent();
		
		/// <summary>
		///Determine if the path request has completed (regardless of it if failed or succeeded) 
		/// </summary>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		bool HasCompleted();
		
		/// <summary>
		///Determine if the path request has successfully completed 
		/// </summary>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		bool HasSuccessfullyCompleted();
		
		/// <summary>
		///Determine if the path request has failed to find a solution 
		/// </summary>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		bool HasFailed();
	}

	public class NPathRequest : IComparable<NPathRequest>,NIPathRequestQuery
	{
		#region private Fields
		/// <summary>
		/// 优先级
		/// </summary>
		/// <value>The priority.</value>
		private int m_priority;
		private NPathPlanParams              m_pathPlanParams;
		private NPool<NPathPlanner>.NNode m_pathPlanner;
		private float                       m_replanTimeRemaining;      // The number of seconds that must elapse before we re-plan this path request.
		private NIPathAgent                  m_agent;
		#endregion

		#region public Fields
		/// <summary>
		/// 优先级
		/// </summary>
		/// <value>The priority.</value>
		public int Priority
		{
			get{return m_priority;}
		}

		public NPool<NPathPlanner>.NNode PathPlanner
		{
			get { return m_pathPlanner; }
		}
		
		public NPathPlanParams PlanParams
		{
			get { return m_pathPlanParams; }
		}
		
		public NIPathAgent Agent
		{
			get { return m_agent; }
		}
		#endregion

		#region Initialization
		public NPathRequest()
		{
			m_priority = 0;
			m_replanTimeRemaining = 0.0f;
		}
		
		public void Set(NPathPlanParams planParams, NPool<NPathPlanner>.NNode pathPlanner, NIPathAgent agent)
		{
			m_pathPlanParams = planParams;
			m_pathPlanner = pathPlanner;
			m_replanTimeRemaining = planParams.ReplanInterval;
			m_agent = agent;
		}
		#endregion

		public void Update(float deltaTimeInSeconds)
		{
			m_replanTimeRemaining -= Convert.ToSingle(deltaTimeInSeconds);
			m_replanTimeRemaining = Math.Max(m_replanTimeRemaining, 0.0f);
		}
		
		public bool IsReadyToReplan()
		{
			return (m_replanTimeRemaining <= 0.0f);
		}
		
		public void RestartReplanTimer()
		{
			m_replanTimeRemaining = m_pathPlanParams.ReplanInterval;
		}

		#region IComparable<Request> Members
		public int CompareTo(NPathRequest other)
		{
			if (m_priority > other.Priority)
			{
				return -1;
			}
			else if (m_priority < other.Priority)
			{
				return 1;
			}
			else
			{
				return 0;
			}
		}
		#endregion

		#region Interface function

		public LinkedList<NNode> GetSolutionPath()
		{
			return m_pathPlanner.Item.Solution;
		}
		

		public Vector3[] GetSolutionPath(NIPathTerrain terrain)
		{
			if (!HasSuccessfullyCompleted())
			{
				return null;
			}
			
			LinkedList<NNode> path = GetSolutionPath();
			
			if ( path == null || path.Count == 0 )
			{
				return null;
			}
			
			Vector3[] pathPoints = new Vector3[path.Count];
			int i = 0;
			foreach (NNode node in path)
			{
				Vector3 nodePos = terrain.GetPathNodePos(node.Index);
				pathPoints[i] = nodePos;
				i++;
			}
			
			// Set the first position to be the start position
			pathPoints[0] = GetStartPos();
			
			// Set the last position to be the goal position.
			int lastIndex = Mathf.Clamp(i, 0, path.Count - 1);
			System.Diagnostics.Debug.Assert(lastIndex > 1 && lastIndex < path.Count);
			pathPoints[lastIndex] = GetGoalPos();
			
			return pathPoints;
		}
		

		public Vector3 GetStartPos()
		{
			return m_pathPlanParams.StartPos;
		}
		

		public Vector3 GetGoalPos()
		{
			return m_pathPlanParams.GoalPos;
		}

		public NIPathAgent GetPathAgent()
		{
			return Agent;
		}

		public bool HasCompleted()
		{
			return m_pathPlanner.Item.HasPlanCompleted();
		}

		public bool HasSuccessfullyCompleted()
		{
			return m_pathPlanner.Item.HasPlanSucceeded();
		}

		public bool HasFailed()
		{
			return m_pathPlanner.Item.HasPlanFailed();	
		}
		#endregion
	}
}
