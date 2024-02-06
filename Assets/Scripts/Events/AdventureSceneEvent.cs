using System;
using StandardData;
using UnityEngine;

public class AdventureSceneEvent : MonoBehaviour
{
    public Action<AdventureSceneEvent, AdventureSceneEventRoomInfoArgs> OnRoomInitialize;
    public Action<AdventureSceneEvent, bool> OnGameStart;
    public Action<AdventureSceneEvent, AdventureSceneEventPlayerPositionArgs> OnPositionChanged;

    public void CallRoomInitialize(ushort roomId, stAdventurePlayerInfo[] playersInfo)
    {
        OnRoomInitialize?.Invoke(this,
            new AdventureSceneEventRoomInfoArgs() { roomInfo = new stAdventureRoomInfo(){roomId = roomId,  playersInfo = playersInfo }});
    }

    public void CallGameStart(bool gameStart)
    {
        OnGameStart?.Invoke(this, gameStart);
    }

    public void CallPositionChanged(stAdventurePlayerPositionFromSever playerPositions)
    {
        OnPositionChanged?.Invoke(this,
            new AdventureSceneEventPlayerPositionArgs() { playerPositions = playerPositions});
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

public class AdventureSceneEventPlayerPositionArgs : EventArgs
{
    public stAdventurePlayerPositionFromSever playerPositions;
}

