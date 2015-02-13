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
	}
}
