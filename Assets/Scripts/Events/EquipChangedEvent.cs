using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class EquipChangedEvent : MonoBehaviour
{
    public Action<EquipChangedEvent, EquipChangedEventArgs> OnEquipChanged;

    public void CallOnEquipChanged(EquipmentType type, [CanBeNull] EquipmentSO equipment)
    {
        OnEquipChanged?.Invoke(this, new EquipChangedEventArgs() {type = type, equipment = equipment });
    }


}

public class EquipChangedEventArgs : EventArgs
{
    public EquipmentType type;
    [CanBeNull] public EquipmentSO equipment;
}