using UnityEngine;
using GameEvent;
using System.Collections.Generic ;
namespace GameLogical.GameEnitity{ 
	//enitity type
	public enum EnitityType{
		ENITITY_TYPE_NULL,
		ENITITY_TYPE_CHARACTER, 
		ENITITY_TYPE_PVP_CHARACTER,
		ENITITY_TYPE_ENEMY_CHARACTER,
		ENITITY_TYPE_MONSTER,
		ENITITY_TYPE_PET,
		ENITITY_TYPE_ENEMY_PET,
		ENITITY_TYPE_CITY,
		ENITITY_TYPE_WOMEN,
		ENITITY_TYPE_BULLET,
		ENITYTY_TYPE_BACK_UP_PET,
	}
	
	public enum AttackType{
		ATTACK_TYPE_NULL,
		ATTACK_TYPE_NEAR,
		ATTACK_TYPE_FAR ,
	}

	public class AICommon{
		public static float AI_THINK_DELTA_TIME = 0.3f ;
		public static float AI_MONSTER_COME_BACK_DISTANCE = 10.0f;
		public static float AI_MONSTER_CHANGE_WAY_DISTANCE= 5.0f ;
		public static float AI_MONSTER_TEAM_GAP	= 15.0f ;
		public static float AI_ATTACK_Y_GAP	= 600.0f ;
		public static float AI_ATTACK_X_GAP = 9.0f ;
		public static int   AI_MONSTER_WAY_NUM = 4 ;
		public static float AI_PET_MOVE_MAX_X = 125.0f ;
		public static float AI_MONSTER_ATTACK_CITY_GAP = 10.0f ;
		public static float AI_MONSTER_BE_DRAW_DIS = 25.0f ;
		public static float AI_MONSTER_MISPLACE = 2.0f;
		public static float AI_MONSTER_Y_WAY = 10.0f;
		public static float AI_MONSTER_X_MISPLACE = 10.0f;
		public static float AI_MONSTER_X_ATTACK_NEAR = 15.0f ;
	}

	public class FightCommon{
		public static float REFRAIN_COEFFICIENT 	= 1.2f;
		public static float BE_REFRAIN_COEFFICIENT 	= 0.9f;
	}

	//create monster data
	public class CreateMonsterData{
		public int moudleID ;
		public int rewardID ;
		public int lastHp = -1	;
		public Vector3	pos	;
	}
	//effect data
	public class EffectData{
		public		int			speed			;
		public		int			speedPercent	;
		
		public		int			hpPercent		;
		public      int         hpMax			;

		public		int			changeHpPercent ;

		public		int			changeAddHpPercent ;

		public		int			cutCoolDownTime	;
		
		public      int         mpMax			;
		public      int         mpPercent		;
		public      int 		mp				;
		
		public 		int			attack			;
		public      int         attackPrecent   ;
		
		public 		int			waveHurtPercentMin ;
		public		int			waveHurtPercentMax ;
		
		public 		int			changeAttack	;
		public		int			changeAttackPercent;
		public		int			changeHurt		 ;
		public		int			changeHurtPercent;

		public 		int			changeSkillAttack	;
		public		int			changeSkillAttackPercent;
		public		int			changeSkillHurt		 ;
		public		int			changeSkillHurtPercent;

		public		int			changeSunAttack ;
		public		int			changeSunAttackPercent ;
		public		int			changeSunHurt	;
		public		int			changeSunHurtPercent   ;

		public		int			changeMoonAttack ;
		public		int			changeMoonAttackPercent ;
		public		int			changeMoonHurt	;
		public		int			changeMoonHurtPercent   ;

		public		int			changeStarAttack ;
		public		int			changeStarAttackPercent ;
		public		int			changeStarHurt	;
		public		int			changeStarHurtPercent   ;

		public		int			changeIceAttack	;
		public		int			changeIceAttackPercent ;		
		public		int			changeIceHurt	;
		public		int			changeIceHurtPercent ;
		
		public		int			changeFireAttack	;
		public		int			changeFireAttackPercent ;		
		public		int			changeFireHurt	;
		public		int			changeFireHurtPercent ;
		
		public		int			changeEarthAttack	;
		public		int			changeEarthAttackPercent ;		
		public		int			changeEarthHurt	;
		public		int			changeEarthHurtPercent ;
		
		public		int			changeThunderAttack	;
		public		int			changeThunderAttackPercent ;		
		public		int			changeThunderHurt	;
		public		int			changeThunderHurtPercent ;
		
		public		int			changeLightAttack	;
		public		int			changeLightAttackPercent ;		
		public		int			changeLightHurt	;
		public		int			changeLightHurtPercent ;
		
		public		int			changeWindAttack	;
		public		int			changeWindAttackPercent ;		
		public		int			changeWindHurt	;
		public		int			changeWindHurtPercent       ;

		public		int			skillRate		;

		public int spell ;
		/** 暴击(数字1代表1%) */
		public int crit;
		public int critPrecent ;
		public int critHurt ;
		public int critHurtPercent ;
		/** 闪避(数字1代表1%) */
		public int duck;
		public int duckPrecent ;
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

		
		public bool  mustBingo;
		public bool invincibility;
		public int 	oneHurt;
		public bool cantHp	;
		public int  reLiveRate ;
		public int	relive	;
		public bool cantSkill;
		public List<int> resistanceList ;
		public bool realHurt ;

		public bool bloodBoom;
		
