using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Net.Sockets;
using NetWork.NetDefine;
using NetWork.DataBuffer;
using NetWork.NetModule;
using NetWork.MsgBuffer;
using UnityEngine ;
using GameLogical.GameEnitity;

using GameLogical;

namespace NetWork.NetSession
{
    public class NetSessionImpl : INetSession
    {
        public bool Init()
        {
            usingStateLock = new object();
            if (usingStateLock == null)
                return false;

            rcvAsyncEventArgs = new SocketAsyncEventArgs();
            if (rcvAsyncEventArgs == null)
                return false;
            else
            {
                rcvAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(NetModuleMgr.GetInstance().NetIOComplete);
                rcvAsyncEventArgs.UserToken = this;
            }

            sendAsyncEventArgs = new SocketAsyncEventArgs();
            if (sendAsyncEventArgs == null)
                return false;
            else
            {
                sendAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(NetModuleMgr.GetInstance().NetIOComplete);
                sendAsyncEventArgs.UserToken = this;             
            }

            rcvDataBuffer = DataBufferPool.GetInstance().MallocRcvBuffer();
            if (rcvDataBuffer == null)
                return false;

            sendDataBufferMgr = DataBufferPool.GetInstance().MallocSendBufferMgr();
            if (sendDataBufferMgr == null)
            {
                DataBufferPool.GetInstance().FreeRcvBuffer(rcvDataBuffer);
                rcvDataBuffer = null;
                return false;
            }

            return true;
        }

        public void Release()
        {
            CloseSocket();
            usingStateLock = null;           
            rcvAsyncEventArgs = null;
            sendAsyncEventArgs = null;
            if (rcvDataBuffer != null)
            {
                DataBufferPool.GetInstance().FreeRcvBuffer(rcvDataBuffer);
                rcvDataBuffer = null;
            }

            if (sendDataBufferMgr != null)
            {
                DataBufferPool.GetInstance().FreeSendBufferMgr(sendDataBufferMgr);
                sendDataBufferMgr = null;
            }
        }
        
        public void Reset()
        {
            if (rcvDataBuffer != null)
                rcvDataBuffer.Reset();

            if (sendDataBufferMgr != null)
                sendDataBufferMgr.Reset();

            usingState.Reset((Int32)NETSESSIONSTATE.netSessionStateNull);
            setSecurityPolicy = false;
            postRcving     = false;
            remotePort     = 0;
            sessionIndex   = -1;
            sendByteCounts = 0;
            rcvByteCounts  = 0;
            netSocket      = null;
            remoteAddr     = null;
            rcvAsyncEventArgs.SetBuffer(null, 0, 0);
            sendAsyncEventArgs.SetBuffer(null, 0, 0);
        }

        public bool Update()
        {
            if (IsNeedClose())
                return false;

            return true;
        }

        public void SetRemoteAddress(string remoteAddress, Int32 remoteProcPort)
        {
            remotePort = remoteProcPort;
            remoteAddr = new string(remoteAddress.ToCharArray());
        }

        public void CloseSocket()
        {
			MonoBehaviour.print("close socket" + this.GetSessionID());
			if (netSocket != null && GameDataCenter.GetInstance().SocketReset != 1)
            {
	//			gameGlobal.g_curPage.hide();
	//			gameGlobal.g_mainUI.hide();
	//			gameGlobal.g_mainUI.gameObject.SetActive(false);
	//			gameGlobal.g_LoginUI.show();
	//			SocketReset message = new SocketReset();
				//message.ob = this.gameObject.transform.parent.gameObject ;
	//			EnitityMgr.GetInstance().OnMessage(message);

				GameDataCenter.GetInstance().SocketReset = 1;
//				gameGlobal.g_SelectPlayerUI.m_CanClick = true;

                netSocket.Shutdown(SocketShutdown.Both);
                netSocket.Close();
                netSocket = null;
            }
        }

