#nullable enable
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
        LobbySceneManager.Instance.EventLobbyScene.OnEquipChanged += Event_EquipChanged;
    }

    private void OnDisable()
    {
        
        LobbySceneManager.Instance.EventLobbyScene.OnEquipChanged -= Event_EquipChanged;
    }


    private void Event_EquipChanged(LobbySceneEvent lobbySceneEvent, LobbySceneEquipChangedEventArgs lobbySceneEquipChangedEventArgs)
    {
        EquipmentType type = lobbySceneEquipChangedEventArgs.type;
        if (type == EquipmentType.Armor)
        {
            if (lobbySceneEquipChangedEventArgs.equipmentInfo == null)
            {
                _armorImage.sprite = _armorSprite;
            }
            else
            {
                _armorImage.sprite = lobbySceneEquipChangedEventArgs.equipmentInfo.ItemIcon;
            }
        }
        else if (type == EquipmentType.Weapon)
        {
            if (lobbySceneEquipChangedEventArgs.equipmentInfo == null)
            {
                _weaponImage.sprite = _weaponSprite;
            }
            else
            {
                _weaponImage.sprite = lobbySceneEquipChangedEventArgs.equipmentInfo.ItemIcon;
            }
        }
        else if (type == EquipmentType.Helmet)
        {
            if (lobbySceneEquipChangedEventArgs.equipmentInfo == null)
            {
                _helmetImage.sprite = _helmetSprite;
            }
            else
            {
                _helmetImage.sprite = lobbySceneEquipChangedEventArgs.equipmentInfo.ItemIcon;
            }
        }
        else if (type == EquipmentType.Shoes)
        {
            if (lobbySceneEquipChangedEventArgs.equipmentInfo == null)
            {
                _shoesImage.sprite = _shoesSprite;
            }
            else
            {
                _shoesImage.sprite = lobbySceneEquipChangedEventArgs.equipmentInfo.ItemIcon;
            }
        }
        else if (type == EquipmentType.Character)
        {
            if (lobbySceneEquipChangedEventArgs.equipmentInfo == null)
            {
                _characterImage.sprite = _characterSprite;
            }
            else
            {
                _characterImage.sprite = lobbySceneEquipChangedEventArgs.equipmentInfo.ItemIcon;
            }
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
