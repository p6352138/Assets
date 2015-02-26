using UnityEngine;
using System.Collections;

namespace GameLogic.Navigation{
	/// <summary>
	///Responsible for determining if a plan has succeded. If you would like to define your own success condition for a
	///plan, then inherit from this abstract class.
	/// 负责确定一个计划已经成功。如果你想定义一个你自己的成功条件计划，然后从这个抽象类继承。
	/// </summary>
	public abstract class SuccessCondition
	{
		/// <summary>
		///确定计划已经成功。如果该计划成功，返回真，否则为假。 
		/// </summary>
		/// <param name="currentNode">
		///The node currently being evaluated by the planner.
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public abstract bool Evaluate(NNode currentNode);
	}

	/// <summary>
	/// 成功条件，当前节点是否就是目标节点
	/// </summary>
	public class ReachedGoalNode_SuccessCondition : SuccessCondition
	{
		private NNode m_goalNode;
		
		public void Awake(NNode goalNode)
		{
			m_goalNode = goalNode;
		}
		
		public override bool Evaluate(NNode currentNode)
		{
			if (m_goalNode == currentNode)
			{
				return true;
			}
			
			return false;
		}
	}
}
