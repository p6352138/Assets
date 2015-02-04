using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace NetWork.NetDefine
{
    // 网络块全局数据
    public enum NETGLOBALDATA : int
    {
        maxSendPackLength = 4608,     // 最大的数据包长度(应用层)
        maxSendDataLength = 4096,     // 最大的网络数据长度(系统网络层)
    }

    // 网络数据加密操作类型
    public enum ENCRYPTOPT : byte
    {
        codeEncryptNull = 0,          // 不做操作
        codeEncrypt,                  // 执行加密操作
        codeUnencrypt,                // 执行解密操作
    }

    // 网络消息的使用类型
    public enum NETMSGUSETYPE
    {
        appUseMsg = 0,               // 应用层使用的网络消息
        netUseMsg = 1,               // 网络层使用的网络消息
        
    }

    // 会话的状态
    public enum NETSESSIONSTATE : byte
    {
        netSessionStateNull     = 0,  // 默认空值
        netSessionStateUsing    = 1,  // 使用中状态
        netSessionStateShutdown = 2,  // 关闭状态
        netSessionStateMax      = 3,
    }

    // 会话使用的状态类型
    public enum NETSESSIONSTATETYPE : byte
    {
        netSessionAppState  = 1,      // 应用层使用的状态
        netSessionSendState = 2,      // 发送使用状态
        netSessionRcvState  = 3,      // 接收使用状态
    }

    // 添加、删除会话,连接失败等事件类型
    public enum ADDORDELSESSIONEVENT
    {
        nullOperation = 0,            // 默认空值
        addNetSession = 1,            // 添加会话(包括连接成功、接收连接成功)
        delNetSession = 2,            // 删除会话
        asyncConnectFailed = 3,       // 异步连接失败
    }

    // 添加、删除会话的地方
    public enum EVENTSESSIONPLACE
    {
        nullPlace = 0,                // 默认空值

        // 删除会话的地方
        appDelSession = 1,            // 应用层删除会话
        sendDelSession = 2,           // 发送遇到问题删除
        rcvDelSession = 3,            // 接收遇到问题删除

        // 添加会话的地方
        connectAddSession = 4,        // 主动连接成功
        acceptAddSession = 5,         // 接收连接成功
    }

    // 网络消息包头
    public struct NetMsgHead
    {
        public byte msgEncrypt;       // 数据加密类型
        public byte msgCompressed;    // 数据是否压缩
        public byte msgCheckNumber;   // 效验值
        public byte msgUseType;       // 网络消息类型(网络底层、上层)
        public UInt16 msgLength;      // 消息长度
        public UInt16 msgCommand;     // 消息命令

        // 重置数据
        public void Reset()
        {
            msgEncrypt     = 0;       // 数据加密类型
            msgCompressed  = 0;       // 数据是否压缩
            msgCheckNumber = 0;       // 效验值
            msgUseType     = 0;       // 网络消息类型(网络底层、上层)
            msgLength      = 0;       // 消息长度
            msgCommand     = 0;       // 消息命令
        }
    }
	
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct NetMsgHead2{
		public byte msgCompressed ;
		public int  msgLength ;
		
		        // 重置数据
        public void Reset()
        {
            msgCompressed  = 0;
            msgLength = 0;
        }  
	}

    // 会话状态
    public struct NetSessionState
    {
        public Int32 appUseState;        // 应用层使用状态
        public Int32 sendUseState;       // 发送使用状态
        public Int32 rcvUseState;        // 接收使用状态 

        // 重置数据
        public void Reset(Int32 setValue)
        {
            appUseState  = setValue;
            sendUseState = setValue;
            rcvUseState  = setValue;
        }       
    }
}
