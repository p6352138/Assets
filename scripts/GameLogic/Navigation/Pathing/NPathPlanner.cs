using UnityEngine;
using System.Collections;

namespace GameLogic.Navigation{
	public class NPathPlanner : NAStarPlanner  {
		#region Fields
		private NIPathTerrain m_pathTerrain;
		#endregion
		
		#region Properties
		public NIPathTerrain PathTerrain
		{
			get { return m_pathTerrain; }
		}
		#endregion
		
		public override void Start(NIPlanningWorld world)
		{
			base.Start(world);
			
			System.Diagnostics.Debug.Assert(world is NIPathTerrain);
			m_pathTerrain = world as NIPathTerrain;
		}
	}
}
