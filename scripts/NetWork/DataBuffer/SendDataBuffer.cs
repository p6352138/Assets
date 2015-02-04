using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using AppUtility;
using NetWork.NetDefine;



namespace NetWork.DataBuffer
{
    class SendDataBuffer : CircuitBuffer
    {
        // 发送数据完成
        public bool SendComplete(Int32 sendLength)
        {
            return ReadData(null, sendLength);
        }

        // 获得发送缓冲数据
        public void GetSendBuffer(ref byte[] sendDataBuffer, ref Int32 bufferOffset, ref Int32 dataLength)
        {
            sendDataBuffer = GetDataBuffer();
            bufferOffset = GetReadPosition();
            dataLength = GetReadLength();
            if (dataLength > (int)NETGLOBALDATA.maxSendDataLength)
                dataLength = (int)NETGLOBALDATA.maxSendDataLength;
        }

        // 压入数据
        public bool PushSendData(UInt16 msgCommand, byte[] dataBuffer, Int32 bufferOffset, UInt16 bufferLength)
        {
            if (GetFreeSpace() < (bufferLength + NetGlobalData.GetInstance().GetMsgHeadSize()))
            {
                Trace.Assert(false, "PushSendData so long");
                return false;
            }

            //msgHead.msgCommand = msgCommand;
            msgHead.msgLength  = bufferLength;
            //msgHead.msgUseType = (byte)NETMSGUSETYPE.appUseMsg;

            byte[] msgHeadBuffer = new byte[NetGlobalData.GetInstance().GetMsgHeadSize()];
            if (msgHeadBuffer == null)
                return false;

            /*if (!Utilitys.StructToBytes<NetMsgHead>(msgHead, msgHeadBuffer, NetGlobalData.GetInstance().GetMsgHeadSize()))
                return false;*/

            // 写入数据头
            //if (!WriteData(msgHeadBuffer, 0, msgHeadBuffer.Length, ENCRYPTOPT.codeEncryptNull))
            //    return false;
			byte[] abyte0 = Utilitys.LittleEndianToBytes32Bit(dataBuffer.Length);
			
			//for(int i = 0; i<bs)
			if (!WriteData(abyte0, 0, abyte0.Length , ENCRYPTOPT.codeEncryptNull))
                return false;
            // 写入数据
            if (!WriteData(dataBuffer, bufferOffset, bufferLength, ENCRYPTOPT.codeEncryptNull))
                return false;

            return true;
        }

        // 构造、析构函数
        #region
        public SendDataBuffer() : base()
        {
            msgHead = new NetMsgHead2();
            msgHead.Reset();
        }
        ~SendDataBuffer()
        {
        }
        #endregion

        private NetMsgHead2 msgHead;
    }
}
