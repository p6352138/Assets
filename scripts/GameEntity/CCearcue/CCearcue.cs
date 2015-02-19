using UnityEngine;
using System.Collections;
using GameLogic.AI;

namespace GameEntity{
	public interface CCreature {
		void Think();
		void Update(float deltaTime);
		void OnMessage(EventMessageBase message);
		EnitityType GetEnitityType();
		AIState  GetEnitityAiState();
		void Release();
		GameObject GetRenderObject();
		void Play(string name,WrapMode mode);
		int  GetId();
	}
}
