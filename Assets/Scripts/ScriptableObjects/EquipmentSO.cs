using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Equipment_", menuName = "Scriptable Objects/Equipment")]
public class EquipmentSO : ScriptableObject
{
    public int ItemId;
    public bool IsEquipped;
    public Sprite ItemSprite;
    public Sprite CharacterEquipmentSprite;
    public EquipmentType Type;
    public string Name;
    public string Description;
    public int StatHp;
    public int StatAtk;
    public int StatDef;
    public int StatSpd;


}
