using UnityEngine;
using System.Collections;

namespace GameLogic.Navigation{

	/// <summary>
	/// 寻路目标点继承自这个接口
	/// </summary>
	public interface NINavTarget {
		/// <summary>
		/// 返回目标位置
		/// </summary>
		/// <returns>The nav target position.</returns>
		Vector3 GetNavTargetPosition();
	}
	
	public class NNavTargetPos : NINavTarget
	{
		private Vector3 m_targetPos;
		private NIPathTerrain m_pathTerrain;
		
		public NNavTargetPos(Vector3 targetPos, NIPathTerrain pathTerrain)
		{
			m_targetPos = targetPos;
			m_pathTerrain = pathTerrain;
		}
		
		#region ITarget Members
		public Vector3 GetNavTargetPosition()
		{
			return m_pathTerrain.GetValidPathFloorPos( m_targetPos );
		}
		#endregion
	}
	
	public class NNavTargetGameObject : NINavTarget
	{
		private GameObject m_targetGameObject;
		private NIPathTerrain m_pathTerrain;
		
		public NNavTargetGameObject(GameObject targetGameObject, NIPathTerrain pathTerrain)
		{
			m_targetGameObject = targetGameObject;
			m_pathTerrain = pathTerrain;
		}
		
		#region ITarget Members
		public Vector3 GetNavTargetPosition()
		{
			return m_pathTerrain.GetValidPathFloorPos( m_targetGameObject.transform.position );
		}
		#endregion
	}
}
