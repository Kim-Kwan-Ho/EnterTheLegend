using StandardData;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class MyCharacterDataSender : BaseBehaviour
{
    [SerializeField] private MyCharacter _player;
    private stTeamBattleRoomPlayerInfo _playerInfo;


    private void Start()
    {
        StartCoroutine(CoSendPlayerPosition());
        _playerInfo = new stTeamBattleRoomPlayerInfo
        {
            RoomId = TeamBattleSceneManager.Instance.RoomId,
            PlayerIndex = TeamBattleSceneManager.Instance.PlayerIndex
        };
    }

    private void OnEnable()
    {
        BattleSceneManager.Instance.EventBattleScene.OnGameStart += Event_OnGameStart;
        _player.EventState.OnStateChanged += Event_SendPlayerStateChanged;
        _player.EventDirection.OnDirectionChanged += Event_SendPlayerDirectionChanged;
    }

    private void OnDisable()
    {
        BattleSceneManager.Instance.EventBattleScene.OnGameStart -= Event_OnGameStart;
        _player.EventState.OnStateChanged -= Event_SendPlayerStateChanged;
        _player.EventDirection.OnDirectionChanged -= Event_SendPlayerDirectionChanged;

    }

    private void Event_OnGameStart(BattleSceneEvent battleSceneEvent, bool loadInfo)
    {
        StartCoroutine(CoSendPlayerPosition());
    }
    private IEnumerator CoSendPlayerPosition()
    {
        while (true)
        {
            stTeamBattlePlayerPositionToServer position = new stTeamBattlePlayerPositionToServer();
            position.Header.MsgID = MessageIdUdp.TeamBattlePlayerPositionToServer;
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
        _player = GetComponent<MyCharacter>();
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _player);
    }
#endif
}
