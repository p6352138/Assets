using UnityEngine;
using System.Collections;

namespace GameLogical.GameEnitity.AI{
	public class PetAIDataBass{
		public 		AIState				state 		;
		public		float				time		;
	}
	
	//move state ,go forward to destObject
	//if destObject is null ,go forward to destPos
	public class PetMoveAIData : PetAIDataBass{
		public 		int			destObjectId	;
		//public		Vector3				destPos		;
	}
	
	/// <summary>
	/// Pet patrol AI data.
	/// </summary>
	public class PetPatrolAIData : PetAIDataBass{
		public      Vector3[]	 patrolPathList ;
		public		int			 destPathIndex	;
		
		public PetPatrolAIData(){
			patrolPathList = new Vector3[2] ;
		}
	}
	
	/// <summary>
	/// Pet satand AI data.
	/// </summary>
	public class PetSatandAIData : PetAIDataBass{
		
	}
	
	//lock at targetObject to hit
	public class PetAttackStateData : PetAIDataBass{
		public		int			targetObjectId ;
	}

	public class EnemyPlayerAIDataBass{
		public 		float		deltaTime	;
		public		float		remedyTime	= 3.0f ;
	}
}

