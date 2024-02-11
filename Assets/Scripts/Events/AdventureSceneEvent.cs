using System;
using StandardData;
using UnityEngine;

public class AdventureSceneEvent : MonoBehaviour
{
    public Action<AdventureSceneEvent, AdventureSceneEventRoomInfoArgs> OnRoomInitialize;
    public Action<AdventureSceneEvent, bool> OnGameStart;
    public Action<AdventureSceneEvent, AdventureSceneEventPlayerPositionChangedArgs> OnPlayerPositionChanged;
    public Action<AdventureSceneEvent, AdventureSceneEventPlayerStateChangedArgs> OnPlayerStateChanged;
    public Action<AdventureSceneEvent, AdventureSceneEventPlayerDirectionChangedArgs> OnPlayerDirectionChanged;


    public void CallRoomInitialize(ushort roomId, stAdventurePlayerInfo[] playersInfo)
    {
        OnRoomInitialize?.Invoke(this,
            new AdventureSceneEventRoomInfoArgs() { roomInfo = new stAdventureRoomInfo(){roomId = roomId,  playersInfo = playersInfo }});
    }

    public void CallGameStart(bool gameStart)
    {
        OnGameStart?.Invoke(this, gameStart);
    }

    public void CallPlayerPositionChanged(stAdventurePlayerPositionFromSever playerPositions)
    {
        OnPlayerPositionChanged?.Invoke(this,
            new AdventureSceneEventPlayerPositionChangedArgs() { playerPositions = playerPositions});
    }

    public void CallPlayerStateChanged(ushort playerIndex, State state)
    {
        OnPlayerStateChanged?.Invoke(this,
            new AdventureSceneEventPlayerStateChangedArgs() { playerIndex = playerIndex, state = state });
    }
    public void CallPlayerDirectionChanged(ushort playerIndex, Direction direction)
    {
        OnPlayerDirectionChanged?.Invoke(this,
            new AdventureSceneEventPlayerDirectionChangedArgs() { playerIndex = playerIndex, direction = direction });
    }
}

public class AdventureSceneEventRoomInfoArgs : EventArgs
{
    public stAdventureRoomInfo roomInfo;
    
}
public struct stAdventureRoomInfo
{
    public ushort roomId;
    public stAdventurePlayerInfo[] playersInfo;
}

public class AdventureSceneEventPlayerPositionChangedArgs : EventArgs
{
    public stAdventurePlayerPositionFromSever playerPositions;
}

public class AdventureSceneEventPlayerStateChangedArgs : EventArgs
{
    public ushort playerIndex;
    public State state;
}

public class AdventureSceneEventPlayerDirectionChangedArgs : EventArgs
{
    public ushort playerIndex;
    public Direction direction;
}