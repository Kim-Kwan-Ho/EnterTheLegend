using UnityEngine;
using UnityEngine.UI;

public class Inventory : BaseBehaviour
{

    [Header("Toggles")]
    [SerializeField]
    private Toggle[] _toggles;

    [Header("Slots")]
    [SerializeField]
    private GameObject _characterSlot;
    [SerializeField]
    private GameObject _weaponSlot;
    [SerializeField]
    private GameObject _helmetSlot;
    [SerializeField]
    private GameObject _armorSlot;
    [SerializeField]
    private GameObject _shoesSlot;



    [Header("Item Info")]
    [SerializeField]
    private ItemInfo _itemInfo;
    [SerializeField]
    private Vector2 _infoOffSet;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject EquipmentItemPrefab;

    private void Awake()
    {
        SetToggles();
    }

    private void OnEnable()
    {
        LobbySceneManager.Instance.EventLobbyScene.OnLobbyInitialize += Event_LobbyInitialize;
    }

    private void OnDisable()
    {
        LobbySceneManager.Instance.EventLobbyScene.OnLobbyInitialize -= Event_LobbyInitialize;
    }


    private void SetToggles()
    {
        for (int i = 0; i < _toggles.Length; i++)
        {
            int c = i;
            _toggles[i].onValueChanged.AddListener((value) => _toggles[c].transform.GetChild(0).gameObject.SetActive(value));
            _toggles[i].onValueChanged.AddListener((value) => _itemInfo.gameObject.SetActive(false));
            _toggles[c].transform.GetChild(0).gameObject.SetActive(false);
        }
        _toggles[0].transform.GetChild(0).gameObject.SetActive(true);

    }
    private void Event_LobbyInitialize(LobbySceneEvent lobbySceneEvent,
        LobbySceneInitializeArgs lobbySceneInitializeArgs)
    {

        for (int i = 0; i < lobbySceneInitializeArgs.playerData.ItemCount; i++)
        {
            EquipmentInfoSO itemInfo = Utilities.ResourceLoader<EquipmentInfoSO>("ItemInfos", lobbySceneInitializeArgs.playerData.Items[i]);
            EquipmentItem item = Instantiate(EquipmentItemPrefab).GetComponent<EquipmentItem>();
            item.SetEquipmentItem(itemInfo);
            item.GetComponent<Button>().onClick.AddListener(() => OpenItemInfo(item.transform.position, itemInfo));
            if (itemInfo.Equipment.Type == EquipmentType.Character)
            {
                item.transform.SetParent(_characterSlot.transform);
            }
            else if (itemInfo.Equipment.Type == EquipmentType.Weapon)
            {
                item.transform.SetParent(_weaponSlot.transform);
            }
            else if (itemInfo.Equipment.Type == EquipmentType.Helmet)
            {
                item.transform.SetParent(_helmetSlot.transform);
            }
            else if (itemInfo.Equipment.Type == EquipmentType.Armor)
            {
                item.transform.SetParent(_armorSlot.transform);
            }
            else if (itemInfo.Equipment.Type == EquipmentType.Shoes)
            {
                item.transform.SetParent(_shoesSlot.transform);
            }
        }
    }

    private void OpenItemInfo(Vector2 position, EquipmentInfoSO itemInfo)
    {
        _itemInfo.SetItemInfo(position + _infoOffSet, itemInfo);
    }


#if UNITY_EDITOR

    protected override void OnBindField()
    {
        base.OnBindField();
        _toggles = GetComponentsInChildren<Toggle>();
        _characterSlot = GameObject.Find("CharacterSlot");
        _weaponSlot = GameObject.Find("WeaponSlot");
        _helmetSlot = GameObject.Find("HelmetSlot");
        _armorSlot = GameObject.Find("ArmorSlot");
        _shoesSlot = GameObject.Find("ShoesSlot");
        _itemInfo = GetComponentInChildren<ItemInfo>();
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _toggles);
        CheckNullValue(this.name, _characterSlot);
        CheckNullValue(this.name, _weaponSlot);
        CheckNullValue(this.name, _helmetSlot);
        CheckNullValue(this.name, _armorSlot);
        CheckNullValue(this.name, _shoesSlot);
        CheckNullValue(this.name, _itemInfo);
    }
#endif
}
