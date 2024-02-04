using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using StandardData;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ClientToServerEvent))]

public class ServerManager : SingletonMonobehaviour<ServerManager>
{
    private Thread _tcpListenerThread = null;
    private TcpClient _socketConnection = null;

    public string Ip = "127.0.0.1";
    public int Port = 9001;
    private bool _clientReady = false;
    private byte[] _buffer = new byte[NetworkSize.BufferSize];
    private byte[] _tempBuffer = new byte[NetworkSize.TempBufferSize];
    private bool _isTempByte = false;
    private int _tempByteSize = 0;


    private Thread _udpListenerThread;

    private NetworkStream _stream = null;

    private UdpClient _udpSocketReceive;
    private UdpClient _udpSocketSend;
    private IPEndPoint _IPEndPointReceive;
    private IPEndPoint _IPEndPointSend;


    public ClientToServerEvent EventClientToServer;

    protected override void Awake()
    {
        base.Awake();
        _IPEndPointSend = new IPEndPoint(IPAddress.Parse(Ip), 9999);
        _IPEndPointSend = new IPEndPoint(IPAddress.Parse(Ip), 2409);
        Init();
    }

    private void OnEnable()
    {
        EventClientToServer.OnUdpSend += Event_OnUdpMessageSend;
        EventClientToServer.OnTcpSend += Event_OnTcpMessageSend;
    }

    private void OnDisable()
    {
        EventClientToServer.OnUdpSend -= Event_OnUdpMessageSend;
        EventClientToServer.OnTcpSend -= Event_OnTcpMessageSend;
    }
  
    public void Init()
    {
        ConnectToServer();
    }

    private void Event_OnUdpMessageSend(ClientToServerEvent clientToServerEvent,
        ClientToServerEventArgs clientToServerEventArgs)
    {
        _udpSocketSend.Send(clientToServerEventArgs.bytes, clientToServerEventArgs.bytes.Length, _IPEndPointSend);
    }

