using UnityEngine;
using System.Collections;
using GameEvent ;


namespace GameLogical.GameEnitity.AI
{
	public class MonsterDeadState: CStateBase<CMonster>{
		protected static MonsterDeadState instance;
		public void Release(){
			
		}
		public void Enter(CMonster type){
			GameObject ob = gameGlobal.g_rescoureMgr.GetGameObjectResource("death") ;//gameGlobal.g_rescoureMgr.GetGameObjectResource("object/effect/death") as GameObject;
			GameObject sceneOb = MonoBehaviour.Instantiate(ob) as GameObject;
			sceneOb.transform.position = type.GetRenderObject().transform.position ;
			sceneOb.transform.FindChild("creature").animation.wrapMode = WrapMode.Once ;
			sceneOb.transform.FindChild("creature").animation.Play("effect");
			
			//talk
			/*MonsterMoudleData monstermoudle = (MonsterMoudleData)common.fileMgr.GetInstance().GetData(type.m_data.moudleID,
				common.CsvType.CSV_TYPE_MONSTER);
			type.TalkLv(monstermoudle.talkLvInDead, monstermoudle.talkIDInDead);*/
		}
		public void Execute(CMonster type, float time){
			
		}
		public void Exit(CMonster type){
			
		}
		public void OnMessage(CMonster type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_DEATH ;
		}
		public static MonsterDeadState getInstance(){
			if(instance ==null){
				instance = new MonsterDeadState();
			}
			
			return instance;
		}
	}
	
}

