using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppUtility;
using System.Diagnostics;

namespace NetWork.NetSession
{
    // 网络会话池
    class NetSessionPool : Singleton<NetSessionPool>
    {
        // 初始化
        public bool Init(Int32 initSize, Int32 extendSize)
        {
            initPoolSize = initSize;
            extendPoolSize = extendSize;

            netSessionListlock = new object();
            if (netSessionListlock == null)
                return false;

            netSessionList = new List<NetSessionImpl>();
            if (netSessionList == null)
                return false;

            if (!MakeNetSession(initPoolSize))
                return false;

            return true;
        }

        // 释放
        public void Release()
        {
            if (netSessionList != null)
            {
                foreach (NetSessionImpl netSession in netSessionList)
                {
                    if (netSession != null)
                        netSession.Release();
                }

                netSessionList.Clear();
                netSessionList = null;
            }

            netSessionListlock = null;
        }

        // 生成 NetSession 对象
        public NetSessionImpl MallocNetSession()
        {
            NetSessionImpl netSession = null;
            lock (netSessionListlock)
            {
                if (netSessionList.Count() <= 0)
                    MakeNetSession(extendPoolSize);

                if (netSessionList.Count() <= 0)
                    return null;

                netSession = netSessionList[0];
                netSessionList.RemoveAt(0);
            }

            if (netSession != null)
                netSession.SetSessionID(sessionIndex++);

            return netSession;
        }

        // 释放 NetSession 对象
        public void FreeNetSession(NetSessionImpl netSession)
        {
            Trace.Assert(netSession != null, "netSession is null");
            if (netSession == null)
                return;

            netSession.Reset();

            lock (netSessionListlock)
            {
                netSessionList.Add(netSession);
            }
        }

        // 私有方法
        #region
        private bool MakeNetSession(Int32 makeCounts)
        {
            Trace.Assert(netSessionList != null, "netSessionList is null");
            if (netSessionList == null)
                return false;

            NetSessionImpl netSession = null;
            for (Int32 i = 0; i < makeCounts; ++i)
            {
                netSession = new NetSessionImpl();
                if (netSession == null)
                    return false;
                
                if (!netSession.Init())
                {
                    netSession = null;
                    return false;
                }

                netSessionList.Add(netSession);
            }

            return true;
        }
        #endregion

        // 构造、析构函数
        #region
        public NetSessionPool()
        {
            sessionIndex = 100;
            initPoolSize = 0;
            extendPoolSize = 0;
            netSessionList = null;
        }

        ~NetSessionPool()
        {
        }
        #endregion

        private Int32 sessionIndex;                    // 记录会话的索引号
        private Int32 initPoolSize;                    // 会话池的初始大小
        private Int32 extendPoolSize;                  // 会话池的扩展大小
        private object netSessionListlock;             // 网络会话链表锁
        private List<NetSessionImpl> netSessionList;   // 网络会话链表
    }
}
