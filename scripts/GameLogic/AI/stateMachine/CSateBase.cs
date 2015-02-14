﻿using UnityEngine;
using System.Collections;

namespace GameLogic.AI{
	public interface CStateBase<TYPE> {
		void Release();
		void Enter(TYPE type);
		void Execute(TYPE type, float time);
		void Exit(TYPE type);
		void OnMessage(TYPE type, EventMessageBase data);
		AIState  GetState();
	}
}
