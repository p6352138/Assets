using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using AppUtility;
using NetWork.NetSession;
using NetWork.NetFrame;
using NetWork.NetModule;

namespace NetWork.MsgBuffer
{
    struct MsgBufferNode
    {
        public UInt16 msgCommand;    // 消息命令标记        
        public Int32  msgOffset;        // 消息在缓冲中的起始位置
        public Int32  msgLength;        // 消息的长度
        public INetSession netSession;  // 接收消息的网络会话

        // 重置
        public void Reset()
        {
            msgCommand = 0;  // 消息命令标记        
            msgOffset = 0;      // 消息在缓冲中的起始位置
            msgLength = 0;      // 消息的长度
            netSession = null;  // 接收消息的网络会话
        }
    }
    // 网络消息缓冲,包含分离的多个网络消息
    class MsgBuffer
    {
        // 初始化
        public bool Init(Int32 bufferSize)
        {
            Trace.Assert(bufferSize > 0, "bufferSize is low 0.");
            if (bufferSize <= 0)
                return false;

            msgbufferSize = bufferSize;
            msgSetBuffer = new byte[msgbufferSize];
            if (msgSetBuffer == null)
                return false;

            msgBufferNodeList = new List<MsgBufferNode>();
            if (msgBufferNodeList == null)
            {
                msgSetBuffer = null;
                return false;
            }

            return true;
        }

        // 释放
        public void Release()
        {
            msgbufferSize = 0;
            msgSetLength = 0;
            msgSetBuffer = null;
            if (msgBufferNodeList != null)
            {
                msgBufferNodeList.Clear();
                msgBufferNodeList = null;
            }
        }

        // 重置数据
        public void Reset()
        {
            msgSetLength = 0;
            if (msgBufferNodeList != null)            
                msgBufferNodeList.Clear();
        }

        public bool PushNetMessage(UInt16 msgCommand, byte[] dataBuffer, Int32 bufferOffset, Int32 dataLength, INetSession netSession)
        {
            if (dataLength > GetFreeLength())
            {
                Trace.Assert(false, "PushNetMessage dataLength is too long");
                return false;
            }

            MsgBufferNode msgBufferNode = new MsgBufferNode();
            msgBufferNode.Reset();
            msgBufferNode.msgLength = dataLength;
            msgBufferNode.msgOffset = msgSetLength;
            msgBufferNode.netSession = netSession;
            msgBufferNode.msgCommand = msgCommand;
            Buffer.BlockCopy(dataBuffer, bufferOffset, msgSetBuffer, msgSetLength, dataLength);
            msgBufferNodeList.Add(msgBufferNode);
            msgSetLength += dataLength;

            return true;
        }

        // 处理网络消息
        public void HandleMessage(HandleNetMessage msgHandler)
        {
            if (msgHandler == null)
            {
                Trace.Assert(false, "msgHandler is null");
                return;
            }

            if (msgBufferNodeList.Count() == 0 || msgSetLength <= 0)
                return;

            // 处理所有接收到的消息
            foreach (MsgBufferNode bufferNode in msgBufferNodeList)
            {
                if (bufferNode.msgOffset + bufferNode.msgLength > msgSetLength)
                {
                    Trace.Assert(false, "msgBuffer is out of range");
                    return;
                }

                // 调用消息处理函数
                msgHandler(bufferNode.msgCommand,
                           bufferNode.netSession,
                           msgSetBuffer,
                           bufferNode.msgOffset,
                           bufferNode.msgLength);
            }

            msgSetLength = 0;
            msgBufferNodeList.Clear();
        }

        // 获得剩余可写缓冲区的长度
        public Int32 GetFreeLength()
        {
            return msgbufferSize - msgSetLength;
        }

        // 构造、析构函数
        #region
        public MsgBuffer()
        {
            msgbufferSize = 0;
            msgSetLength = 0;
            msgSetBuffer = null;
            msgBufferNodeList = null;
        }
        ~MsgBuffer()
        {
        }
        #endregion

        private Int32               msgbufferSize;     // 缓冲区的大小
        private Int32               msgSetLength;      // 消息数据的长度
        private byte[]              msgSetBuffer;      // 包含多个网络消息的缓冲
        private List<MsgBufferNode> msgBufferNodeList; // 消息位置和长度标记
    }
}
