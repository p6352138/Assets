using UnityEngine;
using System.Collections;
using GameLogic.AI;
using GameEntity;

public class NAnimationEventPlayer : MonoBehaviour {
	void RunOver(){
		EventMessageBase message = new EventMessageBase ();
		message.eventMessageAction = (int)EnitityCommon.EnitityAction.ENITITY_ACTION_MOVEOVER;
		message.eventMessageModel = EventMessageModel.eEventMessageModel_PLAY_MOVE_STATE;
		CCearcueMgr.GetInstance ().player.OnMessage (message);
	}

	void Hero_Injurt()
	{
		EventMessageBase message = new EventMessageBase ();
		message.eventMessageModel = EventMessageModel.eEventMessageModel_PLAY_ATTACK_STATE;
		message.eventMessageAction = 20;
		CCearcueMgr.GetInstance().MonsterBeAttack(CCearcueMgr.GetInstance().player.GetAttackArea (),message);
	}
	
	void enemy_attack()
	{
		;
	}
	
	void MonAttOver()
	{
		EventMessageBase message = new EventMessageBase ();
		message.eventMessageAction = (int)EnitityCommon.EnitityAction.ENITITY_ACTION_FIGHT_FINISH;
		message.eventMessageModel = EventMessageModel.eEventMessageModel_PLAY_STATE;
		CCearcueMgr.GetInstance ().player.OnMessage (message);
	}
}
