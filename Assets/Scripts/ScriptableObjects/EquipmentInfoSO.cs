using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Objects/Equipment/ItemInfo")]
public class EquipmentInfoSO : ScriptableObject // 아이템 정보 (로비씬) 
{
    public string Name;
    public string Description;
    public bool IsEquipped;
    public Sprite ItemIcon;
    public EquipmentSO Equipment;
}
