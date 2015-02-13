using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using GameEvent ;
using GameLogical.GameSkill.Buff ;
using GameLogical.GameEnitity ;
using common ;
using Bass2D ;

namespace GameLogical.GameSkill{
	public class CEnemyPlayerSkill : CSkillBass
	{
		public override bool	canUse(){
			SkillMoudleData skillModuleData = (SkillMoudleData)fileMgr.GetInstance().GetData(m_data.moudleID,CsvType.CSV_TYPE_SKILL) ;
			if(skillModuleData.activateType == 2)
				return true ;
			//if(pet.effectData.cantSkill == true)
			//	return false ;
			/*if(CLineSmoothMgr.GetInstance().buleStone < skillModuleData.blueStone)
				return false ;
			if(CLineSmoothMgr.GetInstance().greenStone < skillModuleData.greenStone)
				return false ;
			if(CLineSmoothMgr.GetInstance().redStone < skillModuleData.redStone)
				return false ;*/
			if(isFreezed == true)
				return false ;




			/*CLineSmoothMgr.GetInstance().buleStone -= skillModuleData.blueStone ;
			CLineSmoothMgr.GetInstance().greenStone-= skillModuleData.greenStone;
			CLineSmoothMgr.GetInstance().redStone  -= skillModuleData.redStone ;*/

			/*if(GameDataCenter.GetInstance().pvpPlayerInfo.mpStone < skillModuleData.greenStone){
				return false ;
			}

			EventMessageUseSkill useSkillMessage = new EventMessageUseSkill();
			useSkillMessage.id = EnitityMgr.GetInstance().enemyPlayer.GetId();
			useSkillMessage.skillID = m_data.id ;
			useSkillMessage.costMp  = skillModuleData.blueStone + skillModuleData.greenStone + skillModuleData.redStone ;
			EventMgr.GetInstance().OnEventMgr(useSkillMessage);

			EventMessageFreezeTimeOut messageData = new EventMessageFreezeTimeOut();
			messageData.skillID = m_data.id ;
			
			GameEvent.EventMgr.GetInstance().AddTimeEvent(skillModuleData.coolDown / 1000.0f,messageData);*/

			
			return 	true	;
		}
		
		public override void useSkill (object destObject)
		{
			int random = Random.Range(0,100);
			if(random < 10){
				common.common.shakeCamera(true);
			}
			
			SkillMoudleData skillModuleData = (SkillMoudleData)fileMgr.GetInstance().skillCsvData.dataDic[m_data.moudleID] ;
			//GameDataCenter.GetInstance().pvpPlayerInfo.mpStone  -= skillModuleData.greenStone ;
			isFreezed = true ;



			EventMessageEnititySelect eventData = (EventMessageEnititySelect)destObject ;
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
				EnitityMgr.GetInstance().CreateEnitity(EnitityType.ENITITY_TYPE_BULLET,bulletData);
			}
			else if(skillModuleData.isBullet == 2){
				//single target
				if(skillModuleData.range == (int)BuffRangeType.BUFF_RANGE_SINGLE){
					SingleBuffCreateData singleBuff = new SingleBuffCreateData() ;
					singleBuff.buffModuleID = skillModuleData.buffer	;
					singleBuff.buffRate     = skillModuleData.buffRate  ;
					singleBuff.srcCreatureID = m_data.carryID ;
					singleBuff.destCreatureID= creatureList[0].GetId() ;
					singleBuff.rangeType = BuffRangeType.BUFF_RANGE_SINGLE ;
					CBuffMgr.GetInstance().CreateBuff(singleBuff);
					PlayEffect(creatureList[0].GetId());
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
						allTargetBuff.destCreatures = EnitityMgr.GetInstance().GetPetList();
					}
					else if(skillModuleData.useObject == 2){
						allTargetBuff.destCreatures = EnitityMgr.GetInstance().GetMonsterList();
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
		
		public	override void 	OnMessage(EventMessageBase	message){
			
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
				loadData.parent   = creature.GetRenderObject().transform ;
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
	}


}


