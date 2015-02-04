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
    class RcvDataBuffer : CircuitBuffer
    {
        // 接收数据完成
        public bool RcvComplete(Int32 rcvDataLength)
        {
            return WriteData(null, 0, rcvDataLength, ENCRYPTOPT.codeUnencrypt);
        }

        // 获得接收缓冲区
        public void GetRcvBuffer(ref byte[] rcvDataBuffer, ref Int32 bufferOffset, ref Int32 bufferLength)
        {
            rcvDataBuffer = GetDataBuffer();
            bufferOffset = GetWritePosition();
            bufferLength = GetWriteLength();
        }

        // 获得单个消息
        // 发生错误时返回 false,上层需要处理
        public bool GetRcvMessage(ref UInt16 msgCommand, ref byte[] msgBuffer, ref Int32 bufferOffset, ref Int32 msgLength, ref bool setSecurityPolicy)
        {
            msgCommand = 0;
            msgLength  = 0;
            msgBuffer  = null;
            bufferOffset = 0;
			
            // 判断是否设置安全策略
            /*if (!setSecurityPolicy)
            {
                Int32 policyMsgLength = Encoding.ASCII.GetBytes(policySecurity).Length;
                if (GetDataLength() > policyMsgLength)
                {
                    Trace.Assert(false, "Security policy msg length err");
                    return false;
                }

                if (GetDataLength() < policyMsgLength)
                    return true;
                else
                {
                    byte[] policyMsg = new byte[GetDataLength()];
                    Buffer.BlockCopy(GetDataBuffer(), GetReadPosition(), policyMsg, 0, GetDataLength());
                    if (Encoding.Default.GetString(policyMsg) == policySecurity)
                    {
                        setSecurityPolicy = true;
                        return true;
                    }
                    else
                        return false;
                }
            }*/

            if (GetDataLength() <= NetGlobalData.GetInstance().GetMsgHeadSize())
                return true;

            if (!ReadyReadData(NetGlobalData.GetInstance().GetMsgHeadSize()))
            {
                Trace.Assert(false, "ReadyReadData failed");
                return false;
            }

            msgHead.Reset();
            /*if (!Utilitys.BytesToStruct<NetMsgHead>(GetDataBuffer(), GetReadPosition(), NetGlobalData.GetInstance().GetMsgHeadSize(), ref msgHead))
            {
                Trace.Assert(false, "BytesToStruct failed");
                return false;
            }*/
			
		    if (!Utilitys.BytesToStruct<NetMsgHead2>(GetDataBuffer(), GetReadPosition(), NetGlobalData.GetInstance().GetMsgHeadSize(), ref msgHead))
            {
                Trace.Assert(false, "BytesToStruct failed");
                return false;
            }
		
		    //is zip 
			if(msgHead.msgCompressed == 0x01){
				msgCommand = 1 ;
			}
			
			//byte[] bytes = GetDataBuffer() ;
			msgHead.msgLength = Utilitys.BytesToBigEndian32Bit(GetDataBuffer(),GetReadPosition());

            // 检查消息的长度有效性
            if (msgHead.msgLength > (UInt16)NETGLOBALDATA.maxSendPackLength)
            {
                Trace.Assert(false, "msgHead.msgLength is too long");
                return false;
            }

            // TODO<szk>:添加检查效验值

            // 检查数据是否足够长
            if ((msgHead.msgLength + NetGlobalData.GetInstance().GetMsgHeadSize()) > GetDataLength())
                return true;

            // 准备读取消息+头
            if (!ReadyReadData(msgHead.msgLength + NetGlobalData.GetInstance().GetMsgHeadSize()))
            {
                Trace.Assert(false, "ReadyReadData failed");
                return false;
            }

            // TODO<szk>:添加解密

            Trace.Assert(GetReadPosition() >= 0, "Read Position is err");

            msgBuffer = GetDataBuffer();
            bufferOffset = GetReadPosition() + NetGlobalData.GetInstance().GetMsgHeadSize();
            msgLength = msgHead.msgLength;
            //msgCommand = msgHead.msgCommand;

            return true;
        }

        // 构造、析构函数
        #region
        public RcvDataBuffer()
        {
            //msgHead = new NetMsgHead();
			msgHead = new NetMsgHead2();
            msgHead.Reset();
        }
        ~RcvDataBuffer()
        {
        }
        #endregion

        //private NetMsgHead  msgHead;
		private NetMsgHead2  msgHead;
        const string policySecurity =
@"<?xml version='1.0'?>
<cross-domain-policy>
        <allow-access-from domain=""*"" to-ports=""*"" />
</cross-domain-policy>";
    }
}
