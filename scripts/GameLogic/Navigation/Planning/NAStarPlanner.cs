using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameLogic.Navigation{
	public class NAStarPlanner : NPlanner {
		#region Fields
		protected NBinaryHeap<NNode>                     m_openNodes;
		protected NPool<NNode>                           m_nodePool;
		protected Dictionary<int, NPool<NNode>.NNode >   m_expandedNodes;
		protected NNode                                  m_startNode;
		protected NNode                                  m_goalNode;
		protected LinkedList<NNode>                      m_solution;
		protected SuccessCondition                       m_successCondition;
		private ReachedGoalNode_SuccessCondition         m_reachedGoalNodeSuccessCondition;    // Ideally this would be stored outside the planner.
		#endregion

		#region properties
		public LinkedList<NNode> Solution
		{
			get { return m_solution; }
		}
		#endregion

		#region public function
		public NAStarPlanner()
		{
		}

		public override void Awake (int maxNumberOfNodes)
		{
			base.Awake (maxNumberOfNodes);

			m_openNodes = new NBinaryHeap<NNode>();
			m_openNodes.Capacity = maxNumberOfNodes;
			m_nodePool = new NPool<NNode>(maxNumberOfNodes);
			m_expandedNodes = new Dictionary<int, NPool<NNode>.NNode>(maxNumberOfNodes);
			m_solution = new LinkedList<NNode>();
			m_reachedGoalNodeSuccessCondition = new ReachedGoalNode_SuccessCondition();
		}

		public override void Start (NIPlanningWorld world)
		{
			base.Start (world);
		}

		public override int Update (int numCyclesToConsume)
		{
			//如果不是正在寻路则终止循环
			if (m_planStatus != ePlanStatus.kPlanning)
			{
				return 0;
			}
			
			int numCyclesConsumed = 0;

			//一帧执行几次寻路计算
			while (numCyclesConsumed < numCyclesToConsume)
			{
				m_planStatus = RunSingleAStarCycle();
				numCyclesConsumed++;
				if (m_planStatus == ePlanStatus.kPlanFailed)
				{
					break;
				}
				else if (m_planStatus == ePlanStatus.kPlanSucceeded)
				{
					break;
				}
			}
			
			return numCyclesConsumed;
		}

		public void StartANewPlan(int startNodeIndex, int goalNodeIndex)
		{
			if ( startNodeIndex	== NNode.kInvalidIndex || goalNodeIndex == NNode.kInvalidIndex )
			{
				m_planStatus = NPlanner.ePlanStatus.kPlanFailed;
				return;
			}
			
			// 清楚数据
			m_nodePool.Clear();
			m_openNodes.Clear();
			m_solution.Clear();
			m_expandedNodes.Clear();
			
			// 设置起始位置
			m_startNode = GetNode(startNodeIndex).Item;
			m_goalNode = GetNode(goalNodeIndex).Item;
			
			// 初始化成功条件赋值终点位置
			m_reachedGoalNodeSuccessCondition.Awake(m_goalNode);
			m_successCondition = m_reachedGoalNodeSuccessCondition;
			
			// 初始化G,H,F 将初始点加入路径列表设置状态为正在寻路中
			m_startNode.G = 0.0f;
			m_startNode.H = World.GetHCost(m_startNode.Index, m_goalNode.Index);
			m_startNode.F = m_startNode.G + m_startNode.H;
			m_startNode.Parent = null;
			OpenNode(m_startNode);
			
			m_planStatus = ePlanStatus.kPlanning;
		}
		#endregion

		#region protected function
		/// <summary>
		/// Update the current path plan by running a single cycle of the A* search. A "single A* cycle"
		/// expands a single node, and all of its neighbors. To run a full A* search, just run this function
		/// repeatedly until the function returns kSuccessfullySolvedPath, or  kFailedToSolvePath. 
		/// Note that the openNodes variable is a binary heap data structure.
		/// 
		/// Assumptions:		The start node has already been added to the openNodes, and the start node is the only node currently
		///                     stored inside openNodes.
		/// </summary>
		/// <returns>
		/// Return the status of the path being solved. The path has either been solved, we failed to solve the path, or
		/// we are still in progress of solving the path.
		/// </returns>
		protected ePlanStatus RunSingleAStarCycle()
		{
			// Note: This failure condition must be tested BEFORE we remove an item from the open heap.
			if (m_openNodes.Count == 0)
			{
				return ePlanStatus.kPlanFailed;
			}
			
			// The current least costing pathnode is considered the "current node", which gets removed from the open list and added to the closed list.
			NNode currentNode = m_openNodes.Remove();
			CloseNode(currentNode);
			
			if ( PlanSucceeded(currentNode) )
			{
				ConstructSolution();
				return ePlanStatus.kPlanSucceeded;
			}
			else if ( PlanFailed(currentNode) )
			{
				return ePlanStatus.kPlanFailed;
			}
			
			int[] neighbors = null;
			int numNeighbors = World.GetNeighbors(currentNode.Index, ref neighbors);
			for (int i = 0; i < numNeighbors; i++)
			{
				float actualCostFromCurrentNodeToNeighbor, testCost;
				int neighborIndex = neighbors[i];
				if (neighborIndex == NNode.kInvalidIndex)
				{
					// This neighbor is off the map.
					continue;
				}
				
				NPool<NNode>.NNode neighbor = GetNode(neighborIndex);
				
				if (m_expandedNodes.Count == m_maxNumberOfNodes)
				{
					UnityEngine.Debug.LogWarning("Pathplan failed because it reached the max node count. Try increasing " +
					                             "the Max Number Of Nodes Per Planner variable on the PathManager, through " +
					                             "the Inspector window.");
					return ePlanStatus.kPlanFailed;
				}
				
				switch (neighbor.Item.State)
				{
				case NNode.eState.kBlocked:
				case NNode.eState.kClosed:
					// Case 1: Ignore
					continue;
					
				case NNode.eState.kUnvisited:
					// Case 2: Add to open list
					RecordParentNodeAndPathCosts(neighbor.Item, currentNode);
					OpenNode(neighbor.Item);
					break;
					
				case NNode.eState.kOpen:
					// Case 3: Update scores
					actualCostFromCurrentNodeToNeighbor = World.GetGCost(currentNode.Index, neighbor.Item.Index);
					testCost = currentNode.G + actualCostFromCurrentNodeToNeighbor;
					if (testCost < neighbor.Item.G)
					{
						RecordParentNodeAndPathCosts(neighbor.Item, currentNode);
						// Maintain the heap property.
						m_openNodes.Remove(neighbor.Item);
						m_openNodes.Add(neighbor.Item);
					}
					
					break;
					
				default:
					System.Diagnostics.Debug.Assert(false, "PathNode is in an invalid state when running a single cycle of A*");
					break;
				};
			}
			
			return ePlanStatus.kPlanning;
		}

		/// <summary>
		/// 计算父节点G,H,F的数据.
		/// </summary>
		/// <param name="node">Node.</param>
		/// <param name="parentNode">Parent node.</param>
		protected void RecordParentNodeAndPathCosts(NNode node, NNode parentNode)
		{
			node.G = parentNode.G + World.GetGCost(parentNode.Index, node.Index);
			node.H = World.GetHCost(node.Index, m_goalNode.Index);
			node.F = node.G + node.H;
			node.Parent = parentNode;
		}

		protected NPool<NNode>.NNode GetNode(int nodeIndex)
		{
			NPool<NNode>.NNode node;
			if ( !m_expandedNodes.TryGetValue(nodeIndex, out node) )
			{
				node = CreateNode(nodeIndex);
			}
			return node;
		}

		/// <summary>
		/// 检测是否寻路失败
		/// </summary>
		/// <returns><c>true</c>, if failed was planed, <c>false</c> otherwise.</returns>
		/// <param name="currentNode">Current node.</param>
		protected bool PlanFailed(NNode currentNode)
		{
			if (currentNode == m_startNode)
			{
				return false;
			}
			
			if (m_openNodes.Count == m_maxNumberOfNodes)
			{
				UnityEngine.Debug.LogWarning("Pathplan failed because it reached the max node count. Try increasing " +
				                             "the Max Number Of Nodes Per Planner variable on the PathManager, through " +
				                             "the Inspector window.");
				return true;
			}
			
			return false;
		}

		/// <summary>
		/// 将节点添加路径队列
		/// </summary>
		protected void ConstructSolution()
		{
			for (NNode nextNode = m_goalNode; nextNode != m_startNode; nextNode = nextNode.Parent)
			{
				m_solution.AddFirst(nextNode);
			}
			
			m_solution.AddFirst(m_startNode);
		}

		/// <summary>
		/// 判断寻路是否成功
		/// </summary>
		/// <returns><c>true</c>, if succeeded was planed, <c>false</c> otherwise.</returns>
		/// <param name="currentNode">Current node.</param>
		protected bool PlanSucceeded(NNode currentNode)
		{
			return m_successCondition.Evaluate(currentNode);
		}

		protected NPool<NNode>.NNode CreateNode(int nodeIndex)
		{
			System.Diagnostics.Debug.Assert(m_nodePool.ActiveCount == m_expandedNodes.Count);
			
			NPool<NNode>.NNode newNode = m_nodePool.Get();
			m_expandedNodes[nodeIndex] = newNode;
			NNode.eState nodeState = NNode.eState.kUnvisited;
			if (World.IsNodeBlocked(nodeIndex))
			{
				nodeState = NNode.eState.kBlocked;
			}
			newNode.Item.Awake(nodeIndex, nodeState);
			
			System.Diagnostics.Debug.Assert(m_nodePool.ActiveCount == m_expandedNodes.Count);
			
			return newNode;
		}
		
		protected void OpenNode(NNode node)
		{
			System.Diagnostics.Debug.Assert(node.State != NNode.eState.kOpen);
			node.State = NNode.eState.kOpen;
			m_openNodes.Add(node);
		}
		
		protected void CloseNode(NNode node)
		{
			node.State = NNode.eState.kClosed;
		}
		#endregion
	}
}