		public  EffectData(){
			speed = 0 ;
			speedPercent = 0 ;
			hpPercent = 0 ;
			mpMax = 0 ;
			mpPercent = 0 ;
			mp = 0 ;
			
			attack = 0 ;
			attackPrecent = 0 ;
			
			waveHurtPercentMin = 0 ;
			waveHurtPercentMax = 0 ;

			spell= 0 ;
			crit = 0 ;
			duck = 0 ;
			ice  = 0 ;
			fire = 0 ;
			earth= 0 ;
			thunder = 0 ;
			light= 0 ;
			wind = 0 ;
			
			mustBingo = false ;
			invincibility = false ;
			oneHurt = 0 ;
			cantHp = false ;
			reLiveRate = 0 ;
			relive = 0 ;
			cantSkill = false ;
			skillRate = 0 ;
			
			resistanceList = new List<int>() ;
			realHurt = false ;

			bloodBoom = false ;
		}
	}
	
	
	//message
	public enum EnitityAction{
		ENITITY_ACTION_NULL ,
		ENITITY_ACTION_CREATE,
		ENITITY_ACTION_FIGHT,
		ENITITY_ACTION_ESCAPE,
		ENITITY_ACTION_CHANGE_PET,
		ENITITY_ACTION_CHANGE_PET_UP,
		//attack animation event action
		ENITITY_ACTION_FIGHT_SATRT, 
		ENITITY_ACTION_FIGHT_FINISH,
		ENITITY_ACTION_DEATH,
		ENITITY_ACTION_WEAK,
		ENITYTY_ACTION_SKILL,
		ENITYTY_ACTION_SKILL_START,
		ENITITY_ACTION_SKILL_FINISH,
		//
		ENITITY_ACTION_ENTER_COLLIDER,
		ENITITY_ACTION_EXIT_COLLIDER,
		//skill
		ENITITY_ACTION_SELECT_ENITITY,
		ENITITY_ACTION_SELECT_SKILL,
		ENITITY_ACTION_CANCEL_SELECT_SKILL,
		ENITITY_ACTION_LOCK_PET,
		ENITITY_ACTION_BE_DRAW,
		ENITITY_ACTION_START_ANGRY_SKILL,
	}
	
	//create enitity message
	public class EventMessageCreateEnitity : EventMessageBase{
		public EnitityType type ;
		public int         moudleID ;
		
		public EventMessageCreateEnitity()
		{
			eventMessageModel  = EventMessageModel.eEventMessageModel_Enitity  ;
			eventMessageAction = (int)EnitityAction.ENITITY_ACTION_CREATE;
		}
	}
	
	//
	public class EventMessageFight : EventMessageBase{
		public int scrCreatureId ;
		public int destCreatureId;
		public string	  audioName ;
		public string 	  beEffectName;
		
		public EventMessageFight()
		{
			eventMessageModel  = EventMessageModel.eEventMessageModel_Enitity  ;
			eventMessageAction = (int)EnitityAction.ENITITY_ACTION_FIGHT;
		}
	}
	
	//enter collider
	public class EventMessageEnterCollider : EventMessageBase{
		public GameObject scrObject ;
		public GameObject destObject;
		public AreaType	  type ;
		public EventMessageEnterCollider(){
			eventMessageModel  = EventMessageModel.eEventMessageModel_Enitity  ;
			eventMessageAction = (int)EnitityAction.ENITITY_ACTION_ENTER_COLLIDER;
		}
	}
	
	public class EventMessageExitCollider : EventMessageBase{
		public GameObject scrObject ;
		public GameObject destObject;
		public AreaType	  type ;
		public EventMessageExitCollider(){
			eventMessageModel  = EventMessageModel.eEventMessageModel_Enitity  ;
			eventMessageAction = (int)EnitityAction.ENITITY_ACTION_EXIT_COLLIDER;
		}
	}
	
	public class EventMessageMonsterEscape : EventMessageBase{
		public int id ;
		public EventMessageMonsterEscape(){
			eventMessageModel  = EventMessageModel.eEventMessageModel_Enitity  ;
			eventMessageAction = (int)EnitityAction.ENITITY_ACTION_ESCAPE;
		}
	}
	
	//skill
	public class EventMessageEnititySelect : EventMessageBase{
		public Vector3  pos;
		public	object	id ;
		public int		scrId ;
		public EventMessageEnititySelect(){
			eventMessageModel  = EventMessageModel.eEventMessageModel_Enitity  ;
			eventMessageAction = (int)EnitityAction.ENITITY_ACTION_SELECT_ENITITY ;
		}
	}
	
	public class EventMessageSelectSkill : EventMessageBase{
		public int index	;
		public EventMessageSelectSkill(){
			eventMessageModel  = EventMessageModel.eEventMessageModel_Enitity  ;
			eventMessageAction = (int)EnitityAction.ENITITY_ACTION_SELECT_SKILL ;
		}
	}
	
	public class EventMessageCancelSelectSkill : EventMessageBase{
		public EventMessageCancelSelectSkill(){
			eventMessageModel  = EventMessageModel.eEventMessageModel_Enitity  ;
			eventMessageAction = (int)EnitityAction.ENITITY_ACTION_CANCEL_SELECT_SKILL ;
		}
	}


	//start use angry skill
	public class EventMessageEnitityStartAngrySkill : EventMessageBase{
		public Vector3  pos ;
		public object	id ;
		public int		scrId ;
		public int 		skillId ;
		public EventMessageEnitityStartAngrySkill(){
			eventMessageModel  = EventMessageModel.eEventMessageModel_Enitity  ;
			eventMessageAction = (int)EnitityAction.ENITITY_ACTION_START_ANGRY_SKILL ;
		} 
	}
}
