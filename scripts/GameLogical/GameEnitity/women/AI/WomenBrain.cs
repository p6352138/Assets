using UnityEngine;
using System.Collections;
using GameEvent ;

namespace GameLogical.GameEnitity.AI{
	public class WomenBrain : CBassBrian
	{
		protected CWomen m_women ;
		public void Init(CWomen women){
			m_women = women ;
		}
		
		public void ThinkAbout(){
			switch(m_women.GetEnitityAiState()){
			case AIState.AI_STATE_ESCAPE:{
				EnitityMgr.GetInstance().DestroyEnitity(m_women);
			}
				break ;
			}
		}
		public void OnMessage(EventMessageBase message){
			switch((EnitityAction)message.eventMessageAction){
			case EnitityAction.ENITITY_ACTION_DEATH:{
				EventMessageDeathEnd deathMessage = message as EventMessageDeathEnd ;
				int id = int.Parse(deathMessage.ob.name);
				WomenBeCoughtStateData data = m_women.m_womenAIData as WomenBeCoughtStateData ;
				if(id == data.monsterID){
					CWomenEscapeState state = new CWomenEscapeState();
					
					m_women.SetState(state);
				}
			}
				break ;
				
			case EnitityAction.ENITITY_ACTION_ESCAPE:{
				if(m_women.GetEnitityAiState() == AIState.AI_STATE_BE_COUGHT){
					EventMessageMonsterEscape escape = message as EventMessageMonsterEscape ;
					WomenBeCoughtStateData data = m_women.m_womenAIData as WomenBeCoughtStateData ;
					if(escape.id == data.monsterID){
						EnitityMgr.GetInstance().DestroyEnitity(m_women);
					}
				}

			}
				break ;

			case EnitityAction.ENITITY_ACTION_BE_DRAW:{
				if(m_women.GetEnitityAiState() == AIState.AI_STATE_BE_COUGHT){
					EventMessageBeDraw beDraw = message as EventMessageBeDraw ;
					WomenBeCoughtStateData data = m_women.m_womenAIData as WomenBeCoughtStateData ;
					if(beDraw.id == data.monsterID){
						CWomenEscapeState state = new CWomenEscapeState();
						
						m_women.SetState(state);
					}
				}
			}
				break ;

			}
		}
	}
}


