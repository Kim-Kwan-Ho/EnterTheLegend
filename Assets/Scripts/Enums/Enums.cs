public enum LoginPlatform
{
    Guest = 0,
    PlayFabAccount = 1,
    Facebook = 2,
    Google = 3
}


public enum Direction
{
    UpLeft = 0,
    Up = 1,
    UpRight = 2,
    Left = 3,
    Right = 4,
    DownLeft = 5,
    Down = 6,
    DownRight = 7,
    Previous = 8,
}

public enum State
{
    Idle = 0,
    Move = 1,
    Run = 2,
    Dash = 3,
    Attack = 4,
    Stun = 5,
    Death = 6
}

public enum GameSceneState
{
    Loading,
    WaitingPlayer,
    StartGame,
    MyPlayerDeath,
    ClearFailed,
    ClearSucceed
}

public enum EquipmentType
{
    Character,
    Weapon,
    Helmet,
    Armor,
    Shoes
}
public enum GameRoomType
{
    Duel,
    TeamBattle,
    TrioDefense
}