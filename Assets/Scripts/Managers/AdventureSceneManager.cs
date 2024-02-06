using System.Collections.Generic;
using System.Runtime.InteropServices;
using StandardData;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AdventureSceneEvent))]
public class AdventureSceneManager : SingletonMonobehaviour<AdventureSceneManager>
{

    [HideInInspector] public AdventureSceneState SceneState;
    [Header("Prefabs")]
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _otherPlayerPrefab;
    [SerializeField] private GameObject _playerControllerPrefab;


    private AdventureSceneState _state;
    public AdventureSceneState State { get { return _state; } }

    private Creature[] _players = new Creature[GameRoomSize.AdventureRoomSize];
    private GameObject _playerController;
    
    private Dictionary<ushort, Creature> _playerDic = new Dictionary<ushort, Creature>();

    private ushort _roomId;
    public ushort RoomId { get { return _roomId; } }
    private ushort _playerIndex;
    public ushort PlayerIndex { get { return _playerIndex; } }
    [Header("Events")]
    public AdventureSceneEvent EventAdventureScene;


    protected override void Awake()
    {
        base.Awake();
        _state = AdventureSceneState.Loading;
    }
    private void OnEnable()
    {
        EventAdventureScene.OnRoomInitialize += Event_InitializeRoom;
        EventAdventureScene.OnPositionChanged += Event_PlayerPositionChanged;
        EventAdventureScene.OnGameStart += Event_OnGameStart;

    }

    private void OnDisable()
    {
        EventAdventureScene.OnRoomInitialize -= Event_InitializeRoom;
        EventAdventureScene.OnPositionChanged -= Event_PlayerPositionChanged;
        EventAdventureScene.OnGameStart -= Event_OnGameStart;
    }

    private void Event_InitializeRoom(AdventureSceneEvent adventureSceneEvent, AdventureSceneEventRoomInfoArgs adventureSceneEventArgs)
    {
        stAdventureRoomInfo roomInfo = adventureSceneEventArgs.roomInfo;
        _roomId = roomInfo.roomId;
        for (ushort i = 0; i < GameRoomSize.AdventureRoomSize; i++)
        {

            if (roomInfo.playersInfo[i].Name == MyGameManager.Instance.PlayerName)
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
            _playerDic[roomInfo.playersInfo[i].Index] = _players[i];

        }

        stAdventureRoomPlayerLoadInfo playerLoadInfo = new stAdventureRoomPlayerLoadInfo();
        playerLoadInfo.Header.MsgID = MessageIdTcp.AdventurePlayerLoadInfo;
        playerLoadInfo.Header.PacketSize = (ushort)Marshal.SizeOf(playerLoadInfo);
        playerLoadInfo.RoomId = _roomId;
        ServerManager.Instance.EventClientToServer.CallOnTcpSend(Utilities.GetObjectToByte(playerLoadInfo));

        _state = AdventureSceneState.WaitingPlayer;
    }

    private void Event_OnGameStart(AdventureSceneEvent adventureSceneEvent, bool loadInfo)
    {
        if (loadInfo)
        {
            // 게임 시작 작성
        }
        else
        {
            // 여기 로드 실패 작성
        }
    }

    private void Event_PlayerPositionChanged(AdventureSceneEvent adventureSceneEvent,
        AdventureSceneEventPlayerPositionArgs adventureSceneEventArgs)
    {
        stAdventurePlayerPositionFromSever playerPositions = adventureSceneEventArgs.playerPositions;
        for (int i = 0; i < playerPositions.PlayerPosition.Length; i++)
        {
            if (playerPositions.PlayerPosition[i].PlayerIndex == _playerIndex)
                return;
            _playerDic[playerPositions.PlayerPosition[i].PlayerIndex].EventMovement.CallPositionMovement(
                new Vector2(playerPositions.PlayerPosition[i].PositionX, playerPositions.PlayerPosition[i].PositionY));
        }
        
    }


    public MyPlayer GetMyPlayer()
    {
        return _players[_playerIndex] as MyPlayer;
    }

#if UNITY_EDITOR

    protected override void OnBindField()
    {
        base.OnBindField();
        EventAdventureScene = GetComponent<AdventureSceneEvent>();
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _playerPrefab);
        CheckNullValue(this.name, _otherPlayerPrefab);
        CheckNullValue(this.name, EventAdventureScene);
    }
#endif
}
