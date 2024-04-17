using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Equipment/Items/WeaponItem")]
public class WeaponEquipmentSO : EquipmentSO
{
    [Header("Weapon")]
    public WeaponSkillSO WeaponSkill;
    public WeaponType WeaponType;
}
