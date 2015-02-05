using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace AppUtility
{
    class WriteReadList<TYPE>
    {
        public delegate void HandleElement(TYPE element);
        public delegate void FreeElement(TYPE element);

        public bool InitWriteReadList(HandleElement eleHandle, FreeElement eleFree)
        {
            if (eleHandle == null || eleFree == null)
            {
                Trace.Assert(false, "InitWriteReadList input param error.");
                return false;
            }

            elementHandle = eleHandle;
            elementFree   = eleFree;

            visitorGuard = new object();
            if (visitorGuard == null)
                return false;

            elementListA = new List<TYPE>();
            if (elementListA == null)
            {
                visitorGuard = null;
                return false;
            }

            elementListB = new List<TYPE>();
            if (elementListB == null)
            {
                visitorGuard = null;
                elementListA = null;
                return false;
            }

            writeElementList = elementListA;
            readElementList  = elementListB;

            return true;
        }

        public void ReleaseWriteReadList()
        {
            writeElementList = null;
            readElementList = null;
            visitorGuard = null;
            
            if (elementListA != null)
            {
                if (elementFree != null)
                {
                    foreach (TYPE element in elementListA)
                    {
                        if (element != null)
                            elementFree(element);
                    }
                }
                  
                elementListA.Clear();
                elementListA = null;
            }

            if (elementListB != null)
            {
                if (elementFree != null)
                {
                    foreach (TYPE element in elementListB)
                    {
                        if (element != null)
                            elementFree(element);
                    }
                }

                elementListB.Clear();
                elementListB = null;
            }

            elementFree = null;
            elementHandle = null;
        }

        public void HandleList()
        {
            if (elementHandle == null)
            {
                Trace.Assert(false, "elementHandle is null");
                return;
            }

            ExchangeWriteReadList();

            if (readElementList.Count() > 0)
            {
                foreach (TYPE element in readElementList)
                {
                    if (element != null)
                    {
                        elementHandle(element);

                        elementFree(element);
                    }
                }

                readElementList.Clear();
            }
        }

        public void AddElement(TYPE element)
        {
            if (element == null || visitorGuard == null)
            {
                Trace.Assert(false, "AddElement error");
                return;
            }

            lock (visitorGuard)
            {
                writeElementList.Add(element);
            }
        }

        public TYPE GetLastWriteElement()
        {
            if (writeElementList == null || writeElementList.Count() <= 0)
                return default(TYPE);

            return writeElementList[writeElementList.Count() - 1];
        }

        public Int32 ExchangeWriteReadList()
        {
            Trace.Assert(writeElementList != null && readElementList != null);
            if (writeElementList == null || readElementList == null)
                return 0;

            lock (visitorGuard)
            {
                if (writeElementList.Count() > 0)
                {
                    List<TYPE> changeList = writeElementList;
                    writeElementList = readElementList;
                    readElementList  = changeList;
                    
                    if (writeElementList.Count() > 0)
                    {
                        readElementList.InsertRange(0, writeElementList);
                        writeElementList.Clear();
                    }                          
                }

                return readElementList.Count();  
            }
        }

        public void Begion()
        {
            visitorIter = 0;
        }

        public void Next()
        {
            ++visitorIter;
        }

        public bool IsEnd()
        {
            if (readElementList == null)
                return true;

            return visitorIter >= readElementList.Count();
        }

        public TYPE GetCurrentElement()
        {
            Trace.Assert(readElementList != null && visitorIter < readElementList.Count(), "visitorIter is error");
            if (readElementList == null || visitorIter >= readElementList.Count())
                return default(TYPE);

            return readElementList[visitorIter];
        }

        public void FreeReadedElement()
        {
            Trace.Assert(elementFree != null, "elementFree is null");
            if(readElementList != null && readElementList.Count() > 0)
            {
                Int32 iterEndPosition = visitorIter < readElementList.Count() ? visitorIter : (readElementList.Count() - 1);
                for(Int32 i = 0; i <= iterEndPosition; ++i)
                {
                    if(readElementList[i] != null)
                        elementFree(readElementList[i]);
                }

                readElementList.RemoveRange( 0, (iterEndPosition + 1));
            }
        }

        #region
        public WriteReadList()
        {
            visitorIter = 0;
            visitorGuard = null;
            elementListA = null;
            elementListB = null;
            writeElementList = null;
            readElementList = null;
        }
        ~WriteReadList()
        {
        }
        #endregion

        private Int32         visitorIter;        
        private List<TYPE>    elementListA;
        private List<TYPE>    elementListB;
        private List<TYPE>    writeElementList;
        private List<TYPE>    readElementList;
        private HandleElement elementHandle;
        private FreeElement   elementFree;
        protected object      visitorGuard;
    }
}
