using UnityEngine;
using System.Collections;
using System;

namespace GameLogic.Navigation{
	/// <summary>
	/// 路点
	/// </summary>
	public class NNode : IComparable<NNode> 
	{
		//<summer>节点状态</summer>
		public enum eState
		{
			/// <summary>
			/// 未访问
			/// </summary>
			kUnvisited = 0,
			/// <summary>
			/// 可以被访问.
			/// </summary>
			kOpen,
			/// <summary>
			/// 访问过.
			/// </summary>
			kClosed,
			/// <summary>
			/// 障碍物.
			/// </summary>
			kBlocked
		};
		
		#region Constants
		public const int   kInvalidIndex = -1;
		#endregion

		#region private fileds
		private float       m_f;
		private float       m_g;
		private float       m_h;
		private eState      m_state;
		private NNode        m_parent;
		private int         m_index;
		#endregion

		#region Properties
		public eState State
		{
			get { return m_state; }
			set { m_state = value; }
		}

		public NNode Parent
		{
			get { return m_parent; }
			set { m_parent = value; }
		}

		public int Index
		{
			get { return m_index; }
			set { m_index = value; }
		}

		/// <summary>
		/// Gets or sets the f.
		/// </summary>
		/// <value>The f.</value>
		public float F
		{
			get { return m_f; }
			set { m_f = value; }
		}

		/// <summary>
		/// Gets or sets the g.
		/// </summary>
		/// <value>The g.</value>
		public float G
		{
			get { return m_g; }
			set { m_g = value; }
		}

		/// <summary>
		/// Gets or sets the h.
		/// </summary>
		/// <value>The h.</value>
		public float H
		{
			get { return m_h; }
			set { m_h = value; }
		}
		#endregion
		public NNode()
		{
			Awake(kInvalidIndex, NNode.eState.kUnvisited);
		}

		public int CompareTo(NNode other)
		{
			if (m_f < other.m_f)
			{
				return -1;
			}
			else if (m_f > other.m_f)
			{
				return 1;
			}
			else
			{
				return 0;
			}
		}

		public override string ToString()
		{
			return "Node:" + m_f.ToString();
		}
		
		public void Awake(int nodeIndex, eState state)
		{
			m_index = nodeIndex;
			m_f = float.MaxValue;
			m_state = state;
			m_parent = null;
			m_g = float.MaxValue;
			m_h = float.MaxValue;
		}
	}
}
