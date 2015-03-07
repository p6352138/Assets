using UnityEngine;
using System.Collections;
using GameEntity;
using GameLogic.AI;

public class NAnimationEvent : MonoBehaviour {

	void RunOver(){
		EventMessageBase message = new EventMessageBase ();
		message.eventMessageAction = (int)EnitityCommon.EnitityAction.ENITITY_ACTION_MOVEOVER;
		message.eventMessageModel = EventMessageModel.eEventMessageModel_PLAY_MOVE_STATE;
		CCearcueMgr.GetInstance ().player.OnMessage (message);
	}

	void Hero_Injurt()
	{
		EventMessageBase message = new EventMessageBase ();
		message.eventMessageAction = 5;
		message.eventMessageModel = EventMessageModel.eEventMessageModel_PLAY_STATE;
		CCearcueMgr.GetInstance ().player.OnMessage (message);
	}

	void enemy_attack()
	{
		;
	}

	void MonAttOver()
	{
		EventMessageBase message = new EventMessageBase ();
		message.eventMessageAction = (int)EnitityCommon.EnitityAction.ENITITY_ACTION_FIGHT_FINISH;
		message.eventMessageModel = EventMessageModel.eEventMessageModel_MONSTER_STATE;
		message.modleId = int.Parse (this.name);
		CCearcueMgr.GetInstance ().MonsterOnMessage (message);
	}
}
