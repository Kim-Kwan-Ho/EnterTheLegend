using System;
using StandardData;
using UnityEngine;

public class LobbySceneEvent : MonoBehaviour
{
    public Action<LobbySceneEvent, LobbySceneInitializeArgs> OnLobbyInitialize;
    public Action<LobbySceneEvent, LobbyScenePlayerStatChangedEventArgs> OnPlayerStatChanged;
    public Action<LobbySceneEvent, LobbySceneEquipChangedEventArgs> OnEquipChanged;


    public void CallLobbyInitialize(stResponsePlayerData playerData)
    {
        OnLobbyInitialize?.Invoke(this, new LobbySceneInitializeArgs() { playerData = playerData });
    }

    public void CallStatChanged(PlayerStat playerStat)
    {
        OnPlayerStatChanged?.Invoke(this, new LobbyScenePlayerStatChangedEventArgs() { playerStat = playerStat });
    }

    public void CallOnEquipChanged(EquipmentType type, EquipmentInfoSO equipmentInfo)
    {
        OnEquipChanged?.Invoke(this, new LobbySceneEquipChangedEventArgs() { type = type, equipmentInfo = equipmentInfo });
    }
}

public class LobbySceneInitializeArgs : EventArgs
{
    public stResponsePlayerData playerData;
}
public class LobbyScenePlayerStatChangedEventArgs : EventArgs
{
    public PlayerStat playerStat;
}

public class LobbySceneEquipChangedEventArgs : EventArgs
{
    public EquipmentType type;
    public EquipmentInfoSO equipmentInfo;
}