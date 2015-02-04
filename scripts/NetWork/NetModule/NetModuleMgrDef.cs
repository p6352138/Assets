using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWork.NetDefine;
using NetWork.NetSession;

namespace NetWork.NetModule
{
    // 接收到新的连接
    public delegate void NetSesssionConnected(INetSession netSession, EVENTSESSIONPLACE eventPlace, bool connectSuccess);

    // 网络会话关闭
    public delegate void NetSessionClosed(INetSession netSession, EVENTSESSIONPLACE eventPlace);

    // 处理网络消息
    public delegate void HandleNetMessage(UInt16 msgCmd, INetSession netSession, byte[] msgBuffer, Int32 bufferOffset, Int32 msgLength);

    struct NetModuleInit
    {
        public Int32 bufferReserves;        // 缓冲区(发送、接收)池保留的数量
        public Int32 sendBufSize;           // 发送缓冲区大小
        public Int32 sendBufExtend;         // 发送缓冲区扩展大小（大于单个数据包大小）
        public Int32 rcvBufSize;            // 接收缓冲区大小
        public Int32 rcvBufExtend;          // 接收缓冲区扩展大小（大于单个数据包大小）
        public Int32 sessionInitCount;      // 会话池初始大小
        public Int32 sessionExtendCount;    // 会话池扩展大小
        public Int32 msgBufferCounts;       // 网络消息缓冲初始化数量
        public Int32 msgBufferSize;         // 网络消息缓冲大小
        public NetSessionClosed netSessionClosedCallbackFunc;         // 网络会话关闭回调函数
        public NetSesssionConnected netSessionConnectedCallbackFunc;  // 网络连接成功回调函数

        // 重置
        public void Reset()
        {
            bufferReserves = 0;        // 缓冲区(发送、接收)池保留的数量
            sendBufSize = 0;           // 发送缓冲区大小
            sendBufExtend = 0;         // 发送缓冲区扩展大小（大于单个数据包大小）
            rcvBufSize = 0;            // 接收缓冲区大小
            rcvBufExtend = 0;          // 接收缓冲区扩展大小（大于单个数据包大小）
            sessionInitCount = 0;      // 会话池初始大小
            sessionExtendCount = 0;    // 会话池扩展大小
            msgBufferCounts = 0;       // 网络消息缓冲初始化数量
            msgBufferSize = 0;         // 网络消息缓冲大小
            netSessionClosedCallbackFunc = null;    // 网络会话关闭回调函数
            netSessionConnectedCallbackFunc = null; // 网络连接成功回调函数
        }
    }  
}
