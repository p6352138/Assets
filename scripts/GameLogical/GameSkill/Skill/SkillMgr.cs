using UnityEngine;
using System.Collections;
using AppUtility ;
using GameEvent ;
using System.Collections.Generic;
using GameLogical.GameSkill.Buff ;

namespace GameLogical.GameSkill.Skill{
	class SkillMgr : Singleton<SkillMgr> , IEvent {
		protected	Dictionary<int,CSkillBass>	m_skillMap ;
		static	protected	int		index	;
		static  public		int		PLAYER_CARRY_SKILL_NUM = 4;
		
		public void Init(){
			m_skillMap = new Dictionary<int, CSkillBass>();
			EventMgr.GetInstance().AddLinsener(this);
		}
		
		public int	CreateSkill(SkillType type,int moudleID,int carryID){
			index++ ;
			//SkillSimpleDto skillDto = GameDataCenter.GetInstance().playerSkillList[index] ;
			SkillDataBass	data = new SkillDataBass();
			data.moudleID = moudleID;
			data.carryID  = carryID ;
			data.id = index;
			switch(type){
			case SkillType.SKILL_TYPE_PLAYER:{
				CPlayerSkill skill = new CPlayerSkill();
				
				//data.id = index;
				skill.CreateSkill(data);
				if(!m_skillMap.ContainsKey(data.id)){
					m_skillMap.Add(data.id,skill);
				}
				else{
					common.debug.GetInstance().Error("add same skill id");
				}
			}
				break ;

			case SkillType.SKILL_TYPE_ENEMY_PLAYER:{
				CEnemyPlayerSkill skill = new CEnemyPlayerSkill();
				
				//data.id = index;
				skill.CreateSkill(data);
				if(!m_skillMap.ContainsKey(data.id)){
					m_skillMap.Add(data.id,skill);
				}
				else{
					common.debug.GetInstance().Error("add same skill id");
				}
			}
				break ;
			case SkillType.SKILL_TYPE_PET:{
				CPetSkill skill = new CPetSkill();
				skill.CreateSkill(data);
				
				if(!m_skillMap.ContainsKey(data.id)){
					m_skillMap.Add(data.id,skill);
				}
				else{
					common.debug.GetInstance().Error("add same skill id");
				}
			}
				break ;
				
			case SkillType.SKILL_TYPE_MONSTER:{
				CMonsterSkill skill = new CMonsterSkill();
				skill.CreateSkill(data);
				
				if(!m_skillMap.ContainsKey(data.id)){
					m_skillMap.Add(data.id,skill);
				}
				else{
					common.debug.GetInstance().Error("add same skill id");
				}
			}
				break ;

			case SkillType.SKILL_TYPE_ENEMY_PET:{
				CEnemyPetSkill skill = new CEnemyPetSkill();
				skill.CreateSkill(data);
				
				if(!m_skillMap.ContainsKey(data.id)){
					m_skillMap.Add(data.id,skill);
				}
				else{
					common.debug.GetInstance().Error("add same skill id");
				}
			}
				break ;

			}
			
			
			return index ;
		}
		
		public void RemoveSkill(int id){
			if(m_skillMap.ContainsKey(id)){
				m_skillMap.Remove(id);
			}
		}
		
		public void OnMessage(EventMessageBase message){
			if(message.eventMessageModel == EventMessageModel.eEventMessageModel_Skill){
				switch((SkillEventMessageAction)message.eventMessageAction){
				case SkillEventMessageAction.SKILL_ACTION_FREEZE_TIME_OUT:{
					EventMessageFreezeTimeOut freezeMessage = (EventMessageFreezeTimeOut)message ;
					if(m_skillMap.ContainsKey(freezeMessage.skillID)){
						m_skillMap[freezeMessage.skillID].isFreezed = false ;
					}
				}
					break ;

				case SkillEventMessageAction.SKILL_ACTION_SKILL_BUFF:{
					EventMessageSkillBuff skillBuff = (EventMessageSkillBuff)message ;
					if(skillBuff != null){
						CBuffMgr.GetInstance().CreateBuff(skillBuff.rangeBuffCreatureData);
					}
				}
					break ;
				}
			}
		}
		
		public CSkillBass	GetSkill(int id){
			if(m_skillMap.ContainsKey(id)){
				return m_skillMap[id] ;
			}
			else{
				common.debug.GetInstance().Log("can ont find skill:" + id);
				return null ;
			}
		}
		
		public void Clear(){
			m_skillMap.Clear();
		}
	}
}

