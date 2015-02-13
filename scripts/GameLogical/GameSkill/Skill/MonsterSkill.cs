using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using GameEvent ;
using GameLogical.GameSkill.Buff ;
using GameLogical.GameEnitity ;
using common ;

namespace GameLogical.GameSkill{
	public class CMonsterSkill : CSkillBass
	{
		public override bool	canUse(){
			CMonster monster = EnitityMgr.GetInstance().GetEnitity( m_data.carryID ) as CMonster;
			if(monster == null)
				return false ;
			SkillMoudleData skillModuleData = (SkillMoudleData)fileMgr.GetInstance().GetData(m_data.moudleID,CsvType.CSV_TYPE_SKILL) ;
			if(skillModuleData.activateType == 2)
				return true ;
			if(monster.effectData.cantSkill == true)
				return false ;
			if(isFreezed == true)
				return false ;

			//isFreezed = true ;
			
			return 	true	;
		}

		
		public override void useSkill (object destObject){
			SkillMoudleData skillModuleData = (SkillMoudleData)fileMgr.GetInstance().GetData(m_data.moudleID,CsvType.CSV_TYPE_SKILL) ;

			EventMessageFreezeTimeOut messageData = new EventMessageFreezeTimeOut();
			messageData.skillID = m_data.id ;
			GameEvent.EventMgr.GetInstance().AddTimeEvent(skillModuleData.coolDown / 1000.0f,messageData);
			isFreezed = true ;
			
			//GameEvent.EventMgr.GetInstance().AddTimeEvent(skillModuleData.coolDown / 1000.0f,messageData);

			//is bullet
			if(skillModuleData.isBullet == 1){
				if(skillModuleData.useObject == 1){
					BulletData bulletData = new BulletData();
					bulletData.scrID = m_data.carryID ;
					bulletData.destID= (int)destObject;
					if(skillModuleData.effectID.Count < 4)
						common.debug.GetInstance().Error("Skill effect id error:" + skillModuleData.id);
					bulletData.effectID = skillModuleData.effectID[1] ;
					bulletData.effectEndID = skillModuleData.effectID[2] ;
					bulletData.buffID = skillModuleData.buffer ;
					bulletData.pos = EnitityMgr.GetInstance().GetEnitity(m_data.carryID).GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_SKILL).position;
					EnitityMgr.GetInstance().CreateEnitity(EnitityType.ENITITY_TYPE_BULLET,bulletData);
				}
				else if(skillModuleData.useObject == 2){
					List<CCreature> monsterList = EnitityMgr.GetInstance().GetMonsterList();
					for(int i = 0; i<monsterList.Count; ++i){
						BulletData bulletData = new BulletData();
						bulletData.scrID = m_data.carryID ;
						bulletData.destID= (int)monsterList[i].GetId();
						if(skillModuleData.effectID.Count < 4)
							common.debug.GetInstance().Error("Skill effect id error:" + skillModuleData.id);
						bulletData.effectID = skillModuleData.effectID[1] ;
						bulletData.effectEndID = skillModuleData.effectID[2] ;
						bulletData.buffID = skillModuleData.buffer ;
						bulletData.pos = EnitityMgr.GetInstance().GetEnitity(m_data.carryID).GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_SKILL).position;
						EnitityMgr.GetInstance().CreateEnitity(EnitityType.ENITITY_TYPE_BULLET,bulletData);
					}
					
				}
				else if(skillModuleData.useObject == 3){
					BulletData bulletData = new BulletData();
					bulletData.scrID = m_data.carryID ;
					bulletData.destID= (int)m_data.carryID;
					if(skillModuleData.effectID.Count < 4)
						common.debug.GetInstance().Error("Skill effect id error:" + skillModuleData.id);
					bulletData.effectID = skillModuleData.effectID[1] ;
					bulletData.effectEndID = skillModuleData.effectID[2] ;
					bulletData.buffID = skillModuleData.buffer ;
					bulletData.pos = EnitityMgr.GetInstance().GetEnitity(m_data.carryID).GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_SKILL).position;
					EnitityMgr.GetInstance().CreateEnitity(EnitityType.ENITITY_TYPE_BULLET,bulletData);
				}
			}
			else if(skillModuleData.isBullet == 2){
				//single target
				if(skillModuleData.range == (int)BuffRangeType.BUFF_RANGE_SINGLE || skillModuleData.range == (int)BuffRangeType.BUFF_RANGE_CIRCLE){
					if(skillModuleData.useObject == 1){
						SingleBuffCreateData singleBuff = new SingleBuffCreateData() ;
						singleBuff.buffModuleID = skillModuleData.buffer	;
						singleBuff.buffRate     = skillModuleData.buffRate  ;
						singleBuff.srcCreatureID = m_data.carryID ;
						singleBuff.destCreatureID = (int)destObject;
						singleBuff.rangeType = BuffRangeType.BUFF_RANGE_SINGLE ;
						CBuffMgr.GetInstance().CreateBuff(singleBuff);
						PlayEffect(destObject);
					}
					else if(skillModuleData.useObject == 2){
						List<CCreature> monsterList = EnitityMgr.GetInstance().GetMonsterList();
						for(int i = 0; i<monsterList.Count; ++i){
							SingleBuffCreateData singleBuff = new SingleBuffCreateData() ;
							singleBuff.buffModuleID = skillModuleData.buffer	;
							singleBuff.buffRate     = skillModuleData.buffRate  ;
							singleBuff.srcCreatureID = m_data.carryID ;
							singleBuff.destCreatureID = monsterList[i].GetId();
							singleBuff.rangeType = BuffRangeType.BUFF_RANGE_SINGLE ;
							CBuffMgr.GetInstance().CreateBuff(singleBuff);
							PlayEffect(monsterList[i].GetId());
						}
					}
					else if(skillModuleData.useObject == 3){
						SingleBuffCreateData singleBuff = new SingleBuffCreateData() ;
						singleBuff.buffModuleID = skillModuleData.buffer	;
						singleBuff.buffRate     = skillModuleData.buffRate  ;
						singleBuff.srcCreatureID = m_data.carryID ;
						singleBuff.destCreatureID = (int)m_data.carryID;
						singleBuff.rangeType = BuffRangeType.BUFF_RANGE_SINGLE ;
						CBuffMgr.GetInstance().CreateBuff(singleBuff);
						PlayEffect(m_data.carryID);
					}
				}
				//range
				/*else if(skillModuleData.range == (int)BuffRangeType.BUFF_RANGE_CIRCLE){
					RangeBuffCreateData rangeBuff = new RangeBuffCreateData() ;
					rangeBuff.buffModuleID = skillModuleData.buffer ;
					rangeBuff.srcCreatureID= m_data.carryID ;
					rangeBuff.destPos      = (Vector3)destObject ;
					rangeBuff.rangeType = BuffRangeType.BUFF_RANGE_CIRCLE ;
					CBuffMgr.GetInstance().CreateBuff(rangeBuff);
				}*/
				//all
				else if(skillModuleData.range == (int)BuffRangeType.BUFF_RANGE_ALL){
					AllBuffCreatureData allTargetBuff = new AllBuffCreatureData() ;
					allTargetBuff.buffModuleID = skillModuleData.buffer ;
					allTargetBuff.srcCreatureID= m_data.carryID ;
					
					allTargetBuff.rangeType = BuffRangeType.BUFF_RANGE_ALL ;
					
					if(skillModuleData.useObject == 2){
						allTargetBuff.destCreatures = EnitityMgr.GetInstance().GetMonsterList();
					}
					else{
						allTargetBuff.destCreatures = EnitityMgr.GetInstance().GetPetList();
					}
					
					CBuffMgr.GetInstance().CreateBuff(allTargetBuff);
				}
				//CBuffMgr.GetInstance().CreateBuff(buffData);
				

			}

		}
		
		
		public	override void 	OnMessage(EventMessageBase	message){
			
		}
	}
}


