using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using AppUtility;
using NetWork.NetDefine;
using NetWork.MsgBuffer;
using NetWork.DataBuffer;
using NetWork.NetSession;
using UnityEngine ;
namespace NetWork.NetModule
{
    // 添加、删除会话事件结构
    public struct AddOrDelSessionEvent
    {
        public ADDORDELSESSIONEVENT  eventType;   // 事件类型
        public EVENTSESSIONPLACE     eventPlace;  // 添加事件时表示连接或接收连接、删除事件时表示删除的地方
        public NetSessionImpl        netSession;  // 网络会话

        // 重置操作
        public void Reset()
        {
            netSession = null;
            eventPlace = EVENTSESSIONPLACE.nullPlace;
            eventType  = ADDORDELSESSIONEVENT.nullOperation;
        }
    }

    class NetModuleMgr : Singleton<NetModuleMgr>
    {
        // 初始化对象
        public bool Init(ref NetModuleInit netModuleInit, HandleNetMessage netMsgHandle)
        {
            if (netMsgHandle == null)
            {
                Trace.Assert(false, "netMsgHandle is null");
                return false;
            }

            Trace.Assert(netModuleInit.netSessionClosedCallbackFunc != null, "netModuleInit.netSessionClosedCallbackFunc is null");
            Trace.Assert(netModuleInit.netSessionConnectedCallbackFunc != null, "netModuleInit.netSessionConnectedCallbackFunc is null");
            if (netModuleInit.netSessionClosedCallbackFunc == null || netModuleInit.netSessionConnectedCallbackFunc == null)
                return false;

            netSessionClosedCallbackFunc = netModuleInit.netSessionClosedCallbackFunc;
            netSessionConnectedCallbackFunc = netModuleInit.netSessionConnectedCallbackFunc;

            netSessionIDList = new List<int>();
            if (netSessionIDList == null)
                return false;

            netSessionDic = new Dictionary<int,NetSessionImpl>();
            if (netSessionDic == null)
            {
                Trace.Assert(false, "netSessionDic is null");
                return false;
            }

            addOrDelEventList = new WriteReadList<AddOrDelSessionEvent>();
            if (addOrDelEventList == null ||
                !addOrDelEventList.InitWriteReadList(HandleAddOrDelEvent, FreeEvent))
            {
                Trace.Assert(false, "addOrDelEventList is error");
                return false;
            }

            // 初始化网络消息池
            if (!MsgBufferPool.GetInstance().Init(netModuleInit.msgBufferCounts,
                                                  netModuleInit.msgBufferSize))
            {
                Trace.Assert(false, "MsgBufferPool init false");
                return false;
            }

            // 初始化消息管理器
            if (!MsgBufferMgr.GetInstance().Init(netMsgHandle))
            {
                Trace.Assert(false, "MsgBufferMgr init false");
                return false;
            }

            // 初始化数据缓冲区
            if (!DataBufferPool.GetInstance().Init(netModuleInit.bufferReserves,
                                                   netModuleInit.sendBufSize,
                                                   netModuleInit.sendBufExtend,
                                                   netModuleInit.rcvBufSize,
                                                   netModuleInit.rcvBufExtend))
            {
                Trace.Assert(false, "DataBufferPool init false");
                return false;
            }

            // 初始化网络会话池
            if (!NetSessionPool.GetInstance().Init(netModuleInit.sessionInitCount,
                                                   netModuleInit.sessionExtendCount))
            {
                Trace.Assert(false, "NetSessionPool init false");
                return false;
            }

            return true;
        }

        // 关闭对象
        public void Shutdown()
        {
            CloseAllSession();
            NetSessionPool.GetInstance().Release();
            DataBufferPool.GetInstance().Release();
            MsgBufferMgr.GetInstance().Release();
            MsgBufferPool.GetInstance().Release();
			if(netSessionIDList != null)
			{
	            netSessionIDList.Clear();
       	   		netSessionIDList = null;
			}
			if(addOrDelEventList != null)
			{
	            addOrDelEventList.ReleaseWriteReadList();
	            addOrDelEventList = null;
			}
        }

