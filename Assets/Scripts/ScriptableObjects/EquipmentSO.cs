using CustomizableCharacters;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Objects/Equipment/Item")]
public class EquipmentSO : ScriptableObject  // 아이템 정보 (게임씬)
{
    public int ItemId;
    public EquipmentType Type;
    public CustomizationData ItemCustomData;
    
    public ushort StatHp;
    public ushort StatAttack;
    public ushort StatDefense;
}
