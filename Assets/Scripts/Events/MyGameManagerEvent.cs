using System;
using JetBrains.Annotations;
using StandardData;
using UnityEngine;

public class MyGameManagerEvent : MonoBehaviour
{
    public Action<MyGameManagerEvent, GameDataInitializeEventArgs> OnGameDataInitialize;
    public Action<MyGameManagerEvent, GameEquipChangedEventArgs> OnEquipChanged;


    public void CallDataInitialize(stResponsePlayerData data)
    {
        OnGameDataInitialize?.Invoke(this, new GameDataInitializeEventArgs() { data = data });
    }
    public void CallOnEquipChanged(EquipmentType type, [CanBeNull] EquipmentSO equipment)
    {
        OnEquipChanged?.Invoke(this, new GameEquipChangedEventArgs() { type = type, equipment = equipment });
    }

}


public class GameDataInitializeEventArgs : EventArgs
{
    public stResponsePlayerData data;
}
public class GameEquipChangedEventArgs : EventArgs
{
    public EquipmentType type;
    [CanBeNull] public EquipmentSO equipment;
}