    public void Event_OnTcpMessageSend(ClientToServerEvent clientToServerEvent,
        ClientToServerEventArgs clientToServerEventArgs)
    {
        if (_socketConnection.Connected)
        {
            _socketConnection.GetStream().Write(clientToServerEventArgs.bytes, 0 , clientToServerEventArgs.bytes.Length);
        }
    }
    private void ConnectToServer()
    {
        _tcpListenerThread = new Thread(new ThreadStart(TcpListenForIncomingRequest));
        _tcpListenerThread.IsBackground = true;
        _tcpListenerThread.Start();

        _udpListenerThread = new Thread(new ThreadStart(UdpListenForIncomingRequest));
        _udpListenerThread.IsBackground = true;
        _udpListenerThread.Start();

        _IPEndPointSend = new IPEndPoint(IPAddress.Parse(Ip), 9999);
        _udpSocketSend = new UdpClient();


    }
    private void TcpListenForIncomingRequest()
    {
        try
        {
            _socketConnection = new TcpClient(Ip, Port);
            _stream = _socketConnection.GetStream();
            _clientReady = true;

            while (true)
            {
                if (!IsConnected(_socketConnection))
                {
                    DisConnect();
                    break;
                }
                if (_clientReady)
                {
                    if (_stream.DataAvailable)
                    {
                        Array.Clear(_buffer, 0, _buffer.Length);
                        int messageLength = _stream.Read(_buffer, 0, _buffer.Length);
                        byte[] processBuffer = new byte[messageLength + _tempByteSize];
                        if (_isTempByte)
                        {
                            Array.Copy(_tempBuffer, 0, processBuffer, 0, _tempByteSize);
                            Array.Copy(_buffer, 0, processBuffer, _tempByteSize, messageLength);
                        }
                        else
                        {
                            Array.Copy(_buffer, 0, processBuffer, 0, messageLength);
                        }

                        if (_tempByteSize + messageLength > 0)
                        {
                            TcpOnIncomingData(processBuffer);
                        }
                    }
                    else if (_tempByteSize > 0)
                    {
                        byte[] processBuffer = new byte[_tempByteSize];
                        Array.Copy(_tempBuffer, 0, processBuffer, 0, _tempByteSize);
                        TcpOnIncomingData(processBuffer);
                    }
                }
                else
                {
                    break;
                }
                Thread.Sleep(100);
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log(socketException.ToString());
            _clientReady = false;
        }
    }



    private void TcpOnIncomingData(byte[] data)
    {
        if (data.Length < NetworkSize.HeaderSize)
        {
            Array.Copy(data, 0, _tempBuffer, _tempByteSize, data.Length);
            _isTempByte = true;
            _tempByteSize += data.Length;
            return;
        }

        byte[] headerDataByte = new byte[NetworkSize.HeaderSize];
        Array.Copy(data, 0, headerDataByte, 0, headerDataByte.Length);
        stHeaderTcp headerData = Utilities.GetObjectFromByte<stHeaderTcp>(headerDataByte);
        if (headerData.PacketSize > data.Length)
        {
            Array.Copy(data, 0, _tempBuffer, _tempByteSize, data.Length);
            _isTempByte = true;
            _tempByteSize += data.Length;
            return;
        }

        byte[] msgData = new byte[headerData.PacketSize];
        Array.Copy(data, 0, msgData, 0, headerData.PacketSize);

        TcpIncomingDataProcess(headerData.MsgID, msgData);

        if (data.Length == msgData.Length)
        {
            _isTempByte = false;
            _tempByteSize = 0;
        }
        else
        {
            Array.Clear(_tempBuffer, 0, _tempBuffer.Length);
            Array.Copy(data, msgData.Length, _tempBuffer, 0, data.Length - (msgData.Length));
            _isTempByte = true;
            _tempByteSize = data.Length - (msgData.Length);
        }
    }
    private void TcpIncomingDataProcess(ushort msgId, byte[] msgData)
    {
        switch (msgId)
        {
            case MessageIdTcp.AdventureMatched:
                MySceneManager.Instance.EventSceneChanged.CallSceneChanged("AdventureScene", true, 3);
                break;
            case MessageIdTcp.CreateAdventureRoom:
                stCreateAdventureRoomTcp createRoom = Utilities.GetObjectFromByte<stCreateAdventureRoomTcp>(msgData);
                AdventureSceneManager.Instance.EventAdventureScene.CallRoomInitialize(createRoom.RoomId,
                    createRoom.PlayerIndex,
                    createRoom.playersInfo);
                break;
            case MessageIdTcp.AdventureRoomLoadInfo:
                stAdventureRoomLoadInfo loadInfo = Utilities.GetObjectFromByte<stAdventureRoomLoadInfo>(msgData);
                if (loadInfo.IsAllSucceed)
                {

                }
                else
                {
                    // 여기다가 실패창 설정
                }
                break;

        }
    }

    private void UdpListenForIncomingRequest()
    {
        try
        {
            while (true)
            {
                if (_udpSocketReceive == null) continue;

                byte[] processBuffer = new byte[NetworkSize.BufferSize];
                processBuffer = _udpSocketReceive.Receive(ref _IPEndPointReceive);
                byte[] headerDataByte = new byte[NetworkSize.HeaderSize];
                Array.Copy(processBuffer, 0, headerDataByte, 0, headerDataByte.Length);
                stHeaderUdp header = Utilities.GetObjectFromByte<stHeaderUdp>(headerDataByte);
                UdpIncomingDataProcess(header.MsgID, processBuffer);
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("UDPSocketException " + socketException.ToString());
        }
    }
    private void UdpIncomingDataProcess(ushort msgId, byte[] msgData)
    {
        switch (msgId)
        {
            case MessageIdUdp.PlayerPosition:
                stPlayerPosition playerPosition = Utilities.GetObjectFromByte<stPlayerPosition>(msgData);
                Vector2 pos = new Vector2(playerPosition.PositionX, playerPosition.PositionY);
                AdventureSceneManager.Instance.EventAdventureScene.CallPositionChanged(playerPosition.PlayerIndex, pos);
                break;
        }
    }

    private bool IsConnected(TcpClient client)
    {
        if (client?.Client == null || !client.Client.Connected)
            return false;

        try
        {
            return !(client.Client.Poll(0, SelectMode.SelectRead) && client.Client.Receive(new byte[1], SocketFlags.Peek) == 0);
        }
        catch
        {
            return false;
        }
    }

    public void DisConnect()
    {
        _clientReady = false;
        _socketConnection.Close();
        _stream.Close();
        _udpSocketSend.Close();
        _udpSocketReceive.Close();

        _socketConnection.Close();
        _socketConnection = null;

        _tcpListenerThread.Abort();
        _tcpListenerThread = null;

        _udpListenerThread.Abort();
        _udpListenerThread = null;
    }

    private void OnApplicationQuit()
    {
        DisConnect();
    }

#if UNITY_EDITOR

    protected override void OnBindField()
    {
        base.OnBindField();
        EventClientToServer = GetComponent<ClientToServerEvent>();
    }

    private void OnValidate()
    {

    }

#endif
}
