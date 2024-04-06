using System.Collections.Generic;
using System.Runtime.InteropServices;
using StandardData;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(TeamBattleSceneEvent))]
public class TeamBattleSceneManager : BattleSceneManager
{
    [Header("Players")]
    [SerializeField]
    private Creature[] _players = new Creature[GameRoomSize.TeamBattleRoomSize];


    [Header("Events")]
    public TeamBattleSceneEvent EventTeamBattleScene;


    protected override void Awake()
    {
        base.Awake();
        _sceneState = GameSceneState.Loading;
    }
    private void OnEnable()
    {
        EventBattleScene.OnRoomInitialize += Event_InitializeTeamBattleRoom;
        EventBattleScene.OnPlayerPositionChanged += Event_PlayerPositionChanged;
        EventBattleScene.OnGameStart += Event_OnGameStart;
        EventBattleScene.OnPlayerStateChanged += Event_PlayerStateChanged;
        EventBattleScene.OnPlayerDirectionChanged += Event_PlayerDirectionChanged;
    }

    private void OnDisable()
    {
        EventBattleScene.OnRoomInitialize -= Event_InitializeTeamBattleRoom;
        EventBattleScene.OnPlayerPositionChanged -= Event_PlayerPositionChanged;
        EventBattleScene.OnGameStart -= Event_OnGameStart;
        EventBattleScene.OnPlayerStateChanged -= Event_PlayerStateChanged;
        EventBattleScene.OnPlayerDirectionChanged -= Event_PlayerDirectionChanged;
    }

    private void Event_InitializeTeamBattleRoom(BattleSceneEvent battleSceneEvent, BattleSceneEventRoomInfoArgs battleSceneEventArgs)
    {
        stBattleRoomInfo roomInfo = battleSceneEventArgs.roomInfo;
        _roomId = roomInfo.roomId;
        _playerIndex = battleSceneEventArgs.roomInfo.playerIndex;
        for (ushort i = 0; i < GameRoomSize.TeamBattleRoomSize; i++)
        {
            if (i == _playerIndex)
            {
                _player.transform.position = _players[i].transform.position;
                Destroy(_players[i].gameObject);
                _players[i] = _player;
            }
            _players[i].Initialize();
        }

        stTeamBattleRoomPlayerLoadInfo playerLoadInfo = new stTeamBattleRoomPlayerLoadInfo();
        playerLoadInfo.Header.MsgID = MessageIdTcp.TeamBattlePlayerLoadInfo;
        playerLoadInfo.Header.PacketSize = (ushort)Marshal.SizeOf(playerLoadInfo);
        playerLoadInfo.RoomId = _roomId;
        playerLoadInfo.PlayerIndex = _playerIndex;
        byte[] msg = Utilities.GetObjectToByte(playerLoadInfo);
        ServerManager.Instance.EventClientToServer.CallOnTcpSend(msg);
        _sceneState = GameSceneState.WaitingPlayer;
    }

    private void Event_OnGameStart(BattleSceneEvent teamBattleSceneEvent, bool loadInfo)
    {
        if (loadInfo)
        {
            _sceneState = GameSceneState.StartGame;
            // 게임 시작 작성
        }
        else
        {
            // 여기 로드 실패 작성
        }
    }

    private void Event_PlayerPositionChanged(BattleSceneEvent battleSceneEvent,
        BattleScenePlayerPositionChangedEventArgs battleSceneEventArgs)
    {
        stBattlePlayerPositionFromSever playerPositions = battleSceneEventArgs.playerPositions;
        for (int i = 0; i < playerPositions.PlayerPosition.Length; i++)
        {
            if (playerPositions.PlayerPosition[i].PlayerIndex == _playerIndex)
                continue;
            _players[playerPositions.PlayerPosition[i].PlayerIndex].EventMovement.CallPositionMovement(
                new Vector2(playerPositions.PlayerPosition[i].PositionX, playerPositions.PlayerPosition[i].PositionY));
        }

    }

    private void Event_PlayerStateChanged(BattleSceneEvent battleSceneEvent,
        BattleSceneEventPlayerStateChangedEventArgs battleSceneEventPlayerStateChangedArgs)
    {
        if (battleSceneEventPlayerStateChangedArgs.playerIndex == _playerIndex)
            return;
        _players[battleSceneEventPlayerStateChangedArgs.playerIndex].EventState
            .CallStateChangedEvent(battleSceneEventPlayerStateChangedArgs.state);
    }

    private void Event_PlayerDirectionChanged(BattleSceneEvent battleSceneEvent,
        BattleScenePlayerDirectionChangedEventArgs battleSceneEventPlayerDirectionChangedArgs)
    {
        if (battleSceneEventPlayerDirectionChangedArgs.playerIndex == _playerIndex)
            return;

        _players[battleSceneEventPlayerDirectionChangedArgs.playerIndex].EventDirection
            .CallDirectionChanged(battleSceneEventPlayerDirectionChangedArgs.direction);
    }


#if UNITY_EDITOR

    protected override void OnBindField()
    {
        base.OnBindField();
        EventTeamBattleScene = GetComponent<TeamBattleSceneEvent>();
        _players = FindObjectsOfType<OtherPlayer>().OrderBy(t => t.name).ToArray();
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        CheckNullValue(this.name, EventTeamBattleScene);
        CheckNullValue(this.name, _players);
    }
#endif
}
