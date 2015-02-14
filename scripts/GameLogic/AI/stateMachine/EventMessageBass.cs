using UnityEngine;
using System.Collections;

namespace GameLogic.AI{
	public enum EventMessageModel
	{
		eEventMessageModel_Null         ,   //
		eEVentMessageModel_Smooth       ,   // smooth
		eEventMessageModel_Enitity		,	// enitity
		eEventMessageModel_Buff			,	// buff
		eEventMessageModel_Skill		,	// skill
		eEventMessageModel_Level		,	// level
		eEventMessageModel_PLAY_STATE	,	// PLAY_STATE
		eEventMessageModel_Guide		,
	}
	
	public class EventMessageBase
	{
		public EventMessageModel  	  eventMessageModel  ;
		public int                    eventMessageAction ;
		public EventMessageBase()
		{
			eventMessageModel  = EventMessageModel.eEventMessageModel_Null  ;
			eventMessageAction = 0;
		}
	}
}
