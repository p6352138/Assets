using UnityEngine;
using System.Collections;

namespace GameLogical.GameEnitity.AI{
		//////////////////////////////////////////////////////// state data ////////////////////////////////////////////////////////////
	public class CityAIDataBass{
		public 		AIState				state 		;
	}
	

	public class CityNormalStateData : CityAIDataBass{
		
		public CityNormalStateData(){
			state = AIState.AI_STATE_DOOR_NARMOL ;
		}
	}
	
	public class CityDoorBreakStateData : CityAIDataBass{
		
		public CityDoorBreakStateData(){
			state = AIState.AI_STATE_DOOR_BREAK ;
		}
	}
}

