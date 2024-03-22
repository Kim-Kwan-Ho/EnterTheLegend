using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Objects/Equipment/ItemInfo")]
public class EquipmentInfoSO : ScriptableObject // ������ ���� (�κ��) 
{
    public string Name;
    public string Description;
    public bool IsEquipped;
    public Sprite ItemIcon;
    public EquipmentSO Equipment;
}
