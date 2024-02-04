using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using StandardData;
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
    public AdventureSceneState State
    {
        get { return _state; }
    }

    private MyPlayer _player;
    private OtherPlayer[] _otherPlayers = new OtherPlayer[GameRoomSize.AdventureRoomSize - 1];
    private GameObject _playerController;
    
    private Dictionary<ushort, Creature> _playerDic = new Dictionary<ushort, Creature>();

    private ushort _roomId;
    private ushort _playerIndex;

    [Header("Events")]
    public AdventureSceneEvent EventAdventureScene;

    [SerializeField] private string _myName = "a";

    protected override void Awake()
    {
        base.Awake();
        _state = AdventureSceneState.Loading;
    }

    private void Start()
    {
        stAdventureRoomLoaded roomLoaded = new stAdventureRoomLoaded();
        roomLoaded.Header.MsgID = MessageIdTcp.AdventureRoomLoaded;
        roomLoaded.Header.PacketSize = (ushort)Marshal.SizeOf(roomLoaded);
        ServerManager.Instance.EventClientToServer.CallOnTcpSend(Utilities.GetObjectToByte(roomLoaded));
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
        _playerIndex = roomInfo.playerIndex;
        for (int i = 0; i < GameRoomSize.AdventureRoomSize; i++)
        {
            if (roomInfo.playersInfo[i].Name == _myName)
            {
                _player = Instantiate(_playerPrefab).GetComponent<MyPlayer>();
                _player.Initialize();
                _playerDic[roomInfo.playersInfo[i].Index] = _player;
                _playerController = Instantiate(_playerControllerPrefab);
            }
            else
            {
                _otherPlayers[i] = Instantiate(_otherPlayerPrefab).GetComponent<OtherPlayer>();
                _otherPlayers[i].Initialize();
                _playerDic[roomInfo.playersInfo[i].Index] = _otherPlayers[i];
            }
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
        stAdventurePlayerPosition playerPosition = adventureSceneEventArgs.playerPosition;
        if (playerPosition.index ==  _playerIndex)
            return;
        _playerDic[playerPosition.index].EventMovement.CallPositionMovement(playerPosition.position);
    }


    public MyPlayer GetMyPlayer()
    {
        return _player;
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
