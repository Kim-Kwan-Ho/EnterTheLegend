using System;
using CustomizableCharacters;
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

    public void CallStatChanged(CharacterStat characterStat)
    {
        OnPlayerStatChanged?.Invoke(this, new LobbyScenePlayerStatChangedEventArgs() { characterStat = characterStat });
    }

    public void CallOnEquipChanged(EquipmentType type, EquipmentInfoSO equipmentInfo, CustomizationCategory category = null)
    {
        OnEquipChanged?.Invoke(this, new LobbySceneEquipChangedEventArgs() { type = type, equipmentInfo = equipmentInfo, category = category});
    }
}

public class LobbySceneInitializeArgs : EventArgs
{
    public stResponsePlayerData playerData;
}
public class LobbyScenePlayerStatChangedEventArgs : EventArgs
{
    public CharacterStat characterStat;
}

public class LobbySceneEquipChangedEventArgs : EventArgs
{
    public EquipmentType type;
    public EquipmentInfoSO equipmentInfo;
    public CustomizationCategory category;
}