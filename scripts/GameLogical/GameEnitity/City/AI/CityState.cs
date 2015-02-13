using UnityEngine;
using System.Collections;
using GameEvent ;
namespace GameLogical.GameEnitity.AI{
	
	public class CCityNormalState : CStateBase<CCity>
	{
		public void Release(){
			
		}
		
		public void Enter(CCity type){
			
		}
		
		public void Execute(CCity type, float time){
		/*	type.m_curCoolDown1 += time ;
			type.m_curCoolDown2 += time ;
			if(type.m_curCoolDown1 > type.m_coolDown)
			{
				CMonster monster = EnitityMgr.GetInstance().FindDangerousMonster();
				if(monster != null){
					type.m_leftPaoTran.animation.Play("fire");

					GameObject ob =	RescourseMgr.GetInstance().GetGameObjectResource("kaipao");
					MuscClip.MusicClipMgr.GetInstance().MusicClips("hit_biggun");
					GameObject sceneOb = (GameObject)MonoBehaviour.Instantiate(ob);
					sceneOb.transform.position = type.m_ArrowPos[0] ;
					sceneOb.transform.FindChild("creature").animation.Play("effect");
					
					
					BulletData bulletData = new BulletData();
					bulletData.scrID = type.GetId();
					bulletData.destID= monster.GetId();
					bulletData.follow = false ;
					if(type.level == 3){
						bulletData.effectID = 53003;
					}
					else if(type.level == 2){
						bulletData.effectID = 53002;
					}
					else{
						bulletData.effectID = 53001;
					}
					
					bulletData.effectEndID = 53004;

					bulletData.pos = type.m_ArrowPos[0] ;
					CCreature creature = EnitityMgr.GetInstance().CreateEnitity(EnitityType.ENITITY_TYPE_BULLET,bulletData);
					//creature.GetRenderObject().transform.position = type.m_ArrowPos[0] ;

				}
				type.m_curCoolDown1 = 0.0f ;
			}

			if(type.m_curCoolDown2 > type.m_coolDown)
			{
				CMonster monster = EnitityMgr.GetInstance().FindDangerousMonster();
				if(monster != null){
					type.m_rightPaoTran.animation.Play("fire");
					GameObject ob =	RescourseMgr.GetInstance().GetGameObjectResource("kaipao");
					GameObject sceneOb = (GameObject)MonoBehaviour.Instantiate(ob);
					sceneOb.transform.position = type.m_ArrowPos[1] ;
					sceneOb.transform.FindChild("creature").animation.Play("effect");
					
					
					BulletData bulletData = new BulletData();
					bulletData.scrID = type.GetId();
					bulletData.destID= monster.GetId();
					bulletData.follow = false ;
					if(type.level == 3){
						bulletData.effectID = 53003;
					}
					else if(type.level == 2){
						bulletData.effectID = 53002;
					}
					else{
						bulletData.effectID = 53001;
					}
					
					bulletData.effectEndID = 53004;

					bulletData.pos = type.m_ArrowPos[1] ;
					CCreature creature = EnitityMgr.GetInstance().CreateEnitity(EnitityType.ENITITY_TYPE_BULLET,bulletData);


				}
				type.m_curCoolDown2 = 0.0f ;
			}*/
		}
		
		public void Exit(CCity type){
			
		}
		
		public void OnMessage(CCity type, EventMessageBase data){
			
		}
		
		public AIState  GetState(){
			return AIState.AI_STATE_DOOR_NARMOL ;
		}
	}
	
	public class CCityDoorBreakState : CStateBase<CCity>
	{
		public void Release(){
			
		}
		
		public void Enter(CCity type){
			GameObject obj = gameGlobal.g_rescoureMgr.GetGameObjectResource("dangerEffect") ;//gameGlobal.g_rescoureMgr.GetGameObjectResource("object/scene/dangerEffect") as GameObject;
			obj = MonoBehaviour.Instantiate(obj) as GameObject;
			obj.transform.position = new Vector3(60.0f,40.0f,-1.0f);
			//gameGlobal.g_AudioPlay.MusicClips("feedback_danger",obj);
			MuscClip.MusicClipMgr.GetInstance().MusicClips("feedback_danger");
		}
		
		public void Execute(CCity type, float time){
			if(type.m_curCoolDown1 == -1.0f)
				return ;
			
			type.m_curCoolDown1 += time ;
			type.m_curCoolDown2 += time ;
			if(type.m_curCoolDown1 > type.m_coolDown)
			{
				CMonster monster = EnitityMgr.GetInstance().FindDangerousMonster();
				if(monster != null){
					type.m_leftPaoTran.animation.Play("fire");
					
					GameObject ob =	RescourseMgr.GetInstance().GetGameObjectResource("kaipao");
					MuscClip.MusicClipMgr.GetInstance().MusicClips("hit_biggun");
					GameObject sceneOb = (GameObject)MonoBehaviour.Instantiate(ob);
					sceneOb.transform.position = type.m_ArrowPos[0] ;
					sceneOb.transform.FindChild("creature").animation.Play("effect");
					
					
					BulletData bulletData = new BulletData();
					bulletData.scrID = type.GetId();
					bulletData.destID= monster.GetId();
					bulletData.follow = false ;
					if(type.level == 3){
						bulletData.effectID = 53003;
					}
					else if(type.level == 2){
						bulletData.effectID = 53002;
					}
					else{
						bulletData.effectID = 53001;
					}
					
					bulletData.effectEndID = 53004;

					bulletData.pos = type.m_ArrowPos[0] ;
					CCreature creature = EnitityMgr.GetInstance().CreateEnitity(EnitityType.ENITITY_TYPE_BULLET,bulletData);
				}
				type.m_curCoolDown1 = 0.0f ;
			}
			
			if(type.m_curCoolDown2 > type.m_coolDown)
			{
				CMonster monster = EnitityMgr.GetInstance().FindDangerousMonster();
				if(monster != null){
					type.m_rightPaoTran.animation.Play("fire");
					GameObject ob =	RescourseMgr.GetInstance().GetGameObjectResource("kaipao");
					GameObject sceneOb = (GameObject)MonoBehaviour.Instantiate(ob);
					sceneOb.transform.position = type.m_ArrowPos[1] ;
					sceneOb.transform.FindChild("creature").animation.Play("effect");
					
					
					BulletData bulletData = new BulletData();
					bulletData.scrID = type.GetId();
					bulletData.destID= monster.GetId();
					bulletData.follow = false ;
					if(type.level == 3){
						bulletData.effectID = 53003;
					}
					else if(type.level == 2){
						bulletData.effectID = 53002;
					}
					else{
						bulletData.effectID = 53001;
					}
					
					bulletData.effectEndID = 53004;

					bulletData.pos = type.m_ArrowPos[1] ;

					CCreature creature = EnitityMgr.GetInstance().CreateEnitity(EnitityType.ENITITY_TYPE_BULLET,bulletData);
				}
				type.m_curCoolDown2 = 0.0f ;
			}
		}
		
		public void Exit(CCity type){
			
		}
		
		public void OnMessage(CCity type, EventMessageBase data){
			
		}
		
		public AIState  GetState(){
			return AIState.AI_STATE_DOOR_BREAK ;
		}
	}
}


