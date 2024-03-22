using System.Collections.Generic;
using System.Runtime.InteropServices;
using StandardData;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(TeamBattleSceneEvent))]
public class TeamBattleSceneManager : SingletonMonobehaviour<TeamBattleSceneManager>
{

    public GameSceneState SceneState;
    [Header("Prefabs")]
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _otherPlayerPrefab;
    [SerializeField] private GameObject _playerControllerPrefab;

    private GameSceneState _state;
    public GameSceneState State { get { return _state; } }

    private Creature[] _players = new Creature[GameRoomSize.TeamBattleRoomSize];
    private GameObject _playerController;

    private ushort _roomId;
    public ushort RoomId { get { return _roomId; } }
    private ushort _playerIndex;
    public ushort PlayerIndex { get { return _playerIndex; } }
    [Header("Events")]
    public TeamBattleSceneEvent EventTeamBattleScene;


    protected override void Awake()
    {
        base.Awake();
        _state = GameSceneState.Loading;

    }
    private void Start()
    {
        EventTeamBattleScene.OnRoomInitialize += Event_InitializeRoom;
        EventTeamBattleScene.OnPlayerPositionChanged += Event_PlayerPositionChanged;
        EventTeamBattleScene.OnGameStart += Event_OnGameStart;
        EventTeamBattleScene.OnPlayerStateChanged += Event_PlayerStateChanged;
        EventTeamBattleScene.OnPlayerDirectionChanged += Event_PlayerDirectionChanged;

    }

    private void OnDestroy()
    {
        EventTeamBattleScene.OnRoomInitialize -= Event_InitializeRoom;
        EventTeamBattleScene.OnPlayerPositionChanged -= Event_PlayerPositionChanged;
        EventTeamBattleScene.OnGameStart -= Event_OnGameStart;
        EventTeamBattleScene.OnPlayerStateChanged -= Event_PlayerStateChanged;
        EventTeamBattleScene.OnPlayerDirectionChanged -= Event_PlayerDirectionChanged;
    }

    private void Event_InitializeRoom(TeamBattleSceneEvent teamBattleSceneEvent, TeamBattleSceneEventRoomInfoArgs teamBattleSceneEventArgs)
    {
        stTeamBattleRoomInfo roomInfo = teamBattleSceneEventArgs.roomInfo;
        _roomId = roomInfo.roomId;
        for (ushort i = 0; i < GameRoomSize.TeamBattleRoomSize; i++)
        {

            if (roomInfo.playersInfo[i].Name == MyGameManager.Instance.Nickname)
            {
                _playerIndex = i;
                _players[i] = Instantiate(_playerPrefab).GetComponent<MyPlayer>();
                _playerController = Instantiate(_playerControllerPrefab);
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

        _state = GameSceneState.WaitingPlayer;
    }

    private void Event_OnGameStart(TeamBattleSceneEvent teamBattleSceneEvent, bool loadInfo)
    {
        if (loadInfo)
        {
            _state = GameSceneState.StartGame;
            // 게임 시작 작성
        }
        else
        {
            // 여기 로드 실패 작성
        }
    }

    private void Event_PlayerPositionChanged(TeamBattleSceneEvent teamBattleSceneEvent,
        TeamBattleSceneEventPlayerPositionChangedArgs teamBattleSceneEventArgs)
    {
        stTeamBattlePlayerPositionFromSever playerPositions = teamBattleSceneEventArgs.playerPositions;
        for (int i = 0; i < playerPositions.PlayerPosition.Length; i++)
        {
            if (playerPositions.PlayerPosition[i].PlayerIndex == _playerIndex)
                continue;
            _players[playerPositions.PlayerPosition[i].PlayerIndex].EventMovement.CallPositionMovement(
                new Vector2(playerPositions.PlayerPosition[i].PositionX, playerPositions.PlayerPosition[i].PositionY));
        }
        
    }

    private void Event_PlayerStateChanged(TeamBattleSceneEvent teamBattleSceneEvent,
        TeamBattleSceneEventPlayerStateChangedArgs teamBattleSceneEventPlayerStateChangedArgs)
    {
        _players[teamBattleSceneEventPlayerStateChangedArgs.playerIndex].EventState
            .CallStateChangedEvent(teamBattleSceneEventPlayerStateChangedArgs.state);
    }

    private void Event_PlayerDirectionChanged(TeamBattleSceneEvent teamBattleSceneEvent,
        TeamBattleSceneEventPlayerDirectionChangedArgs teamBattleSceneEventPlayerDirectionChangedArgs)
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

    private void OnValidate()
    {
        CheckNullValue(this.name, _playerPrefab);
        CheckNullValue(this.name, _otherPlayerPrefab);
        CheckNullValue(this.name, EventTeamBattleScene);
    }
#endif
}
