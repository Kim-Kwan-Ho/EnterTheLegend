using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterChange : BaseBehaviour
{
    [SerializeField]
    private GameObject _characterChangePanel;
    [SerializeField]
    private Button _characterChangeButton;
    [SerializeField]
    private CharacterEquipments _characterEquipments;
    [SerializeField]
    private Inventory _inventory;

    private void Start()
    {
        LobbySceneManager.Instance.EventLobbyScene.OnLobbyInitialize += Event_LobbyInitialize;
        _characterChangeButton.onClick.AddListener(() => _characterChangePanel.SetActive(true));
        _characterChangePanel.SetActive(false);
    }

    private void OnDestroy()
    {
        LobbySceneManager.Instance.EventLobbyScene.OnLobbyInitialize += Event_LobbyInitialize;
    }

    private void Event_LobbyInitialize(LobbySceneEvent lobbySceneEvent,
        LobbySceneInitializeArgs lobbySceneInitializeArgs)
    {
        _characterEquipments.SetEquipment(lobbySceneInitializeArgs.weaponEquip, lobbySceneInitializeArgs.helmetEquip, lobbySceneInitializeArgs.armorEquip, lobbySceneInitializeArgs.shoesEquip);
        _inventory.SetInventory(lobbySceneInitializeArgs.ownedItems.ToArray());
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _characterChangePanel = GameObject.Find("CharacterChangePanel");
        _characterChangeButton = FindGameObjectInChildren<Button>("CharacterChangeButton");
        _characterEquipments = GetComponentInChildren<CharacterEquipments>();
        _inventory = GetComponentInChildren<Inventory>();
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _characterChangePanel);
        CheckNullValue(this.name, _characterChangeButton);
        CheckNullValue(this.name, _characterEquipments);
        CheckNullValue(this.name, _inventory);
    }
#endif
}
