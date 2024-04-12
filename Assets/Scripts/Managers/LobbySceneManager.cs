using StandardData;
using System.Runtime.InteropServices;
using UnityEngine;


[RequireComponent(typeof(LobbySceneEvent))]
public class LobbySceneManager : SingletonMonobehaviour<LobbySceneManager>
{
    [Header("Events")]
    public LobbySceneEvent EventLobbyScene;


    [Header("Player Property")]

    [SerializeField]
    private int _credit;
    public int Credit { get { return _credit; } }
    [SerializeField]
    private int _gold;
    public int Gold
    { get { return _gold; } }



    [Header("Player Stats")]
    private CharacterStat _characterStat = new CharacterStat();

    [Header("Character Equipments")]
    [SerializeField] private EquipmentInfoSO _characterEquip;
    [SerializeField] private EquipmentInfoSO _weaponEquip;
    [SerializeField] private EquipmentInfoSO _helmetEquip;
    [SerializeField] private EquipmentInfoSO _armorEquip;
    [SerializeField] private EquipmentInfoSO _shoesEquip;

    private bool _isInitialize;


    private void OnEnable()
    {
        _isInitialize = true;
        EventLobbyScene.OnLobbyInitialize += Event_LobbySceneInitialize;
        EventLobbyScene.OnEquipChanged += Event_EquipChanged;

    }

    private void OnDisable()
    {
        EventLobbyScene.OnLobbyInitialize -= Event_LobbySceneInitialize;
        EventLobbyScene.OnEquipChanged -= Event_EquipChanged;
    }



    private void Event_LobbySceneInitialize(LobbySceneEvent lobbySceneEvent, LobbySceneInitializeArgs lobbySceneInitializeArgs)
    {
        MyGameManager.Instance.SetPlayerNickname(lobbySceneInitializeArgs.playerData.Nickname);
        _credit = lobbySceneInitializeArgs.playerData.Credit;
        _gold = lobbySceneInitializeArgs.playerData.Gold;


        EquipmentInfoSO itemInfo = null;
        for (int i = 0; i < lobbySceneInitializeArgs.playerData.EquipedItems.Length; i++)
        {
            int equipType = lobbySceneInitializeArgs.playerData.EquipedItems[i];
            itemInfo = Utilities.ResourceLoader<EquipmentInfoSO>("ItemInfos", lobbySceneInitializeArgs.playerData.EquipedItems[i]);
            if (equipType == 0 || itemInfo == null)
                continue;

            EquipmentType type = EquipmentType.None;
            if (equipType < 20000)
            {
                type = EquipmentType.Character;
            }
            else if (equipType < 30000)
            {
                type = EquipmentType.Weapon;
            }
            else if (equipType < 40000)
            {
                type = EquipmentType.Helmet;
            }
            else if (equipType < 50000)
            {
                type = EquipmentType.Armor;
            }
            else if (equipType < 60000)
            {
                type = EquipmentType.Shoes;
            }
            EventLobbyScene.CallOnEquipChanged(type, itemInfo);
        }

        _isInitialize = false;
    }


    private void Event_EquipChanged(LobbySceneEvent lobbySceneEvent,
        LobbySceneEquipChangedEventArgs lobbySceneEquipChangedEventArgs)
    {
        stPlayerEquipChangedInfo equipChangedInfo = new stPlayerEquipChangedInfo();
        equipChangedInfo.Header.MsgID = MessageIdTcp.PlayerEquipChanged;
        equipChangedInfo.Header.PacketSize = (ushort)Marshal.SizeOf(equipChangedInfo);

        EquipmentInfoSO beforeItem = null;

        if (lobbySceneEquipChangedEventArgs.type == EquipmentType.Weapon)
        {
            beforeItem = _weaponEquip;
            _weaponEquip = lobbySceneEquipChangedEventArgs.equipmentInfo;
        }
        else if (lobbySceneEquipChangedEventArgs.type == EquipmentType.Helmet)
        {
            beforeItem = _helmetEquip;
            _helmetEquip = lobbySceneEquipChangedEventArgs.equipmentInfo;
        }
        else if (lobbySceneEquipChangedEventArgs.type == EquipmentType.Armor)
        {
            beforeItem = _armorEquip;
            _armorEquip = lobbySceneEquipChangedEventArgs.equipmentInfo;
        }
        else if (lobbySceneEquipChangedEventArgs.type == EquipmentType.Shoes)
        {
            beforeItem = _shoesEquip;
            _shoesEquip = lobbySceneEquipChangedEventArgs.equipmentInfo;
        }
        else if (lobbySceneEquipChangedEventArgs.type == EquipmentType.Character)
        {
            beforeItem = _characterEquip;
            _characterEquip = lobbySceneEquipChangedEventArgs.equipmentInfo;
        }

        if (beforeItem != null)
        {
            beforeItem.IsEquipped = false;
            _characterStat.ChangeStat(false, beforeItem.Equipment.StatHp, beforeItem.Equipment.StatAttack, beforeItem.Equipment.StatDefense, beforeItem.Equipment.AttackDistance);
            EventLobbyScene.CallStatChanged(_characterStat);
        }

        if (lobbySceneEquipChangedEventArgs.equipmentInfo == null)
            equipChangedInfo.AfterItem = 0;
        else
        {
            equipChangedInfo.AfterItem = lobbySceneEquipChangedEventArgs.equipmentInfo.Equipment.ItemId;
            lobbySceneEquipChangedEventArgs.equipmentInfo.IsEquipped = true;
            _characterStat.ChangeStat(true, lobbySceneEquipChangedEventArgs.equipmentInfo.Equipment.StatHp,
                lobbySceneEquipChangedEventArgs.equipmentInfo.Equipment.StatAttack, lobbySceneEquipChangedEventArgs.equipmentInfo.Equipment.StatDefense,
                lobbySceneEquipChangedEventArgs.equipmentInfo.Equipment.AttackDistance);
            EventLobbyScene.CallStatChanged(_characterStat);
        }

        if (_isInitialize)
            return;
        equipChangedInfo.ItemType = (ushort)lobbySceneEquipChangedEventArgs.type;
        byte[] msg = Utilities.GetObjectToByte(equipChangedInfo);
        ServerManager.Instance.EventClientToServer.CallOnTcpSend(msg);

    }

#if UNITY_EDITOR

    protected override void OnBindField()
    {
        base.OnBindField();
        EventLobbyScene = GetComponent<LobbySceneEvent>();
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, EventLobbyScene);
    }

#endif
}


