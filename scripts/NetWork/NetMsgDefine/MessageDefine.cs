using System ;
using AppUtility ;
using System.IO ;
using System.Runtime.Serialization.Formatters.Binary;
namespace NetWork
{
	public enum emNETMSG
	{
	    emNET_MSG_JSON_UNZIP = 0,
		emNET_MSG_JSON_ZIP   = 1,
	}
	
	struct stLoginServer : BaseMsg
	{
	    public UInt32 playerName;
		
		public byte[] toStream()
		{
			MemoryStream fs = new MemoryStream();
            BinaryWriter  bw = new BinaryWriter(fs);
            bw.Write( playerName );
			
			return fs.ToArray() ;
		}
		
		public void toStruct(byte[] b, int index)
		{
			MemoryStream fs1 = new MemoryStream(b, index, 4);
            BinaryReader bw1 = new BinaryReader(fs1);
			playerName = bw1.ReadUInt32() ;
		}
	}
	
	struct stLogonServerRet : BaseMsg
	{
	    public UInt32 playerName;
        public UInt32 playerTeamID;
        public float fPosX;
        public float fPosY;
        public float fPosZ;

        public float fRotX;
        public float fRotY;
        public float fRotZ;
        public float fRotW;
		
		public byte[] toStream()
		{
			MemoryStream fs = new MemoryStream();
            BinaryWriter  bw = new BinaryWriter(fs);
            bw.Write( playerName );
			bw.Write( playerTeamID );
			bw.Write( fPosX );
			bw.Write( fPosY );
			bw.Write( fPosZ );
			bw.Write( fRotX );
			bw.Write( fRotY );
			bw.Write( fRotZ );
			bw.Write( fRotW );
			
			return fs.ToArray() ;
		}
		
		public void toStruct(byte[] b, int index)
		{
			MemoryStream fs1 = new MemoryStream(b, index,36 );
            BinaryReader bw1 = new BinaryReader(fs1);
			playerName   = bw1.ReadUInt32() ;
			playerTeamID = bw1.ReadUInt32() ;
			fPosX = bw1.ReadSingle() ;
			fPosY = bw1.ReadSingle() ;
			fPosZ = bw1.ReadSingle() ;
			fRotX = bw1.ReadSingle() ;
			fRotY = bw1.ReadSingle() ;
			fRotZ = bw1.ReadSingle() ;
			fRotW = bw1.ReadSingle() ;
		}
	}
	
	struct stPlayerCommand : BaseMsg
	{
        public UInt32 dwPlayerName;
        public UInt32 dwTermID;
        public UInt32 dwPlayerState;

        public float fPosX;
        public float fPosY;
        public float fPosZ;

        public float fRotX;
        public float fRotY;
        public float fRotZ;
        public float fRotW;

        public float speed;
		
		public byte[] toStream()
		{
			MemoryStream fs = new MemoryStream();
            BinaryWriter  bw = new BinaryWriter(fs);
            bw.Write( dwPlayerName );
			bw.Write( dwTermID );
			bw.Write( dwPlayerState );
			bw.Write( fPosX );
			bw.Write( fPosY );
			bw.Write( fPosZ );
			bw.Write( fRotX );
			bw.Write( fRotY );
			bw.Write( fRotZ );
			bw.Write( fRotW );
			bw.Write( speed );
			
			return fs.ToArray() ;
		}
		
		public void toStruct(byte[] b, int index)
		{
			MemoryStream fs1 = new MemoryStream(b, index, 44);
            BinaryReader bw1 = new BinaryReader(fs1);
			dwPlayerName = bw1.ReadUInt32() ;
			dwTermID = bw1.ReadUInt32() ;
			dwPlayerState = bw1.ReadUInt32() ;
			fPosX = bw1.ReadSingle() ;
			fPosY = bw1.ReadSingle() ;
			fPosZ = bw1.ReadSingle() ;
			fRotX = bw1.ReadSingle() ;
			fRotY = bw1.ReadSingle() ;
			fRotZ = bw1.ReadSingle() ;
			fRotW = bw1.ReadSingle() ;
			speed = bw1.ReadSingle() ;
		}
	}    
	
