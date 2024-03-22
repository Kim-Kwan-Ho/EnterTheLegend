using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using PimDeWitte.UnityMainThreadDispatcher;
using StandardData;
using System.Runtime.InteropServices;

[RequireComponent(typeof(ClientToServerEvent))]

public class ServerManager : SingletonMonobehaviour<ServerManager>
{

    private string _id = null;
    public string ID { get { return _id; } }
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
        LoginSceneManager.Instance.EventLoginScene.OnLogout += Event_Logout;
    }

    private void OnDestroy()
    {
        EventClientToServer.OnUdpSend -= Event_OnUdpMessageSend;
        EventClientToServer.OnTcpSend -= Event_OnTcpMessageSend;
        LoginSceneManager.Instance.EventLoginScene.OnLoginSuccess -= Event_ConnectToServer;
        LoginSceneManager.Instance.EventLoginScene.OnLogout -= Event_Logout;
    }


    private void Event_ConnectToServer(LoginSceneEvents loginSceneEvents,
        LoginSceneEventLoginSucceedArgs loginSceneEventLoginSucceedArgs)
    {
        _id = loginSceneEventLoginSucceedArgs.id;
        _ip = loginSceneEventLoginSucceedArgs.ip;
        _port = loginSceneEventLoginSucceedArgs.port;
        OpenServer();
    }

    private void Event_Logout(LoginSceneEvents loginSceneEvents,
        LoginSceneEventLogoutArgs loginSceneEventLogoutArgs)
    {
        _id = null;
        _ip = null;
        _port = 0;
        DisConnect();
    }
    private void OpenServer()
    {
        _tcpListenerThread = new Thread(new ThreadStart(TcpListenForIncomingRequest));
        _tcpListenerThread.IsBackground = true;
        _tcpListenerThread.Start();


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
            UnityMainThreadDispatcher.Instance().Enqueue(() => UIManager.Instance.OpenNoticePopup("Unable to connect to the game server. Contact rlarhksgh010@naver.com"));
            DisConnect();
            Thread.Sleep(2000);
            UnityMainThreadDispatcher.Instance().Enqueue(() => Application.Quit());
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
                _IPEndPointSend = new IPEndPoint(IPAddress.Parse(_ip), setPort.UdpPortSend);
                _udpSocketSend = new UdpClient();
                _IPEndPointReceive = new IPEndPoint(IPAddress.Any, setPort.UdpPortReceive);
                _udpSocketReceive = new UdpClient(_IPEndPointReceive);
                _udpListenerThread = new Thread(new ThreadStart(UdpListenForIncomingRequest));
                _udpListenerThread.IsBackground = true;
                _udpListenerThread.Start();
                break;
            case MessageIdTcp.ResponsePlayerData:
                stResponsePlayerData playerData = Utilities.GetObjectFromByte<stResponsePlayerData>(msgData);
                UnityMainThreadDispatcher.Instance().Enqueue(() => { MySceneManager.Instance.EventSceneChanged.CallSceneChanged("LobbyScene", playerData, true, 3); });
                break;
            case MessageIdTcp.CreateTeamBattleRoom:
                stCreateTeamBattleRoom createRoom = Utilities.GetObjectFromByte<stCreateTeamBattleRoom>(msgData);
                UnityMainThreadDispatcher.Instance().Enqueue(() => { MySceneManager.Instance.EventSceneChanged.CallSceneChanged("TeamBattleScene", createRoom, true, 3); });
                break;
            case MessageIdTcp.TeamBattleRoomLoadInfo:
                stTeamBattleRoomLoadInfo loadInfo = Utilities.GetObjectFromByte<stTeamBattleRoomLoadInfo>(msgData);
                TeamBattleSceneManager.Instance.EventTeamBattleScene.CallGameStart(loadInfo.IsAllSucceed);
                break;
            case MessageIdTcp.TeamBattleRoomPlayerStateChangedFromServer:
                stTeamBattleRoomPlayerStateChangedFromServer stateChanged =
                    Utilities.GetObjectFromByte<stTeamBattleRoomPlayerStateChangedFromServer>(msgData);
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    TeamBattleSceneManager.Instance.EventTeamBattleScene.CallPlayerStateChanged(stateChanged.PlayerIndex, (State)stateChanged.State);
                });
                break;
            case MessageIdTcp.TeamBattleRoomPlayerDirectionChangedFromServer:
                stTeamBattleRoomPlayerDirectionChangedFromServer directionChanged =
                    Utilities.GetObjectFromByte<stTeamBattleRoomPlayerDirectionChangedFromServer>(msgData);
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    TeamBattleSceneManager.Instance.EventTeamBattleScene.CallPlayerDirectionChanged(directionChanged.PlayerIndex, (Direction)directionChanged.Direction);
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
                if (_udpSocketReceive == null || _IPEndPointReceive == null)
                    continue;

                byte[] processBuffer = new byte[NetworkSize.BufferSize];
                processBuffer = _udpSocketReceive.Receive(ref _IPEndPointReceive);
                byte[] headerDataByte = new byte[NetworkSize.HeaderSize];
                Array.Copy(processBuffer, 0, headerDataByte, 0, headerDataByte.Length);
                stHeaderUdp header = Utilities.GetObjectFromByte<stHeaderUdp>(headerDataByte);
                UdpIncomingDataProcess(header.MsgID, processBuffer);
                Thread.Sleep(10);
            }
        }
        catch (SocketException socketException)
        {
            DisConnect();
            UnityMainThreadDispatcher.Instance().Enqueue(() => UIManager.Instance.OpenNoticePopup("Unable to connect to the game server. Contact rlarhksgh010@naver.com"));
            Debug.Log("UDPSocketException " + socketException.ToString());
            Thread.Sleep(2000);
            UnityMainThreadDispatcher.Instance().Enqueue(() => Application.Quit());
            
        }
    }
    private void UdpIncomingDataProcess(ushort msgId, byte[] msgData)
    {
        switch (msgId)
        {
            case MessageIdUdp.TeamBattlePlayerPositionFromServer:
                stTeamBattlePlayerPositionFromSever playerPosition =
                    Utilities.GetObjectFromByte<stTeamBattlePlayerPositionFromSever>(msgData);

                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    TeamBattleSceneManager.Instance.EventTeamBattleScene.CallPlayerPositionChanged(playerPosition);
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
        _socketConnection = null;
        _stream.Close();
        _stream = null;
        _udpSocketSend.Close();
        _udpSocketSend = null;
        _udpSocketReceive.Close();
        _udpSocketSend = null;

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
