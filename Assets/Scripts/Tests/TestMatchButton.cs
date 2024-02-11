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
        byte[] msg = Utilities.GetObjectToByte(requestMatch);
        ServerManager.Instance.EventClientToServer.CallOnTcpSend(msg);

    }
}