        // 刷新操作
        public void Update()
        {
            // 处理网络消息
            HandleNetMsg();

            // 处理添加、删除连接事件
            addOrDelEventList.HandleList();

            // 刷新所有会话
            UpdateAllNetSession();
        }

        // 连接远程地址
        public INetSession Connect(string remoteAddr, Int32 remotePort, bool isSync, ref bool isSuccess)
        {
            isSuccess = false;            
            if (remoteAddr == null || remotePort <= 0)
            {
                Trace.Assert(false, "Connect param error");
                return null;
            }

            NetSessionImpl netSession = NetSessionPool.GetInstance().MallocNetSession();
            if (netSession == null)
            {
                Trace.Assert(false, "malloc session null");
                return null;
            }

            // 同步连接
            if (isSync)
            {
                if (!netSession.PostConnect(remoteAddr, remotePort, true))
                {
                    NetSessionPool.GetInstance().FreeNetSession(netSession);
                    return null;
                }

                // 设置会话的状态
                NetSessionState sessionState = new NetSessionState();
                sessionState.Reset((Int32)NETSESSIONSTATE.netSessionStateUsing);
                netSession.SetSessionState(ref sessionState);

                // 投递接收操作
                if (!netSession.PostRcv())
                {
                    // 关闭会话，返回会话池
                    NetSessionPool.GetInstance().FreeNetSession(netSession);
                    return null;
                }

                // 添加到活动会话队列
                netSessionDic.Add(netSession.GetSessionID(), netSession);

                isSuccess = true;
                return (INetSession)netSession;
            }
            // 异步连接
            else
            {
                 if (netSession.PostConnect(remoteAddr, remotePort, false))
                     isSuccess = true;
            }

            return null;
        }

        // 关闭网络会话
        public void CloseNetSession(Int32 netSessionID)
        {
            if (!netSessionDic.ContainsKey(netSessionID))
                return;

            NetSessionImpl netSession = netSessionDic[netSessionID];
            if (netSession != null)
                CloseNetSession(netSession, EVENTSESSIONPLACE.appDelSession);
        }

        // 添加网络会话
        public bool AddNetEvent(NetSessionImpl netSession, ADDORDELSESSIONEVENT eventType, EVENTSESSIONPLACE eventPlace)
        {
            if(netSession == null)
                return false;

            AddOrDelSessionEvent sessionEvent = new AddOrDelSessionEvent();
            sessionEvent.netSession = netSession;
            sessionEvent.eventType  = eventType;
            sessionEvent.eventPlace = eventPlace;

            addOrDelEventList.AddElement(sessionEvent);

            return true;
        }

        // 处理添加、删除网络会话事件
        #region
        public void HandleAddOrDelEvent(AddOrDelSessionEvent addOrDelEvent)
        {
            NetSessionImpl netSession = addOrDelEvent.netSession;
            if (netSession == null)
                return;

            switch (addOrDelEvent.eventType)
            {
                case ADDORDELSESSIONEVENT.addNetSession:
                    {
                        // 设置会话的状态
                        NetSessionState sessionState = new NetSessionState();
                        sessionState.Reset((Int32)NETSESSIONSTATE.netSessionStateUsing);
                        netSession.SetSessionState(ref sessionState);

                        if (!netSession.PostRcv())
                        {
                            // 关闭会话，返回会话池
                            NetSessionPool.GetInstance().FreeNetSession(netSession);
                        }
                        else
                        {
                            netSessionDic.Add(netSession.GetSessionID(), netSession);

                            // 通知上层
                            if (netSessionConnectedCallbackFunc != null)
                                netSessionConnectedCallbackFunc((INetSession)netSession, addOrDelEvent.eventPlace, true);
                        }
                    }
                    break;
                case ADDORDELSESSIONEVENT.delNetSession:
                    {
                        // 通知应用层
                        if (addOrDelEvent.eventPlace != EVENTSESSIONPLACE.appDelSession)
                        {
                            if (netSessionClosedCallbackFunc != null)
                                netSessionClosedCallbackFunc(addOrDelEvent.netSession, addOrDelEvent.eventPlace);

                            NetSessionState sessionState = new NetSessionState();
                            sessionState.Reset((Int32)NETSESSIONSTATE.netSessionStateMax);
                            sessionState.appUseState = (Int32)NETSESSIONSTATE.netSessionStateShutdown;
                            addOrDelEvent.netSession.SetSessionState(ref sessionState);
                        }
                    }
                    break;
                case ADDORDELSESSIONEVENT.asyncConnectFailed:
                    {
                        // 通知上层
                        if (netSessionConnectedCallbackFunc != null)
                            netSessionConnectedCallbackFunc((INetSession)netSession, addOrDelEvent.eventPlace, false);

                        // 关闭会话，返回会话池
                        NetSessionPool.GetInstance().FreeNetSession(netSession);
                    }
                    break;
                default:
                    break;
            }
        }

