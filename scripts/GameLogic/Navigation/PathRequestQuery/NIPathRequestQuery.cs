using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
}
