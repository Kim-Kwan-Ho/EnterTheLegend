using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : BaseBehaviour
{
    [Header("Slots")]
    [SerializeField] private GameObject _characterSlot;
    [SerializeField] private GameObject _weaponSlot;
    [SerializeField] private GameObject _helmetSlot;
    [SerializeField] private GameObject _armorSlot;
    [SerializeField] private GameObject _shoesSlot;
    [SerializeField] private ItemInfo _itemInfo;

    [Header("Prefabs")] 
    [SerializeField] 
    private GameObject EquipmentItemPrefab;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _itemInfo.SetItemInfo(Vector2.zero);
        }
    }

    public void SetInventory(EquipmentSO[] equipments)
    {
        for (int i = 0; i < equipments.Length; i++)
        {
            EquipmentItem item = Instantiate(EquipmentItemPrefab).GetComponent<EquipmentItem>();
            item.SetEquipmentItem(equipments[i]);
            if (equipments[i].Type == EquipmentType.Character)
            {
                item.transform.SetParent(_characterSlot.transform);
            }
            else if (equipments[i].Type == EquipmentType.Weapon)
            {
                item.transform.SetParent(_weaponSlot.transform);
            }
            else if (equipments[i].Type == EquipmentType.Helmet)
            {
                item.transform.SetParent(_helmetSlot.transform);
            }
            else if (equipments[i].Type == EquipmentType.Armor)
            {
                item.transform.SetParent(_armorSlot.transform);
            }
            else if (equipments[i].Type == EquipmentType.Shoes)
            {
                item.transform.SetParent(_shoesSlot.transform);
            }
        }

    }




#if UNITY_EDITOR

    protected override void OnBindField()
    {
        base.OnBindField();
        _characterSlot = GameObject.Find("CharacterSlot");
        _weaponSlot = GameObject.Find("WeaponSlot");
        _helmetSlot = GameObject.Find("HelmetSlot");
        _armorSlot = GameObject.Find("ArmorSlot");
        _shoesSlot = GameObject.Find("ShoesSlot");
        _itemInfo = GetComponentInChildren<ItemInfo>();
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _characterSlot);
        CheckNullValue(this.name, _weaponSlot);
        CheckNullValue(this.name, _helmetSlot);
        CheckNullValue(this.name, _armorSlot);
        CheckNullValue(this.name, _shoesSlot);
        CheckNullValue(this.name, _itemInfo);
    }
#endif
}
