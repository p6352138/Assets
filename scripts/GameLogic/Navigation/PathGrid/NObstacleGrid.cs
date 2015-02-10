using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameLogic.Navigation{
	public class NObstacleGrid {

		#region private Properties

		#endregion

		#region public function
		public static void DebugShowObstacleGrid(List<Bounds> boundslist){
			foreach(Bounds item in boundslist)
				GetObstructedCells(item);
		}

		public static int[] GetObstructedCells(Bounds bounds)
		{
			int[] result = {1};

			//calculation Obstacle bounds position in world coordtion
			Vector3 upperLeftPos = new Vector3(bounds.min.x,NGrid.Origin.y,bounds.max.z);
			Vector3 upperRightPos = new Vector3(bounds.max.x,NGrid.Origin.y,bounds.max.z);
			Vector3 lowerLeftPos = new Vector3(bounds.min.x,NGrid.Origin.y,bounds.min.z);
			Vector3 lowerRightPos = new Vector3(bounds.max.x,NGrid.Origin.y,bounds.min.z);

			Vector3 horizDir = (upperRightPos - upperLeftPos).normalized;
			Vector3 vertDir = (upperLeftPos - lowerLeftPos).normalized;
			float horizLength = bounds.size.x;
			float vertLength = bounds.size.z;

			//debug show Obstacle in pathgrid
			Color red = new Color(1,0,0);
			Debug.DrawLine(upperLeftPos, upperRightPos,red);
			Debug.DrawLine(upperRightPos, lowerRightPos,red);
			Debug.DrawLine(lowerRightPos, lowerLeftPos,red);
			Debug.DrawLine(lowerLeftPos, upperLeftPos,red);

			return result;
		}
		#endregion
	}
}
