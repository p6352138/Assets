using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using AppUtility.Encrypt;


namespace AppUtility
{
    class Utilitys
    {
        public static TableEncrypt GetTableEncrypt()
        {
            return TableEncrypt.GetInstance();
        }

        public static TYPE BytesToStruct<TYPE>(byte[] buffer, Int32 bufferOffset, Int32 bufferLength) where TYPE : new()
        {
            if (buffer == null || bufferOffset < 0)
            {
                Trace.Assert(false, "BytesToStruct err param");
                return default(TYPE);
            }

            TYPE retObj = new TYPE();
            Int32 structSize = Marshal.SizeOf(retObj);
            if (structSize > bufferLength)            
                return default(TYPE);

            IntPtr structPtr = Marshal.AllocHGlobal(structSize);
            Marshal.Copy(buffer, bufferOffset, structPtr, structSize);
            retObj = (TYPE)Marshal.PtrToStructure(structPtr, retObj.GetType());
            Marshal.FreeHGlobal(structPtr);

            return retObj;
        }

        public static bool BytesToStruct<TYPE>(byte[] buffer, Int32 bufferOffset, Int32 bufferLength, ref TYPE typeObj)
        {
            if (typeObj == null || buffer == null || bufferOffset < 0)
            {
                Trace.Assert(false, "BytesToStruct err param");
                return false;
            }

            Int32 structSize = Marshal.SizeOf(typeObj);
            if (structSize > bufferLength)
                return false;

            IntPtr structPtr = Marshal.AllocHGlobal(structSize);
            Marshal.Copy(buffer, bufferOffset, structPtr, structSize);
            typeObj = (TYPE)Marshal.PtrToStructure(structPtr, typeObj.GetType());
            Marshal.FreeHGlobal(structPtr);

            return true;
        }

        public static bool StructToBytes<TYPE>(TYPE structType, byte[] buffer, Int32 bufferSize)
        {
            Int32 structSize = Marshal.SizeOf(structType);
            if(structSize > bufferSize)
                return false;

            IntPtr structPtr = Marshal.AllocHGlobal(structSize);
            Marshal.StructureToPtr(structType, structPtr, false);
            Marshal.Copy(structPtr, buffer, 0, structSize);
            Marshal.FreeHGlobal(structPtr);

            return true;
        }

        public static byte[] StructToBytes<TYPE>(TYPE structType)
        {
            Int32  structSize = Marshal.SizeOf(structType);
            byte[] buffer = new byte[structSize];
            IntPtr structPtr = Marshal.AllocHGlobal(structSize);
            Marshal.StructureToPtr(structType, structPtr, false);
            Marshal.Copy(structPtr, buffer, 0, structSize);
            Marshal.FreeHGlobal(structPtr);

            return buffer;
        }
		
		public static int BytesToBigEndian32Bit(byte[] stream,int pos){
			//byte[] bytes = GetDataBuffer() ;
			int result ;
			result = stream[pos + 4] & 0xff;

	        result |= ((stream[pos + 3] << 8) & 0xff00);
	
	        result |= ((stream[pos + 2] << 16) & 0xff0000);
	
	        result |= (int)((stream[pos + 1] << 24) & 0xff000000);
			return result ;
		}
		
		public static byte[] LittleEndianToBytes32Bit(int data){
			byte[] bytes = new byte[4] ;
			bytes[3] = (byte) (0xff & data);
	
	        bytes[2] = (byte) ((0xff00 & data) >> 8);
	
	        bytes[1] = (byte) ((0xff0000 & data) >> 16);
	
	        bytes[0] = (byte) ((0xff000000 & data) >> 24);
			
			return bytes ;
		}
    }
}
