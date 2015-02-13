using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using GameEvent ;
using GameLogical.GameSkill.Buff ;
using GameLogical.GameEnitity ;
using common ;
using Bass2D ;

namespace GameLogical.GameSkill{
	public class CEnemyPetSkill : CSkillBass
	{
		public override bool	canUse(){
			CPet pet = EnitityMgr.GetInstance().GetEnitity( m_data.carryID ) as CPet;
			if(pet == null)
				return false ;
			SkillMoudleData skillModuleData = (SkillMoudleData)fileMgr.GetInstance().GetData(m_data.moudleID,CsvType.CSV_TYPE_SKILL) ;
			if(skillModuleData.activateType == 2)
				return true ;
			if(pet.effectData.cantSkill == true)
				return false ;
			if(isFreezed == true)
				return false ;
			return true ;
		}
		
		public override void useSkill (object destObject){
			//angry skill
			EventMessageEnititySelect eventData = destObject as EventMessageEnititySelect;
			if(eventData != null){
				useAngrySkill(eventData) ;
				return ;
			}
			
			SkillMoudleData skillModuleData = (SkillMoudleData)fileMgr.GetInstance().GetData(m_data.moudleID,CsvType.CSV_TYPE_SKILL) ;
			isFreezed = true ;
			EventMessageFreezeTimeOut messageData = new EventMessageFreezeTimeOut();
			messageData.skillID = m_data.id ;
			GameEvent.EventMgr.GetInstance().AddTimeEvent(skillModuleData.coolDown / 1000.0f,messageData);

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
					List<CCreature> petList = EnitityMgr.GetInstance().GetMonsterList();
					
					//all
					if(skillModuleData.range == 3){
						for(int i = 0; i<petList.Count; ++i){
							BulletData bulletData = new BulletData();
							bulletData.scrID = m_data.carryID ;
							bulletData.destID= (int)petList[i].GetId();
							if(skillModuleData.effectID.Count < 4)
								common.debug.GetInstance().Error("Skill effect id error:" + skillModuleData.id);
							
							bulletData.effectID = skillModuleData.effectID[1] ;
							bulletData.effectEndID = skillModuleData.effectID[2] ;
							bulletData.buffID = skillModuleData.buffer ;
							bulletData.pos = EnitityMgr.GetInstance().GetEnitity(m_data.carryID).GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_SKILL).position;
							EnitityMgr.GetInstance().CreateEnitity(EnitityType.ENITITY_TYPE_BULLET,bulletData);
						}
					}
					else{
						int tempID = -1 ;
						int tempHp = int.MaxValue ;
						for(int i = 0; i<petList.Count; ++i){
							if(petList[i].GetFightCreatureData().blood < tempHp && petList[i].GetFightCreatureData().blood > 0){
								tempID = petList[i].GetId();
								tempHp = petList[i].GetFightCreatureData().blood ;
							}
							
						}
						
						
						BulletData bulletData = new BulletData();
						bulletData.scrID = m_data.carryID ;
						bulletData.destID= tempID;
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
				if(skillModuleData.range == (int)BuffRangeType.BUFF_RANGE_SINGLE){
					
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
						List<CCreature> petList = EnitityMgr.GetInstance().GetMonsterList();
						
						//all
						if(skillModuleData.range == 3){
							for(int i = 0; i<petList.Count; ++i){
								BulletData bulletData = new BulletData();
								bulletData.scrID = m_data.carryID ;
								bulletData.destID= (int)petList[i].GetId();
								if(skillModuleData.effectID.Count < 4)
									common.debug.GetInstance().Error("Skill effect id error:" + skillModuleData.id);
								
								bulletData.effectID = skillModuleData.effectID[1] ;
								bulletData.effectEndID = skillModuleData.effectID[2] ;
								bulletData.buffID = skillModuleData.buffer ;
								bulletData.pos = EnitityMgr.GetInstance().GetEnitity(m_data.carryID).GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_SKILL).position;
								EnitityMgr.GetInstance().CreateEnitity(EnitityType.ENITITY_TYPE_BULLET,bulletData);
							}
						}
						else{
							int tempID = -1 ;
							int tempHp = int.MaxValue ;
							for(int i = 0; i<petList.Count; ++i){
								
								if(petList[i].GetFightCreatureData().blood < tempHp && petList[i].GetFightCreatureData().blood > 0){
									tempID = petList[i].GetId();
									tempHp = petList[i].GetFightCreatureData().blood ;
								}
							}
							
							/*BulletData bulletData = new BulletData();
							bulletData.scrID = m_data.carryID ;
							bulletData.destID= tempID;
							if(skillModuleData.effectID.Count < 4)
								common.debug.GetInstance().Error("Skill effect id error:" + skillModuleData.id);
							
							bulletData.effectID = skillModuleData.effectID[1] ;
							bulletData.effectEndID = skillModuleData.effectID[2] ;
							bulletData.buffID = skillModuleData.buffer ;
<<<<<<< .mine
							bulletData.pos = EnitityMgr.GetInstance().GetEnitity(m_data.carryID).GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_SKILL).position;
							EnitityMgr.GetInstance().CreateEnitity(EnitityType.ENITITY_TYPE_BULLET,bulletData);
=======
							EnitityMgr.GetInstance().CreateEnitity(EnitityType.ENITITY_TYPE_BULLET,bulletData);*/
							
							SingleBuffCreateData singleBuff = new SingleBuffCreateData() ;
							singleBuff.buffModuleID = skillModuleData.buffer	;
							singleBuff.buffRate     = skillModuleData.buffRate  ;
							singleBuff.srcCreatureID = m_data.carryID ;
							singleBuff.destCreatureID = tempID;
							singleBuff.rangeType = BuffRangeType.BUFF_RANGE_SINGLE ;
							CBuffMgr.GetInstance().CreateBuff(singleBuff);
							PlayEffect(tempID);
						}
					}
					else if(skillModuleData.useObject == 3){
						SingleBuffCreateData singleBuff = new SingleBuffCreateData() ;
						singleBuff.buffModuleID = skillModuleData.buffer	;
						singleBuff.buffRate     = skillModuleData.buffRate  ;
						singleBuff.srcCreatureID = m_data.carryID ;
						singleBuff.destCreatureID = m_data.carryID;
						singleBuff.rangeType = BuffRangeType.BUFF_RANGE_SINGLE ;
						CBuffMgr.GetInstance().CreateBuff(singleBuff);
						PlayEffect(m_data.carryID);
					}
					//singleBuff.destCreatureID= (int)destObject		  ;
					
				}
				//range
				else if(skillModuleData.range == (int)BuffRangeType.BUFF_RANGE_CIRCLE){
					RangeBuffCreateData rangeBuff = new RangeBuffCreateData() ;
					rangeBuff.buffModuleID  = skillModuleData.buffer ;
					rangeBuff.buffRate      = skillModuleData.buffRate  ;
					rangeBuff.srcCreatureID = m_data.carryID ;
					CCreature creature = EnitityMgr.GetInstance().GetEnitity((int)destObject);
					if(skillModuleData.useObject == 1){
						rangeBuff.destCreatures = EnitityMgr.GetInstance().FindAreaCreatureList(creature,EnitityType.ENITITY_TYPE_MONSTER,m_data.range) ;
					}
					else{
						rangeBuff.destCreatures = EnitityMgr.GetInstance().FindAreaCreatureList(creature,EnitityType.ENITITY_TYPE_PET,m_data.range) ;
					}

					rangeBuff.destPos = creature.GetRenderObject().transform.position;
					rangeBuff.rangeType = BuffRangeType.BUFF_RANGE_CIRCLE ;
					
					
					//
					PlayRangeEffect(rangeBuff);
				}
				//all
				else if(skillModuleData.range == (int)BuffRangeType.BUFF_RANGE_ALL){
					AllBuffCreatureData allTargetBuff = new AllBuffCreatureData() ;
					allTargetBuff.buffModuleID = skillModuleData.buffer ;
					allTargetBuff.srcCreatureID= m_data.carryID ;
					
					allTargetBuff.rangeType = BuffRangeType.BUFF_RANGE_ALL ;
					
					if(skillModuleData.useObject == 1){
						allTargetBuff.destCreatures = EnitityMgr.GetInstance().GetPetList();
					}
					else{
						allTargetBuff.destCreatures = EnitityMgr.GetInstance().GetMonsterList();
					}
					
					CBuffMgr.GetInstance().CreateBuff(allTargetBuff);
					if(allTargetBuff.destCreatures.Count != 0){
						PlayEffect(allTargetBuff.destCreatures[0].GetId());
					}
				}
				//CBuffMgr.GetInstance().CreateBuff(buffData);
				//
			}
		}
		
		public void useAngrySkill(EventMessageEnititySelect eventData){
			int random = Random.Range(0,100);
			if(random < 10){
				common.common.shakeCamera(true);
			}
			
			GameObject effectOb = null ;
			GameObject sceneOb  = null ;
			GameObject sceneOb2 = null ;
			effectOb = gameGlobal.g_rescoureMgr.GetGameObjectResource("shifa") as GameObject;
			sceneOb  = GameObject.Instantiate(effectOb) as GameObject;
			effectOb = gameGlobal.g_rescoureMgr.GetGameObjectResource("shifa2") as GameObject;
			sceneOb2 = GameObject.Instantiate(effectOb) as GameObject;
			
			CCreature creature = EnitityMgr.GetInstance().GetEnitity(eventData.scrId);
			Transform destCreature = creature.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT) ;
			
			//attach point
			if(destCreature != null){
				sceneOb.transform.position = destCreature.FindChild("foot").position ;
				sceneOb.transform.parent = destCreature.FindChild("foot");
				sceneOb.transform.FindChild("creature").animation.Play("effect");
				sceneOb2.transform.position = destCreature.FindChild("body").position ;
				sceneOb2.transform.parent = destCreature.FindChild("body");
				sceneOb2.transform.FindChild("creature").animation.Play("effect");
			}
			
			SkillMoudleData skillModuleData = (SkillMoudleData)fileMgr.GetInstance().skillCsvData.dataDic[m_data.moudleID] ;
			//CLineSmoothMgr.GetInstance().mpStone  -= skillModuleData.greenStone ;
			isFreezed = true ;
			
			if(eventData == null)
				return ;
			List<CCreature> creatureList = eventData.id  as List<CCreature>;
			if(creatureList == null || creatureList.Count < 1)
				return ;
			//is bullet
			if(skillModuleData.isBullet == 1){
				BulletData bulletData = new BulletData();
				bulletData.scrID = m_data.carryID ;
				bulletData.destID= creatureList[0].GetId();
				if(skillModuleData.effectID.Count < 4)
					common.debug.GetInstance().Error("Skill effect id error:" + skillModuleData.id);
				bulletData.effectID = skillModuleData.effectID[1] ;
				bulletData.effectEndID = skillModuleData.effectID[2] ;
				bulletData.buffID = skillModuleData.buffer ;
				bulletData.pos = EnitityMgr.GetInstance().GetEnitity(m_data.carryID).GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_SKILL).position;
				EnitityMgr.GetInstance().CreateEnitity(EnitityType.ENITITY_TYPE_BULLET,bulletData);
			}
			else if(skillModuleData.isBullet == 2){
				//single target
				if(skillModuleData.range == (int)BuffRangeType.BUFF_RANGE_SINGLE){
					if(creatureList[0].GetEnitityAiState() != GameLogical.GameEnitity.AI.AIState.AI_STATE_WEAK && 
					   creatureList[0].GetEnitityAiState() != GameLogical.GameEnitity.AI.AIState.AI_STATE_DEATH){
						SingleBuffCreateData singleBuff = new SingleBuffCreateData() ;
						singleBuff.buffModuleID = skillModuleData.buffer	;
						singleBuff.buffRate     = skillModuleData.buffRate  ;
						singleBuff.srcCreatureID = m_data.carryID ;
						singleBuff.destCreatureID= creatureList[0].GetId() ;
						singleBuff.rangeType = BuffRangeType.BUFF_RANGE_SINGLE ;
						CBuffMgr.GetInstance().CreateBuff(singleBuff);
						PlayEffect(creatureList[0].GetId());
					}
					
				}
				//range
				else if(skillModuleData.range == (int)BuffRangeType.BUFF_RANGE_CIRCLE){
					RangeBuffCreateData rangeBuff = new RangeBuffCreateData() ;
					rangeBuff.buffModuleID  = skillModuleData.buffer ;
					rangeBuff.buffRate      = skillModuleData.buffRate  ;
					rangeBuff.srcCreatureID = m_data.carryID ;
					rangeBuff.destCreatures = (List<CCreature>)eventData.id ;
					rangeBuff.destPos = eventData.pos ;
					rangeBuff.rangeType = BuffRangeType.BUFF_RANGE_CIRCLE ;
					
					
					//
					PlayRangeEffect(rangeBuff);
				}
				//all
				else if(skillModuleData.range == (int)BuffRangeType.BUFF_RANGE_ALL){
					AllBuffCreatureData allTargetBuff = new AllBuffCreatureData() ;
					allTargetBuff.buffModuleID = skillModuleData.buffer ;
					allTargetBuff.buffRate     = skillModuleData.buffRate  ;
					allTargetBuff.srcCreatureID= m_data.carryID ;
					allTargetBuff.rangeType = BuffRangeType.BUFF_RANGE_ALL ;
					
					if(skillModuleData.useObject == 1)
					{
						allTargetBuff.destCreatures = EnitityMgr.GetInstance().GetMonsterList();
					}
					else if(skillModuleData.useObject == 2){
						allTargetBuff.destCreatures = EnitityMgr.GetInstance().GetPetList();
					}
					
					for(int i = 0; i<allTargetBuff.destCreatures.Count; ++i){
						if(allTargetBuff.destCreatures[i].GetEnitityAiState() != GameLogical.GameEnitity.AI.AIState.AI_STATE_DEATH &&
						   allTargetBuff.destCreatures[i].GetEnitityAiState() != GameLogical.GameEnitity.AI.AIState.AI_STATE_WEAK){
							SingleBuffCreateData singleBuff = new SingleBuffCreateData() ;
							singleBuff.buffModuleID = skillModuleData.buffer	;
							singleBuff.buffRate     = skillModuleData.buffRate  ;
							singleBuff.srcCreatureID = m_data.carryID ;
							singleBuff.destCreatureID= allTargetBuff.destCreatures[i].GetId() ;
							singleBuff.rangeType = BuffRangeType.BUFF_RANGE_SINGLE ;
							CBuffMgr.GetInstance().CreateBuff(singleBuff);
						}
						
					}
					
					PlayEffect(m_data.carryID);
				}
				//CBuffMgr.GetInstance().CreateBuff(buffData);
			}
			else
			{
				common.debug.GetInstance().Error("SkillMoudleData is bullet data error:" + m_data.moudleID);
			}
		}
		
		protected void PlayRangeEffect(RangeBuffCreateData rangeBuffData){
			SkillMoudleData skillModuleData = (SkillMoudleData)fileMgr.GetInstance().skillCsvData.dataDic[m_data.moudleID] ;
			//GameObject effectOb = null ;
			//GameObject sceneOb  = null ;
			ResourceMoudleData resourceMoudleData = null ;
			if(skillModuleData.effectID[0] != -1){
				resourceMoudleData = (ResourceMoudleData)fileMgr.GetInstance().GetData(skillModuleData.effectID[0],CsvType.CSV_TYPE_RESOUCE) ;
				MGResouce.LoadEffectData loadData = new MGResouce.LoadEffectData();
				loadData.packName = resourceMoudleData.packagePath ;
				loadData.name	  = resourceMoudleData.path 	   ;
				loadData.fun	  = FinshLoadRangeEffect;
				
				//effectOb = gameGlobal.g_rescoureMgr.GetGameObjectResource(resourceMoudleData.path) as GameObject;
				//sceneOb  = GameObject.Instantiate(effectOb) as GameObject;
				CCreature creature = EnitityMgr.GetInstance().GetEnitity(m_data.carryID);
				//loadData.parent   = creature.GetRenderObject().transform ;
				loadData.pos      = rangeBuffData.destPos ;
				MGResouce.BundleMgr.Instance.LoadFightEffect(loadData);
				
				//Transform sceneCreature = sceneOb.transform.FindChild("creature") ;
				//Transform destCreature = creature.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT) ;
				
				//attach point
				/*if(sceneCreature != null && destCreature != null){
					Amin_2D_Ex aminEx = sceneCreature.GetComponent<Amin_2D_Ex>() ;
					if(aminEx.attachPos != ""){
						sceneOb.transform.position = destCreature.FindChild(aminEx.attachPos).position ;
						sceneOb.transform.parent = destCreature.FindChild(aminEx.attachPos);
					}
					else{
						sceneOb.transform.position = creature.GetRenderObject().transform.position ;
						sceneOb.transform.parent = creature.GetRenderObject().transform ;
					}
				}
				else{
					sceneOb.transform.position = creature.GetRenderObject().transform.position ;
					sceneOb.transform.rotation = creature.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_ROOT).rotation ;
					sceneOb.transform.parent = creature.GetRenderObject().transform ;
				}
				
				
				sceneOb.transform.FindChild("creature").animation.Play("effect");*/
			}
			
			
			if(skillModuleData.effectID[2] != -1 && skillModuleData.isBullet == 2){
				resourceMoudleData = (ResourceMoudleData)fileMgr.GetInstance().GetData(skillModuleData.effectID[2],CsvType.CSV_TYPE_RESOUCE);
				
				MGResouce.LoadEffectData loadData = new MGResouce.LoadEffectData();
				loadData.packName = resourceMoudleData.packagePath ;
				loadData.name	  = resourceMoudleData.path 	   ;
				loadData.fun	  = FinshLoadRangeBuffEffect;
				loadData.pos	  = rangeBuffData.destPos	;
				MGResouce.BundleMgr.Instance.LoadFightEffect(loadData);
				//effectOb = gameGlobal.g_rescoureMgr.GetGameObjectResource(resourceMoudleData.path) as GameObject;
				//sceneOb  = GameObject.Instantiate(effectOb) as GameObject;
				
				EventMessageSkillBuff message = new EventMessageSkillBuff();
				message.rangeBuffCreatureData = new RangeBuffCreateData();
				message.rangeBuffCreatureData.buffModuleID = rangeBuffData.buffModuleID ;
				message.rangeBuffCreatureData.buffRate     = skillModuleData.buffRate  ;
				message.rangeBuffCreatureData.destPos = rangeBuffData.destPos ;
				message.rangeBuffCreatureData.rangeType = rangeBuffData.rangeType ;
				message.rangeBuffCreatureData.srcCreatureID = rangeBuffData.srcCreatureID ;
				message.rangeBuffCreatureData.destCreatures = new List<CCreature>();
				for(int i = 0 ; i<rangeBuffData.destCreatures.Count; ++i){
					message.rangeBuffCreatureData.destCreatures.Add(rangeBuffData.destCreatures[i]) ;
				}
				EventMgr.GetInstance().AddTimeEvent(0.5f,message);
				
				
			}
			else{
				CBuffMgr.GetInstance().CreateBuff(rangeBuffData);
			}
		}
		
		
		public void FinshLoadRangeEffect(Object ob,MGResouce.LoadEffectData data){
			if(ob != null){
				if(data.parent != null){
					GameObject sceneOb = MonoBehaviour.Instantiate(ob) as GameObject ;
					Transform sceneCreature = sceneOb.transform.FindChild("creature") ;
					Transform destCreature = data.parent.FindChild(gameGlobal.CREATURE_POIN_ROOT) ;
					//attach point
					if(sceneCreature != null && destCreature != null){
						Amin_2D_Ex aminEx = sceneCreature.GetComponent<Amin_2D_Ex>() ;
						if(aminEx.attachPos != ""){
							sceneOb.transform.position = destCreature.FindChild(aminEx.attachPos).position ;
							sceneOb.transform.parent = destCreature.FindChild(aminEx.attachPos);
						}
						else{
							sceneOb.transform.position = data.parent.position ;
							sceneOb.transform.parent = data.parent ;
						}
					}
					else{
						sceneOb.transform.position = data.parent.position ;
						sceneOb.transform.rotation = data.parent.FindChild(gameGlobal.CREATURE_ROOT).rotation ;
						sceneOb.transform.parent = data.parent ;
					}
					sceneOb.transform.FindChild("creature").animation.Play("effect");
				}
			}
		}
		
		public void FinshLoadRangeBuffEffect(Object ob,MGResouce.LoadEffectData data){
			if(ob != null){
				GameObject sceneOb = MonoBehaviour.Instantiate(ob) as GameObject ;
				sceneOb.transform.position = data.pos ;
			}
		}
		
		public	override void 	OnMessage(EventMessageBase	message){
			
		}
	}
}


