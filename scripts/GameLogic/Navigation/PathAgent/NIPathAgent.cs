using UnityEngine;
using System.Collections;

namespace GameLogic.Navigation{
	/// <summary>
	///These are the objects that can navigate. Inherit from this interface to define your own type
	///of entity that can navigate around the world.
	/// </summary>
	public interface NIPathAgent {
		/// <summary>
		///Get the position of the path agent's feet (the position where his feet touch the ground).
		/// </summary>
		/// <returns>
		/// A <see cref="Vector3"/>
		/// </returns>
		Vector3 GetPathAgentFootPos();
		
		/// <summary>
		///Called when the agent's path request successfully completes. 
		/// </summary>
		/// <param name="request">
		/// A <see cref="IPathRequestQuery"/>
		/// </param>
		void OnPathAgentRequestSucceeded(NIPathRequestQuery request);
		
		/// <summary>
		///Called when the agent's path request fails to complete.
		/// </summary>
		void OnPathAgentRequestFailed();
	}
}
