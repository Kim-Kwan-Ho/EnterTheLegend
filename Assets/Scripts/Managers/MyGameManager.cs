using StandardData;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using JetBrains.Annotations;


[RequireComponent(typeof(MyGameManagerEvent))]
public class MyGameManager : SingletonMonobehaviour<MyGameManager>
{
    [Header("Events")]
    [SerializeField]
    public MyGameManagerEvent EventGameManager;

    [SerializeField]
    private string _nickname = "Client_1";
    public string Nickname { get { return _nickname; } }

    public void SetPlayerNickname(string name)
    {
        _nickname = name; 
    }


    private GameRoomType _roomType;
    public GameRoomType RoomType
    { get { return _roomType; } }


    public void SetGameRoomType(GameRoomType roomType)
    {
        this._roomType = roomType;
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        EventGameManager = GetComponent<MyGameManagerEvent>();
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, EventGameManager);
    }
#endif
}
