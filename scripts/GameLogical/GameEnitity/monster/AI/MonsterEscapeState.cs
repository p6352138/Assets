using UnityEngine;
using System.Collections;
using GameEvent ;
using System.Collections.Generic;
using GameLogical.GameLevel;
using GameMessgeHandle;
using GameLogical.Guide;

namespace GameLogical.GameEnitity.AI
{
	public class MonsterEscapeState: CStateBase<CMonster>{
		protected static MonsterEscapeState instance;
		public void Release(){
			
		}
		public void Enter(CMonster type){

			List<CCreature> monsterList = EnitityMgr.GetInstance().GetMonsterList();
			float tempDis ;
			for(int i = 0; i<monsterList.Count; ++i){
				//not come back
				tempDis = Vector3.Distance(monsterList[i].GetRenderObject().transform.position,type.GetRenderObject().transform.position) ;
				if(tempDis<type.m_data.misPlace && monsterList[i].GetEnitityAiState() == AIState.AI_STATE_ESCAPE &&
				   monsterList[i].GetId() != type.GetId()){
					type.m_stateMachine.ChangeState(MonsterMisplaceState.getInstance());
					return;
				}
			}


			Transform shader  = type.m_object.transform.FindChild(gameGlobal.CREATURE_SHADOW);
			shader.localPosition = new Vector3(shader.localPosition.x, shader.localPosition.y, (-1) * shader.localPosition.z);

			type.Play("walk",WrapMode.Loop);
			//talk
			/*MonsterMoudleData monstermoudle = (MonsterMoudleData)common.fileMgr.GetInstance().GetData(type.m_data.moudleID,
				common.CsvType.CSV_TYPE_MONSTER);
			type.TalkLv(monstermoudle.talkLvInRobWomen, monstermoudle.talkIDInRobWomen);*/
			//type.talkBlink.transform.localEulerAngles  = new Vector3(0, 0, 0);
			//if(type.nameBlink != null)
			//	type.nameBlink.transform.localEulerAngles  = new Vector3(0, 0, 0);
			
			CCreature creature = EnitityMgr.GetInstance().CreateEnitity(EnitityType.ENITITY_TYPE_WOMEN,type.GetId());
			CWomen women = creature as CWomen;
			common.debug.GetInstance().AppCheckSlow(women);

			CWomenBeCoughtState state = new CWomenBeCoughtState();
			WomenBeCoughtStateData stateData = new WomenBeCoughtStateData();
			stateData.monsterID = type.id ;
			women.m_womenAIData = stateData ;
			
			women.SetState(state);
		}
		
		public void Execute(CMonster type, float time){
			type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_ROOT).localRotation = Quaternion.LookRotation(Vector3.forward);
			//type.GetRenderObject().transform.localRotation = Quaternion.LookRotation(Vector3.forward);
			if(type.GetRenderObject().transform.position.x >35.0f&&NewPlayerGuide.isGuide&&NewPlayerGuide.curGuide==106)
			{
				//GuideStopMessage stop_play_message = new GuideStopMessage();
				//EventMgr.GetInstance().OnEventMgr(stop_play_message);

				GuideCheckMessage msg = new GuideCheckMessage();
				NewPlayerGuide.GetInstance().OnMessage(msg);
			}

			if(NewPlayerGuide.curGuide==108&&NewPlayerGuide.isGuide&&type.GetRenderObject().transform.position.x >40.0f )
			{
				//GuideStopMessage stop_play_message = new GuideStopMessage();
				//EventMgr.GetInstance().OnEventMgr(stop_play_message);
				
				GuideCheck2Message msg = new GuideCheck2Message();
				if(GameLevelMgr.GetInstance().m_curWave==3)
				{
					msg.guideStep = 109;
				}
				NewPlayerGuide.GetInstance().OnMessage(msg);
				gameGlobal.g_fightSceneUI.CutCoolDownTime(1000.0f,1);
			}

			if(type.GetRenderObject().transform.position.x < 120.0f){
				//type.GetRenderObject().transform.position += Vector3.right * time * type.monsterSpeed * 0.5f ;
				Vector3 pos = type.GetRenderObject().transform.position ;
				pos += Vector3.right * time * type.monsterSpeed * 0.5f ;
				pos.z = pos.y ;
				type.GetRenderObject().transform.position = pos ;
			}
			else{
				MonsterMoudleData monsterMoudleData = type.getMonsterMoudleData();
				if(monsterMoudleData!=null){ 
					EventMessageMonsterEscape escapeMessage = new EventMessageMonsterEscape();
					escapeMessage.id = type.id ;
					EventMgr.GetInstance().OnEventMgr(escapeMessage);
					if(monsterMoudleData.strength>1){
						type.m_stateMachine.ChangeState(MonsterMoveState.getInstance());
						return;
					}	
				}
				EnitityMgr.GetInstance().DestroyEnitity(type);
			}
		}
		public void Exit(CMonster type){
			
		}
		public void OnMessage(CMonster type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_ESCAPE ;
		}
		
		public static MonsterEscapeState getInstance(){
			if(instance ==null){
				instance = new MonsterEscapeState();
			}
			
			return instance;
		}
		
		
	}
}

