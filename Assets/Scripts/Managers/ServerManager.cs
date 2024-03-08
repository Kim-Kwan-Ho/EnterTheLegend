using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using PimDeWitte.UnityMainThreadDispatcher;
using StandardData;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ClientToServerEvent))]

public class ServerManager : SingletonMonobehaviour<ServerManager>
{

    private string _id = null;
    private string _ip = null;
    private int _port = 0;
    private bool _clientReady = false;
    private byte[] _buffer = new byte[NetworkSize.BufferSize];
    private byte[] _tempBuffer = new byte[NetworkSize.TempBufferSize];
    private bool _isTempByte = false;
    private int _tempByteSize = 0;

    [Header("Tcp")]
    private Thread _tcpListenerThread = null;
    private TcpClient _socketConnection = null;
    private NetworkStream _stream = null;

    [Header("Udp")]
    private Thread _udpListenerThread = null;
    private UdpClient _udpSocketReceive = null;
    private UdpClient _udpSocketSend = null;
    private IPEndPoint _IPEndPointReceive = null;
    private IPEndPoint _IPEndPointSend = null;


    public ClientToServerEvent EventClientToServer;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyGameObject();
    }

    private void Start()
    {
        EventClientToServer.OnUdpSend += Event_OnUdpMessageSend;
        EventClientToServer.OnTcpSend += Event_OnTcpMessageSend;
        LoginSceneManager.Instance.EventLoginScene.OnLoginSuccess += Event_ConnectToServer;
    }

    private void OnDestroy()
    {
        EventClientToServer.OnUdpSend -= Event_OnUdpMessageSend;
        EventClientToServer.OnTcpSend -= Event_OnTcpMessageSend;
        LoginSceneManager.Instance.EventLoginScene.OnLoginSuccess -= Event_ConnectToServer;
    }


    private void Event_ConnectToServer(LoginSceneEvents loginSceneEvents,
        LoginSceneEventLoginSucceedArgs loginSceneEventLoginSucceedArgs)
    {
        Debug.Log("서버 시작");
        _id = loginSceneEventLoginSucceedArgs.Id;
        _ip = loginSceneEventLoginSucceedArgs.Ip;
        _port = loginSceneEventLoginSucceedArgs.Port;
        OpenServer();
    }


    private void OpenServer()
    {
        _tcpListenerThread = new Thread(new ThreadStart(TcpListenForIncomingRequest));
        _tcpListenerThread.IsBackground = true;
        _tcpListenerThread.Start();

        _udpListenerThread = new Thread(new ThreadStart(UdpListenForIncomingRequest));
        _udpListenerThread.IsBackground = true;
        _udpListenerThread.Start();
    }



    private void Event_OnUdpMessageSend(ClientToServerEvent clientToServerEvent,
        ClientToServerEventArgs clientToServerEventArgs)
    {
        _udpSocketSend.Send(clientToServerEventArgs.bytes, clientToServerEventArgs.bytes.Length, _IPEndPointSend);
    }

    private void Event_OnTcpMessageSend(ClientToServerEvent clientToServerEvent,
        ClientToServerEventArgs clientToServerEventArgs)
    {
        if (_socketConnection.Connected)
        {
            _socketConnection.GetStream().Write(clientToServerEventArgs.bytes, 0, clientToServerEventArgs.bytes.Length);
        }
    }


    private void TcpListenForIncomingRequest()
    {
        try
        {
            _socketConnection = new TcpClient(_ip, _port);
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
                Thread.Sleep(10);
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
            case MessageIdTcp.SetUdpPort:
                stSetUdpPort setPort = Utilities.GetObjectFromByte<stSetUdpPort>(msgData);
                MyGameManager.Instance.SetPlayerName(setPort.Name);
                _IPEndPointSend = new IPEndPoint(IPAddress.Parse(_ip), setPort.UdpPortSend);
                _udpSocketSend = new UdpClient();
                _IPEndPointReceive = new IPEndPoint(IPAddress.Any, setPort.UdpPortReceive);
                _udpSocketReceive = new UdpClient(_IPEndPointReceive);
                break;

            case MessageIdTcp.CreateAdventureRoom:
                stCreateAdventureRoom createRoom = Utilities.GetObjectFromByte<stCreateAdventureRoom>(msgData);
                UnityMainThreadDispatcher.Instance().Enqueue(() => { MySceneManager.Instance.EventSceneChanged.CallSceneChanged("AdventureScene", createRoom, true, 3); });
                break;
            case MessageIdTcp.AdventureRoomLoadInfo:
                stAdventureRoomLoadInfo loadInfo = Utilities.GetObjectFromByte<stAdventureRoomLoadInfo>(msgData);
                AdventureSceneManager.Instance.EventAdventureScene.CallGameStart(loadInfo.IsAllSucceed);
                break;
            case MessageIdTcp.AdventureRoomPlayerStateChangedFromServer:
                stAdventureRoomPlayerStateChangedFromServer stateChanged =
                    Utilities.GetObjectFromByte<stAdventureRoomPlayerStateChangedFromServer>(msgData);
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    AdventureSceneManager.Instance.EventAdventureScene.CallPlayerStateChanged(stateChanged.PlayerIndex, (State)stateChanged.State);
                });
                break;
            case MessageIdTcp.AdventureRoomPlayerDirectionChangedFromServer:
                stAdventureRoomPlayerDirectionChangedFromServer directionChanged =
                    Utilities.GetObjectFromByte<stAdventureRoomPlayerDirectionChangedFromServer>(msgData);
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    AdventureSceneManager.Instance.EventAdventureScene.CallPlayerDirectionChanged(directionChanged.PlayerIndex, (Direction)directionChanged.Direction);
                });
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
            case MessageIdUdp.AdventurePlayerPositionFromServer:
                stAdventurePlayerPositionFromSever playerPosition =
                    Utilities.GetObjectFromByte<stAdventurePlayerPositionFromSever>(msgData);

                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    AdventureSceneManager.Instance.EventAdventureScene.CallPlayerPositionChanged(playerPosition);
                });
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
        if (!_clientReady)
            return;

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
