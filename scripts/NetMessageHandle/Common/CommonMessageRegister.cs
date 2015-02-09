using UnityEngine;
using System.Collections;
using NetWork.NetFrame;
using NetWork;

namespace GameMessgeHandle{
	public class CommonMessageRegister
	{
		static public string COMMON_MSG			= 		"common#msg" 		;
		static public string COMMON_SUBMIT_BUG	= 		"common#submit_bug"	;
		static public string COMMON_SEND_PET_EMAIL = 	"common#send_pet_email";
		static public string COMMON_SEND_ITEM_EMAIL = 	"common#send_item_email";

		static public void RegisterMessage(){
			MessgeHandle.RegisterMsgHanle(COMMON_MSG,CommonMessageHandle.CommonMsg);
			MessgeHandle.RegisterMsgHanle(COMMON_SEND_PET_EMAIL,CommonMessageHandle.SendPetEmail);
			MessgeHandle.RegisterMsgHanle(COMMON_SEND_ITEM_EMAIL,CommonMessageHandle.SendItemEmail);
		}
	}

}

