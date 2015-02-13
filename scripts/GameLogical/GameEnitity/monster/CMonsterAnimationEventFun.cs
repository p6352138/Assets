using UnityEngine;
using System.Collections;

namespace GameLogical.GameEnitity{
	public class CMonsterAnimationEventFun : MonoBehaviour
	{
		public void Attack(){
			EventMessageFightStart message = new EventMessageFightStart();
			message.ob = this.gameObject.transform.parent.gameObject ;
			EnitityMgr.GetInstance().OnMessage(message);
			//message.creature = 
			//EnitityMgr.GetInstance().OnMessage();
			//this.gameObject.transform.parent ;
		}
		
		public void AttackEnd(){
			animation.Play("stand");
			EventMessageFightEnd message = new EventMessageFightEnd();
			message.ob = this.gameObject.transform.parent.gameObject ;
			EnitityMgr.GetInstance().OnMessage(message);
		}
		
		public void DeathEnd(){
			EventMessageDeathEnd message = new EventMessageDeathEnd();
			message.ob = this.gameObject.transform.parent.gameObject ;
			EnitityMgr.GetInstance().OnMessage(message);
		}
		

	}
}