	struct stPlayerCommandRet : BaseMsg
	{
        public UInt32 dwPlayerName;
        public UInt32 dwTermID;
        public UInt32 dwPlayerState;

        public float fPosX;
        public float fPosY;
        public float fPosZ;

        public float fRotX;
        public float fRotY;
        public float fRotZ;
        public float fRotW;

        public float speed;
		
		public byte[] toStream()
		{
			MemoryStream fs = new MemoryStream();
            BinaryWriter  bw = new BinaryWriter(fs);
            bw.Write( dwPlayerName );
			bw.Write( dwTermID );
			bw.Write( dwPlayerState );
			bw.Write( fPosX );
			bw.Write( fPosY );
			bw.Write( fPosZ );
			bw.Write( fRotX );
			bw.Write( fRotY );
			bw.Write( fRotZ );
			bw.Write( fRotW );
			bw.Write( speed );
			
			return fs.ToArray() ;
		}
		
		public void toStruct(byte[] b, int index)
		{
			MemoryStream fs1 = new MemoryStream(b, index, 44);
            BinaryReader bw1 = new BinaryReader(fs1);
			dwPlayerName = bw1.ReadUInt32() ;
			dwTermID = bw1.ReadUInt32() ;
			dwPlayerState = bw1.ReadUInt32() ;
			fPosX = bw1.ReadSingle() ;
			fPosY = bw1.ReadSingle() ;
			fPosZ = bw1.ReadSingle() ;
			fRotX = bw1.ReadSingle() ;
			fRotY = bw1.ReadSingle() ;
			fRotZ = bw1.ReadSingle() ;
			fRotW = bw1.ReadSingle() ;
			speed = bw1.ReadSingle() ;
		}
	}

    struct stPlayerDataRet  : BaseMsg
    {
        public UInt32 dwPlayerName;
        public UInt32 dwTermID;
        public UInt32 dwPlayerState;

        public float fPosX;
        public float fPosY;
        public float fPosZ;

        public float fRotX;
        public float fRotY;
        public float fRotZ;
        public float fRotW;

        public float speed;
		
				public byte[] toStream()
		{
			MemoryStream fs = new MemoryStream();
            BinaryWriter  bw = new BinaryWriter(fs);
            bw.Write( dwPlayerName );
			bw.Write( dwTermID );
			bw.Write( dwPlayerState );
			bw.Write( fPosX );
			bw.Write( fPosY );
			bw.Write( fPosZ );
			bw.Write( fRotX );
			bw.Write( fRotY );
			bw.Write( fRotZ );
			bw.Write( fRotW );
			bw.Write( speed );
			
			return fs.ToArray() ;
		}
		
		public void toStruct(byte[] b, int index)
		{
			MemoryStream fs1 = new MemoryStream(b, index, 44);
            BinaryReader bw1 = new BinaryReader(fs1);
			dwPlayerName = bw1.ReadUInt32() ;
			dwTermID = bw1.ReadUInt32() ;
			dwPlayerState = bw1.ReadUInt32() ;
			fPosX = bw1.ReadSingle() ;
			fPosY = bw1.ReadSingle() ;
			fPosZ = bw1.ReadSingle() ;
			fRotX = bw1.ReadSingle() ;
			fRotY = bw1.ReadSingle() ;
			fRotZ = bw1.ReadSingle() ;
			fRotW = bw1.ReadSingle() ;
			speed = bw1.ReadSingle() ;
		}
    }

    public struct stBallDataRet : BaseMsg
    {
        public UInt32 dwID;
        public UInt32 dwBallState;
        public UInt32 dwScrPlayerID;
        public UInt32 dwDestPlayerID;

        public float fPosX;
        public float fPosY;
        public float fPosZ;

        public float fSpeedX;
        public float fSpeedY;
        public float fSpeedZ;
		
