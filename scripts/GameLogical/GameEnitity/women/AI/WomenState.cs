using UnityEngine;
using System.Collections;
using GameEvent ;

namespace GameLogical.GameEnitity.AI{
	public class CWomenBeCoughtState : CStateBase<CWomen>{
		public void Release(){
			
		}
		public void Enter(CWomen type){
			type.Play("struggle1",WrapMode.Loop);
			MuscClip.MusicClipMgr.GetInstance().MusicClips("feedback_cry");
		}
		public void Execute(CWomen type, float time){
			WomenBeCoughtStateData data = type.m_womenAIData as WomenBeCoughtStateData	;
			if(EnitityMgr.GetInstance().GetEnitity(data.monsterID) != null){
				Vector3 pos = EnitityMgr.GetInstance().GetEnitityPos(type.GetId(),data.monsterID);
				pos.z += 0.2f ;
				type.GetRenderObject().transform.position = pos ;
			}
			else{
				EnitityMgr.GetInstance().DestroyEnitity(type);
			}
		}
		public void Exit(CWomen type){
			
		}
		public void OnMessage(CWomen type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_BE_COUGHT ;
		}
	}
	
	public class CWomenEscapeState : CStateBase<CWomen>{
		public void Release(){
			
		}
		public void Enter(CWomen type){
			type.Play("run",WrapMode.Loop);
			type.GetRenderObject().transform.localRotation = Quaternion.LookRotation(Vector3.back);
		}
		public void Execute(CWomen type, float time){
			
			float disVec = EnitityMgr.GetInstance().city.GetRenderObject().transform.position.x - type.GetRenderObject().transform.position.x ;
			/*if(disVec > 0){
				type.GetRenderObject().transform.localRotation = Quaternion.LookRotation(Vector3.forward);
			}
			else{

			}*/
			
			if(Mathf.Abs( disVec )< 2.0f){
				type.Think();
			}
			else{
				Vector3 pos = type.GetRenderObject().transform.position ;
				pos.x -= time * type.womenSpeed ;
				type.GetRenderObject().transform.position = pos ;
			}
		}
		public void Exit(CWomen type){
			
		}
		public void OnMessage(CWomen type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_ESCAPE ;
		}
	}
}


