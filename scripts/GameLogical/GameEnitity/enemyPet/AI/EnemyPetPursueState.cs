using UnityEngine;
using System.Collections;
using GameEvent ;

namespace GameLogical.GameEnitity.AI{
	public class EnemyPetPursueState : CStateBase<CEnemyPet>
	{
		protected static EnemyPetPursueState instance;
		public void Release(){
			
		}
		public void Enter(CEnemyPet type){
			//talk
			//PetMoudleData  petMoudleData = (PetMoudleData)common.fileMgr.GetInstance().GetData(type.m_data.moudleID, common.CsvType.CSV_TYPE_PET);
			
			//type.TalkLv(petMoudleData.talkLvInSpeed, petMoudleData.talkIDInSpeed);
			
			type.Play("walk",WrapMode.Loop);
		}
		public void Execute(CEnemyPet type, float time){
			if(type.m_targetCreature!=null && type.m_targetCreature.GetRenderObject()!=null){
				Vector3 dis = type.m_targetCreature.GetRenderObject().transform.position - type.GetRenderObject().transform.position;
				//trun aroud
				float disVec = type.m_targetCreature.GetRenderObject().transform.position.x - type.GetRenderObject().transform.position.x ;
				float disY	 = type.m_targetCreature.GetRenderObject().transform.position.y - type.GetRenderObject().transform.position.y ;
				if(disVec > 0){
					type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_ROOT).localRotation = Quaternion.LookRotation(Vector3.forward) ;
					Vector3 pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition ;
					pos.z = -0.5f ;
					type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition = pos ;

					pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT).localPosition ;
					pos.z = -0.2f ;
					type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT).localPosition = pos ;
				}
				else{
					type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_ROOT).localRotation = Quaternion.LookRotation(Vector3.back) ;
					Vector3 pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition ;
					pos.z = 0.5f ;
					type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition = pos ;

					pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT).localPosition ;
					pos.z = 0.2f ;
					type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT).localPosition = pos ;
				}
				
				//on the attack area .change to attack state
				if(Mathf.Abs(disVec) < type.attackArea){
					type.m_stateMachine.ChangeState(EnemyPetAttackState.getInstance());
				}
				//move to target
				else{
					type.GetRenderObject().transform.position += dis.normalized * time * type.speed ;
					Vector3 typePos = type.GetRenderObject().transform.position ;
					typePos.z = typePos.y ;
					type.GetRenderObject().transform.position = typePos ;
				}
			}else{
				type.m_stateMachine.ChangeState(EnemyPetMoveState.getInstance());
			}
		}
		public void Exit(CEnemyPet type){
			
		}
		public void OnMessage(CEnemyPet type, EventMessageBase data){
	
		}
		public AIState  GetState(){
			return AIState.AI_STATE_PATRAL ;
		}
		
		public static EnemyPetPursueState getInstance(){
			if(instance==null){instance = new EnemyPetPursueState();}
			return instance;
		}
	}
}


