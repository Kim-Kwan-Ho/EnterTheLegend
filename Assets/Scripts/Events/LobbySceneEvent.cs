using System;
using JetBrains.Annotations;
using StandardData;
using UnityEngine;

public class LobbySceneEvent : MonoBehaviour
{
    public Action<LobbySceneEvent, LobbySceneInitializeArgs> OnLobbyInitialize;
    public Action<LobbySceneEvent, LobbySceneEquipChangedArgs> OnEquipChanged;

    public void CallLobbyInitialize(stResponsePlayerData data)
    {
        OnLobbyInitialize?.Invoke(this, new LobbySceneInitializeArgs(){data = data});
    }
    public void CallOnEquipChanged(EquipmentType type, [CanBeNull] EquipmentSO equipment)
    {
        OnEquipChanged?.Invoke(this, new LobbySceneEquipChangedArgs() {type = type, equipment = equipment });
    }


}

public class LobbySceneEquipChangedArgs : EventArgs
{
    public EquipmentType type;
    [CanBeNull] public EquipmentSO equipment;
}

public class LobbySceneInitializeArgs : EventArgs
{
    public stResponsePlayerData data;
}