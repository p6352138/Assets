using UnityEngine;
using System.Collections;
using GameEvent ;
using GameLogical.GameSkill;
using GameLogical.GameSkill.Skill ;
using System.Collections.Generic ;
using common;

namespace GameLogical.GameEnitity.AI
{
	public class MonsterAttackState: CStateBase<CMonster>{
		protected static MonsterAttackState instance;
		public void Release(){
			
		}
		public void Enter(CMonster type){

			
			/*List<CCreature> monsterList = EnitityMgr.GetInstance().GetMonsterList();
			float tempDis ;
			for(int i = 0; i<monsterList.Count; ++i){
				//not come back
				tempDis = Vector3.Distance(monsterList[i].GetRenderObject().transform.position,type.GetRenderObject().transform.position) ;
				if(tempDis<type.m_data.misPlace && monsterList[i].GetEnitityAiState() == AIState.AI_STATE_ACTTACK &&
				   monsterList[i].GetId() != type.GetId()){
					CPet pet = type.m_targetCreature as CPet ;
					pet.RemoveAttack(type.GetId());
					type.m_stateMachine.ChangeState(MonsterMisplaceState.getInstance());
					return;
				}
			}*/
			
			Think(type);

			FirstThink(type);

			type.getMonsterData().curAttackCD = 0.0f ;
			List<CMonsterSkill> skills =  type.GetCanUseSkillList();
			//have skill
			if(skills.Count > 0){
				int random = Random.Range(0,100);
				//can use skill
				if(random < 30){
					if(skills.Count != 0){
						int skillRandom = Random.Range(0,skills.Count * 10);
						int index = skillRandom / 10 ;
						type.m_curUsingSkill = skills[index].GetSkillData().id ;
						type.Play("skill2",WrapMode.Once);
						
						//talk
						/*MonsterMoudleData monstermoudle = (MonsterMoudleData)common.fileMgr.GetInstance().GetData(type.m_data.moudleID,
							common.CsvType.CSV_TYPE_MONSTER);
						type.TalkLv(monstermoudle.talkLvInSkill, monstermoudle.talkIDInSkill);*/
					}
				}
				//attack
				else{
					type.Play("attack",WrapMode.Once);
					//talk
					/*MonsterMoudleData monstermoudle = (MonsterMoudleData)common.fileMgr.GetInstance().GetData(type.m_data.moudleID,
						common.CsvType.CSV_TYPE_MONSTER);
					type.TalkLv(monstermoudle.talkLvInAttack, monstermoudle.talkIDInAttack);*/
				}
			}
			//attack
			else{
				type.Play("attack",WrapMode.Once);
				//talk
				/*MonsterMoudleData monstermoudle = (MonsterMoudleData)common.fileMgr.GetInstance().GetData(type.m_data.moudleID,
					common.CsvType.CSV_TYPE_MONSTER);
				type.TalkLv(monstermoudle.talkLvInAttack, monstermoudle.talkIDInAttack);*/
			}

			type.m_monsterAIData.time = 0.0f ;
		}
		public void Execute(CMonster type, float time){
			type.getMonsterData().curAttackCD += time ;
			if(type.getMonsterData().curAttackCD >= type.getMonsterData().attackCD){
				if(!Think(type)){
					Action(type,time);
				}

				type.getMonsterData().curAttackCD = 0.0f ;
			}


		}

