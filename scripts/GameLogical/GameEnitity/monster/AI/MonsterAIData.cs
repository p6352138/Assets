using UnityEngine;
using System.Collections;
using GameEvent;

namespace GameLogical.GameEnitity.AI{
	//////////////////////////////////////////////////////// state data ////////////////////////////////////////////////////////////
	public class MonsterAIDataBass{
		public 		AIState				state 		;
		public		float				time		;
		public 		int					wayIndex	;
	}
	
	//move state ,go forward to destObject
	//if destObject is null ,go forward to destPos
	public class MonsterMoveStateData : MonsterAIDataBass{
		public 		int					destObjectId	;
		public		Vector3				destPos			;
		
		public MonsterMoveStateData(){
			destObjectId = -1 ;
			destPos = Vector3.zero ;
		}
	}
	
	//lock at targetObject to hit
	public class MonsterAttackStateData : MonsterAIDataBass{
		public		int			targetObjectId ;
		public MonsterAttackStateData(){
			targetObjectId = -1 ;
		}
	}
	
	public class MonsterPursueStateData : MonsterAIDataBass{
		public 		int 		targetObjectId ;
	}

	public class MonsterChangeWayStateData : MonsterAIDataBass{
		public      Vector3		destPos		   ;
	}
	
	//////////////////////////////////////////////////////  message //////////////////////////////////////////////////////////////
	//ai message action type
	public enum MonsterAction{
		MONSTER_ACTION_NULL,
		MONSTER_ACTION_STAND,
		MONSTER_ACTION_MOVE,
		MONSTER_ACTION_ACTTACK,
		MONSTER_ACTION_BE_HURT,
		MONSTER_ACTION_DEATH,
	}
	
	//monster state action message
	public class AIMonsterMsg : EventMessageBase{
		public CCreature	targetObject	;
		public AIMonsterMsg(){
			eventMessageAction = (int)MonsterAction.MONSTER_ACTION_ACTTACK ;
		}
	}
}

