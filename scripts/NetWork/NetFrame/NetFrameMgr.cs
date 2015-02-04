using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using AppUtility;
using NetWork.NetSession;
using NetWork.NetModule;
using UnityEngine ;

namespace NetWork.NetFrame
{
    class NetFrameMgr : Singleton<NetFrameMgr>
    {
        // 初始化
        public bool Init(ref NetModuleInit netModuleInit)
        {
            msgHandleDic = new Dictionary<ushort, HandleMsg>();
            if (msgHandleDic == null)
                return false;

            // 初始化网络底层
            if (!NetModuleMgr.GetInstance().Init(ref netModuleInit, this.HandleNetMessage))
                return false;

            return true;
        }

        // 关闭
        public void Shutdown()
        {
            // 关闭网络底层
            NetModuleMgr.GetInstance().Shutdown();

            if(msgHandleDic != null)
            {
				MonoBehaviour.print("clear msgHandleDic");
                msgHandleDic.Clear();
                msgHandleDic = null;
            }
        }

        // 刷新操作
        public void Update()
        {
            // 刷新网络底层
            NetModuleMgr.GetInstance().Update();
        }

        // 注册消息处理函数
        public bool RegisterMsgHanle(UInt16 msgCmd, HandleMsg msgHandle)
        {
            if (msgHandle == null)
            {
                Trace.Assert(false, "msgHandle is null");
                return false;
            }

            if (msgHandleDic.ContainsKey(msgCmd))
            {
                Trace.Assert(false, "reregister cmd" + msgCmd);
                return false;
            }

            msgHandleDic.Add(msgCmd, msgHandle);

            return true;
        }

        // 取消注册的消息处理函数
        public void UnregisterMsgHandle(UInt16 msgCmd)
        {
            if (msgHandleDic.ContainsKey(msgCmd))
                msgHandleDic.Remove(msgCmd);
        }

        // 连接远程地址
        // 同步连接时返回连接成功的网络会话
        // isSuccess 返回函数是否操作成功
        public INetSession Connect(string remoteAddr, Int32 remotePort, bool isSync, ref bool isSuccess)
        {
            return NetModuleMgr.GetInstance().Connect(remoteAddr, remotePort, isSync, ref isSuccess);
        }

        // 关闭网络会话
        public void CloseNetSession(Int32 netSessionID)
        {
            NetModuleMgr.GetInstance().CloseNetSession(netSessionID);
        }

        // 网络层回调函数
        #region
        public void HandleNetMessage(UInt16 msgCmd, INetSession netSession, byte[] msgBuffer, Int32 bufferOffset, Int32 msgLength)
        {
            if (netSession == null || msgBuffer == null)
                return;

            if (!msgHandleDic.ContainsKey(msgCmd))
            {
                Trace.Assert(false, "Unregister cmd : " + msgCmd);
                return;
            }

            HandleMsg handleFunc = msgHandleDic[msgCmd];
            if (handleFunc == null)
            {
                Trace.Assert(false, msgCmd + " MsgHandle is null");
                return;
            }

            // 调用消息处理函数
            handleFunc(netSession, msgBuffer, bufferOffset, msgLength);
        }
        #endregion

        // 构造、析构函数
        #region
        public NetFrameMgr()
        {
            msgHandleDic = null;
        }
        ~NetFrameMgr()
        {
        }
        #endregion

        private Dictionary<ushort, HandleMsg> msgHandleDic;   // 处理网络消息的函数集合
    }
}
