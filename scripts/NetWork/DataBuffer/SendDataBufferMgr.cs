using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using NetWork.NetDefine;


namespace NetWork.DataBuffer
{
    class SendDataBufferMgr
    {
        // 初始化对象
        public bool Init()
        {
            sendBufferListLock = new object();
            if (sendBufferListLock == null)
                return false;

            sendBufferList = new List<SendDataBuffer>();
            if (sendBufferList == null)
            {
                sendBufferListLock = null;
                return false;
            }

            SendDataBuffer sendBuffer = DataBufferPool.GetInstance().MallocSendBuffer();
            if (sendBuffer == null)
            {
                sendBufferList = null;
                sendBufferListLock = null;
                return false;
            }

            sendBufferList.Add(sendBuffer);

            return true;
        }

        // 释放对象
        public void Release()
        {
            if (sendBufferList != null)
            {
                foreach(SendDataBuffer sendBuffer in sendBufferList)
                {
                    if (sendBuffer != null)
                        DataBufferPool.GetInstance().FreeSendBuffer(sendBuffer);
                }

                sendBufferList.Clear();
                sendBufferList = null;
            }

            sendBufferListLock = null;
        }

        // 重置所有发送缓冲区
        public void Reset()
        {
            Trace.Assert(!isAsyncSending, "isAsyncSending is true");
            isAsyncSending = false;
            if (sendBufferList != null)
            {
                // 释放多余的发送缓冲区
                while (sendBufferList.Count() > 1)
                {
                    if (sendBufferList[0] != null)
                    {
                        DataBufferPool.GetInstance().FreeSendBuffer(sendBufferList[0]);
                    }

                    sendBufferList.RemoveAt( 0 );
                }

                if (sendBufferList.Count() > 0 && sendBufferList[0] != null)
                    sendBufferList[0].Reset();
            }
        }

        // 是否正在发送数据
        public bool IsSending()
        {
            lock (sendBufferListLock)
            {
                return isAsyncSending;
            }
        }

        // 压入发送数据
        public bool PushSendData(UInt16 msgCommand, byte[] dataBuffer, Int32 bufferOffset, UInt16 bufferLength)
        {
            SendDataBuffer sendBuffer = null;
            lock (sendBufferListLock)
            {
                if (sendBufferList.Count() > 0)
                {
                    sendBuffer = sendBufferList[sendBufferList.Count() - 1];
                    if (sendBuffer != null && sendBuffer.GetFreeSpace() < (bufferLength + NetGlobalData.GetInstance().GetMsgHeadSize()))
                        sendBuffer = null;
                }

                if (sendBuffer == null)
                {
                    sendBuffer = DataBufferPool.GetInstance().MallocSendBuffer();
                    if (sendBuffer == null)
                        return false;

                    // 添加新的 Buffer
                    sendBufferList.Add(sendBuffer);
                }

                return sendBuffer.PushSendData(msgCommand, dataBuffer, bufferOffset, bufferLength);
            }
        }

        // 获得发送缓冲数据
        public void GetSendBuffer(ref byte[] sendDataBuffer, ref Int32 bufferOffset, ref Int32 dataLength)
        {
            lock (sendBufferListLock)
            {
                if (sendBufferList.Count() <= 0 || isAsyncSending)
                {
                    sendDataBuffer = null;
                    bufferOffset = 0;
                    dataLength = 0;

                    return;
                }

                SendDataBuffer sendBuffer = sendBufferList[0];
                if (sendBuffer == null)
                {
                    Trace.Assert(false, "sendBuffer is null");
                    sendDataBuffer = null;
                    bufferOffset = 0;
                    dataLength = 0;
                    sendBufferList.RemoveAt(0);

                    return;
                }

                sendBuffer.GetSendBuffer(ref sendDataBuffer, ref bufferOffset, ref dataLength);
                if (sendDataBuffer != null && bufferOffset >= 0 && dataLength > 0)
                    isAsyncSending = true;
            }
        }

        // 发送消息成功(对写入线程和发送线程分离)
        public bool SendComplete(Int32 sendLength)
        {
            lock (sendBufferListLock)
            {                
                if (!isAsyncSending || sendBufferList.Count() <= 0)
                {
                    Trace.Assert(isAsyncSending, "isAsyncSending is false");
                    Trace.Assert(sendBufferList.Count() > 0, "sendBufferList is empty");
                    return false;
                }

                SendDataBuffer sendBuffer = sendBufferList[0];
                if (sendBuffer == null)
                {
                    Trace.Assert(false, "sendBufferList element is null");
                    return false;
                }

                // 释放多余的缓冲区
                bool retValue = sendBuffer.SendComplete(sendLength);
                if (sendBufferList.Count() > 1 && sendBuffer.IsEmpty())
                {
                    DataBufferPool.GetInstance().FreeSendBuffer(sendBuffer);
                    sendBufferList.RemoveAt(0);
                }

                // 重置发送标记
                isAsyncSending = false;

                return retValue;
            }
        }

        // 判断所有缓冲区是否都为空
        public bool IsEmpty()
        {
            lock (sendBufferListLock)
            {
                if (sendBufferList.Count() <= 0)
                {
                    Trace.Assert(false, "sendBufferList is empty");
                    return true;
                }

                if (sendBufferList.Count() > 1)
                    return false;

                SendDataBuffer sendBuffer = sendBufferList[0];
                if (sendBuffer == null)
                {
                    Trace.Assert(false, "sendBufferList element is null");
                    return true;
                }

                return sendBuffer.IsEmpty();
            }
        }

        // 构造、析构函数
        #region
        public SendDataBufferMgr()
        {
            isAsyncSending = false;
            sendBufferList = null;
            sendBufferListLock = null;
        }

        ~SendDataBufferMgr()
        {
            sendBufferList = null;
            sendBufferListLock = null;
        }
        #endregion

        private bool                 isAsyncSending;        // 是否正在异步发送过程中
        private object               sendBufferListLock;    // sendBufferList的锁对象
        private List<SendDataBuffer> sendBufferList;        // 发送数据缓冲的链表
    }
}
