using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using AppUtility;
using NetWork.NetFrame;


namespace NetWork.NetDefine
{
    class NetGlobalData : Singleton<NetGlobalData>
    {
        public Int32 GetMsgHeadSize()
        {
            return netMsgHeadSize;
        }

        #region
        public NetGlobalData()
        {
            //NetMsgHead2 netMsgHead = new NetMsgHead2();
            //netMsgHeadSize = Marshal.SizeOf(netMsgHead);
			netMsgHeadSize = 5 ;
			//netMsgHeadSize = Marshal.SizeOf(netMsgHead2);
        }
        ~NetGlobalData()
        {
        }
        #endregion
        
        private Int32 netMsgHeadSize;
    }
}
