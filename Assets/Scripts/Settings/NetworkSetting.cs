using System.Runtime.InteropServices;
namespace StandardData
{
    public static class NetworkSize
    {

        public const int HeaderSize = 4;
        public const int BufferSize = 2048;
        public const int TempBufferSize = 1048;
        public const int MaxUDPNameLength = 10;
        public const int MaxNicknameLength = 10;
        public const int MaxIdLength = 10;
        public const int EquipedItemLength = 5;
        public const int MaxItemLength = 100;
    }

    public static class GameRoomSize
    {
        public const int TeamBattleRoomSize = 6;
    }
    public static class MessageIdTcp
    {

        public const ushort RequestForMatch = 1;
        public const ushort TeamBattleRoomLoaded = 2;
        public const ushort CreateTeamBattleRoom = 3;
        public const ushort TeamBattlePlayerLoadInfo = 4;
        public const ushort TeamBattleRoomLoadInfo = 5;
        public const ushort SetUdpPort = 6;
        public const ushort TeamBattleRoomPlayerStateChangedToServer = 7;
        public const ushort BattleRoomPlayerStateChangedFromServer = 77;
        public const ushort TeamBattleRoomPlayerDirectionChangedToServer = 8;
        public const ushort BattleRoomPlayerDirectionChangedFromServer = 88;
        public const ushort RequestPlayerData = 9;
        public const ushort ResponsePlayerData = 999;
        public const ushort PlayerEquipChanged = 10;


    }

    public static class MessageIdUdp
    {
        public const ushort TeamBattlePlayerPositionToServer = 1;
        public const ushort BattlePlayerPositionFromServer = 2;

    }

    public static class TcpSendCycle
    {
    }

    public static class UdpSendCycle
    {
        public const float TeamBattleRoomSendCycle = 0.016f;
    }



    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct stHeaderTcp
    {
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort MsgID;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort PacketSize;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct stSetUdpPort
    {
        public stHeaderTcp Header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NetworkSize.MaxUDPNameLength)]
        public string Name;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort UdpPortSend;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort UdpPortReceive;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct stRequestPlayerData
    {
        public stHeaderTcp Header;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NetworkSize.MaxIdLength)]
        public string Id;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct stResponsePlayerData
    {
        public stHeaderTcp Header;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NetworkSize.MaxNicknameLength)]
        public string Nickname;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public int Credit;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public int Gold;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NetworkSize.EquipedItemLength)]
        public int[] EquipedItems;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort ItemCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NetworkSize.MaxItemLength)]
        public int[] Items;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct stPlayerEquipChangedInfo
    {
        public stHeaderTcp Header;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort ItemType;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public int AfterItem;

    }



    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct stRequestForMatch
    {
        public stHeaderTcp Header;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort MatchType;

    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct stTeamBattleRoomPlayerLoadInfo
    {
        public stHeaderTcp Header;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort RoomId;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort PlayerIndex;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct stCreateTeamBattleRoom
    {
        public stHeaderTcp Header;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort RoomId;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort PlayerIndex;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)GameRoomSize.TeamBattleRoomSize)]
        public stBattlePlayerInfo[] PlayersInfo;
    }
    public struct stBattlePlayerInfo
    {
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort Index;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = (int)(NetworkSize.MaxNicknameLength))]
        public string Nickname;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct stTeamBattleRoomPlayerInfo
    {
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort RoomId;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort PlayerIndex;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct stTeamBattleRoomLoadInfo
    {
        public stHeaderTcp Header;
        [MarshalAs(UnmanagedType.Bool, SizeConst = 1)]
        public bool IsAllSucceed;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct stTeamBattleRoomPlayerStateChangedToServer
    {
        public stHeaderTcp Header;
        public stTeamBattleRoomPlayerInfo PlayerInfo;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort State;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct stBattleRoomPlayerStateChangedFromServer
    {
        public stHeaderTcp Header;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort PlayerIndex;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort State;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct stTeamBattleRoomPlayerDirectionChangedToServer
    {
        public stHeaderTcp Header;
        public stTeamBattleRoomPlayerInfo PlayerInfo;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort Direction;
    }
    public struct stBattleRoomPlayerDirectionChangedFromServer
    {
        public stHeaderTcp Header;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort PlayerIndex;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort Direction;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct stTeamBattleRoomPlayerOnAttack
    {
        public stHeaderTcp Header;
        public stTeamBattleRoomPlayerInfo PlayerInfo;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort State;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct stHeaderUdp
    {
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort MsgID;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct stTeamBattlePlayerPositionToServer
    {
        public stHeaderUdp Header;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort RoomId;
        public stPlayerPosition PlayerPosition;
    }

    public struct stBattlePlayerPositionFromSever
    {
        public stHeaderUdp Header;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameRoomSize.TeamBattleRoomSize)]
        public stPlayerPosition[] PlayerPosition;

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct stPlayerPosition
    {
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort PlayerIndex;
        [MarshalAs(UnmanagedType.R4, SizeConst = 4)]
        public float PositionX;
        [MarshalAs(UnmanagedType.R4, SizeConst = 4)]
        public float PositionY;
    }

}