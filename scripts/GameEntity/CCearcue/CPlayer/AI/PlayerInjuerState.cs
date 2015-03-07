using UnityEngine;
using System.Collections;
using GameLogic.AI;
using GameEntity;

public class PlayerInjuerState : CStateBase<CPlayer> {

	protected static PlayerInjuerState instance;
	
	public void Release()
	{
		;
	}
	
	public void Enter(CPlayer type)
	{
		type.Play (PlayerPlayAnimation.INJURT, WrapMode.Once);
	}
	
	public void Execute(CPlayer type, float time)
	{
		;
	}
	
	public void Exit(CPlayer type)
	{
		;
	}
	
	public void OnMessage(CPlayer type, EventMessageBase data)
	{
		;
	}
	
	public AIState GetState()
	{
		return AIState.AI_STATE_STAND;
	}
	
	public static PlayerInjuerState GetInstance(){
		if (instance == null)
			instance = new PlayerInjuerState ();
		return instance;
	}
}
