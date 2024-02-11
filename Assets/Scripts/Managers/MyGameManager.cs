using StandardData;
using UnityEngine;

public class MyGameManager : SingletonMonobehaviour<MyGameManager>
{
    [SerializeField] private string _playerName = "Client_1";
    public string PlayerName
    {
        get { return _playerName; }
    }

    public void SetPlayerName(string name)
    {
        _playerName = name;
    }
    protected override void Awake()
    {
        base.Awake();
        DontDestroyGameObject();
    }

    private GameRoomType _roomType;
    public GameRoomType RoomType
    { get { return _roomType; } }


    public void SetGameRoomType(GameRoomType roomType)
    {
        this._roomType = roomType;
    }


}