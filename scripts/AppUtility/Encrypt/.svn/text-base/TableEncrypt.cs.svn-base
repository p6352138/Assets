using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppUtility.Encrypt
{
    class TableEncrypt : Singleton<TableEncrypt>
    {
        private enum KEYVALUE : byte
        {
            segmentValue = 27,
            disturbValue = 53,
        }

        public Int32 Encrypt(byte[] src, Int32 srcOffset, Int32 srcLen)
        {
            return Encrypt(src, srcOffset, srcLen, null, 0);
        }

        public Int32 Unencrypt(byte[] src, Int32 srcOffset, Int32 srcLen)
        {
            return Unencrypt(src, srcOffset, srcLen, null, 0);
        }

        public Int32 Encrypt(byte[] src, Int32 srcOffset, Int32 srcLen, byte[] des)
        {
            return Encrypt(src, srcOffset, srcLen, des, 0);
        }

        public Int32 Unencrypt(byte[] src, Int32 srcOffset, Int32 srcLen, byte[] des)
        {
            return Unencrypt(src, srcOffset, srcLen, des, 0);
        }

        public Int32 Encrypt( byte[] src, Int32 srcOffset, Int32 srcLen, byte[] des, Int32 desSize )
        {
            //if( src == null )
            //    return  0;

            //if( des == null )
            //{
            //    for (Int32 i = 0,j = srcOffset; i < srcLen; ++i,++j)
            //    {
            //        src[j]  = codeTable[src[j]];
            //        src[j] ^= (byte)KEYVALUE.disturbValue;
            //    }
            //}
            //else
            //{
            //    if( desSize < srcLen )
            //        return ( 0 );

            //    for (Int32 i = 0,j = srcOffset; i < srcLen; ++i,++j)
            //    {
            //        des[i]  = codeTable[src[j]];
            //        des[i] ^= (byte)KEYVALUE.disturbValue;
            //    }
            //}

            return srcLen;
        }

        public Int32 Unencrypt(byte[] src, Int32 srcOffset, Int32 srcLen, byte[] des, Int32 desSize)
        {
            //if( src == null )
            //    return  0;

            //if( des == null )
            //{
            //    for (Int32 i = 0,j = srcOffset; i < srcLen; ++i,++j)
            //    {
            //        src[j] ^= (byte)KEYVALUE.disturbValue;
            //        src[j]  = uncodeTalbe[src[j]];	
            //    }
            //}
            //else
            //{
            //    if( desSize < srcLen )
            //        return ( 0 );

            //    for (Int32 i = 0,j = srcOffset; i < srcLen; ++i,++j)
            //    {
            //        des[i]  = src[j];
            //        des[i] ^= (byte)KEYVALUE.disturbValue;
            //        des[i]  = uncodeTalbe[des[i]];
            //    }
            //}

            return srcLen;
        }

        // 构造、析构函数
        #region
        public TableEncrypt()
        {
            maxVlue     = 255;
            codeTable   = new byte[256];
            uncodeTalbe = new byte[256];

            Int32 segmentIter  = 0;
            Int32 segmentInner = 0;
            Int32 operationPos = 0;

            for (; segmentIter <= maxVlue; segmentIter += (byte)KEYVALUE.segmentValue)
	        {
                operationPos = segmentIter;
                segmentInner = (segmentIter + (byte)KEYVALUE.segmentValue) - 1;
                if (segmentInner > maxVlue)
                    segmentInner = maxVlue;

                for (; segmentInner >= segmentIter; --segmentInner, ++operationPos)
		        {
                    codeTable[operationPos] = (byte)segmentInner;
                    uncodeTalbe[segmentInner] = (byte)operationPos;
		        }
	        }
        }

        ~TableEncrypt()
        {
            codeTable   = null;
            uncodeTalbe = null;
        }
        #endregion

        private  Int32  maxVlue;
	    private  byte[] codeTable;
	    private  byte[] uncodeTalbe;
    }
}
