using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace AppUtility
{
    class Singleton<TYPE> where TYPE : new()
    {
        public static TYPE GetInstance()
        {
            if (instance == null)
            {
                instance = new TYPE();
            }

            Trace.Assert(instance != null, "Singleton new is null");
    
            return instance;
        }

        #region
        protected Singleton()
        {
        }

        private Singleton(ref Singleton<TYPE> singleInstance)
        {
        }

        private Singleton(Singleton<TYPE> singleInstance)
        {
        }

        private static TYPE instance;

        #endregion
    }
}
