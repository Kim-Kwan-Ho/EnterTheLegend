using UnityEngine;
using UnityEngine.UI;

public class CharacterChange : BaseBehaviour
{
    [Header("Buttons")]
    [SerializeField]
    private Button _characterChangeButton;
    [SerializeField]
    private Button _closeButton;


    [Header("Character & Stat &Equipment Change")]
    [SerializeField]
    private GameObject _characterChangePanel;
    [SerializeField]
    private CharacterEquipments _characterEquipments;
    [SerializeField]
    private CharacterStat _characterStat;
    [SerializeField]
    private Inventory _inventory;

    private void Start()
    {
        LobbySceneManager.Instance.EventLobbyScene.OnLobbyInitialize += Event_LobbyInitialize;
        _characterChangeButton.onClick.AddListener(() => _characterChangePanel.SetActive(true));
        _closeButton.onClick.AddListener(()=> _characterChangePanel.SetActive(false));
        _characterChangePanel.SetActive(false);
    }

    private void OnDestroy()
    {
        LobbySceneManager.Instance.EventLobbyScene.OnLobbyInitialize += Event_LobbyInitialize;
    }

    private void Event_LobbyInitialize(LobbySceneEvent lobbySceneEvent,
        LobbySceneInitializeArgs lobbySceneInitializeArgs)
    {
        _characterEquipments.SetEquipment(lobbySceneInitializeArgs.characterEquip,lobbySceneInitializeArgs.weaponEquip, lobbySceneInitializeArgs.helmetEquip, lobbySceneInitializeArgs.armorEquip, lobbySceneInitializeArgs.shoesEquip);
        _inventory.SetInventory(lobbySceneInitializeArgs.ownedItems.ToArray());
        _characterStat.SetStat(lobbySceneInitializeArgs);
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _characterChangePanel = GameObject.Find("CharacterChangePanel");
        _characterChangeButton = FindGameObjectInChildren<Button>("CharacterChangeButton");
        _characterEquipments = GetComponentInChildren<CharacterEquipments>();
        _characterStat = GetComponentInChildren<CharacterStat>();
        _inventory = GetComponentInChildren<Inventory>();
        _closeButton = FindGameObjectInChildren<Button>("CloseButton");
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _characterChangePanel);
        CheckNullValue(this.name, _characterChangeButton);
        CheckNullValue(this.name, _characterEquipments);
        CheckNullValue(this.name, _characterStat);
        CheckNullValue(this.name, _inventory);
    }
#endif
}
