using UnityEngine;
using System.Collections;
using GameEvent ;

namespace GameLogical.GameEnitity{
	public enum AreaType{
		AREA_TYPE_ATTACK,
		AREA_TYPE_EYE_SHOT,
	}
	
	public class EnitityColliderEventFun : MonoBehaviour
	{
		public AreaType type ;

		void OnTriggerEnter(Collider collision) {
			/*EventMessageEnterCollider message = new EventMessageEnterCollider();
			message.scrObject = this.gameObject.transform.parent.gameObject ;
			message.destObject= collision.gameObject.transform.parent.gameObject ;
			if(message.scrObject == message.destObject)
				return ;
			message.type = type ;
			//print("scr:" + message.scrObject.name + " dest:" + message.destObject.name);
			EventMgr.GetInstance().OnEventMgr(message);*/
	    }
		
		void OnTriggerExit(Collider collision) {
			/*EventMessageExitCollider message = new EventMessageExitCollider();
			message.scrObject = this.gameObject.transform.parent.gameObject ;
			message.destObject= collision.gameObject.transform.parent.gameObject ;
			message.type = type ;
			EventMgr.GetInstance().OnEventMgr(message);*/
	    }
	}
}


