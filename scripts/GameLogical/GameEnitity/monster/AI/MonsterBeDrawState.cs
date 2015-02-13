using UnityEngine;
using System.Collections;
using GameEvent ;

namespace GameLogical.GameEnitity.AI
{
	public class MonsterBeDrawState : CStateBase<CMonster>
	{
		protected static MonsterBeDrawState instance;
		public void Release(){
			
		}
		public void Enter(CMonster type){
			type.m_monsterAIData.time = 0.0f ;
			EventMessageBeDraw beDrawMessage = new EventMessageBeDraw();
			beDrawMessage.id = type.m_data.id ;
			EnitityMgr.GetInstance().OnMessage(beDrawMessage);

			if(gameGlobal.g_fightSceneUI != null)
			{
				GameObject sceneOb = MonoBehaviour.Instantiate( gameGlobal.g_fightSceneUI.m_objList["attention"] ) as GameObject ;
				sceneOb.transform.parent = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_HEAD);
				sceneOb.transform.localPosition = Vector3.zero ; 
				sceneOb.transform.FindChild("creature").animation.Play("effect");
			}

			type.Play("stand",WrapMode.Loop);
		}

		public void Execute(CMonster type, float time){
			type.m_monsterAIData.time += time ;
			if(type.m_monsterAIData.time >= AICommon.AI_THINK_DELTA_TIME){
				type.SetState(MonsterPursueState.getInstance());
			}
		}

		public void Exit(CMonster type){
			
		}
		public void OnMessage(CMonster type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_BE_DRAW ;
		}
		public static MonsterBeDrawState getInstance(){
			if(instance ==null){
				instance = new MonsterBeDrawState();
			}
			
			return instance;
		}
	}
}


