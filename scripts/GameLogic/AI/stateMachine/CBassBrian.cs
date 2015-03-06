using UnityEngine;
using System.Collections;

namespace GameLogic.AI{
	public enum AIState{
		AI_STATE_NULL,
		AI_STATE_STAND,
        AI_STATE_MOVE,
        AI_STATE_ATTACK,
	}
	
	public enum BulletState{
		
		STATE_BULLET_START,
		STATE_BULLET_MOVE,
		STATE_BULLET_END,
	}
	
	public interface CBassBrian{
		void ThinkAbout();
		void OnMessage();
	}
}
