using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using GameEvent ;
using AppUtility ;
using GameLogical.GameSkill.Buff ;
namespace GameLogical.GameSkill{
	
	public enum SkillType{
		SKILL_TYPE_NULL,
		SKILL_TYPE_PLAYER,
		SKILL_TYPE_ENEMY_PLAYER,
		SKILL_TYPE_PET,
		SKILL_TYPE_MONSTER,
		SKILL_TYPE_ENEMY_PET,
	}

	public enum SkillWorkType{
		SKILL_WORK_TYPE_NULL   = -1 ,
		SKILL_WORK_TYPE_REMEDY = 102 ,
		SKILL_WORK_TYPE_CUT_BLOOD = 201 ,
		SKILL_WORK_TYPE_CUT_BLOOD_BUFF = 204,
		SKILL_WORK_TYPE_CUT_BLOOD_PERCENT = 208 ,
		SKILL_WORK_TYPE_CAN_NOT_REMEDY = 601,
		SKILL_WORK_TYPE_REMOVE_DEBUFF  = 612,
		SKILL_WORK_TYPE_ATTACK  = 614,
		SKILL_WORK_TYPE_STOP1 = 701 ,
		SKILL_WORK_TYPE_STOP2 = 702 ,
		SKILL_WORK_TYPE_CAN_NOT_SKILL = 703 ,
	}
	
	//宠物技能
	public class PetSkillDto: BassStruct
	{


	public string id;

	/** 技能位置 */
	public int seat;
	/**
	 * 技能配置Id
	 */
	public int skillConfigId;
	/**
	 * 是否开放
	 */
	public int isOpen;
	/**
	 * 升级下级技能的需要消耗的游戏币
	 */
	public int nextSkillGold;

	/**
	 * 宠物技能最大等级
	 */
	public int maxLv;

	/**
	 * 宠物等级限制
	 */
	public int skillLvLimit;

	public PetSkillDto(Dictionary<string,object> dic)
	{
			this.parseData(dic);
	}
		
	}
	
	//fight player skill data
	public class SkillDataBass 
	{
		public 		int		moudleID	;
		public		int		id  		;
		public		int		carryID		;
		public		int		rate		;
		public		bool	isRange		;
		public		float	range	= 10.0f	;
	}
	
	
	//net message
	
	public class SkillSimpleDto : BassStruct{
		public string id;

		/** 技能格位置 */
		public int seat;
		/**
		 * 技能配置Id
		 */
		public int skillConfigId;
		public SkillSimpleDto(Dictionary<string,object> dic){
			if(dic != null){
				this.parseData(dic);
			}
		}
	}
	
	public class SkillDto : BassStruct{

		public string id;
		/** 类型(1:角色 2:宠物) */
		public int type;
		/** 所属角色ID或宠物ID */
		public string ownId;
		/** 技能等级 */
		public int lv;
		/** 否是是主动学习的技能(0:否 1:是) */
		public int isLearnSkill;
		/** 技能格位置 */
		public int seat;
		/**
		 * 名称
		 */
		public string name;
		/**
		 * 图片
		 */
		public int imageId;
		/**
		 * 特效ID
		 */
		public int effectId;
	
		/**
		 * CD时间
		 */
		public int CDTime;
		
		/**
		 * 施放对象（1、敌方；2、己方）
		 */
		public int dischargeObj;
		
		/**
		 * 施放范围（1、单体；2、圆形；3、全屏）
		 */
		public int dischargeRange;
	
		/** 黄能量 */
		public int yellowEnergy = 0;
		/** 蓝能量 */
		public int blueEnery = 0;
		/** 红能量 */
		public int redEnergy = 0;
		/**
		 * 宠物的法力消耗
		 */
		public int spellRequest = 0;

		
		/**
		 * 技能BUFFER以及产生buffer的概率
		 */
		public List<SkillBufferRateDto> skillBufferRateDtoList ;
		
		public SkillDto(Dictionary<string,object> dic){
			skillBufferRateDtoList = new List<SkillBufferRateDto>();
			if(dic != null){
				this.parseData(dic);
			}
		}
	}
	
	//event message
	public enum SkillEventMessageAction{
		SKILL_ACTION_NULL,
		SKILL_ACTION_FREEZE_TIME_OUT,
		SKILL_ACTION_USE_SKILL,
		SKILL_ACTION_SKILL_BUFF,
	}
	
	public class EventMessageFreezeTimeOut : EventMessageBase{
		public	int		skillID		;
		
		public	EventMessageFreezeTimeOut(){
			eventMessageModel = EventMessageModel.eEventMessageModel_Skill ;
			eventMessageAction= (int)SkillEventMessageAction.SKILL_ACTION_FREEZE_TIME_OUT ;
		}
	}

	public class EventMessageUseSkill : EventMessageBase{
		public int id		;
		public int skillID	;
		public int costMp 	;
		public EventMessageUseSkill(){
			eventMessageModel = EventMessageModel.eEventMessageModel_Skill ;
			eventMessageAction= (int)SkillEventMessageAction.SKILL_ACTION_USE_SKILL ;
		}
	}

	public class EventMessageSkillBuff : EventMessageBase{
		public RangeBuffCreateData rangeBuffCreatureData ;
		public EventMessageSkillBuff(){
			eventMessageModel = EventMessageModel.eEventMessageModel_Skill ;
			eventMessageAction= (int)SkillEventMessageAction.SKILL_ACTION_SKILL_BUFF ;
		}
	}
}


