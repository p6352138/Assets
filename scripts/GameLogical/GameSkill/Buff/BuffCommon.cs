using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using AppUtility ;
using GameEvent ;
using GameLogical.GameEnitity ;

namespace GameLogical.GameSkill{
	
	//ragnge type
	public enum BuffRangeType 
	{
		BUFF_RANGE_NULL,
		BUFF_RANGE_SINGLE,
		BUFF_RANGE_CIRCLE,
		BUFF_RANGE_ALL,
		BUFF_RANGE_PASSTIVITY_FIGHT,
		BUFF_RANGE_PASSTIVITY,
	}
	public class BuffCreateBassData{
		public		BuffRangeType	rangeType;
		public		int			srcCreatureID	;
		public		List<int>	buffModuleID = new List<int>()	;
		public 		List<int>	buffRate	 = new List<int>()  ;
	}
	
	//single
	public class SingleBuffCreateData : BuffCreateBassData{
		public		int		destCreatureID		;	
	}
	
	//range
	public class RangeBuffCreateData : BuffCreateBassData{
		public		Vector3				destPos				;
		public		List<CCreature>		destCreatures		;
	}

	//all
	public class AllBuffCreatureData : BuffCreateBassData{
		public		List<CCreature> 	destCreatures	;
	}

	public class BuffDataBass{
		public 		BuffRangeType	rangeType	;
		public 		int		srcCreatureID		;
		public		float	deltaTime 			;
		public		float	lastTime  			;
		public		int		id					;
		public		int		moudleId			;
		public		string	effectName			;
		public 		GameObject	effectOb		;
	}
	
	public class SingleBuff : BuffDataBass{
		public 		int		destCreatureID		;
	}
	
	public class RangeBuff  : BuffDataBass{
		public		Vector3 desPos				;
	}
	
	public class BuffDataHealth	: BuffDataBass{
		public		int		hp ;
	}
	
	public class BuffDataSpeedPercent : BuffDataBass{
		public		float	speedPercent;
	}
	
	//net message
	public class SkillBufferRateDto : BassStruct{

		/**
		 * BUFFERID
		 */
		public int bufferId;
		/**
		 * 产生概率
		 */
		public int rate;
		
		public SkillBufferRateDto(Dictionary<string,object> dic){
			if(dic != null){
				this.parseData(dic);
			}
		}
	}
	
	//event message
	
	public enum BuffMessageAction{
		BUFF_MESSAGE_NULL,
		BUFF_MESSAGE_EXCUTE,
		BUFF_MESSAGE_ATTACK,
		BUFF_MESSAGE_BE_ATTACK,
		BUFF_MESSAGE_EFFECT_EXCUTE,
	}

	public class EffectExcuteEventMessage : EventMessageBase{
		public string effectName ;
		public int 	  scrId 	 ;
		public int	  destId 	 ;
		public string argument	 ;

		public EffectExcuteEventMessage(){
			eventMessageModel = EventMessageModel.eEventMessageModel_Buff ;
			eventMessageAction= (int) BuffMessageAction.BUFF_MESSAGE_EFFECT_EXCUTE;
		}
	}
	
	public class LastBuffExcuteTimeEvent: EventMessageBase
	{
		public int id ;
		public LastBuffExcuteTimeEvent(){
			eventMessageModel = EventMessageModel.eEventMessageModel_Buff ;
			eventMessageAction= (int) BuffMessageAction.BUFF_MESSAGE_EXCUTE;
			id = -1 ;
		}
	}
	
	
	public class EventMessageAttack : EventMessageBase{
		public int scrID ;
		public int destID;
		public int hurt  ;
		public string effectFun ;
		public string param ;
		
		public EventMessageAttack()
		{
			eventMessageModel  = EventMessageModel.eEventMessageModel_Buff  ;
			eventMessageAction = (int)BuffMessageAction.BUFF_MESSAGE_ATTACK;
		}
	}
	
	public class EventMessageBeAttack : EventMessageBase{
		public int scrID ;
		public int destID;
		public int hurt  ;
		public string effectFun ;
		public string param ;
		
		public EventMessageBeAttack()
		{
			eventMessageModel  = EventMessageModel.eEventMessageModel_Buff  ;
			eventMessageAction = (int)BuffMessageAction.BUFF_MESSAGE_BE_ATTACK;
		}
	}
}

