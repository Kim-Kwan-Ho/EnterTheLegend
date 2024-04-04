using StandardData;
using System;
using UnityEngine;

public class BattleSceneEvent : MonoBehaviour
{
    public Action<BattleSceneEvent, BattleSceneEventRoomInfoArgs> OnRoomInitialize;
    public Action<BattleSceneEvent, bool> OnGameStart;
    public Action<BattleSceneEvent, BattleScenePlayerPositionChangedEventArgs> OnPlayerPositionChanged;
    public Action<BattleSceneEvent, BattleSceneEventPlayerStateChangedEventArgs> OnPlayerStateChanged;
    public Action<BattleSceneEvent, BattleScenePlayerDirectionChangedEventArgs> OnPlayerDirectionChanged;
    public void CallRoomInitialize(ushort roomId, ushort playerIndex, stBattlePlayerInfo[] playersInfo)
    {
        OnRoomInitialize?.Invoke(this,
            new BattleSceneEventRoomInfoArgs() { roomInfo = new stBattleRoomInfo() { roomId = roomId, playerIndex = playerIndex, playersInfo = playersInfo } });
    }

    public void CallGameStart(bool gameStart)
    {
        OnGameStart?.Invoke(this, gameStart);
    }

    public void CallPlayerPositionChanged(stBattlePlayerPositionFromSever playerPositions)
    {
        OnPlayerPositionChanged?.Invoke(this,
            new BattleScenePlayerPositionChangedEventArgs() { playerPositions = playerPositions });
    }

    public void CallPlayerStateChanged(ushort playerIndex, State state)
    {
        OnPlayerStateChanged?.Invoke(this,
            new BattleSceneEventPlayerStateChangedEventArgs() { playerIndex = playerIndex, state = state });
    }
    public void CallPlayerDirectionChanged(ushort playerIndex, Direction direction)
    {
        OnPlayerDirectionChanged?.Invoke(this,
            new BattleScenePlayerDirectionChangedEventArgs() { playerIndex = playerIndex, direction = direction });
    }
}


public class BattleSceneEventRoomInfoArgs : EventArgs
{
    public stBattleRoomInfo roomInfo;
}
public struct stBattleRoomInfo
{
    public ushort roomId;
    public ushort playerIndex;
    public stBattlePlayerInfo[] playersInfo;
}

public class BattleScenePlayerPositionChangedEventArgs : EventArgs
{
    public stBattlePlayerPositionFromSever playerPositions;
}
public class BattleSceneEventPlayerStateChangedEventArgs : EventArgs
{
    public ushort playerIndex;
    public State state;
}

public class BattleScenePlayerDirectionChangedEventArgs : EventArgs
{
    public ushort playerIndex;
    public Direction direction;
}