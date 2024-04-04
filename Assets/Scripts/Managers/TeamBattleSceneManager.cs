using System.Collections.Generic;
using System.Runtime.InteropServices;
using StandardData;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(TeamBattleSceneEvent))]
public class TeamBattleSceneManager : BattleSceneManager
{

    [Header("Prefabs")]
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _otherPlayerPrefab;

  

    private Creature[] _players = new Creature[GameRoomSize.TeamBattleRoomSize];
    private GameObject _playerController;


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

    private void OnDestroy()
    {
        EventBattleScene.OnRoomInitialize -= Event_InitializeTeamBattleRoom;
        EventBattleScene.OnPlayerPositionChanged -= Event_PlayerPositionChanged;
        EventBattleScene.OnGameStart -= Event_OnGameStart;
        EventBattleScene.OnPlayerStateChanged -= Event_PlayerStateChanged;
        EventBattleScene.OnPlayerDirectionChanged -= Event_PlayerDirectionChanged;
    }

    private void Event_InitializeTeamBattleRoom(BattleSceneEvent teamBattleSceneEvent, BattleSceneEventRoomInfoArgs teamBattleSceneEventArgs)
    {
        stBattleRoomInfo roomInfo = teamBattleSceneEventArgs.roomInfo;
        _roomId = roomInfo.roomId;
        _playerIndex = teamBattleSceneEventArgs.roomInfo.playerIndex;
        for (ushort i = 0; i < GameRoomSize.TeamBattleRoomSize; i++)
        {
            if (i == _playerIndex)
            {
                _players[i] = Instantiate(_playerPrefab).GetComponent<MyPlayer>();
                _player = (MyPlayer)_players[i];
            }
            else
            {
                _players[i] = Instantiate(_otherPlayerPrefab).GetComponent<OtherPlayer>();
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

    private void Event_PlayerPositionChanged(BattleSceneEvent teamBattleSceneEvent,
        BattleScenePlayerPositionChangedEventArgs teamBattleSceneEventArgs)
    {
        stBattlePlayerPositionFromSever playerPositions = teamBattleSceneEventArgs.playerPositions;
        for (int i = 0; i < playerPositions.PlayerPosition.Length; i++)
        {
            if (playerPositions.PlayerPosition[i].PlayerIndex == _playerIndex)
                continue;
            _players[playerPositions.PlayerPosition[i].PlayerIndex].EventMovement.CallPositionMovement(
                new Vector2(playerPositions.PlayerPosition[i].PositionX, playerPositions.PlayerPosition[i].PositionY));
        }
        
    }

    private void Event_PlayerStateChanged(BattleSceneEvent teamBattleSceneEvent,
        BattleSceneEventPlayerStateChangedEventArgs teamBattleSceneEventPlayerStateChangedArgs)
    {
        _players[teamBattleSceneEventPlayerStateChangedArgs.playerIndex].EventState
            .CallStateChangedEvent(teamBattleSceneEventPlayerStateChangedArgs.state);
    }

    private void Event_PlayerDirectionChanged(BattleSceneEvent teamBattleSceneEvent,
        BattleScenePlayerDirectionChangedEventArgs teamBattleSceneEventPlayerDirectionChangedArgs)
    {
        _players[teamBattleSceneEventPlayerDirectionChangedArgs.playerIndex].EventDirection
            .CallDirectionChanged(teamBattleSceneEventPlayerDirectionChangedArgs.direction);
    }

    public MyPlayer GetMyPlayer()
    {
        return _players[_playerIndex] as MyPlayer;
    }

#if UNITY_EDITOR

    protected override void OnBindField()
    {
        base.OnBindField();
        EventTeamBattleScene = GetComponent<TeamBattleSceneEvent>();
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        CheckNullValue(this.name, _playerPrefab);
        CheckNullValue(this.name, _otherPlayerPrefab);
        CheckNullValue(this.name, EventTeamBattleScene);
    }
#endif
}
