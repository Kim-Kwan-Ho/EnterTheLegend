using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class LobbySceneEvent : MonoBehaviour
{
    public Action<LobbySceneEvent, LobbySceneInitializeArgs> OnLobbyInitialize;

    public void CallLobbyInitialize(string nickname, int credit, int gold, [CanBeNull] EquipmentSO characterEquip,
        [CanBeNull] EquipmentSO weaponEquip, [CanBeNull] EquipmentSO helmetEquip, [CanBeNull] EquipmentSO armorEquip,
        [CanBeNull] EquipmentSO shoesEquip, List<EquipmentSO> ownedItems)
    {
        OnLobbyInitialize?.Invoke(this,
            new LobbySceneInitializeArgs()
            {
                nickname = nickname, credit = credit, gold = gold, characterEquip = characterEquip,
                weaponEquip = weaponEquip,
                helmetEquip = helmetEquip, armorEquip = armorEquip, shoesEquip = shoesEquip, ownedItems = ownedItems
            });
    }

}



public class LobbySceneInitializeArgs : EventArgs
{
    public string nickname;
    public int credit;
    public int gold;
    [CanBeNull] public EquipmentSO characterEquip;
    [CanBeNull] public EquipmentSO weaponEquip;
    [CanBeNull] public EquipmentSO helmetEquip;
    [CanBeNull] public EquipmentSO armorEquip;
    [CanBeNull] public EquipmentSO shoesEquip;
    public List<EquipmentSO> ownedItems;

}