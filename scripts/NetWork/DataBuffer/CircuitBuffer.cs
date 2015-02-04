using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using AppUtility;
using NetWork.NetDefine;

namespace NetWork.DataBuffer
{
    class CircuitBuffer
    {
        // 初始化
        // extSize 需要大于单个的数据块
        public bool Init(Int32 bufSize, Int32 extSize)
        {
            Trace.Assert(extSize >= (Int32)NETGLOBALDATA.maxSendPackLength, "extSize is shot");
            if (bufSize <= 0 || extSize < (Int32)NETGLOBALDATA.maxSendPackLength)
                return false;

            Release();
            bufferSize = bufSize;
            extendSize = extSize;

            Int32 totalBufferSize = bufferSize + extendSize;
            dataBuffer = new byte[totalBufferSize];
            if (dataBuffer == null)
                return false;

            return true;
        }

        // 释放所有数据
        public void Release()
        {
            bufferSize = 0;
            extendSize = 0;
            dataLength = 0;
            readPosition = 0;
            writePosition = 0;            
            dataBuffer = null;
        }

        // 重置读、写位置
        public void Reset()
        {
            dataLength = 0;
            readPosition = 0;
            writePosition = 0;            
        }

        // 获得Buffer的大小
        public Int32 GetBufferSize()
        {
            return bufferSize;
        }

        // 获得剩余的空间
        public Int32 GetFreeSpace()
        {
            return bufferSize - dataLength;
        }

        // 读取数据
        public bool ReadData(byte[] readBuffer, Int32 readLength)
        {
            if (!PopData(readBuffer, readLength))
                return false;
            
            readPosition += readLength;
            readPosition %= bufferSize;
            dataLength   -= readLength;

            return true;
        }

        // 写入数据
        public bool WriteData(byte[] writeBuf, Int32 writeLength)
        {
            return WriteData(writeBuf, 0, writeLength, ENCRYPTOPT.codeEncryptNull);
        }

        // 写入数据
        public bool WriteData(byte[] writeBuf, Int32 bufOffset, Int32 writeLength, ENCRYPTOPT encryptOption)
        {
            if (!PushData(writeBuf, bufOffset, writeLength, encryptOption))
                return false;

            writePosition += writeLength;
            writePosition %= bufferSize;
            dataLength    += writeLength;

            return true;
        }

        // 判断缓冲是否为空
        public bool IsEmpty()
        {
            return (dataLength == 0);
        }

        // 获得缓冲区
        public byte[] GetDataBuffer()
        {
            return dataBuffer;
        }

        // 获得数据的长度
        public Int32 GetDataLength()
        {
            return dataLength;
        }

        // 获得读取位置
        public Int32 GetReadPosition()
        {
            if (dataLength > 0)
                return readPosition;
            else
                return (-1);
        }

        // 获得写入位置
        public Int32 GetWritePosition()
        {
            if (dataLength < bufferSize)
                return writePosition;
            else
                return (-1);
        }

        // 获得可读取数据的长度
        public Int32 GetReadLength()
        {
            if (writePosition != readPosition)
                return ((writePosition > readPosition) ? (writePosition - readPosition) : (bufferSize - readPosition));   // 可能有切断
            else
                return ((dataLength != 0) ? (bufferSize - readPosition) : (0));
        }

        // 获得可写入数据的长度
        public Int32 GetWriteLength()
        {
            if (writePosition != readPosition)
                return ((writePosition > readPosition) ? (bufferSize - writePosition) : (readPosition - writePosition));    // 可能不是所有有用空间大小
            else
                return ((dataLength == 0) ? (bufferSize - writePosition) : (0));
        }

        // 准备读取的数据
        public bool ReadyReadData(Int32 willReadLength)
        {
            if (dataLength < willReadLength)
                return false;

            if ((writePosition <= readPosition) && ((bufferSize - readPosition) < willReadLength))
            {
                Int32 copyLength = willReadLength - (bufferSize - readPosition);
                Trace.Assert(copyLength <= extendSize, "copyLength > extendSize");
                if (copyLength > extendSize)
                    return false;

                Buffer.BlockCopy(dataBuffer, 0, dataBuffer, bufferSize, copyLength);
            }

            return true;
        }

