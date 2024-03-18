using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class CharacterEquipments : BaseBehaviour
{
    [Header("Equipments Images")]
    [SerializeField]
    private Image _characterImage;
    [SerializeField]
    private Image _helmetImage;
    [SerializeField]
    private Image _armorImage;
    [SerializeField]
    private Image _weaponImage;
    [SerializeField]
    private Image _shoesImage;

    [Header("Equipments Unequiped Sprites")]
    [SerializeField]
    private Sprite _characterSprite;
    [SerializeField]
    private Sprite _helmetSprite;
    [SerializeField]
    private Sprite _armorSprite;
    [SerializeField]
    private Sprite _weaponSprite;
    [SerializeField]
    private Sprite _shoesSprite;

    private void OnEnable()
    {
        MyGameManager.Instance.EventGameManager.OnEquipChanged += Event_EquipChanged;
    }

    private void OnDisable()
    {
        MyGameManager.Instance.EventGameManager.OnEquipChanged -= Event_EquipChanged;
    }

    public void SetEquipment([CanBeNull] EquipmentSO weaponEquip, [CanBeNull] EquipmentSO helmetEquip, [CanBeNull] EquipmentSO armorEquip, [CanBeNull] EquipmentSO shoesEquip)
    {
        if (weaponEquip == null)
        {
            _weaponImage.sprite = _weaponSprite;
        }
        else
        {
            _weaponImage.sprite = weaponEquip.ItemSprite;
        }

        if (helmetEquip == null)
        {
            _helmetImage.sprite = _helmetSprite;
        }
        else
        {
            _helmetImage.sprite = helmetEquip.ItemSprite;
        }

        if (armorEquip == null)
        {
            _armorImage.sprite = _armorSprite;
        }
        else
        {
            _armorImage.sprite = armorEquip.ItemSprite;
        }

        if (shoesEquip == null)
        {
            _shoesImage.sprite = _shoesSprite;
        }
        else
        {
            _shoesImage.sprite = shoesEquip.ItemSprite;
        }
        
    }

    private void Event_EquipChanged(MyGameManagerEvent myGameManagerEvent, GameEquipChangedEventArgs equipChangedEventArgs)
    {
        EquipmentType type = equipChangedEventArgs.type;
        if (type == EquipmentType.Armor)
        {
            if (equipChangedEventArgs.equipment == null)
            {
                _armorImage.sprite = _armorSprite;
            }
            else
            {
                _armorImage.sprite = equipChangedEventArgs.equipment.ItemSprite;
            }
        }
        else if (type == EquipmentType.Weapon)
        {
            if (equipChangedEventArgs.equipment == null)
            {
                _weaponImage.sprite = _weaponSprite;
            }
            else
            {
                _weaponImage.sprite = equipChangedEventArgs.equipment.ItemSprite;
            }
        }
        else if (type == EquipmentType.Helmet)
        {
            if (equipChangedEventArgs.equipment == null)
            {
                _helmetImage.sprite = _helmetSprite;
            }
            else
            {
                _helmetImage.sprite = equipChangedEventArgs.equipment.ItemSprite;
            }
        }
        else if (type == EquipmentType.Shoes)
        {
            if (equipChangedEventArgs.equipment == null)
            {
                _shoesImage.sprite = _shoesSprite;
            }
            else
            {
                _shoesImage.sprite = equipChangedEventArgs.equipment.ItemSprite;
            }
        }
        else if (type == EquipmentType.Character)
        {

        }

    }





#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();

        _characterImage = FindGameObjectInChildren<Image>("CharacterImage");
        _helmetImage = FindGameObjectInChildren<Image>("HelmetImage");
        _armorImage = FindGameObjectInChildren<Image>("ArmorImage");
        _weaponImage = FindGameObjectInChildren<Image>("WeaponImage");
        _shoesImage = FindGameObjectInChildren<Image>("ShoesImage");
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _characterImage);
        CheckNullValue(this.name, _helmetImage);
        CheckNullValue(this.name, _armorImage);
        CheckNullValue(this.name, _weaponImage);
        CheckNullValue(this.name, _shoesImage);

        CheckNullValue(this.name, _characterSprite);
        CheckNullValue(this.name, _helmetSprite);
        CheckNullValue(this.name, _armorSprite);
        CheckNullValue(this.name, _weaponSprite);
        CheckNullValue(this.name, _shoesSprite);
    }


#endif
}
