using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
namespace StandardData
{
    public static class NetworkSize
    {

        public const int HeaderSize = 4;
        public const int BufferSize = 2048;
        public const int TempBufferSize = 1048;
        public const int MaxNameSize = 10;
    }

    public static class GameRoomSize
    {
        public const int BattleRoomSize = 4;
        public const int AdventureRoomSize = 3;
    }
    public static class MessageIdTcp
    {
        public const ushort AdventureMatched = 1;
        public const ushort AdventureRoomLoaded = 2;
        public const ushort CreateAdventureRoom = 3;
        public const ushort AdventurePlayerLoadInfo = 4;
        public const ushort AdventureRoomLoadInfo = 5;
    }

    public static class MessageIdUdp
    {
        public const ushort PlayerPosition = 0x00;

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
    public struct stAdventureMatched
    {
        public stHeaderTcp Header;
        [MarshalAs(UnmanagedType.Bool, SizeConst = 1)]
        public bool Matched;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct stAdventureRoomLoaded
    {
        public stHeaderTcp Header;
        [MarshalAs(UnmanagedType.Bool, SizeConst = 1)]
        public bool Loaded;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct stCreateAdventureRoomTcp
    {
        public stHeaderTcp Header;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort RoomId;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort PlayerIndex;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)GameRoomSize.AdventureRoomSize)]
        public stAdventurePlayerInfo[] playersInfo;
    }
    public struct stAdventurePlayerInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = (int)(NetworkSize.MaxNameSize))]
        public string Name;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort Index;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct stAdventureRoomPlayerLoadInfo
    {
        public stHeaderTcp Header;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort RoomId;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort Index;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct stAdventureRoomLoadInfo
    {
        public stHeaderTcp Header;
        [MarshalAs(UnmanagedType.Bool, SizeConst = 1)]
        public bool IsAllSucceed;
    }




    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct stHeaderUdp
    {
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort MsgID;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct stPlayerPosition
    {
        public stHeaderUdp Header;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort RoomId;
        [MarshalAs(UnmanagedType.U2, SizeConst = 2)]
        public ushort PlayerIndex;
        [MarshalAs(UnmanagedType.R4, SizeConst = 4)]
        public float PositionX;
        [MarshalAs(UnmanagedType.R4, SizeConst = 4)]
        public float PositionY;
    }



}