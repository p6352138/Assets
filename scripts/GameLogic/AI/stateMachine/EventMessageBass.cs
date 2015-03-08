using UnityEngine;
using System.Collections;

namespace GameLogic.AI{
	public enum EventMessageModel
	{
		eEventMessageModel_Null         		,   //
		eEventMessageModel_MONSTER_STATE		,
		eEventMessageModel_PLAY_STATE			,	// PLAY_STATE
		eEventMessageModel_PLAY_MOVE_STATE		,
		eEventMessageModel_PLAY_ATTACK_STATE	,
	}
	
	public class EventMessageBase
	{
		public EventMessageModel  	  eventMessageModel  ;
		public int                    eventMessageAction ;
		public int 					  modleId;
		public EventMessageBase()
		{
			eventMessageModel  = EventMessageModel.eEventMessageModel_Null  ;
			eventMessageAction = 0;
			modleId = 0;
		}
	}
}
