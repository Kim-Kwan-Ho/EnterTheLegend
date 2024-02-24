using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentItem : BaseBehaviour
{
    [SerializeField]
    private Image _itemImage;
    private EquipmentSO _equipment;


    public void SetEquipmentItem(EquipmentSO equipment)
    {
        _equipment = equipment;
        _itemImage.sprite = _equipment.ItemSprite;
    }



#if UNITY_EDITOR

    protected override void OnBindField()
    {
        base.OnBindField();
        _itemImage = GetComponentInChildrenExceptThis<Image>();
    }
    
    private void OnValidate()
    {
        CheckNullValue(this.name,_itemImage); 
    }

#endif
}
