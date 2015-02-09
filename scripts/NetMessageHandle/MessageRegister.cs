using System;
using NetWork.NetFrame;
using NetWork;
//using GameMessageHandle;

namespace GameMessgeHandle
{
    public class MessageRegister
    {
        static public void RegisterMessage()
        {
            NetFrameMgr.GetInstance().RegisterMsgHanle((ushort)NetWork.emNETMSG.emNET_MSG_JSON_UNZIP, MessgeHandle.HandleMsg2000);
			NetFrameMgr.GetInstance().RegisterMsgHanle((ushort)NetWork.emNETMSG.emNET_MSG_JSON_ZIP, MessgeHandle.HandleMsg2001);
			//NetFrameMgr.GetInstance().RegisterMsgHanle((ushort)NetWork.emNETMSG.emNET_MSG_JSON_ZIP, MessgeHandle.HandleMsg2001);
			
			//LoginMessageRegister.RegisterMessage();
			//LevelMessageRegister.RegisterMessage();
			//BuildingMessageRegister.RegisterMessage();
			CommonMessageRegister.RegisterMessage();
			//PlayerMessageRegister.RegisterMessage();
			//PackageMessageRegister.RegisterMessage();
			//ChatMessageRegister.RegisterMessage();
			//FightAreaMessageRegister.RegisterMessage();
			//AchieveMessageRegister.RegisterMessage();
        }
    }
}