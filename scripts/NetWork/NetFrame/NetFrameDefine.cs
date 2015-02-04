using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWork.NetSession;

namespace NetWork.NetFrame
{
    // 处理网络消息
    public delegate void HandleMsg(INetSession netSession, byte[] msgBuffer, Int32 bufferOffset, Int32 msgLength);
	
	//json message handle
	public delegate void JsonHandleMsg(string errorCode,object data);
}
