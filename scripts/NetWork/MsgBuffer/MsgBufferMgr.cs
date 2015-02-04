using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using AppUtility;
using NetWork.NetSession;
using NetWork.NetModule;

namespace NetWork.MsgBuffer
{
    class MsgBufferMgr : WriteReadList<MsgBuffer>
    {
        public static MsgBufferMgr GetInstance()
        {
            if (instance == null)
            {
                instance = new MsgBufferMgr();
            }

            Trace.Assert(instance != null, "MsgBufferMgr new is null");

            return instance;
        }

        // 初始化
        public bool Init(HandleNetMessage msgHandler)
        {
            if (msgHandler == null)
                return false;

            netMsgHandle = msgHandler;

            if (!InitWriteReadList(this.HandleMsgBuffer, this.FreeMsgBuffer))
            {
                return false;
            }

            MsgBuffer msgBuffer = MsgBufferPool.GetInstance().MallocMsgBuffer();
            if (msgBuffer == null)
            {
                ReleaseWriteReadList();
                return false;
            }

            AddElement(msgBuffer);

            return true;
        }

        // 释放
        public void Release()
        {
            ReleaseWriteReadList();
        }

        // 添加网络消息
        public void PushNetMessage(UInt16 msgCommand, byte[] dataBuffer, Int32 bufferOffset, Int32 dataLength, INetSession netSession)
        {
            if (dataBuffer == null || netSession == null || dataLength <= 0)
            {
                Trace.Assert(dataBuffer != null && netSession != null && dataLength > 0, "PushNetMessage input param error");
                return;
            }

            bool bAddToWriteList = false;
            lock (visitorGuard)
            {
                MsgBuffer msgBuffer = GetLastWriteElement();
                if (msgBuffer == null || msgBuffer.GetFreeLength() < dataLength)
                {
                    bAddToWriteList = true;
                    msgBuffer = MsgBufferPool.GetInstance().MallocMsgBuffer();
                    if (msgBuffer == null)
                    {
                        Trace.Assert(false, "MallocMsgBuffer is null");
                        return;
                    }
                }

                if (!msgBuffer.PushNetMessage(msgCommand, dataBuffer, bufferOffset, dataLength, netSession))
                {
                    if (bAddToWriteList)
                        MsgBufferPool.GetInstance().FreeMsgBuffer(msgBuffer);

                    return;
                }

                if (bAddToWriteList)
                    AddElement(msgBuffer);
            }
        }

        // MsgBuffer处理回调函数
        public void HandleMsgBuffer(MsgBuffer msgBuffer)
        {
            if (netMsgHandle == null || msgBuffer == null)
            {
                Trace.Assert(false, "HandleMsgBuffer inpu param is error");
                return;
            }

            msgBuffer.HandleMessage(netMsgHandle);
        }

        // MsgBuffer释放函数
        public void FreeMsgBuffer(MsgBuffer msgBuffer)
        {
            if (msgBuffer != null)
                MsgBufferPool.GetInstance().FreeMsgBuffer(msgBuffer);
        }

        // 构造、析构函数
        #region
        public MsgBufferMgr()
        {
            netMsgHandle = null;
        }
        ~MsgBufferMgr()
        {
        }
        #endregion

        private HandleNetMessage netMsgHandle;  // 网络消息处理函数
        private static MsgBufferMgr instance;
        
    }
}
