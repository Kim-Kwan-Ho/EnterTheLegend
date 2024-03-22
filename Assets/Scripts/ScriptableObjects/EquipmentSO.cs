using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Objects/Equipment/Item")]
public class EquipmentSO : ScriptableObject  // 아이템 정보 (게임씬)
{
    public int ItemId;
    public EquipmentType Type;
    
    public Vector2 EquipmentSpriteOffSet;
    public List<Sprite> CharacterEquipmentSprite;
    
    public ushort StatHp;
    public ushort StatAttack;
    public ushort StatDefense;
    public ushort AttackDistance;
}
