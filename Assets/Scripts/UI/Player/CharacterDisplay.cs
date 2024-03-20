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


    private void Awake()
    {
        MyGameManager.Instance.EventGameManager.OnEquipChanged += Event_EquipChanged;
    }
    private void Start()
    {
        LobbySceneManager.Instance.EventLobbyScene.OnLobbyInitialize += Event_LobbyInitialize;
        
    }

    private void OnDestroy()
    {
        MyGameManager.Instance.EventGameManager.OnEquipChanged -= Event_EquipChanged;
        LobbySceneManager.Instance.EventLobbyScene.OnLobbyInitialize -= Event_LobbyInitialize;
    }
    private void Event_EquipChanged(MyGameManagerEvent myGameManagerEvent, GameEquipChangedEventArgs gameEquipChangedEventArgs)
    {
        if (gameEquipChangedEventArgs.type == EquipmentType.Character)
        {
            if (gameEquipChangedEventArgs.equipment == null)
            {
                _characterSpr.sprite = null;
            }
            else
            {
                _characterSpr.sprite = gameEquipChangedEventArgs.equipment.CharacterEquipmentSprite[0];
            }
        }
        else if (gameEquipChangedEventArgs.type == EquipmentType.Weapon)
        {
            if (gameEquipChangedEventArgs.equipment == null)
            {
                _weaponSpr.sprite = null;
            }
            else
            {
                _weaponSpr.transform.localPosition = gameEquipChangedEventArgs.equipment.EquipmentSpriteOffSet;
                _weaponSpr.sprite = gameEquipChangedEventArgs.equipment.CharacterEquipmentSprite[0];
            }
        }
        else if (gameEquipChangedEventArgs.type == EquipmentType.Helmet)
        {
            if (gameEquipChangedEventArgs.equipment == null)
            {
                _helmetSpr.sprite = null;
            }
            else
            {
                _helmetSpr.sprite = gameEquipChangedEventArgs.equipment.CharacterEquipmentSprite[0];
            }
        }
        else if (gameEquipChangedEventArgs.type == EquipmentType.Armor)
        {
            if (gameEquipChangedEventArgs.equipment == null)
            {
                for (int i = 0; i < _armorSprList.Count; i++)
                {
                    _armorSprList[i].sprite = null;
                }
            }
            else
            {
                for (int i = 0; i < gameEquipChangedEventArgs.equipment.CharacterEquipmentSprite.Count; i++)
                {
                    _armorSprList[i].sprite = gameEquipChangedEventArgs.equipment.CharacterEquipmentSprite[i];
                }
                _armorSprList[^1].sprite = gameEquipChangedEventArgs.equipment.CharacterEquipmentSprite[^1];
            }
        }
        else if (gameEquipChangedEventArgs.type == EquipmentType.Shoes)
        {
            if (gameEquipChangedEventArgs.equipment == null)
            {
                _shoesSprs[0].sprite = null;
                _shoesSprs[1].sprite = null;
            }
            else
            {
                _shoesSprs[0].sprite = gameEquipChangedEventArgs.equipment.CharacterEquipmentSprite[0];
                _shoesSprs[1].sprite = gameEquipChangedEventArgs.equipment.CharacterEquipmentSprite[0];
            }
        }
    }

    private void Event_LobbyInitialize(LobbySceneEvent lobbySceneEvent,
        LobbySceneInitializeArgs lobbySceneInitializeArgs)
    {
        if (lobbySceneInitializeArgs.characterEquip != null)
        {
            _characterSpr.sprite = lobbySceneInitializeArgs.characterEquip.CharacterEquipmentSprite[0];
        }
        if (lobbySceneInitializeArgs.weaponEquip != null)
        {
            _weaponSpr.sprite = lobbySceneInitializeArgs.weaponEquip.CharacterEquipmentSprite[0];
            _weaponSpr.transform.localPosition = lobbySceneInitializeArgs.weaponEquip.EquipmentSpriteOffSet;
        }
        if (lobbySceneInitializeArgs.helmetEquip != null)
        {
            _helmetSpr.sprite = lobbySceneInitializeArgs.helmetEquip.CharacterEquipmentSprite[0];
        }
        if (lobbySceneInitializeArgs.shoesEquip != null)
        {
            _shoesSprs[0].sprite = lobbySceneInitializeArgs.shoesEquip.CharacterEquipmentSprite[0];
            _shoesSprs[1].sprite = lobbySceneInitializeArgs.shoesEquip.CharacterEquipmentSprite[0];
        }
        if (lobbySceneInitializeArgs.armorEquip != null)
        {
            for (int i = 0; i < lobbySceneInitializeArgs.armorEquip.CharacterEquipmentSprite.Count; i++)
            {
                _armorSprList[i].sprite = lobbySceneInitializeArgs.armorEquip.CharacterEquipmentSprite[i];
            }
            _armorSprList[^1].sprite = lobbySceneInitializeArgs.armorEquip.CharacterEquipmentSprite[^1];
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