        public void FreeEvent(AddOrDelSessionEvent addOrDelEvent)
        {
        }
        #endregion

        // 私有函数
        #region
        // 处理接收到的网络消息
        private void HandleNetMsg()
        {
            MsgBufferMgr.GetInstance().HandleList();
        }

        // 刷新所有网络会话
        private void UpdateAllNetSession()
        {
            if (netSessionDic.Count() <= 0)
                return;

            if (netSessionIDList.Count() > 0)
                netSessionIDList.Clear();

            NetSessionImpl netSession = null;
            foreach (KeyValuePair<Int32, NetSessionImpl> netSessionPair in netSessionDic)
            {
                netSession = netSessionPair.Value;
                if(netSession == null)
                    netSessionIDList.Add(netSessionPair.Key);

                // 删除关闭的会话
                if(!netSession.Update())
                    netSessionIDList.Add(netSessionPair.Key);
            }

            // 释放关闭的会话
            CloseAllShutdownSession();
        }

        // 释放关闭的会话
        private void CloseAllShutdownSession()
        {
            if (netSessionIDList.Count() <= 0)
                return;

            NetSessionImpl netSession = null;
            foreach(Int32 netSessionID in netSessionIDList)
            {
                if (netSessionDic.ContainsKey(netSessionID))
                {
                    netSession = netSessionDic[netSessionID];
                    if(netSession != null)
                        NetSessionPool.GetInstance().FreeNetSession(netSession);

                    // 从活动表中删除会话
                    netSessionDic.Remove(netSessionID);
                }
            }

            netSessionIDList.Clear();
        }

        // 关闭所有会话
        private void CloseAllSession()
        {
            if(netSessionDic == null || netSessionDic.Count() <= 0)
                return ;

            NetSessionImpl netSession = null;
            foreach (KeyValuePair<Int32, NetSessionImpl> netSessionPair in netSessionDic)
            {
                netSession = netSessionPair.Value;
                if (netSession == null)
                    NetSessionPool.GetInstance().FreeNetSession(netSession);
            }

            netSessionDic.Clear();
            netSessionDic = null;                
        }

        // 关闭会话(会引发删除事件)
        private void CloseNetSession(NetSessionImpl netSession, EVENTSESSIONPLACE eventPlace)
        {
            if (netSession == null)
                return;

            // 修改状态
            NetSessionState sessionState = new NetSessionState();
            sessionState.Reset((Int32)NETSESSIONSTATE.netSessionStateMax);
            if (eventPlace == EVENTSESSIONPLACE.sendDelSession)
                sessionState.sendUseState = (Int32)NETSESSIONSTATE.netSessionStateShutdown;
            else if (eventPlace == EVENTSESSIONPLACE.rcvDelSession)
                sessionState.rcvUseState = (Int32)NETSESSIONSTATE.netSessionStateShutdown;
            else if (eventPlace == EVENTSESSIONPLACE.appDelSession)
                sessionState.appUseState = (Int32)NETSESSIONSTATE.netSessionStateShutdown;
            else
            {
                Trace.Assert(false, "Error eventPlace");
            }

            // 通知 moduleMgr
            NetSessionState retState = netSession.SetSessionState(ref sessionState);
            if (eventPlace == EVENTSESSIONPLACE.sendDelSession)
            {
                if (retState.appUseState == (Int32)NETSESSIONSTATE.netSessionStateUsing)
                {
                    AddNetEvent(netSession, ADDORDELSESSIONEVENT.delNetSession, eventPlace);
                }
            }
            else if (eventPlace == EVENTSESSIONPLACE.rcvDelSession)
            {
                if (retState.appUseState == (Int32)NETSESSIONSTATE.netSessionStateUsing)
                {
                    AddNetEvent(netSession, ADDORDELSESSIONEVENT.delNetSession, eventPlace);
                }
            }
            else
            {
                Trace.Assert(false, "Error eventPlace");
            }

            // 关闭 Socket
            netSession.CloseSocket();
        }
        #endregion

