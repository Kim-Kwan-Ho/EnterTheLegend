using System.Collections.Generic;
using UnityEngine;

public class CharacterDisplay : BaseBehaviour
{
    [Header("Sprite Renderer")]
    [SerializeField]
    private SpriteRenderer _characterSpr;
    [SerializeField]
    private SpriteRenderer _weaponSpr;
    [SerializeField]
    private SpriteRenderer _helmetSpr;
    [SerializeField]
    private List<SpriteRenderer> _armorSprList;
    [SerializeField]
    private SpriteRenderer[] _shoesSprs = new SpriteRenderer[2];



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
        if (lobbySceneEquipChangedEventArgs.type == EquipmentType.Character)
        {
            if (lobbySceneEquipChangedEventArgs.equipmentInfo == null)
            {
                _characterSpr.sprite = null;
            }
            else
            {
                _characterSpr.sprite = lobbySceneEquipChangedEventArgs.equipmentInfo.Equipment.CharacterEquipmentSprite[0];
            }
        }
        else if (lobbySceneEquipChangedEventArgs.type == EquipmentType.Weapon)
        {
            if (lobbySceneEquipChangedEventArgs.equipmentInfo == null)
            {
                _weaponSpr.sprite = null;
            }
            else
            {
                _weaponSpr.transform.localPosition = lobbySceneEquipChangedEventArgs.equipmentInfo.Equipment.EquipmentSpriteOffSet;
                _weaponSpr.sprite = lobbySceneEquipChangedEventArgs.equipmentInfo.Equipment.CharacterEquipmentSprite[0];
            }
        }
        else if (lobbySceneEquipChangedEventArgs.type == EquipmentType.Helmet)
        {
            if (lobbySceneEquipChangedEventArgs.equipmentInfo == null)
            {
                _helmetSpr.sprite = null;
            }
            else
            {
                _helmetSpr.sprite = lobbySceneEquipChangedEventArgs.equipmentInfo.Equipment.CharacterEquipmentSprite[0];
            }
        }
        else if (lobbySceneEquipChangedEventArgs.type == EquipmentType.Armor)
        {
            if (lobbySceneEquipChangedEventArgs.equipmentInfo == null)
            {
                for (int i = 0; i < _armorSprList.Count; i++)
                {
                    _armorSprList[i].sprite = null;
                }
            }
            else
            {
                for (int i = 0; i < lobbySceneEquipChangedEventArgs.equipmentInfo.Equipment.CharacterEquipmentSprite.Count; i++)
                {
                    _armorSprList[i].sprite = lobbySceneEquipChangedEventArgs.equipmentInfo.Equipment.CharacterEquipmentSprite[i];
                }
                _armorSprList[^1].sprite = lobbySceneEquipChangedEventArgs.equipmentInfo.Equipment.CharacterEquipmentSprite[^1];
            }
        }
        else if (lobbySceneEquipChangedEventArgs.type == EquipmentType.Shoes)
        {
            if (lobbySceneEquipChangedEventArgs.equipmentInfo == null)
            {
                _shoesSprs[0].sprite = null;
                _shoesSprs[1].sprite = null;
            }
            else
            {
                _shoesSprs[0].sprite = lobbySceneEquipChangedEventArgs.equipmentInfo.Equipment.CharacterEquipmentSprite[0];
                _shoesSprs[1].sprite = lobbySceneEquipChangedEventArgs.equipmentInfo.Equipment.CharacterEquipmentSprite[0];
            }
        }
    }
    

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _characterSpr = FindGameObjectInChildren<SpriteRenderer>("Hair");
        _helmetSpr = FindGameObjectInChildren<SpriteRenderer>("Helmet");
        _weaponSpr = FindGameObjectInChildren<SpriteRenderer>("Weapon");
        _shoesSprs[0] = FindGameObjectInChildren<SpriteRenderer>("ShinL[Armor]");
        _shoesSprs[1] = FindGameObjectInChildren<SpriteRenderer>("ShinR[Armor]");
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _characterSpr);
        CheckNullValue(this.name, _helmetSpr);
        CheckNullValue(this.name, _weaponSpr);
        CheckNullValue(this.name, _shoesSprs);

    }

#endif
}
