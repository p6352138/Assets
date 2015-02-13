using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using GameEvent;
using GameLogical.GameSkill.Skill ;
using GameLogical.GameSkill ;
using Bass2D ;

namespace GameLogical.GameEnitity.AI
{
	public class PetAttackState: CStateBase<CPet>{
		protected static PetAttackState instance;
		
		public void Release(){
			
		}

		public int GetSkillID ()
		{
			int random = Random.Range(0,100);
			
			SkillLvMoudleData skilllvData = (SkillLvMoudleData)common.fileMgr.GetInstance().GetData(1, 
				common.CsvType.CSV_TYPE_SKILLLV);
				
			int lv = 0;
			int id = 0;
			for(int i = 0; i < skilllvData.skillLvList.Count; i++)
			{
				lv += skilllvData.skillLvList[i];
				if(lv > random)
				{
					id = i + 1;
					return id;
				}
			}
			return 0;
		}
		
		public int GetSkill(List<SkillLimit> skillDic)
		{
			//int totalWeight = 0;
			//for(int i = 0; i < skillDic.Count; i++)
			//{
			//	totalWeight += skillDic[i].pelSkill.GetSkillData().rate;
			//}
			
			//int random = Random.Range(0,totalWeight);
			//int lv = 0;
			for(int i = 0; i < skillDic.Count; i++)
			{
				if(skillDic[i].pelSkill.canUse()){
					return skillDic[i].pelSkill.GetSkillData().id;
				}
				/*lv += skillDic[i].pelSkill.GetSkillData().rate;
				if(lv > random)
				{
					return skillDic[i].pelSkill.GetSkillData().id;
				}*/
			}
			return 0;
		}
		
		public void Enter(CPet type){
			
//			Think(type);
			List<CCreature> petList = EnitityMgr.GetInstance().GetPetList();
			float tempDis ;
			for(int i = 0; i<petList.Count; ++i){
				//not come back
				tempDis = Vector3.Distance(petList[i].GetRenderObject().transform.position,type.GetRenderObject().transform.position) ;
				if(tempDis< type.m_data.misPlace &&  petList[i].GetId() != type.GetId() &&
				   petList[i].GetEnitityAiState() != AI.AIState.AI_STATE_MISPLACE){
					type.m_stateMachine.ChangeState(PetMisplaceState.getInstance());
					return;
				}
			}


			type.m_data.curAttackCD = 0.0f ;
			//List<CPetSkill> skills =  type.GetCanUseSkillList();
			List<SkillLimit> skillDic = type.GetCanUseSkillList2();
			//have skill
			if(skillDic.Count > 0)
			{
				int random = Random.Range(0,100);
				CPetSkill oneSkill = null;
				int skillID = GetSkill (skillDic);
				if(skillID != 0)
					oneSkill = type.GetSkillByID(skillID);
				
				if(oneSkill != null && type.IsSkillEffect(oneSkill) && random < type.m_data.skillRate + type.effectData.skillRate )
				{
					type.Play("skill2",WrapMode.Once);
					type.m_curUsingSkill = oneSkill.GetSkillData().id ;
					//talk
					/*PetMoudleData  petMoudleData = (PetMoudleData)common.fileMgr.GetInstance().GetData(type.m_data.moudleID, common.CsvType.CSV_TYPE_PET);
					type.TalkLv(petMoudleData.talkLvInSkill, petMoudleData.talkIDInSkill);*/

					GameObject effectOb = null ;
					GameObject sceneOb  = null ;
					effectOb = gameGlobal.g_rescoureMgr.GetGameObjectResource("shifa") as GameObject;
					sceneOb  = GameObject.Instantiate(effectOb) as GameObject;
					
					Transform sceneCreature = sceneOb.transform.FindChild(gameGlobal.CREATURE) ;
					Transform destCreature = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT) ;
					
					//attach point
					if(sceneCreature != null && destCreature != null){
						Amin_2D_Ex aminEx = sceneCreature.GetComponent<Amin_2D_Ex>() ;
						if(aminEx.attachPos != ""){
							sceneOb.transform.position = destCreature.FindChild(aminEx.attachPos.ToString()).position ;
							sceneOb.transform.parent = destCreature.FindChild(aminEx.attachPos.ToString());
						}
						else{
							sceneOb.transform.position = type.GetRenderObject().transform.position ;
							sceneOb.transform.parent = type.GetRenderObject().transform ;
						}
					}
					else{
						sceneOb.transform.position = type.GetRenderObject().transform.position ;
						sceneOb.transform.parent = type.GetRenderObject().transform ;
					}
					sceneOb.transform.FindChild("creature").animation.Play("effect");
				}
				//attack
				else
				{
					type.Play("attack",WrapMode.Once);
					
					//talk
					/*PetMoudleData  petMoudleData = (PetMoudleData)common.fileMgr.GetInstance().GetData(type.m_data.moudleID, common.CsvType.CSV_TYPE_PET);
			
					type.TalkLv(petMoudleData.talkLvInAttack, petMoudleData.talkIDInAttack);*/
				}
			}
			//attack
			else{
				type.Play("attack",WrapMode.Once);
				
				//talk
				/*PetMoudleData  petMoudleData = (PetMoudleData)common.fileMgr.GetInstance().GetData(type.m_data.moudleID, common.CsvType.CSV_TYPE_PET);
			
				type.TalkLv(petMoudleData.talkLvInAttack, petMoudleData.talkIDInAttack);*/
			}
		}
		