		public bool Think(CMonster type){

			if(type.m_targetCreature!=null && type.m_targetCreature.GetRenderObject()!=null ){
				if(type.m_targetCreature.GetEnitityAiState() != AIState.AI_STATE_WEAK && type.m_targetCreature.GetEnitityAiState() != AIState.AI_STATE_DEATH){
					float dis = type.m_targetCreature.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_BE_ATTACK_FORWARD).position.x - type.GetRenderObject().transform.position.x ;
					float disVecX = Mathf.Abs(dis);
					dis = type.m_targetCreature.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_BE_ATTACK_BACK).position.x - type.GetRenderObject().transform.position.x ;
					if(disVecX > Mathf.Abs(dis)){
						disVecX = Mathf.Abs(dis) ;
					}

					float disY = type.m_targetCreature.GetRenderObject().transform.position.y - type.GetRenderObject().transform.position.y ;
					if(disVecX > type.attackArea){
						type.m_stateMachine.ChangeState(MonsterPursueState.getInstance());
						return true ;
					}
					/*else if(Mathf.Abs( disY ) > AICommon.AI_ATTACK_Y_GAP){
						type.m_stateMachine.ChangeState(MonsterChangeWayState.getInstance());

						return true ;
					}*/
					else if(Mathf.Abs(disY) > AICommon.AI_MONSTER_MISPLACE && type.m_stateMachine.GetPreviosState().GetState() != AIState.AI_STATE_MOVE_Y
					        && type.m_stateMachine.GetPreviosState().GetState() != AIState.AI_STATE_MOVE_X){
						type.m_stateMachine.ChangeState(MonstYMoveState.getInstance());
						return true ;
					}
					else{
						Vector3 moveVec = type.GetRenderObject().transform.position - type.m_targetCreature.GetRenderObject().transform.position ;
						
						if(moveVec.x < 0){
							type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_ROOT).localRotation = Quaternion.LookRotation(Vector3.forward) ;
							Vector3 pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition ;
							pos.z = -0.5f ;
							type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition = pos ;

							pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT).localPosition ;
							pos.z = -0.2f ;
							type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT).localPosition = pos ;
						}else if(moveVec.x > 0){
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
				else{
					type.m_stateMachine.ChangeState(MonsterMoveState.getInstance());
					return true ;
				}
			}else{
				type.m_stateMachine.ChangeState(MonsterMoveState.getInstance());
				return true ;
			}
		}

		public void FirstThink(CMonster type){
			List<CCreature> monsterList = EnitityMgr.GetInstance().GetMonsterList();
			for(int i = 0; i<monsterList.Count; ++i){
				CMonster sameWayMonster = monsterList[i] as CMonster ;
				if(sameWayMonster.m_targetCreature != type.m_targetCreature || sameWayMonster.id == type.id)
					continue ;
				float disX = monsterList[i].GetRenderObject().transform.position.x - type.GetRenderObject().transform.position.x;
				float disY = monsterList[i].GetRenderObject().transform.position.y - type.GetRenderObject().transform.position.y;
				if(Mathf.Abs(disX) < AICommon.AI_MONSTER_X_MISPLACE && Mathf.Abs(disY) < AICommon.AI_MONSTER_MISPLACE
				   && sameWayMonster.GetEnitityAiState() == AIState.AI_STATE_ACTTACK
				   && type.m_stateMachine.GetPreviosState().GetState() != AIState.AI_STATE_MOVE_X &&
				   type.GetRenderObject().transform.position.x > type.m_targetCreature.GetRenderObject().transform.position.x)
				{
					type.m_stateMachine.ChangeState(MonstXMoveState.getInstance());
				}
			}
		}

		public void Action(CMonster type, float time){
			List<CMonsterSkill> skills =  type.GetCanUseSkillList();
			//have skill
			if(skills.Count > 0){
				int random = Random.Range(0,100);
				//can use skill
				//if(random < 30){
					if(skills.Count != 0){
						//int skillRandom = Random.Range(0,skills.Count * 10);
						//int index = skillRandom / 10 ;
						type.m_curUsingSkill = skills[0].GetSkillData().id ;
						type.Play("skill2",WrapMode.Once);
					}
				//}
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
		public void Exit(CMonster type){
			
		}
		public void OnMessage(CMonster type, EventMessageBase data){
		
			EnitityAction action = (EnitityAction)data.eventMessageAction;
			//attack
			if(action == EnitityAction.ENITITY_ACTION_FIGHT){

			}
			
			else if(action == EnitityAction.ENITITY_ACTION_FIGHT_FINISH){
				type.Play("stand",WrapMode.Loop);
			}
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
					MonsterMoudleData monsterMoudleData = common.fileMgr.GetInstance().GetData(type.m_data.moudleID,common.CsvType.CSV_TYPE_MONSTER) as MonsterMoudleData ;
					BulletData bulletData = new BulletData();
					bulletData.scrID = type.GetId();
					bulletData.destID= type.m_targetCreature.GetId() ;
					if(monsterMoudleData.AttackEffectID != -1){
						bulletData.effectID = monsterMoudleData.AttackEffectID * 10 + 2;
						bulletData.effectEndID = monsterMoudleData.AttackEffectID * 10 + 3 ;
					}
					else{
						bulletData.effectID = 400032;
						bulletData.effectEndID = 400033 ;
						common.debug.GetInstance().Error("attack effect id error pet id:" + monsterMoudleData.ID);
					}
					bulletData.audioPath = fightStartMessage.audioName ; 
					bulletData.pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ATTACK).position ;
					CCreature creatureBullet = EnitityMgr.GetInstance().CreateEnitity(EnitityType.ENITITY_TYPE_BULLET,bulletData);
					//creatureBullet.GetRenderObject().transform.position = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_BODY).position ;
					//creatureBullet.GetRenderObject().transform.FindChild("creature").animation.Play("effect");
				}
			}
			//skill
			else if(action == EnitityAction.ENITYTY_ACTION_SKILL){
				if(type.m_curUsingSkill != -1 ){
					if(type.m_targetCreature != null){
						CSkillBass skill = SkillMgr.GetInstance().GetSkill(type.m_curUsingSkill);
						skill.useSkill(type.m_targetCreature.GetId());
						//type.Play("stand",WrapMode.Loop);
					}
				}
			}
			else if(action == EnitityAction.ENITYTY_ACTION_SKILL_START){
				if(type.m_curUsingSkill != -1 ){
					CSkillBass skill = SkillMgr.GetInstance().GetSkill(type.m_curUsingSkill);
					skill.PlayStartEffect(type.GetId());
				}
			}
			else if(action == EnitityAction.ENITITY_ACTION_SKILL_FINISH){
				type.m_data.curAttackCD = 0.0f ;
				type.Play("stand",WrapMode.Loop);
			}
		}

		public AIState  GetState(){
			return AIState.AI_STATE_ACTTACK ;
		}
		public static MonsterAttackState getInstance(){
			if(instance ==null){
				instance = new MonsterAttackState();
			}
			
			return instance;
		}


		/*public Vector3 FindFitPosition(CMonster type){
			List<CCreature> monsterList = EnitityMgr.GetInstance().GetMonsterList() ;
			for(int i = 0; i<monsterList.Count ; ++i){

			}
		}*/
	}
}