				public byte[] toStream()
		{
			MemoryStream fs = new MemoryStream();
            BinaryWriter  bw = new BinaryWriter(fs);
            bw.Write( dwID );
			bw.Write( dwBallState );
			bw.Write( dwScrPlayerID );
			bw.Write( dwDestPlayerID );
			bw.Write( fPosX );
			bw.Write( fPosY );
			bw.Write( fPosZ );
			bw.Write( fSpeedX );
			bw.Write( fSpeedY );
			bw.Write( fSpeedZ );
			
			return fs.ToArray() ;
		}
		
		public void toStruct(byte[] b, int index)
		{
			MemoryStream fs1 = new MemoryStream(b, index, 40);
            BinaryReader bw1 = new BinaryReader(fs1);
			dwID = bw1.ReadUInt32() ;
			dwBallState = bw1.ReadUInt32() ;
			dwScrPlayerID = bw1.ReadUInt32() ;
			dwDestPlayerID= bw1.ReadUInt32() ;
			fPosX = bw1.ReadSingle() ;
			fPosY = bw1.ReadSingle() ;
			fPosZ = bw1.ReadSingle() ;
			fSpeedX = bw1.ReadSingle() ;
			fSpeedY = bw1.ReadSingle() ;
			fSpeedZ = bw1.ReadSingle() ;
		}

    }

	struct stStartMatch : BaseMsg
	{
		public UInt32 dwMatchState ;
		public byte[] toStream()
		{
			MemoryStream fs = new MemoryStream();
            BinaryWriter  bw = new BinaryWriter(fs);
            bw.Write( dwMatchState );
			
			return fs.ToArray() ;
		}
		
		public void toStruct(byte[] b, int index)
		{
			MemoryStream fs1 = new MemoryStream(b, index, 4);
            BinaryReader bw1 = new BinaryReader(fs1);
			dwMatchState = bw1.ReadUInt32() ;
		}
	}

	struct stMatchStateRet : BaseMsg
	{
		public UInt32 dwMatchState ;
		public float fMatchRemainTime;
		
		public byte[] toStream()
		{
			MemoryStream fs = new MemoryStream();
            BinaryWriter  bw = new BinaryWriter(fs);
            bw.Write( dwMatchState );
			bw.Write( fMatchRemainTime );
			return fs.ToArray() ;
		}
		
		public void toStruct(byte[] b, int index)
		{
			MemoryStream fs1 = new MemoryStream(b, index, 8);
            BinaryReader bw1 = new BinaryReader(fs1);
			dwMatchState = bw1.ReadUInt32() ;
			fMatchRemainTime = bw1.ReadSingle() ;
		}
	}

	struct stGoal : BaseMsg
	{
		public UInt32 dwTeamID ;
		public UInt32 dwPlayerID;
		
		public byte[] toStream()
		{
			MemoryStream fs = new MemoryStream();
            BinaryWriter  bw = new BinaryWriter(fs);
            bw.Write( dwTeamID );
			bw.Write( dwPlayerID );
			return fs.ToArray() ;
		}
		
		public void toStruct(byte[] b, int index)
		{
			MemoryStream fs1 = new MemoryStream(b, index, 8);
            BinaryReader bw1 = new BinaryReader(fs1);
			dwTeamID = bw1.ReadUInt32() ;
			dwPlayerID = bw1.ReadUInt32() ;
		}
	}

	struct stGoalRet : BaseMsg
	{
		public UInt32 dwTeamOneScores ;
		public UInt32 dwTeamTwoScores ;
	
		public UInt32 playerID;
		
		public byte[] toStream()
		{
			MemoryStream fs = new MemoryStream();
            BinaryWriter  bw = new BinaryWriter(fs);
            bw.Write( dwTeamOneScores );
			bw.Write( dwTeamTwoScores );
			bw.Write( playerID );
			return fs.ToArray() ;
		}
		
		public void toStruct(byte[] b, int index)
		{
			MemoryStream fs1 = new MemoryStream(b, index, 12);
            BinaryReader bw1 = new BinaryReader(fs1);
			dwTeamOneScores = bw1.ReadUInt32() ;
			dwTeamTwoScores = bw1.ReadUInt32() ;
			playerID = bw1.ReadUInt32() ;
		}
	}
}
