using UnityEngine;
using GameEvent ;
namespace GameLogical.GameEnitity{
	//monster data
	public class MonsterData : DynamicCreatureData{
		public 		int        blood     ;
		public      int        maxBlood  ;
		public 		int        attack    ;
		public      AttackType attackType;
		
		public 		float	   attackArea		;
		public 		float	   eyeShotArea 		; 
		
		public      int        maxSpell ;
		public      int		   spell	;
		
		
		
		/** 暴击(数字1代表1%) */
		public int crit;
		/** 闪避(数字1代表1%) */
		public int duck;
		/** 冰属(1代表1%) */
		public int ice;
		/** 火属(1代表1%) */
		public int fire;
		/** 土属(1代表1%) */
		public int earth;
		/** 雷属(1代表1%) */
		public int thunder;
		/** 光属(1代表1%) */
		public int light;
		/** 风属(1代表1%) */
		public int wind;
	}
	
	//monster message
	public class EventMessageFightStart : EventMessageBase{
		public GameObject ob ;
		public string	  audioName ;
		public string 	  beEffectName;
		public EventMessageFightStart()
		{
			eventMessageModel  = EventMessageModel.eEventMessageModel_Enitity  ;
			eventMessageAction = (int)EnitityAction.ENITITY_ACTION_FIGHT_SATRT ;
		}
	}
	
	public class EventMessageFightEnd : EventMessageBase{
		public GameObject ob ;
		public EventMessageFightEnd()
		{
			eventMessageModel  = EventMessageModel.eEventMessageModel_Enitity  ;
			eventMessageAction = (int)EnitityAction.ENITITY_ACTION_FIGHT_FINISH ;
		}
	}
	
	public class EventMessageDeathEnd : EventMessageBase{
		public GameObject ob ;
		public EventMessageDeathEnd()
		{
			eventMessageModel  = EventMessageModel.eEventMessageModel_Enitity  ;
			eventMessageAction = (int)EnitityAction.ENITITY_ACTION_DEATH ;
		}
	}
	
	public class EventMessageWeak : EventMessageBase{
		public int id ;
		public EventMessageWeak(){
			eventMessageModel  = EventMessageModel.eEventMessageModel_Enitity  ;
			eventMessageAction = (int)EnitityAction.ENITITY_ACTION_WEAK ;
		}
	}

	public class EventMessageSkillStart : EventMessageBase{
		public GameObject ob ;
		public EventMessageSkillStart(){
			eventMessageModel  = EventMessageModel.eEventMessageModel_Enitity  ;
			eventMessageAction = (int)EnitityAction.ENITYTY_ACTION_SKILL_START ;
		}
	}

	public class EventMessageSkill : EventMessageBase{
		public GameObject ob ;
		public EventMessageSkill(){
			eventMessageModel  = EventMessageModel.eEventMessageModel_Enitity  ;
			eventMessageAction = (int)EnitityAction.ENITYTY_ACTION_SKILL ;
		}
	}
	
	public class EventMessageSkillEnd : EventMessageBase{
		public GameObject ob ;
		public EventMessageSkillEnd(){
			eventMessageModel  = EventMessageModel.eEventMessageModel_Enitity  ;
			eventMessageAction = (int)EnitityAction.ENITITY_ACTION_SKILL_FINISH ;
		}
	}

	public class EventMessageLockPet : EventMessageBase{
		public int lockPetID ;
		public int lockMonsterID ;
		public EventMessageLockPet(){
			eventMessageModel  = EventMessageModel.eEventMessageModel_Enitity  ;
			eventMessageAction = (int)EnitityAction.ENITITY_ACTION_LOCK_PET ;
		}
	}
	
	public class EventMessageChangePet : EventMessageBase{
		public int scrIndex ;
		public int destIndex;
		public EventMessageChangePet(){
			eventMessageModel  = EventMessageModel.eEventMessageModel_Enitity  ;
			eventMessageAction = (int)EnitityAction.ENITITY_ACTION_CHANGE_PET ;
		}
	}

	public class EventMessageChangePetUp : EventMessageBase{
		public Vector3 pos ;
		
		public EventMessageChangePetUp(){
			eventMessageModel  = EventMessageModel.eEventMessageModel_Enitity  ;
			eventMessageAction = (int)EnitityAction.ENITITY_ACTION_CHANGE_PET_UP ;
		}
	}

	public class EventMessageBeDraw : EventMessageBase{
		public int id ;
		public EventMessageBeDraw (){
			eventMessageModel  = EventMessageModel.eEventMessageModel_Enitity  ;
			eventMessageAction = (int)EnitityAction.ENITITY_ACTION_BE_DRAW ;
		}
	}

}


