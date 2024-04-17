public enum LoginPlatform
{
    Guest = 0,
    PlayFabAccount = 1,
    Facebook = 2,
    Google = 3
}


public enum Direction
{
    Up = 0,
    Left = 1,
    Right = 2,
    Down = 3,
    Previous = 4,
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
    Shoes,
    None,
}

public enum WeaponType
{
    Sword,
    Bow
}
public enum WeaponSkillType
{
    SelfBuff,      
    TeamBuff,  
    EnemyTargetDeBuff, 
    EnemyTeamDeBuff,   
    AutoCast,
    TargetCast,
    None
}

public enum ShoesSkillType
{
    Buff,
    Dash,
    Teleport,
    None,
}
public enum GameRoomType
{
    Duel,
    TeamBattle,
    TrioDefense
}

public enum SceneType
{
    LobbyScene,
    TeamBattleScene
}