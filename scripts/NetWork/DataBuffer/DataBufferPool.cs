using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using AppUtility;
using NetWork.NetDefine;

namespace NetWork.DataBuffer
{
    // 数据(发送、接收)缓冲对象池
    class DataBufferPool : Singleton<DataBufferPool>
    {
        // 初始化
        public bool Init(Int32 reserveSize,
                         Int32 sendBufSize, Int32 sendBufExtend,
                         Int32 rcvBufSize,  Int32 rcvBufExtend)
        {
            reservedSize     = reserveSize;
            sendBufferSize   = sendBufSize;
            sendBufferExtend = sendBufExtend;
            rcvBufferSize    = rcvBufSize;
            rcvBufferExtend  = rcvBufExtend;

            sendBufferGuard = new object();
            if (sendBufferGuard == null)
                return false;

            rcvDataBufList = new List<RcvDataBuffer>();
            if (rcvDataBufList == null)
                return false;

            sendDataBufList = new List<SendDataBuffer>();
            if(sendDataBufList == null)
            {
                rcvDataBufList = null;
                return false;
            }

            sendDataBufMgrList = new List<SendDataBufferMgr>();
            if (sendDataBufMgrList == null)
            {
                sendDataBufList = null;
                rcvDataBufList = null;
                return false;
            }

            return true;
        }

        // 释放
        public void Release()
        {
            if (sendDataBufMgrList != null)
            {
                foreach (SendDataBufferMgr sendDataBufferMgr in sendDataBufMgrList)
                {
                    if (sendDataBufferMgr != null)
                        sendDataBufferMgr.Release();
                }

                sendDataBufMgrList.Clear();
                sendDataBufMgrList = null;
            }

            if (sendDataBufList != null)
            {
                foreach (SendDataBuffer sendDataBuffer in sendDataBufList)
                {
                    if (sendDataBuffer != null)
                        sendDataBuffer.Release();
                }

                sendDataBufList.Clear();
                sendDataBufList = null;
            }
            
            if (rcvDataBufList != null)
            {
                foreach (RcvDataBuffer rcvDataBuffer in rcvDataBufList)
                {
                    if (rcvDataBuffer != null)
                        rcvDataBuffer.Release();
                }

                rcvDataBufList.Clear();
                rcvDataBufList = null;
            }

            sendBufferGuard = null;
        }

        // 获得接收缓冲区
        public RcvDataBuffer MallocRcvBuffer()
        {
            RcvDataBuffer rcvDataBuf = null;
            if (rcvDataBufList.Count() > 0)
            {
                rcvDataBuf = rcvDataBufList[0];
                rcvDataBufList.RemoveAt(0);
            }

            if (rcvDataBuf == null)
            {
                rcvDataBuf = new RcvDataBuffer();
                if (rcvDataBuf == null)
                    return null;

                // 初始化 Buffer
                if (!rcvDataBuf.Init(rcvBufferSize, rcvBufferExtend))
                {
                    rcvDataBuf = null;
                    return null;
                }
            }

            return rcvDataBuf;
        }

        // 获得发送缓冲区管理器
        public SendDataBufferMgr MallocSendBufferMgr()
        {
            SendDataBufferMgr sendDataBufMgr = null;
            if (sendDataBufMgrList.Count() > 0)
            {
                sendDataBufMgr = sendDataBufMgrList[0];
                sendDataBufMgrList.RemoveAt(0);
            }

            if (sendDataBufMgr == null)
            {
                sendDataBufMgr = new SendDataBufferMgr();
                if (sendDataBufMgr == null)
                    return null;

                // 初始化 BufferMgr
                if (!sendDataBufMgr.Init())
                {
                    sendDataBufMgr = null;
                    return null;
                }
            }

            return sendDataBufMgr;
        }

        // 获得发送缓冲区
        public SendDataBuffer MallocSendBuffer()
        {
            SendDataBuffer sendDataBuffer = null;

            lock (sendBufferGuard)
            {
                if (sendDataBufList.Count() > 0)
                {
                    sendDataBuffer = sendDataBufList[0];
                    sendDataBufList.RemoveAt(0);
                }
            }

            if (sendDataBuffer == null)
            {
                sendDataBuffer = new SendDataBuffer();
                if (sendDataBuffer == null)
                    return null;

                // 初始化 Buffer
                if (!sendDataBuffer.Init(sendBufferSize, sendBufferExtend))
                {
                    sendDataBuffer = null;
                    return null;
                }
            }

            return sendDataBuffer;
        }

        // 释放接收缓冲区
        public void FreeRcvBuffer(RcvDataBuffer rcvDataBuf)
        {
            if (rcvDataBuf == null)
            {
                Trace.Assert(false, "rcvDataBuf is null");
                return;
            }

            if (rcvDataBufList.Count() >= reservedSize)
            {
                // 释放
                rcvDataBuf.Release();
                rcvDataBuf = null;
            }
            else
            {
                // 重置、加入缓冲区
                rcvDataBuf.Reset();
                rcvDataBufList.Add(rcvDataBuf);
            }
        }

        // 释放发送缓冲区
        public void FreeSendBuffer(SendDataBuffer sendDataBuf)
        {
            if (sendDataBuf == null)
            {
                Trace.Assert(false, "sendDataBuf is null");
                return;
            }

            lock (sendBufferGuard)
            {
                if (sendDataBufList.Count() >= reservedSize)
                {
                    sendDataBuf.Release();
                    sendDataBuf = null;
                }
                else
                {
                    sendDataBuf.Reset();
                    sendDataBufList.Add(sendDataBuf);
                }
            }
        }

        // 释放发送缓冲区管理器
        public void FreeSendBufferMgr(SendDataBufferMgr sendDataBufMgr)
        {
            if (sendDataBufMgr == null)
            {
                Trace.Assert(false, "sendDataBufMgr is null");
                return;
            }

            if (sendDataBufMgrList.Count() >= reservedSize)
            {
                sendDataBufMgr.Release();
                sendDataBufMgr = null;
            }
            else
            {
                sendDataBufMgr.Reset();
                sendDataBufMgrList.Add(sendDataBufMgr);
            }
        }

        // 获得网络消息头结构的长度
        public Int32 GetNetMsgHeadLength()
        {
            return netMsgHeadLength;
        }

        // 构造、析构函数
        #region
        public DataBufferPool()
        {
            reservedSize = 0;
            sendBufferSize = 0;
            sendBufferExtend = 0;
            rcvBufferSize = 0;
            rcvBufferExtend = 0;
            rcvDataBufList = null;
            sendDataBufMgrList = null;
            netMsgHead = new NetMsgHead();
            netMsgHeadLength = Marshal.SizeOf(netMsgHead);
        }

        ~DataBufferPool()
        {
        }
        #endregion

        private object sendBufferGuard;             // 保证sendDataBufList的多线程安全
        private Int32  reservedSize;                // 保留(发送、接收缓冲去)的最大大小
        private Int32  sendBufferSize;              // 发送缓冲区的大小
        private Int32  sendBufferExtend;            // 发送缓冲区的扩展大小
        private Int32  rcvBufferSize;               // 接收缓冲区的大小
        private Int32  rcvBufferExtend;             // 接收缓冲区的扩展大小
        private Int32  netMsgHeadLength;            // 网络消息头的长度
        private NetMsgHead netMsgHead;              // 网络消息头
        private List<RcvDataBuffer>       rcvDataBufList;     // 接收缓冲区的链表
        private List<SendDataBuffer>      sendDataBufList;    // 发送缓冲区的链表
        private List<SendDataBufferMgr>   sendDataBufMgrList; // 发送缓冲区管理器的链表
    }
}
