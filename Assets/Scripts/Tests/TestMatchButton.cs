using System.Runtime.InteropServices;
using StandardData;
using UnityEngine;

public class TestMatchButton : MonoBehaviour
{
    public void Match()
    {
        stRequestForMatch requestMatch = new stRequestForMatch();
        requestMatch.Header.MsgID = MessageIdTcp.RequestForMatch;
        requestMatch.Header.PacketSize = (ushort)Marshal.SizeOf(requestMatch);
        requestMatch.MatchType = (ushort)GameRoomType.AdventureRoom;
        ServerManager.Instance.EventClientToServer.CallOnTcpSend(Utilities.GetObjectToByte(requestMatch));

    }
}