		public void Execute(CPet type, float time){
			type.m_data.curAttackCD += time ;
			if(type.m_data.curAttackCD >= type.m_data.attackCD){
				if(!Think(type)){
					Action(type,time);
				}
				type.m_data.curAttackCD = 0.0f ;
			}
		}

		public bool Think(CPet type){
			if(type.m_targetCreature!=null && type.m_targetCreature.GetRenderObject()!=null){
				if(type.m_targetCreature.GetEnitityAiState() != AIState.AI_STATE_WEAK && type.m_targetCreature.GetEnitityAiState() != AIState.AI_STATE_DEATH){
					//float dis = Vector3.Distance(type.m_targetCreature.GetRenderObject().transform.position,type.GetRenderObject().transform.position);
					float disX = type.m_targetCreature.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_BE_ATTACK_FORWARD).position.x - type.GetRenderObject().transform.position.x ;
					float dis = Mathf.Abs(disX);

					disX = type.GetRenderObject().transform.position.x - type.m_targetCreature.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_BE_ATTACK_BACK).position.x ;
					if(dis > Mathf.Abs(disX)){
						dis = Mathf.Abs(disX) ;
					}

					float disY = type.m_targetCreature.GetRenderObject().transform.position.y - type.GetRenderObject().transform.position.y ;
					dis = Mathf.Abs(disY);

					if(dis > type.attackArea && dis < type.eyeShotArea){
						type.m_stateMachine.ChangeState(PetPursueState.getInstance());
						return true ;
					}
					else{
						float moveVec = type.GetRenderObject().transform.position.x - type.m_targetCreature.GetRenderObject().transform.position.x ;
						if(moveVec < 0){
							type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_ROOT).localRotation = Quaternion.LookRotation(Vector3.forward) ;
							Vector3 pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition ;
							pos.z = -0.5f ;
							type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition = pos ;

							pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT).localPosition ;
							pos.z = -0.2f ;
							type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT).localPosition = pos ;
						}else if(moveVec > 0){
							type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_ROOT).localRotation = Quaternion.LookRotation(Vector3.back) ;
							Vector3 pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition ;
							pos.z = 0.5f ;
							type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition = pos ;

							pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT).localPosition ;
							pos.z = 0.2f ;
							type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT).localPosition = pos ;
						}
						return false ;
					}
				}
				//fine anthor
				else{
					List<CCreature> monsterList = EnitityMgr.GetInstance().GetMonsterList();
					if(monsterList.Count > 0){
						CCreature targetMonster = null;
						float tempDis ;
						float tempDisX;
						float tempDisY;
						float dis = float.MaxValue ;
						//find the nestest target on eye shot
						for(int i = 0; i<monsterList.Count; ++i){
							if(monsterList[i].GetEnitityAiState() == AIState.AI_STATE_WEAK)
								continue ;
							tempDisX = monsterList[i].GetRenderObject().transform.position.x - type.GetRenderObject().transform.position.x ;
							tempDisY = monsterList[i].GetRenderObject().transform.position.y - type.GetRenderObject().transform.position.y ;
							tempDis  = Mathf.Abs(tempDisX) + Mathf.Abs(tempDisY) * 0.5f;
							if(tempDis<type.attackArea && tempDis < dis){
								dis = tempDis ;
								targetMonster = monsterList[i] ;
							}
						}
						//find one
						if(targetMonster!=null){
							//MonsterPursueStateData pursueData = new MonsterPursueStateData();
							//pursueData.targetObjectId = targetPet.GetId() ;
							type.m_targetCreature = targetMonster;       //set the target

							return false;
						}
						else{
							targetMonster = EnitityMgr.GetInstance().FindDangerousMonster() ;
							if(targetMonster != null){
								if(targetMonster.GetEnitityAiState() == AIState.AI_STATE_ACTTACK_CITY || targetMonster.GetEnitityAiState() == AIState.AI_STATE_ESCAPE){
									type.m_targetCreature = targetMonster ;
									type.m_stateMachine.ChangeState(PetPursueState.getInstance());
								}
								else{
									type.m_stateMachine.ChangeState(PetMoveState.getInstance());
								}
							}
							return true ;
						}

					}
					//
					type.m_stateMachine.ChangeState(PetMoveState.getInstance());
					return true ;
				}
			}
			//fine anthor
			else{

				List<CCreature> monsterList = EnitityMgr.GetInstance().GetMonsterList();
				if(monsterList.Count > 0){
					CCreature targetMonster = null;
					float tempDis ;
					float tempDisX;
					float tempDisY;
					float dis = float.MaxValue ;
					//find the nestest target on eye shot
					for(int i = 0; i<monsterList.Count; ++i){
						if(monsterList[i].GetEnitityAiState() == AIState.AI_STATE_WEAK)
							continue ;
						tempDisX = monsterList[i].GetRenderObject().transform.position.x - type.GetRenderObject().transform.position.x ;
						tempDisY = monsterList[i].GetRenderObject().transform.position.y - type.GetRenderObject().transform.position.y ;
						tempDis  = Mathf.Abs(tempDisX) + Mathf.Abs(tempDisY) ;
						if(tempDis<type.attackArea && tempDis < dis){
							dis = tempDis ;
							targetMonster = monsterList[i] ;
						}
					}
					//find one
					if(targetMonster!=null){
						//MonsterPursueStateData pursueData = new MonsterPursueStateData();
						//pursueData.targetObjectId = targetPet.GetId() ;
						type.m_targetCreature = targetMonster;       //set the target
						//type.m_stateMachine.ChangeState(PetPursueState.getInstance());
						return false;
					}
					else{
						targetMonster = EnitityMgr.GetInstance().FindDangerousMonster() ;
						if(type.m_targetCreature != null){
							type.m_targetCreature = targetMonster ;
							type.m_stateMachine.ChangeState(PetPursueState.getInstance());
						}
						return true ;
					}
				}
				//
				type.m_stateMachine.ChangeState(PetMoveState.getInstance());
				return true ;
			}
		}

		public void Action(CPet type, float time){
			if(type.m_targetCreature!=null && type.m_targetCreature.GetRenderObject()!=null){
				if(type.m_targetCreature.GetEnitityAiState() != AIState.AI_STATE_WEAK && type.m_targetCreature.GetEnitityAiState() != AIState.AI_STATE_DEATH){
					//type.m_data.curAttackCD = -1.0f ;
					List<SkillLimit> skillDic = type.GetCanUseSkillList2();
					//have skill
					if(skillDic.Count > 0){
						//can use skill
						//int random = Random.Range(0,100);
						CPetSkill oneSkill = null;
						int skillID = GetSkill (skillDic);
						if(skillID != 0)
							oneSkill = type.GetSkillByID(skillID);
						
						if(oneSkill != null && type.IsSkillEffect(oneSkill))
						{
							type.Play("skill2",WrapMode.Once);
							type.m_curUsingSkill = oneSkill.GetSkillData().id ;
							//talk
							/*PetMoudleData  petMoudleData = (PetMoudleData)common.fileMgr.GetInstance().GetData(type.m_data.moudleID, common.CsvType.CSV_TYPE_PET);
							
							type.TalkLv(petMoudleData.talkLvInSkill, petMoudleData.talkIDInSkill);*/
							
							GameObject effectOb = null ;
							GameObject sceneOb  = null ;
							effectOb = gameGlobal.g_rescoureMgr.GetGameObjectResource("shifa") as GameObject;
							sceneOb  = GameObject.Instantiate(effectOb) as GameObject;
							
							Transform sceneCreature = sceneOb.transform.FindChild(gameGlobal.CREATURE) ;
							Transform destCreature = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT) ;
							
							//attach point
							if(sceneCreature != null && destCreature != null){
								Amin_2D_Ex aminEx = sceneCreature.GetComponent<Amin_2D_Ex>() ;
								if(aminEx.attachPos != ""){
									sceneOb.transform.position = destCreature.FindChild(aminEx.attachPos).position ;
									sceneOb.transform.parent = destCreature.FindChild(aminEx.attachPos);
								}
								else{
									sceneOb.transform.position = type.GetRenderObject().transform.position ;
									sceneOb.transform.parent = type.GetRenderObject().transform ;
								}
							}
							else{
								sceneOb.transform.position = type.GetRenderObject().transform.position ;
								sceneOb.transform.parent = type.GetRenderObject().transform ;
							}
							sceneOb.transform.FindChild("creature").animation.Play("effect");
						}
						//attack
						else{
							type.Play("attack",WrapMode.Once);
						}
					}
					//attack
					else{
						type.Play("attack",WrapMode.Once);
					}
				}
			}
		}

		public void Exit(CPet type){
			//type.Stop();
		}

		public void OnMessage(CPet type, EventMessageBase data){
			EnitityAction action = (EnitityAction)data.eventMessageAction;
			if(action == EnitityAction.ENITITY_ACTION_FIGHT){
				
				
				
			}
			//attack finish
			else if(action == EnitityAction.ENITITY_ACTION_FIGHT_FINISH){

				type.Play("stand",WrapMode.Loop);
				
				//type.Play("attack",WrapMode.Once);
			}
			//attack
			else if(action == EnitityAction.ENITITY_ACTION_FIGHT_SATRT){
				EventMessageFightStart fightStartMessage = (EventMessageFightStart)data ;
				if(type.m_targetCreature==null || type.m_targetCreature.GetRenderObject()==null){
					return;
				}
				
				//near
				if(type.m_data.attackType == AttackType.ATTACK_TYPE_NEAR){
					EventMessageFight message = new EventMessageFight();
					message.scrCreatureId = type.id ;
					message.destCreatureId= type.m_targetCreature.GetId() ;
					message.audioName = fightStartMessage.audioName ;
					message.beEffectName = fightStartMessage.beEffectName ;
					EventMgr.GetInstance().OnEventMgr(message);
				}
				//far
				else if(type.m_data.attackType == AttackType.ATTACK_TYPE_FAR){
					PetMoudleData petMoudleData = common.fileMgr.GetInstance().GetData(type.m_data.moudleID,common.CsvType.CSV_TYPE_PET) as PetMoudleData ;
					//ResourceMoudleData resData  = common.fileMgr.GetInstance().GetData(petMoudleData.attackArea,common.CsvType.CSV_TYPE_RESOUCE) as ResourceMoudleData ;
					
					//type.m_data.attackArea ;
					BulletData bulletData = new BulletData();
					bulletData.scrID = type.GetId();
					bulletData.destID= type.m_targetCreature.GetId() ;
					if(petMoudleData.AttackEffectID != -1){
						bulletData.effectID = petMoudleData.AttackEffectID * 10 + 2;
						bulletData.effectEndID = petMoudleData.AttackEffectID * 10 + 3 ;
					}
					else{
						bulletData.effectID = 400032;
						bulletData.effectEndID = 400033 ;
						common.debug.GetInstance().Error("attack effect id error pet id:" + petMoudleData.ID);
					}
					
					bulletData.audioPath = fightStartMessage.audioName ; 
					bulletData.pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ATTACK).position ;
					CCreature creatureBullet = EnitityMgr.GetInstance().CreateEnitity(EnitityType.ENITITY_TYPE_BULLET,bulletData);
					//creatureBullet.GetRenderObject().transform.position = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_BODY).position ;
					//creatureBullet.GetRenderObject().transform.FindChild("creature").animation.Play("effect");
				}
				//type.m_data.curAttackCD = 0.0f ;
			}
			else if(action == EnitityAction.ENITYTY_ACTION_SKILL_START){
				if(type.m_curUsingSkill != -1 ){
					CSkillBass skill = SkillMgr.GetInstance().GetSkill(type.m_curUsingSkill);
					skill.PlayStartEffect(type.GetId());
				}
			}
			else if(action == EnitityAction.ENITYTY_ACTION_SKILL){
				if(type.m_curUsingSkill != -1 ){
					if(type.m_targetCreature != null && type.m_targetCreature.GetRenderObject() != null){
						CSkillBass skill = SkillMgr.GetInstance().GetSkill(type.m_curUsingSkill);
						skill.useSkill(type.m_targetCreature.GetId());
					}
					//type.m_data.curAttackCD = 0.0f ;
				}
			}
			else if(action == EnitityAction.ENITITY_ACTION_SKILL_FINISH){
				type.m_data.curAttackCD = 0.0f ;
				type.Play("stand",WrapMode.Loop);
			}
			/*else if(action == EnitityAction.ENITITY_ACTION_LOCK_PET){
				EventMessageLockPet lockPetMessage = data as EventMessageLockPet ;
				CCreature creaure = EnitityMgr.GetInstance().GetEnitity(lockPetMessage.lockMonsterID) ;
				if(creaure != null){
					type.m_targetCreature = creaure ;
				}
			}*/
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_STAND ;
		}
		
		public static PetAttackState getInstance(){
			if(instance==null){instance = new PetAttackState();}
			return instance;
		}
	}
}

