using StandardData;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using static UnityEngine.CullingGroup;

public class MyPlayerDataSender : BaseBehaviour
{
    [SerializeField] private MyPlayer _player;
    private stAdventureRoomPlayerInfo _playerInfo;


    private void Start()
    {
        StartCoroutine(SendPlayerPosition());
        _playerInfo = new stAdventureRoomPlayerInfo();
        _playerInfo.RoomId = AdventureSceneManager.Instance.RoomId;
        _playerInfo.PlayerIndex = AdventureSceneManager.Instance.PlayerIndex;
    }

    private void OnEnable()
    {
        _player.EventState.OnStateChanged += Event_SendPlayerStateChanged;
        _player.EventDirection.OnDirectionChanged += Event_SendPlayerDirectionChanged;
    }

    private void OnDisable()
    {
        _player.EventState.OnStateChanged -= Event_SendPlayerStateChanged;
        _player.EventDirection.OnDirectionChanged -= Event_SendPlayerDirectionChanged;

    }

    private void Update()
    {

    }

    private IEnumerator SendPlayerPosition()
    {
        yield return new WaitUntil(() => AdventureSceneManager.Instance.State == AdventureSceneState.StartGame);
        while (true)
        {
            stAdventurePlayerPositionToServer position = new stAdventurePlayerPositionToServer();
            position.Header.MsgID = MessageIdUdp.AdventurePlayerPositionToServer;
            position.RoomType = (ushort)MyGameManager.Instance.RoomType;
            position.RoomId = AdventureSceneManager.Instance.RoomId;
            position.PlayerPosition.PlayerIndex = AdventureSceneManager.Instance.PlayerIndex;
            position.PlayerPosition.PositionX = _player.transform.position.x;
            position.PlayerPosition.PositionY = _player.transform.position.y;
            byte[] msg = Utilities.GetObjectToByte(position);
            ServerManager.Instance.EventClientToServer.CallOnUdpSend(msg);
            yield return new WaitForSeconds(UdpSendCycle.AdventureRoomSendCycle);
        }
    }

    private void Event_SendPlayerStateChanged(StateEvent stateEvent, StateEventArgs stateEventArgs)
    {
        stAdventureRoomPlayerStateChangedToServer stateChanged = new stAdventureRoomPlayerStateChangedToServer();
        stateChanged.Header.MsgID = MessageIdTcp.AdventureRoomPlayerStateChangedToServer;
        stateChanged.Header.PacketSize = (ushort)Marshal.SizeOf(stateChanged);
        stateChanged.PlayerInfo = _playerInfo;
        stateChanged.State = (ushort)stateEventArgs.state;
        byte[] msg = Utilities.GetObjectToByte(stateChanged);
        ServerManager.Instance.EventClientToServer.CallOnTcpSend(msg);
    }
    private void Event_SendPlayerDirectionChanged(DirectionEvent directionEvent, DirectionEventArgs directionEventArgs)
    {
        stAdventureRoomPlayerDirectionChangedToServer directionChanged = new stAdventureRoomPlayerDirectionChangedToServer();
        directionChanged.Header.MsgID = MessageIdTcp.AdventureRoomPlayerDirectionChangedToServer;
        directionChanged.Header.PacketSize = (ushort)Marshal.SizeOf(directionChanged);
        directionChanged.PlayerInfo = _playerInfo;
        directionChanged.Direction = (ushort)directionEventArgs.direction;
        byte[] msg = Utilities.GetObjectToByte(directionChanged);
        ServerManager.Instance.EventClientToServer.CallOnTcpSend(msg);
    }
    private void Event_SendPlayerOnAttack(AttackEvent attackEvent, AttackEventArgs attackEventArgs)
    {

    }



#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _player = GetComponent<MyPlayer>();
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _player);
    }
#endif
}
