using StandardData;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using static UnityEngine.CullingGroup;

public class MyPlayerDataSender : BaseBehaviour
{
    [SerializeField] private MyPlayer _player;
    private stTeamBattleRoomPlayerInfo _playerInfo;


    private void Start()
    {
        StartCoroutine(CoSendPlayerPosition());
        _playerInfo = new stTeamBattleRoomPlayerInfo();
        _playerInfo.RoomId = TeamBattleSceneManager.Instance.RoomId;
        _playerInfo.PlayerIndex = TeamBattleSceneManager.Instance.PlayerIndex;
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

    private IEnumerator CoSendPlayerPosition()
    {
        yield return new WaitUntil(() => TeamBattleSceneManager.Instance.State == GameSceneState.StartGame);
        while (true)
        {
            stTeamBattlePlayerPositionToServer position = new stTeamBattlePlayerPositionToServer();
            position.Header.MsgID = MessageIdUdp.TeamBattlePlayerPositionToServer;
            position.RoomType = (ushort)MyGameManager.Instance.RoomType;
            position.RoomId = TeamBattleSceneManager.Instance.RoomId;
            position.PlayerPosition.PlayerIndex = TeamBattleSceneManager.Instance.PlayerIndex;
            position.PlayerPosition.PositionX = _player.transform.position.x;
            position.PlayerPosition.PositionY = _player.transform.position.y;
            byte[] msg = Utilities.GetObjectToByte(position);
            ServerManager.Instance.EventClientToServer.CallOnUdpSend(msg);
            yield return new WaitForSeconds(UdpSendCycle.TeamBattleRoomSendCycle);
        }
    }

    private void Event_SendPlayerStateChanged(StateEvent stateEvent, StateEventArgs stateEventArgs)
    {
        stTeamBattleRoomPlayerStateChangedToServer stateChanged = new stTeamBattleRoomPlayerStateChangedToServer();
        stateChanged.Header.MsgID = MessageIdTcp.TeamBattleRoomPlayerStateChangedToServer;
        stateChanged.Header.PacketSize = (ushort)Marshal.SizeOf(stateChanged);
        stateChanged.PlayerInfo = _playerInfo;
        stateChanged.State = (ushort)stateEventArgs.state;
        byte[] msg = Utilities.GetObjectToByte(stateChanged);
        ServerManager.Instance.EventClientToServer.CallOnTcpSend(msg);
    }
    private void Event_SendPlayerDirectionChanged(DirectionEvent directionEvent, DirectionEventArgs directionEventArgs)
    {
        stTeamBattleRoomPlayerDirectionChangedToServer directionChanged = new stTeamBattleRoomPlayerDirectionChangedToServer();
        directionChanged.Header.MsgID = MessageIdTcp.TeamBattleRoomPlayerDirectionChangedToServer;
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
