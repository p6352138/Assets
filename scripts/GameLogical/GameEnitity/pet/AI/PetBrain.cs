/*using UnityEngine;
using System.Collections;
using GameEvent ;

namespace GameLogical.GameEnitity.AI{
	public class PetBrain : CBassBrian
	{
		protected	CPet	m_pet	;
		
		public void Init(CPet pet){
			m_pet = pet ;
		}
		
		public void ThinkAbout(){
			CStateBase<CPet> state = null;
			PetAIDataBass    data  = null;
			switch(m_pet.GetEnitityAiState()){
			case AIState.AI_STATE_MOVE:{
				MoveThink(ref state,ref data);
			}
				break ;
				
			case AIState.AI_STATE_PATRAL:{
				PatralThink(ref state,ref data);
			}
				break ;
				
			case AIState.AI_STATE_ACTTACK:{
				AttackThink(ref state,ref data);
			}
				break ;
			}
			
			if(state != null){
				m_pet.m_petAIData = data ;
				m_pet.SetState(state);
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
		
		/// <summary>
		/// Patrals the think.
		/// </summary>
		/// <param name='state'>
		/// State.
		/// </param>
		/// <param name='aiData'>
		/// Ai data.
		/// </param>
		void PatralThink(ref CStateBase<CPet> state, ref PetAIDataBass aiData){
			//find enemy
			//had find attack target 
			//CCreature target = null;
			if(m_pet.GetAttackAreaList().Count != 0){
				CPetAttackState attackState = new CPetAttackState() ;
				PetAttackStateData stateData = new PetAttackStateData();
				stateData.targetObjectId = (int)m_pet.GetAttackAreaList()[0] ;

				state = attackState ;
				aiData= stateData ;
				return ;
			}
			//had find move to target 
			else if(m_pet.GetEyeShotList().Count != 0){
				CPetMoveState moveState = new CPetMoveState() ;
				PetMoveAIData stateData = new PetMoveAIData();
				stateData.destObjectId =  (int)m_pet.GetEyeShotList()[0] ;;

				state = moveState ;
				aiData= stateData ;
				return ;
			}
			
			PetPatrolAIData data = m_pet.m_petAIData as PetPatrolAIData ;
			Vector3 destPos = data.patrolPathList[data.destPathIndex] ;
			float dis = Vector3.Distance(destPos,m_pet.GetRenderObject().transform.position);
			if(dis<0.5f){
				if(data.destPathIndex < data.patrolPathList.Length - 1){
					data.destPathIndex ++ ;
				}
				else{
					data.destPathIndex = 0 ;
				}
			}
			//m_pet.m_petAIData = data ;
		}
		
		/// <summary>
		/// Moves the think.
		/// </summary>
		/// <param name='state'>
		/// State.
		/// </param>
		/// <param name='aiData'>
		/// Ai data.
		/// </param>
		void MoveThink(ref CStateBase<CPet> state, ref PetAIDataBass aiData){
			PetMoveAIData moveData = m_pet.m_petAIData as PetMoveAIData ;
				
			CCreature destObject = EnitityMgr.GetInstance().GetEnitity(moveData.destObjectId);
			//free move,find target
			if(destObject == null){
				//find near target
				//had find attack target 
				if(m_pet.GetAttackAreaList().Count != 0){
					CPetAttackState attackState = new CPetAttackState() ;
					PetAttackStateData stateData = new PetAttackStateData();
					//stateData.levelTargetObject = m_monster.m_monsterAIData.levelTargetObject ;
					stateData.targetObjectId = (int)m_pet.GetAttackAreaList()[0] ;

					state = attackState ;
					aiData= stateData ;
				}
				//had find move to target 
				else if(m_pet.GetEyeShotList().Count != 0){
					
					CPetMoveState moveState = new CPetMoveState() ;
					PetMoveAIData stateData = new PetMoveAIData();
					//stateData.levelTargetObject = moveData.levelTargetObject ;
					stateData.destObjectId =  (int)m_pet.GetEyeShotList()[0];

					state = moveState ;
					aiData= stateData ;
					return ;
				}
				//move to level targert
				else{
					
					CPetPatrolState patrolState = new CPetPatrolState();
			
					PetPatrolAIData patrolData = new PetPatrolAIData();
					patrolData.patrolPathList[0] = new Vector3(Random.Range(24.5f,37.5f),Random.Range(28.0f,45.0f),0.0f);
					patrolData.patrolPathList[1] = new Vector3(Random.Range(24.5f,37.5f),Random.Range(28.0f,45.0f),0.0f);
					patrolData.destPathIndex = 0 ;
					
					state = patrolState ;
					aiData= patrolData ;
				}
			}
			//can not find one, move on
			else{

				if(m_pet.GetAttackAreaList().Contains(moveData.destObjectId)){
					CPetAttackState attackState = new CPetAttackState() ;
					PetAttackStateData stateData = new PetAttackStateData();
					//stateData.levelTargetObject = m_monster.m_monsterAIData.levelTargetObject ;
					stateData.targetObjectId = moveData.destObjectId ;

					state = attackState ;
					aiData= stateData ;
				}
			}
		}
		
		/// <summary>
		/// Attacks the think.
		/// </summary>
		/// <param name='state'>
		/// State.
		/// </param>
		/// <param name='aiData'>
		/// Ai data.
		/// </param>
		void AttackThink(ref CStateBase<CPet> state, ref PetAIDataBass aiData){
			PetAttackStateData attackData = m_pet.m_petAIData as PetAttackStateData;
			CCreature targetObject = EnitityMgr.GetInstance().GetEnitity(attackData.targetObjectId);
			//miss target,maybe death or disappear
			if(targetObject == null  
				|| targetObject.GetEnitityAiState() == AIState.AI_STATE_WEAK){
				//had find attack target 
				if(m_pet.GetAttackAreaList().Count != 0){
					CPetAttackState attackState = new CPetAttackState() ;
					PetAttackStateData stateData = new PetAttackStateData();
					stateData.targetObjectId = (int)m_pet.GetAttackAreaList()[0] ;
	
					state = attackState ;
					aiData= stateData ;
				}
				//had find move to target 
				else if(m_pet.GetEyeShotList().Count != 0){
					CPetMoveState moveState = new CPetMoveState() ;
					PetMoveAIData stateData = new PetMoveAIData();
					stateData.destObjectId =  (int)m_pet.GetEyeShotList()[0] ;
	
					state = moveState ;
					aiData= stateData ;
					return ;
				}
				//can not find one,move on
				else{
					CPetPatrolState patrolState = new CPetPatrolState();
			
					PetPatrolAIData patrolData = new PetPatrolAIData();
					patrolData.patrolPathList[0] = new Vector3(Random.Range(24.5f,37.5f),Random.Range(28.0f,45.0f),0.0f);
					patrolData.patrolPathList[1] = new Vector3(Random.Range(24.5f,37.5f),Random.Range(28.0f,45.0f),0.0f);
					patrolData.destPathIndex = 0 ;
					
					state = patrolState ;
					aiData= patrolData ;
				}

			}
			//continue attack
			else{
				//check target is in the attack area
				float dis = Vector3.Distance(m_pet.m_object.transform.position,targetObject.GetRenderObject().transform.position) ;
				if(dis < m_pet.attackArea){

					m_pet.ExecuteStateAgain();
				}
				else{
					CPetMoveState moveState = new CPetMoveState() ;
					PetMoveAIData stateData = new PetMoveAIData();
					stateData.destObjectId = targetObject.GetId() ;
					state = moveState ;
					aiData= stateData ;
				}
			}
		}
		
		
		//////////////////////////////handle on message//////////////////////////////
		//enter collider
		void HandleEnterColliderMessage(){
			switch(m_pet.aiState){
			case AIState.AI_STATE_MOVE:{
				PetMoveAIData data = m_pet.m_petAIData as PetMoveAIData ;
				//have not target
				if(data.destObjectId == -1 ){
					data.destObjectId = (int)m_pet.m_eyeShotObjectList[0] ;
				}
			}
				break ;
			}
		}
		
		//Fight start
		void HandleFightStartMessage(){
			switch(m_pet.aiState){
			case AIState.AI_STATE_ACTTACK:{
			//case AIState.AI_STATE_ACTTACK_CITY:{
				PetAttackStateData data = m_pet.m_petAIData as PetAttackStateData ;
				EventMessageFight message = new EventMessageFight();
				message.scrCreatureId = m_pet.id ;
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
			if(fight.destCreatureId == m_pet.id){
				CCreature creature = EnitityMgr.GetInstance().GetEnitity(fight.scrCreatureId);
				switch(creature.GetEnitityType()){
				case EnitityType.ENITITY_TYPE_MONSTER:{
					CMonster monster = creature as CMonster;
					m_pet.blood -= monster.attack ;
					m_pet.BeAttack();
					if(m_pet.blood < 0){
						CPetDeathState weakState = new CPetDeathState();
						m_pet.SetState(weakState);
					}
				}
					break ;
				}
			}
			else if(fight.scrCreatureId == m_pet.id){
				this.ThinkAbout();
			}
		}
		
		//death
		void HandleDeathMessage(){
			EnitityMgr.GetInstance().DestroyEnitity(m_pet);
		}
		
	}
}*/


