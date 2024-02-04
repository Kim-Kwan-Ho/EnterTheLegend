using System;
using UnityEngine;

public class ClientToServerEvent : MonoBehaviour
{
    public Action<ClientToServerEvent, ClientToServerEventArgs> OnTcpSend;
    public Action<ClientToServerEvent, ClientToServerEventArgs> OnUdpSend;

    public void CallOnTcpSend(byte[] bytes)
    {
        OnTcpSend?.Invoke(this, new ClientToServerEventArgs(){ bytes = bytes });
    }
    public void CallOnUdpSend(byte[] bytes)
    {
        OnUdpSend?.Invoke(this, new ClientToServerEventArgs() { bytes = bytes });
    }
}

public class ClientToServerEventArgs : EventArgs
{
    public byte[] bytes;
}
