using CustomizableCharacters;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Objects/Equipment/Item")]
public class EquipmentSO : ScriptableObject  // ������ ���� (���Ӿ�)
{
    public int ItemId;
    public EquipmentType Type;
    public CustomizationData ItemCustomData;
    
    public ushort StatHp;
    public ushort StatAttack;
    public ushort StatDefense;
}
