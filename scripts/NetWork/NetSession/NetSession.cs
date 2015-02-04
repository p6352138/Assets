using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWork.NetSession
{
    public interface INetSession
    {
        // 获得会话的索引 ID
        Int32 GetSessionID();

        // 获得接收到的数据长度
        Int32 GetSendedDataLength();

        // 获得发送的数据长度
        Int32 GetRcvedDataLength();

        // 获得会话连接的远端IP
        string GetRemoteAddress();

        // 获得会话联机的远端端口
        Int32 GetRemotePort();

        // 发送信息
        Int32 SendMessage(UInt16 msgCommand, byte[] messageBuffer);

        // 发送信息
        Int32 SendMessage(UInt16 msgCommand, byte[] messageBuffer, UInt16 bufferLength);

        // 发送信息
        Int32 SendMessage(UInt16 msgCommand, byte[] messageBuffer, Int32 bufferOffset, UInt16 bufferLength);
		
		Int32 SendMessage(string head,Dictionary<string,object> data) ;
    }
}
