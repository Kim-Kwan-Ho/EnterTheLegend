using System;
using StandardData;
using UnityEngine;

public class TeamBattleSceneEvent : MonoBehaviour
{
    public Action<TeamBattleSceneEvent, TeamBattleSceneEventRoomInfoArgs> OnRoomInitialize;
    public Action<TeamBattleSceneEvent, bool> OnGameStart;
    public Action<TeamBattleSceneEvent, TeamBattleSceneEventPlayerPositionChangedArgs> OnPlayerPositionChanged;
    public Action<TeamBattleSceneEvent, TeamBattleSceneEventPlayerStateChangedArgs> OnPlayerStateChanged;
    public Action<TeamBattleSceneEvent, TeamBattleSceneEventPlayerDirectionChangedArgs> OnPlayerDirectionChanged;


    public void CallRoomInitialize(ushort roomId, stTeamBattlePlayerInfo[] playersInfo)
    {
        OnRoomInitialize?.Invoke(this,
            new TeamBattleSceneEventRoomInfoArgs() { roomInfo = new stTeamBattleRoomInfo(){roomId = roomId,  playersInfo = playersInfo }});
    }

    public void CallGameStart(bool gameStart)
    {
        OnGameStart?.Invoke(this, gameStart);
    }

    public void CallPlayerPositionChanged(stTeamBattlePlayerPositionFromSever playerPositions)
    {
        OnPlayerPositionChanged?.Invoke(this,
            new TeamBattleSceneEventPlayerPositionChangedArgs() { playerPositions = playerPositions});
    }

    public void CallPlayerStateChanged(ushort playerIndex, State state)
    {
        OnPlayerStateChanged?.Invoke(this,
            new TeamBattleSceneEventPlayerStateChangedArgs() { playerIndex = playerIndex, state = state });
    }
    public void CallPlayerDirectionChanged(ushort playerIndex, Direction direction)
    {
        OnPlayerDirectionChanged?.Invoke(this,
            new TeamBattleSceneEventPlayerDirectionChangedArgs() { playerIndex = playerIndex, direction = direction });
    }
}

public class TeamBattleSceneEventRoomInfoArgs : EventArgs
{
    public stTeamBattleRoomInfo roomInfo;
    
}
public struct stTeamBattleRoomInfo
{
    public ushort roomId;
    public stTeamBattlePlayerInfo[] playersInfo;
}

public class TeamBattleSceneEventPlayerPositionChangedArgs : EventArgs
{
    public stTeamBattlePlayerPositionFromSever playerPositions;
}

public class TeamBattleSceneEventPlayerStateChangedArgs : EventArgs
{
    public ushort playerIndex;
    public State state;
}

public class TeamBattleSceneEventPlayerDirectionChangedArgs : EventArgs
{
    public ushort playerIndex;
    public Direction direction;
}