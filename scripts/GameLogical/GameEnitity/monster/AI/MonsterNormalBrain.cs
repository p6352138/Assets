/*using UnityEngine;
using System.Collections;
using GameEvent ;
namespace GameLogical.GameEnitity.AI{
	public class MonsterNormalBrain : CBassBrian
	{
		protected	CMonster	m_monster	;
		
		public void Init(CMonster monster){
			m_monster = monster ;
		}
		
		
		public void ThinkAbout(){
			CStateBase<CMonster> state = null;
			MonsterAIDataBass    data  = null;
			switch(m_monster.aiState){
			case AIState.AI_STATE_MOVE:{
				MoveThink(ref state, ref data);
			}
				break ;
			case AIState.AI_STATE_ACTTACK:{
				AttackThink(ref state,ref data);
			}
				break ;
			case AIState.AI_STATE_ACTTACK_CITY:{
				AttackCityThink(ref state,ref data);
			}
				break ;
			case AIState.AI_STATE_ESCAPE:{
				EscapeThink();
			}
				break ;
			}

			if(state != null){
				AIState stateType = (AIState)state.GetState() ;
				//MonoBehaviour.print("monster:" + m_monster.GetRenderObject().name + "stateType:" + stateType.ToString() + "data:" + data);
				
				m_monster.m_monsterAIData = data ;
				m_monster.SetState(state);
			}
		}
		
		public void OnMessage(EventMessageBase message){
			switch((EnitityAction)message.eventMessageAction){
			case EnitityAction.ENITITY_ACTION_ENTER_COLLIDER:{
				HandleEnterColliderMessage();
			}
				break ;
			case EnitityAction.ENITITY_ACTION_FIGHT_SATRT:{
				HandleFightStartMessage();
			}
				break ;
			case EnitityAction.ENITITY_ACTION_FIGHT:{
				HandleFightMessage(message);
			}
				break ;
			case EnitityAction.ENITITY_ACTION_FIGHT_FINISH:{
				this.ThinkAbout();
			}
				break ;
			case EnitityAction.ENITITY_ACTION_DEATH:{
				HandleDeathMessage();
			}
			break ;
				
				
			}

		}
		
		//////////////////////////////handle state think//////////////////////////////
		//move
		void MoveThink( ref CStateBase<CMonster> state, ref MonsterAIDataBass aiData){
			MonsterMoveStateData moveData = m_monster.m_monsterAIData as MonsterMoveStateData ;
				
			CCreature destObject = EnitityMgr.GetInstance().GetEnitity(moveData.destObjectId) ;
			
			if(destObject != null){
				if(m_monster.GetAttackAreaList().Contains(moveData.destObjectId)){
					CCreature creature = EnitityMgr.GetInstance().GetEnitity(moveData.destObjectId);
					if(creature.GetEnitityType() == EnitityType.ENITITY_TYPE_CITY){
						if(creature.GetEnitityAiState() == AIState.AI_STATE_DOOR_NARMOL){
							CMonsterAttackCityState attackState = new CMonsterAttackCityState() ;
							MonsterAttackStateData stateData = new MonsterAttackStateData();
							//stateData.levelTargetObjectId = m_monster.m_monsterAIData.levelTargetObjectId ;
							stateData.targetObjectId = moveData.destObjectId ;
		
							state = attackState ;
							aiData= stateData ;
						}
						else if(creature.GetEnitityAiState() == AIState.AI_STATE_DOOR_BREAK){
							CMonsterEscapeState escapeState = new CMonsterEscapeState();
							MonsterMoveStateData data = new MonsterMoveStateData();
							data.destPos = m_monster.GetRenderObject().transform.position;
							data.destPos.x = 20.0f ;
							state = escapeState ;
						}
	
					}
					else{
						CMonsterAttackState attackState = new CMonsterAttackState() ;
						MonsterAttackStateData stateData = new MonsterAttackStateData();
						//stateData.levelTargetObjectId = m_monster.m_monsterAIData.levelTargetObjectId ;
						stateData.targetObjectId = moveData.destObjectId ;
	
						state = attackState ;
						aiData= stateData ;
					}
	
				}
			}
			else{
				CCreature target = null ;
				if(m_monster.GetAttackAreaList().Count != 0){
					CMonsterAttackState attackState = new CMonsterAttackState() ;
					MonsterAttackStateData stateData = new MonsterAttackStateData();
					//stateData.levelTargetObject = m_monster.m_monsterAIData.levelTargetObject ;
					stateData.targetObjectId = (int)m_monster.GetAttackAreaList()[0] ;

					state = attackState ;
					aiData= stateData ;
				}
				//had find move to target 
				else if(m_monster.GetEyeShotList().Count != 0){
					
					CMonsterMoveState moveState = new CMonsterMoveState() ;
					MonsterMoveStateData stateData = new MonsterMoveStateData();
					//stateData.levelTargetObject = moveData.levelTargetObject ;
					stateData.destObjectId =  (int)m_monster.GetEyeShotList()[0];

					state = moveState ;
					aiData= stateData ;
				}
				//move to level targert
				else{
					target = EnitityMgr.GetInstance().city ;
					
					CMonsterMoveState moveState = new CMonsterMoveState() ;
					MonsterMoveStateData stateData = new MonsterMoveStateData();
					//stateData.levelTargetObject = moveData.levelTargetObject ;
					stateData.destObjectId =  target.GetId();

					state = moveState ;
					aiData= stateData ;
				}
			}

		}
		
		//attack
		void AttackThink(ref CStateBase<CMonster> state, ref MonsterAIDataBass aiData){
			MonsterAttackStateData attackData = m_monster.m_monsterAIData as MonsterAttackStateData;
			//miss target,maybe death or disappear
			CCreature targetObject = EnitityMgr.GetInstance().GetEnitity(attackData.targetObjectId) ;
			
			if(targetObject == null 
				|| targetObject.GetEnitityAiState() == AIState.AI_STATE_WEAK
				|| targetObject.GetEnitityAiState() == AIState.AI_STATE_DEATH){
				//find another target
				CCreature target = null;
				//float	  dis	 = float.MaxValue;
				//EnitityMgr.GetInstance().FindNearCreature(m_monster,EnitityType.ENITITY_TYPE_PET,ref target,ref dis);

				//had find target, attack target or move to target
				//had find attack target 
				if(m_monster.GetAttackAreaList().Count != 0){
					CMonsterAttackState attackState = new CMonsterAttackState() ;
					MonsterAttackStateData stateData = new MonsterAttackStateData();
					//stateData.levelTargetObjectId = m_monster.m_monsterAIData.levelTargetObjectId ;
					stateData.targetObjectId = (int)m_monster.GetAttackAreaList()[0] ;

					state = attackState ;
					aiData= stateData ;
				}
				//had find move to target 
				else if(m_monster.GetEyeShotList().Count != 0){
					//target = (int)m_monster.GetEyeShotList()[0] ;
					if(target!=null && target.GetEnitityAiState() != AIState.AI_STATE_DEATH){
						CMonsterMoveState moveState = new CMonsterMoveState() ;
						MonsterMoveStateData stateData = new MonsterMoveStateData();
						//stateData.levelTargetObjectId = aiData.levelTargetObjectId ;
						stateData.destObjectId =   (int)m_monster.GetEyeShotList()[0];
	
						state = moveState ;
						aiData= stateData ;
						return ;
					}
					//can not find one,move on
					else{
						CMonsterMoveState moveState = new CMonsterMoveState() ;
						MonsterMoveStateData stateData = new MonsterMoveStateData();
						stateData.destPos = EnitityMgr.GetInstance().city.GetRenderObject().transform.position ;
						//stateData.levelTargetObjectId = attackData.levelTargetObjectId ;
						//stateData.destObjectId = attackData.levelTargetObjectId ;
						state = moveState ;
						aiData= stateData ;
					}

				}
				//can not find one,move on
				else{
					CMonsterMoveState moveState = new CMonsterMoveState() ;
					MonsterMoveStateData stateData = new MonsterMoveStateData();
					stateData.destPos = EnitityMgr.GetInstance().city.GetRenderObject().transform.position ;
					//stateData.levelTargetObjectId = attackData.levelTargetObjectId ;
					//stateData.destObjectId = attackData.levelTargetObjectId ;
					state = moveState ;
					aiData= stateData ;
				}
			}
			//continue attack
			else{
				//check target is in the attack area
				//CCreature targetObject = EnitityMgr.GetInstance().GetEnitity(attackData.targetObjectId);
				if(m_monster.GetAttackAreaList().Count != 0){
					if(attackData.targetObjectId == (int)m_monster.GetAttackAreaList()[0])
						m_monster.ExecuteStateAgain();
					else{
						CCreature creature = EnitityMgr.GetInstance().GetEnitity((int)m_monster.GetAttackAreaList()[0]);
						if(creature.GetEnitityType() == EnitityType.ENITITY_TYPE_CITY){
							if(creature.GetEnitityAiState() == AIState.AI_STATE_DOOR_NARMOL){
								CMonsterAttackCityState attackState = new CMonsterAttackCityState() ;
								MonsterAttackStateData stateData = new MonsterAttackStateData();
								//stateData.levelTargetObjectId = m_monster.m_monsterAIData.levelTargetObjectId ;
								stateData.targetObjectId = (int)m_monster.GetAttackAreaList()[0] ;
			
								state = attackState ;
								aiData= stateData ;
							}
							else if(creature.GetEnitityAiState() == AIState.AI_STATE_DOOR_BREAK){
								CMonsterEscapeState escapeState = new CMonsterEscapeState();
								MonsterMoveStateData data = new MonsterMoveStateData();
								data.destPos = m_monster.GetRenderObject().transform.position;
								data.destPos.x = 20.0f ;
								state = escapeState ;
							}
	
						}
						else{
							CMonsterAttackState attackState = new CMonsterAttackState() ;
							MonsterAttackStateData stateData = new MonsterAttackStateData();
							//stateData.levelTargetObjectId = m_monster.m_monsterAIData.levelTargetObjectId ;
							stateData.targetObjectId = (int)m_monster.GetAttackAreaList()[0] ;
		
							state = attackState ;
							aiData= stateData ;
						}
					}
				}
				//float dis = Vector3.Distance(m_monster.m_object.transform.position,targetObject.GetRenderObject().transform.position) ;
				//if(dis < m_monster.attackArea){
				
				//	m_monster.ExecuteStateAgain();
				//}
				else{
					CMonsterMoveState moveState = new CMonsterMoveState() ;
					MonsterMoveStateData stateData = new MonsterMoveStateData();
					//stateData.levelTargetObjectId = attackData.levelTargetObjectId ;
					stateData.destObjectId = targetObject.GetId() ;
					//m_monster.m_monsterAIData = stateData ;
					//m_monster.SetState(moveState);
					state = moveState ;
					aiData= stateData ;
				}
			}
		}
		
		void AttackCityThink(ref CStateBase<CMonster> state, ref MonsterAIDataBass aiData){
			MonsterAttackStateData attackData = m_monster.m_monsterAIData as MonsterAttackStateData;
			CCreature targetObject = EnitityMgr.GetInstance().GetEnitity(attackData.targetObjectId) ;
			if(targetObject.GetEnitityAiState() == AIState.AI_STATE_DOOR_BREAK){
				CMonsterEscapeState escapeState = new CMonsterEscapeState();
				MonsterMoveStateData data = new MonsterMoveStateData();
				data.destPos = m_monster.GetRenderObject().transform.position;
				data.destPos.x = 20.0f ;
				state = escapeState ;
				
			}
			else{
				m_monster.ExecuteStateAgain();
			}
		}
		
		void EscapeThink(){
			EventMessageMonsterEscape escapeMessage = new EventMessageMonsterEscape();
			escapeMessage.id = m_monster.id ;
			EventMgr.GetInstance().OnEventMgr(escapeMessage);
			EnitityMgr.GetInstance().DestroyEnitity(m_monster);
		}
		
		//////////////////////////////handle on message//////////////////////////////
		//enter collider
		void HandleEnterColliderMessage(){
			switch(m_monster.aiState){
			case AIState.AI_STATE_MOVE:{
				MonsterMoveStateData data = m_monster.m_monsterAIData as MonsterMoveStateData ;
				//have not target
				if(data.destObjectId == -1 ){
					data.destObjectId = (int)m_monster.m_eyeShotObjectList[0] ;
				}
			}
				break ;
			}
		}
		
		//Fight start
		void HandleFightStartMessage(){
			switch(m_monster.aiState){
			case AIState.AI_STATE_ACTTACK:
			case AIState.AI_STATE_ACTTACK_CITY:{
				MonsterAttackStateData data = m_monster.m_monsterAIData as MonsterAttackStateData ;
				EventMessageFight message = new EventMessageFight();
				message.scrCreatureId = m_monster.id ;
				message.destCreatureId= data.targetObjectId ;
				EventMgr.GetInstance().OnEventMgr(message);
			}
				break ;
			}
		}
		
		//real fight
		void HandleFightMessage(EventMessageBase message){
			EventMessageFight fight = message as EventMessageFight ;
			//be hurt
			if(fight.destCreatureId == m_monster.id){
				CCreature creature = EnitityMgr.GetInstance().GetEnitity(fight.scrCreatureId);
				switch(creature.GetEnitityType()){
				case EnitityType.ENITITY_TYPE_PET:{
					CPet pet = creature as CPet;
					m_monster.blood -= pet.attack ;
					m_monster.BeAttack();
				}
					break ;
				}
			}
			else if(fight.scrCreatureId == m_monster.id){
				this.ThinkAbout();
			}
		}
		
		//death
		void HandleDeathMessage(){
			EnitityMgr.GetInstance().DestroyEnitity(m_monster);
		}
	}
}*/


