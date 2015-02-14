using UnityEngine;
using System.Collections;
using System;

namespace GameLogic.Navigation{
	public class NNode : IComparable<NNode> 
	{
		public enum eState
		{
			kUnvisited = 0,
			kOpen,
			kClosed,
			kBlocked
		};
		
		#region Constants
		public const int   kInvalidIndex = -1;
		#endregion

		#region private fileds
		private float       m_f;
		#endregion

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
	}
}