        public bool IsReadClose()
        {
            if (usingState.appUseState == (int)NETSESSIONSTATE.netSessionStateShutdown
             || usingState.sendUseState == (int)NETSESSIONSTATE.netSessionStateShutdown
             || usingState.rcvUseState == (int)NETSESSIONSTATE.netSessionStateShutdown)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // 判断会话是否需要关闭
        public bool IsNeedClose()
        {
            if (usingState.appUseState == (int)NETSESSIONSTATE.netSessionStateShutdown
             && usingState.sendUseState == (int)NETSESSIONSTATE.netSessionStateShutdown
             && usingState.rcvUseState == (int)NETSESSIONSTATE.netSessionStateShutdown)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // 获得会话的状态
        public Int32 GetSessionState(NETSESSIONSTATETYPE stateType)
        {
            switch (stateType)
            {
                case NETSESSIONSTATETYPE.netSessionAppState:
                    return usingState.appUseState;                    
                case NETSESSIONSTATETYPE.netSessionRcvState:
                    return usingState.rcvUseState;                    
                case NETSESSIONSTATETYPE.netSessionSendState:
                    return usingState.sendUseState;
                default:
                    {
                        Trace.Assert(false, "GetSessionState input param error");
                        return 0;
                    }
            }
        }

        // 设置会话的状态
        public NetSessionState SetSessionState(ref NetSessionState sessionState)
        {
            lock (usingStateLock)
            {
                if (sessionState.appUseState != (Int32)NETSESSIONSTATE.netSessionStateMax)
                {
                    usingState.appUseState = sessionState.appUseState;
                    if (usingState.appUseState == (Int32)NETSESSIONSTATE.netSessionStateShutdown)
                    {
                        if (!IsSending() && usingState.sendUseState != (Int32)NETSESSIONSTATE.netSessionStateShutdown)
                            usingState.sendUseState = (Int32)NETSESSIONSTATE.netSessionStateShutdown;

                        if (!IsRcving() && usingState.rcvUseState != (Int32)NETSESSIONSTATE.netSessionStateShutdown)
                            usingState.rcvUseState = (Int32)NETSESSIONSTATE.netSessionStateShutdown;
                    }
                }

                if (sessionState.sendUseState != (Int32)NETSESSIONSTATE.netSessionStateMax)
                {
                    usingState.sendUseState = sessionState.sendUseState;
                    if (usingState.sendUseState == (Int32)NETSESSIONSTATE.netSessionStateShutdown)
                    {
                        if (!IsRcving() && usingState.rcvUseState != (Int32)NETSESSIONSTATE.netSessionStateShutdown)
                            usingState.rcvUseState = (Int32)NETSESSIONSTATE.netSessionStateShutdown;
                    }
                }

                if (sessionState.rcvUseState != (Int32)NETSESSIONSTATE.netSessionStateMax)
                {
                    usingState.rcvUseState = sessionState.rcvUseState;
                    if (usingState.rcvUseState == (Int32)NETSESSIONSTATE.netSessionStateShutdown)
                    {
                        if (!IsSending() && usingState.sendUseState != (Int32)NETSESSIONSTATE.netSessionStateShutdown)
                            usingState.sendUseState = (Int32)NETSESSIONSTATE.netSessionStateShutdown;
                    }
                }

                return usingState;
            }
        }

        // 连接远程地址
        public bool PostConnect(string remoteIP, Int32 remotePort, bool isSync)
        {
            netSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);           
            if (netSocket == null)
                return false;

            SetRemoteAddress(remoteIP, remotePort);
            IPAddress remoteAddr = IPAddress.Parse(remoteIP);
            if (isSync)
            {
                // 同步连接
                try
                {
                    netSocket.Connect(remoteAddr, remotePort);
                }
                catch (SocketException excetption)
                {
                    Trace.Assert(false, ("netSocket.Connect error" + excetption.ToString()));
                    return false;
                }
            }
            else
            {
                // 异步连接
                SocketAsyncEventArgs connectAsyncEvent = new SocketAsyncEventArgs();
                if (connectAsyncEvent == null)
                    return false;

                connectAsyncEvent.Completed += new EventHandler<SocketAsyncEventArgs>(NetModuleMgr.GetInstance().NetIOComplete);
                connectAsyncEvent.UserToken = this;
                connectAsyncEvent.RemoteEndPoint = new IPEndPoint(remoteAddr, remotePort);

                netSocket.ConnectAsync(connectAsyncEvent);                
            }

            return true;
        }

        // 投递发送数据操作
        public bool PostSend()
        {
            if (netSocket == null || IsReadClose())
                return false;

            if (sendDataBufferMgr.IsSending())
                return true;

            byte[] sendDataBuffer = null;
            Int32 dataOffset = 0;
            Int32 dataLength = 0;

            sendDataBufferMgr.GetSendBuffer(ref sendDataBuffer, ref dataOffset, ref dataLength);
            if (sendDataBuffer != null && dataOffset >= 0 && dataLength > 0)
            {
                sendAsyncEventArgs.SetBuffer(sendDataBuffer, dataOffset, dataLength);
                if (!netSocket.SendAsync(sendAsyncEventArgs)) // 如果返回 false 表示同步IO完成
                    NetModuleMgr.GetInstance().ProcessSendComplete(sendAsyncEventArgs);
            }

            return true;
        }

        // 处理发送消息完成
        public bool ProcessSendComplete(SocketAsyncEventArgs asyncEvent)
        {
            if (asyncEvent != sendAsyncEventArgs)
            {
                Trace.Assert(false, "sendAsyncEventArgs is not equal");
                return false;
            }

            sendDataBufferMgr.SendComplete(asyncEvent.BytesTransferred);
            sendByteCounts += asyncEvent.BytesTransferred;

            // 再次投递发送
            if (!sendDataBufferMgr.IsEmpty())
                return PostSend();

            return true;
        }

        // 投递接收数据操作
        public bool PostRcv()
        {
            if (netSocket == null || IsReadClose() || postRcving)
            {
                Trace.Assert(false, "postRcving is err");
                return false;
            }

            byte[] rcvBuffer = null;
            Int32 bufferOffset = 0;
            Int32 bufferLength = 0;

            rcvDataBuffer.GetRcvBuffer(ref rcvBuffer, ref bufferOffset, ref bufferLength);
            if (rcvBuffer != null && bufferOffset >= 0 && bufferLength > 0)
            {
                postRcving = true;
                rcvAsyncEventArgs.SetBuffer(rcvBuffer, bufferOffset, bufferLength);
                if (!netSocket.ReceiveAsync(rcvAsyncEventArgs))  // 如果返回 false 表示同步IO完成
                    NetModuleMgr.GetInstance().ProcessRcvComplete(rcvAsyncEventArgs);
            }

            return true;
        }

        // 处理数据发送完成
        public bool ProcessRcvComplete(SocketAsyncEventArgs asyncEvent)
        {
            if (asyncEvent != rcvAsyncEventArgs)
            {
                Trace.Assert(false, "rcvAsyncEventArgs is not equal");
                return false;
            }

            postRcving = false;
            rcvDataBuffer.RcvComplete(asyncEvent.BytesTransferred);
            rcvByteCounts += asyncEvent.BytesTransferred;

            // 弹出消息
            if (!PopMsgFrmRcvBuffer())
                return false;
			
            // 再次投递接收
            return PostRcv();
        }

        // 继承实现通用接口
        #region
        // 获得会话的索引 ID
        public Int32 GetSessionID()
        {
            return sessionIndex;
        }

        // 获得接收到的数据长度
        public Int32 GetSendedDataLength()
        {
            return sendByteCounts;
        }

        // 获得发送的数据长度
        public Int32 GetRcvedDataLength()
        {
            return rcvByteCounts;
        }

        // 获得会话连接的远端IP
        public string GetRemoteAddress()
        {
            return remoteAddr;
        }

        // 获得会话联机的远端端口
        public Int32 GetRemotePort()
        {
            return remotePort;
        }

        // 设置会话的索引 ID
        public void SetSessionID(Int32 sessionID)
        {
            sessionIndex = sessionID;
        }

        // 发送信息
        public Int32 SendMessage(UInt16 msgCommand, byte[] messageBuffer)
        {
            return SendMessage(msgCommand, messageBuffer, 0, (UInt16)messageBuffer.Length);
        }

        // 发送信息
        public Int32 SendMessage(UInt16 msgCommand, byte[] messageBuffer, UInt16 bufferLength)
        {
            return SendMessage(msgCommand, messageBuffer, 0, bufferLength);            
        }
	
	public Int32 SendMessage(string head,Dictionary<string,object> data){
	    Dictionary<string,object> iList = new Dictionary<string, object>();
    	iList["head"] = head ;
	    iList["body"] = data ;
	    string result = AppUtility.Json.Serialize(iList);
			UnityEngine.Debug.Log(result);
	    return SendMessage(0, Encoding.Default.GetBytes(result));
	}
		
		
		/*public Int32 SendMessage(string json){
			byte[] messageBuffer = Encoding.Default.GetBytes(json);
			return 0 ;
		}*/
		
        // 发送信息
        public Int32 SendMessage(UInt16 msgCommand, byte[] messageBuffer, Int32 bufferOffset, UInt16 bufferLength)
        {
            if (messageBuffer == null || bufferOffset < 0 || bufferLength == 0 || bufferLength > (UInt16)NETGLOBALDATA.maxSendPackLength)
            {
                Trace.Assert(false, "SendMessage input param err");
                return 0;
            }

            if (IsReadClose())
                return 0;

            // 压入数据
            if (!sendDataBufferMgr.PushSendData(msgCommand, messageBuffer, bufferOffset, bufferLength))
                return 0;

            // 投递发送
            if (!PostSend())
                return 0;

            return bufferLength;
        }
        #endregion

        // 私有函数
        #region
        // 是否正在接收
        private bool IsSending()
        {
            return sendDataBufferMgr.IsSending();
        }

        // 是否正在发送
        private bool IsRcving()
        {
            return postRcving;
        }

        // 保持连接
        private void MaintianLing()
        {
            if (IsReadClose())
                return;

            // 发送心跳消息
        }
        // 弹出消息
        private bool PopMsgFrmRcvBuffer()
        {
            if (IsReadClose())
                return true;

            byte[] readBuffer = null;
            Int32  readOffset = 0;
            Int32  readLength = 0;
            UInt16 msgCommand = 0;

            do
            {
                // 获得可读数据
                if (!rcvDataBuffer.GetRcvMessage(ref msgCommand, ref readBuffer, ref readOffset, ref readLength, ref setSecurityPolicy))
                    return false;
				
                if (readBuffer != null && readLength > 0)
                {
                    // 从流中读取数据写入消息对象
                    MsgBufferMgr.GetInstance().PushNetMessage(msgCommand, readBuffer, readOffset, readLength, (INetSession)this);

                    // 移动读取位置（数据头 + 数据长度）
                    rcvDataBuffer.ReadData(null, readLength + NetGlobalData.GetInstance().GetMsgHeadSize());
                }
                else
                {	
                    return true;
                }
            }while(true);
        }
        #endregion

        // 构造、析构函数
        #region
        public NetSessionImpl()
        {
            setSecurityPolicy = false;
            postRcving = false;
            remotePort = 0;
            netSocket = null;
            sessionIndex = 0;
            sendByteCounts = 0;
            rcvByteCounts = 0;
            rcvDataBuffer = null;
            usingStateLock = null;
            sendDataBufferMgr = null;
            rcvAsyncEventArgs = null;
            sendAsyncEventArgs = null;          
            usingState = new NetSessionState();
            usingState.Reset((Int32)NETSESSIONSTATE.netSessionStateNull);
        }
        ~NetSessionImpl()
        {
            remoteAddr = null;
        }
        #endregion

        private bool                      setSecurityPolicy;  // 是否设置了安全策略
        private bool                      postRcving;         // 是否正在投递接收
        private Int32                     remotePort;         // 会话连接的远程端口
        private Int32                     sessionIndex;       // 会话的索引
        private Int32                     sendByteCounts;     // 发送的字节数
        private Int32                     rcvByteCounts;      // 接收的字节数
        private Socket                    netSocket;          // Socket 对象
        private string                    remoteAddr;         // 会话连接的远程地址
        private object                    usingStateLock;     // 会话状态的多线程锁
        private NetSessionState           usingState;         // 会话的状态
        private RcvDataBuffer             rcvDataBuffer;      // 管理接收数据缓冲
        private SendDataBufferMgr         sendDataBufferMgr;  // 管理发送数据缓冲
        private SocketAsyncEventArgs      rcvAsyncEventArgs;  // 接收异步事件对象
        private SocketAsyncEventArgs      sendAsyncEventArgs; // 发送异步事件对象
    }
}