        // 网络模型回调函数
        #region
        public void NetIOComplete(object netSession, SocketAsyncEventArgs asyncEvent)
        {
            if (asyncEvent == null)
            {
                Trace.Assert(false, "asyncEvent is null");
                return;
            }

            switch (asyncEvent.LastOperation)
            {
                case SocketAsyncOperation.Send:
                    ProcessSendComplete(asyncEvent);
                    break;
                case SocketAsyncOperation.Receive:
                    ProcessRcvComplete(asyncEvent);
                    break;
                case SocketAsyncOperation.Connect:
                    ProcessConnectComplete(asyncEvent);
                    break;
                default:
                    Trace.Assert(false, "asyncEvent.LastOperation error");
                    break;
            }
        }
        // 处理发送事件
        public void ProcessSendComplete(SocketAsyncEventArgs asyncEvent)
        {
            NetSessionImpl netSession = (NetSessionImpl)asyncEvent.UserToken;
            if (netSession == null)
            {
                Trace.Assert(false, "netSession is null");
                return;
            }

            if (asyncEvent.SocketError != SocketError.Success)
            {
                CloseNetSession(netSession, EVENTSESSIONPLACE.sendDelSession);
            }
            else
            {
                if (!netSession.ProcessSendComplete(asyncEvent))
                {
                    CloseNetSession(netSession, EVENTSESSIONPLACE.sendDelSession);
                }
            }
        }
        // 处理接收事件
        public void ProcessRcvComplete(SocketAsyncEventArgs asyncEvent)
        {
            NetSessionImpl netSession = (NetSessionImpl)asyncEvent.UserToken;
            if (netSession == null)
            {
                Trace.Assert(false, "netSession is null");
                return;
            }

            if (asyncEvent.SocketError != SocketError.Success)
            {
                CloseNetSession(netSession, EVENTSESSIONPLACE.rcvDelSession);
            }
            else
            {
                if (!netSession.ProcessRcvComplete(asyncEvent))
                {
                    CloseNetSession(netSession, EVENTSESSIONPLACE.rcvDelSession);
                }
            }
        }
        // 处理连接事件
        public void ProcessConnectComplete(SocketAsyncEventArgs asyncEvent)
        {
            NetSessionImpl netSession = (NetSessionImpl)asyncEvent.UserToken;
            if (netSession == null)
            {
                Trace.Assert(false, "netSession is null");
                return;
            }

            if (asyncEvent.SocketError != SocketError.Success)
            {
                AddNetEvent(netSession, ADDORDELSESSIONEVENT.asyncConnectFailed, EVENTSESSIONPLACE.connectAddSession);               
            }
            else
            {
                AddNetEvent(netSession, ADDORDELSESSIONEVENT.addNetSession, EVENTSESSIONPLACE.connectAddSession);
            }
        }
        #endregion

        // 构造、析构函数
        #region
        public NetModuleMgr()
        {
            netSessionDic = null;
            netSessionIDList = null;
            addOrDelEventList = null;            
            netSessionClosedCallbackFunc = null;
            netSessionConnectedCallbackFunc = null;
        }
        ~NetModuleMgr()
        {
        }
        #endregion

        private List<Int32>                          netSessionIDList;   // 保存网络会话ID的链表
        private Dictionary<Int32,NetSessionImpl>     netSessionDic;      // 有效网络会话表
        private WriteReadList<AddOrDelSessionEvent>  addOrDelEventList;  // 添加、删除网络会话事件队列
        private NetSessionClosed                     netSessionClosedCallbackFunc;    // 网络会话关闭回调函数
        private NetSesssionConnected                 netSessionConnectedCallbackFunc; // 网络连接成功回调函数
    }
}
