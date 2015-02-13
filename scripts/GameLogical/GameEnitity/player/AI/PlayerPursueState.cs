using UnityEngine;
using System.Collections;
using GameEvent;
using System.Collections.Generic;

namespace GameLogical.GameEnitity.AI
{
	public class PlayerPursueState: CStateBase<CPlayer>{
		protected static PlayerPursueState instance;
		public void Release(){
			
		}
		public void Enter(CPlayer type){
			//talk
			//PetMoudleData  petMoudleData = (PetMoudleData)common.fileMgr.GetInstance().GetData(type.m_data.moudleID, common.CsvType.CSV_TYPE_PET);
			
			//type.TalkLv(petMoudleData.talkLvInSpeed, petMoudleData.talkIDInSpeed);
			
			type.Play("stand",WrapMode.Loop);
		}
		public void Execute(CPlayer type, float time){
			if(type.m_targetCreature!=null && type.m_targetCreature.GetRenderObject()!=null){
				float dis = Vector3.Distance(type.GetRenderObject().transform.position,type.m_targetCreature.GetRenderObject().transform.position);
				//trun aroud
				Vector3 destPos  = type.m_targetCreature.GetRenderObject().transform.position;
				Vector3 disVec = destPos - type.GetRenderObject().transform.position ;
				if(disVec.x > 0){
					type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE).localRotation = Quaternion.LookRotation(Vector3.back) ;
					//type.GetRenderObject().transform.localRotation = Quaternion.LookRotation(Vector3.forward);
					
					//type.talkBlink.transform.localRotation = Quaternion.LookRotation(Vector3.forward);
					
					Vector3 pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition ;
					pos.z = -0.5f ;
					type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition = pos ;
				}
				else{
					type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE).localRotation = Quaternion.LookRotation(Vector3.forward) ;
					//type.GetRenderObject().transform.localRotation = Quaternion.LookRotation(Vector3.back);
					
					//type.talkBlink.transform.localRotation = Quaternion.LookRotation(Vector3.back);
					
					Vector3 pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition ;
					pos.z = 0.5f ;
					type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition = pos ;
				}
				
				//on the attack area .change to attack state
				if(dis < type.attackArea){
					type.m_stateMachine.ChangeState(PlayerAttackState.getInstance());
				}
				//move to target
				else{
					type.GetRenderObject().transform.position += disVec.normalized * time * type.speed ;
					Vector3 typePos = type.GetRenderObject().transform.position ;
					typePos.z = typePos.y/80.0f - 1;
					type.GetRenderObject().transform.position = typePos ;
				}
			}else{
				type.m_stateMachine.ChangeState(PlayerMoveState.getInstance());
			}
		}
		public void Exit(CPlayer type){
			
		}
		public void OnMessage(CPlayer type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_STAND ;
		}
		
		public static PlayerPursueState getInstance(){
			if(instance==null){instance = new PlayerPursueState();}
			return instance;
		}
	}
}

