using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameEvent ;
using GameLogical ;
using GameLogical.GameSkill.Skill ;
using GameLogical.GameSkill ;
using GameLogical.Guide;
using common;

namespace GameLogical.GameEnitity.AI{
	public class PetUseAngrySkillState : CStateBase<CPet>
	{
		protected static PetUseAngrySkillState instance;
		public void Release(){
			
		}
		
		public void Enter(CPet type){
			if(type != null && type.GetRenderObject() != null){
				type.Play("skill2",WrapMode.Once);
				Transform trans = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_ROOT);
				trans.localScale *= 1.3f ;
				Time.timeScale = 0.7f ;
				common.common.blackScreen(true);

				type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE).GetComponent<Bass2D.Amin_2D_Ex>().sortOrder = 2;
			}
		}
		
		public void Execute(CPet type, float time){
			
		}
		
		public void Exit(CPet type){

			//type.m_stateMachine
			//type.m_stateMachine.ChangeState(type.m_stateMachine.GetPreviosState());
		}
		
		public void OnMessage(CPet type, EventMessageBase data){
			EnitityAction action = (EnitityAction)data.eventMessageAction;
			if(action == EnitityAction.ENITYTY_ACTION_SKILL_START){
				if(type.m_curUsingSkill != -1 ){
					CSkillBass skill = SkillMgr.GetInstance().GetSkill(type.m_curUsingSkill);
					skill.PlayStartEffect(type.GetId());
				}
			}
			else if(action == EnitityAction.ENITYTY_ACTION_SKILL){
				if(type.m_curUsingSkill != -1 ){
					EventMgr.GetInstance().OnEventMgr(type.selectMessage);
					
					CSkillBass skill = SkillMgr.GetInstance().GetSkill(type.m_curUsingSkill);
					skill.useSkill(type.selectMessage);

				}
			}
			else if(action == EnitityAction.ENITITY_ACTION_SKILL_FINISH){
				type.m_data.curAttackCD = 0.0f ;
				type.Play("stand",WrapMode.Loop);
				type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_ROOT).localScale = Vector3.one;
				EnitityMgr.GetInstance().m_staticScene-- ;
				type.GetFightCreatureData().isMainRole = false ;

				//
				/*if(type.m_stateMachine.GetPreviosState().GetState() == AIState.AI_STATE_STONE || type.m_stateMachine.GetPreviosState().GetState() == AIState.AI_STATE_DEATH){
					type.m_stateMachine.ChangeState(PetStandState.getInstance());
				}
				else{
					type.m_stateMachine.ChangeState(type.m_stateMachine.GetPreviosState());
				}*/

				type.m_stateMachine.ChangeState(type.m_stateMachine.GetPreviosState());
				Time.timeScale = 1.0f ;
				type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE).GetComponent<Bass2D.Amin_2D_Ex>().sortOrder = 0;
				common.common.blackScreen(false);
				if(NewPlayerGuide.curGuide == 111 )
				{
					List<int> id = new List<int>();
					id.Add(6003);
					GuideCheckMessage msg = new GuideCheckMessage();
					NewPlayerGuide.GetInstance().OnMessage(msg);
					gameGlobal.g_fightSceneUI.LoadTalk(id);
				}
			}
		}
		public AIState  GetState(){
			return AIState.AI_STATE_ACTTACK ;
		}
		
		public static PetUseAngrySkillState getInstance(){
			if(instance==null){instance = new PetUseAngrySkillState();}
			return instance;
		}
	}
	
}

