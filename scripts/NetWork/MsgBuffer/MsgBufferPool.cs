using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using AppUtility;


namespace NetWork.MsgBuffer
{
    // 网络消息缓冲区池
    class MsgBufferPool : Singleton<MsgBufferPool>
    {
        // 初始化
        public bool Init(Int32 initCounts, Int32 bufferSize)
        {
            msgBufferSize = bufferSize;
            initBufferCounts = initCounts;

            msgBufferGuard = new object();
            if (msgBufferGuard == null)
                return false;

            msgBufferList = new List<MsgBuffer>();
            if (msgBufferList == null)
            {
                msgBufferGuard = null;
                return false;
            }

            // 预生成 MsgBuffer
            MsgBuffer msgBuffer = null;
            for (Int32 i = 0; i < initBufferCounts; ++i)
            {
                msgBuffer = MakeMsgBuffer();
                if (msgBuffer == null)
                    return false;

                msgBufferList.Add(msgBuffer);
            }

            return true;
        }

        // 释放操作
        public void Release()
        {
            if (msgBufferList != null)
            {
                foreach (MsgBuffer msgBuffer in msgBufferList)
                {
                    if (msgBuffer != null)
                        msgBuffer.Release();
                }

                msgBufferList.Clear();
                msgBufferList = null;
            }

            msgBufferGuard = null;
            initBufferCounts = 0;
            msgBufferSize = 0;
        }

        // 生成 MsgBuffer
        public MsgBuffer MallocMsgBuffer()
        {
            MsgBuffer msgBuffer = null;

            lock (msgBufferGuard)
            {
                if (msgBufferList.Count() > 0)
                {
                    msgBuffer = msgBufferList[0];
                    msgBufferList.RemoveAt(0);
                }

                if (msgBuffer == null)
                    msgBuffer = MakeMsgBuffer();

                return msgBuffer;
            }
        }

        // 释放 MsgBuffer
        public void FreeMsgBuffer(MsgBuffer msgBuffer)
        {
            if (msgBuffer == null)
            {
                Trace.Assert(false, "msgBuffer is null");
                return;
            }

            lock (msgBufferGuard)
            {
                if (msgBufferList.Count() >= initBufferCounts)
                {
                    msgBuffer.Release();
                    msgBuffer = null;
                }
                else
                {
                    msgBuffer.Reset();
                    msgBufferList.Add(msgBuffer);
                }
            }
        }

        // 私有函数
        #region
        private MsgBuffer MakeMsgBuffer()
        {
            MsgBuffer msgBuffer = new MsgBuffer();
            if (msgBuffer == null)
                return null;

            if (!msgBuffer.Init(msgBufferSize))
            {
                msgBuffer = null;
                return null;
            }

            return msgBuffer;
        }

        #endregion

        // 构造、析构函数
        #region
        public MsgBufferPool()
        {
            initBufferCounts = 0;
            msgBufferSize = 0;
            msgBufferList = null;
        }
        ~MsgBufferPool()
        {
        }
        #endregion

        private Int32           initBufferCounts; // 初始化缓冲区数量
        private Int32           msgBufferSize;    // 缓冲区大小
        private object          msgBufferGuard;   // 保证 msgBufferList 的多线程安全
        private List<MsgBuffer> msgBufferList;    // 网络消息缓冲区链表
    }
}