        // 内部使用的函数
        #region
        // 获得缓冲区尾部可以写入的长度
        protected Int32 GetTailWriteLength()
        {
            if (writePosition != readPosition)
                return ((writePosition > readPosition) ? (bufferSize - writePosition) : (0));
            else
                return ((dataLength == 0) ? (bufferSize - writePosition) : (0));
        }

        // 获得剩余的空间的大小
        protected Int32 GetFreeSpaceLength()
        {
            return bufferSize - dataLength;
        }
        #endregion

        // 弹出数据
        #region
        private bool PopData(byte[] popBuffer, Int32 popLength)
        {
            if (popLength > dataLength)
                return false;

            if (popBuffer != null)
            {                
                Int32 tailReadSpace = 0;
                if ((writePosition <= readPosition) && ((tailReadSpace = (bufferSize - readPosition)) < popLength))
                {
                    Buffer.BlockCopy(dataBuffer, readPosition, popBuffer, 0, tailReadSpace);
                    Buffer.BlockCopy(dataBuffer, 0, popBuffer, tailReadSpace, (popLength - tailReadSpace));
                }
                else
                    Buffer.BlockCopy(dataBuffer, readPosition, popBuffer, 0, popLength);
            }

            return true;
        }

        // 放入数据
        private bool PushData(byte[] pushBuffer, Int32 bufOffset, Int32 pushLength, ENCRYPTOPT encryptOption)
        {
            if (Utilitys.GetTableEncrypt() == null)
                return false;

            if (GetFreeSpaceLength() < pushLength)
                return false;

            if (pushBuffer != null)
            {
                Int32 tailWriteSpace = GetTailWriteLength();
                if ((writePosition >= readPosition) && (tailWriteSpace < pushLength))
                {
                    Buffer.BlockCopy(pushBuffer, bufOffset, dataBuffer, writePosition, tailWriteSpace);

                    if (encryptOption == ENCRYPTOPT.codeEncrypt)
                        Utilitys.GetTableEncrypt().Encrypt(dataBuffer, writePosition, tailWriteSpace);
                    else if (encryptOption == ENCRYPTOPT.codeUnencrypt)
                        Utilitys.GetTableEncrypt().Unencrypt(dataBuffer, writePosition, tailWriteSpace);

                    Int32 leaveWriteLength = pushLength - tailWriteSpace;
                    Buffer.BlockCopy(pushBuffer, bufOffset + tailWriteSpace, dataBuffer, 0, leaveWriteLength);
                    if (encryptOption == ENCRYPTOPT.codeEncrypt)
                        Utilitys.GetTableEncrypt().Encrypt(dataBuffer, 0, leaveWriteLength);
                    else if (encryptOption == ENCRYPTOPT.codeUnencrypt)
                        Utilitys.GetTableEncrypt().Unencrypt(dataBuffer, 0, leaveWriteLength);

                    return true;
                }
                else
                    Buffer.BlockCopy(pushBuffer, bufOffset, dataBuffer, writePosition, pushLength);
            }

            if (encryptOption == ENCRYPTOPT.codeEncrypt)
                Utilitys.GetTableEncrypt().Encrypt(dataBuffer, writePosition, pushLength);
            else if (encryptOption == ENCRYPTOPT.codeUnencrypt)
                Utilitys.GetTableEncrypt().Unencrypt(dataBuffer, writePosition, pushLength);

            return true;
        }

        #endregion

        // 构造、析构函数
        #region
        protected CircuitBuffer()
        {
            Release();
        }

        ~CircuitBuffer()
        {
            Release();
        }
        #endregion

        private Int32   bufferSize;          // 缓冲区的大小
        private Int32   extendSize;          // 缓冲区扩展大小
        private Int32   readPosition;        // 读取数据的位置
        private Int32   writePosition;       // 写入数据的位置
        private Int32   dataLength;          // 数据的有效长度
        private byte[]  dataBuffer;          // 数据缓冲区
    }
}
