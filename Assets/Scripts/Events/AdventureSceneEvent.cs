using System;
using System.Collections;
using System.Collections.Generic;
using StandardData;
using UnityEngine;

public class AdventureSceneEvent : MonoBehaviour
{
    public Action<AdventureSceneEvent, AdventureSceneEventRoomInfoArgs> OnRoomInitialize;
    public Action<AdventureSceneEvent, bool> OnGameStart;
    public Action<AdventureSceneEvent, AdventureSceneEventPlayerPositionArgs> OnPositionChanged;

    public void CallRoomInitialize(ushort roomId, ushort playerIndex ,stAdventurePlayerInfo[] playersInfo)
    {
        OnRoomInitialize?.Invoke(this,
            new AdventureSceneEventRoomInfoArgs() { roomInfo = new stAdventureRoomInfo(){roomId = roomId, playerIndex = playerIndex, playersInfo = playersInfo }});
    }

    public void CallGameStart(bool gameStart)
    {
        OnGameStart?.Invoke(this, gameStart);
    }

    public void CallPositionChanged(ushort index, Vector2 position)
    {
        OnPositionChanged?.Invoke(this,
            new AdventureSceneEventPlayerPositionArgs()
                { playerPosition = new stAdventurePlayerPosition() { index = index, position = position } });
    }


}

public class AdventureSceneEventRoomInfoArgs : EventArgs
{
    public stAdventureRoomInfo roomInfo;
    
}
public struct stAdventureRoomInfo
{
    public ushort roomId;
    public ushort playerIndex;
    public stAdventurePlayerInfo[] playersInfo;
}

public class AdventureSceneEventPlayerPositionArgs : EventArgs
{
    public stAdventurePlayerPosition playerPosition;
}

public struct stAdventurePlayerPosition
{
    public ushort index;
    public Vector2 position;
}
