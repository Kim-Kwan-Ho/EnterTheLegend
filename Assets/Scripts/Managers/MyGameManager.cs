using StandardData;
using UnityEditor.PackageManager;

public class MyGameManager : SingletonMonobehaviour<MyGameManager>
{
    private string _playerName = "Client_1";
    public string PlayerName
    {
        get { return _playerName; }
    }
    protected override void Awake()
    {
        base.Awake();
    }

    private GameRoomType _roomType;
    public GameRoomType RoomType
    { get { return _roomType; } }


    public void SetGameRoomType(GameRoomType roomType)
    {
        this._roomType = roomType;
    }


}
