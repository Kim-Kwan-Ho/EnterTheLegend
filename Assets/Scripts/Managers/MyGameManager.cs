using StandardData;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using JetBrains.Annotations;


[RequireComponent(typeof(MyGameManagerEvent))]
public class MyGameManager : SingletonMonobehaviour<MyGameManager>
{
    [Header("Events")]
    [SerializeField]
    public MyGameManagerEvent EventGameManager;


    [Header("Property")]
    [SerializeField]
    private string _nickname = "Client_1";
    public string Nickname { get { return _nickname; } }
    [SerializeField]
    private int _credit;
    public int Credit { get { return _credit; } }
    [SerializeField]
    private int _gold;
    public int Gold
    { get { return _gold; } }


    [Header("Player Stats")]
    private PlayerStat _playerStat = new PlayerStat();

    [Header("Character Equipments")]
    [SerializeField][CanBeNull] private EquipmentSO _characterEquip;
    [SerializeField][CanBeNull] private EquipmentSO _weaponEquip;
    [SerializeField][CanBeNull] private EquipmentSO _helmetEquip;
    [SerializeField][CanBeNull] private EquipmentSO _armorEquip;
    [SerializeField][CanBeNull] private EquipmentSO _shoesEquip;

    [SerializeField]
    private Dictionary<int, EquipmentSO> _items = new Dictionary<int, EquipmentSO>();
    [SerializeField]
    private List<EquipmentSO> _ownedItems;



    protected override void Awake()
    {
        base.Awake();
        DontDestroyGameObject();
        var items = Resources.LoadAll<EquipmentSO>("Items");
        foreach (var item in items)
        {
            _items[item.ItemId] = item;
        }

    }
    private void OnEnable()
    {
        EventGameManager.OnGameDataInitialize += Event_PlayerDataInitialize;
        EventGameManager.OnEquipChanged += Event_EquipChanged;
    }

    private void Start()
    {
        LoginSceneManager.Instance.EventLoginScene.OnLogout += Event_Logout;
    }



    private void OnDisable()
    {
        EventGameManager.OnGameDataInitialize -= Event_PlayerDataInitialize;
        EventGameManager.OnEquipChanged -= Event_EquipChanged;
    }
    private void OnDestroy()
    {
        LoginSceneManager.Instance.EventLoginScene.OnLogout -= Event_Logout;
    }
    private void Event_PlayerDataInitialize(MyGameManagerEvent gameManagerEvent,
        GameDataInitializeEventArgs gameDataInitializeEventArgs)
    {
        _nickname = gameDataInitializeEventArgs.data.Nickname;
        _credit = gameDataInitializeEventArgs.data.Credit;
        _gold = gameDataInitializeEventArgs.data.Gold;
        _ownedItems = new List<EquipmentSO>(gameDataInitializeEventArgs.data.ItemCount);

        for (int i = 0; i < gameDataInitializeEventArgs.data.ItemCount; i++)
        {
            _ownedItems.Add(_items[gameDataInitializeEventArgs.data.Items[i]]);
        }

        for (int i = 0; i < gameDataInitializeEventArgs.data.EquipedItems.Length; i++)
        {
            int equipType = gameDataInitializeEventArgs.data.EquipedItems[i];
            if (equipType == 0)
                continue;

            if (equipType < 20000)
            {
                _characterEquip = _items[gameDataInitializeEventArgs.data.EquipedItems[i]];
            }
            else if (equipType < 30000)
            {
                _weaponEquip = _items[gameDataInitializeEventArgs.data.EquipedItems[i]];
            }
            else if (equipType < 40000)
            {
                _helmetEquip = _items[gameDataInitializeEventArgs.data.EquipedItems[i]];
            }
            else if (equipType < 50000)
            {
                _armorEquip = _items[gameDataInitializeEventArgs.data.EquipedItems[i]];
            }
            else if (equipType < 60000)
            {
                _shoesEquip = _items[gameDataInitializeEventArgs.data.EquipedItems[i]];
            }
            _items[gameDataInitializeEventArgs.data.EquipedItems[i]].IsEquipped = true;

            var item = _items[gameDataInitializeEventArgs.data.EquipedItems[i]];
            _playerStat.ChangeStat(true, item.StatHp, item.StatAttack, item.StatDefense, item.AttackDistance);
            EventGameManager.CallStatChanged(_playerStat);
        }
    }

    private void Event_EquipChanged(MyGameManagerEvent myGameManagerEvent,
        GameEquipChangedEventArgs equipChangedEventArgs)
    {
        stPlayerEquipChangedInfo equipChangedInfo = new stPlayerEquipChangedInfo();
        equipChangedInfo.Header.MsgID = MessageIdTcp.PlayerEquipChanged;
        equipChangedInfo.Header.PacketSize = (ushort)Marshal.SizeOf(equipChangedInfo);

        EquipmentSO beforeItem = new EquipmentSO();


        if (equipChangedEventArgs.type == EquipmentType.Weapon)
        {
            beforeItem = _weaponEquip;
            _weaponEquip = equipChangedEventArgs.equipment;
        }
        else if (equipChangedEventArgs.type == EquipmentType.Helmet)
        {
            beforeItem = _helmetEquip;
            _helmetEquip = equipChangedEventArgs.equipment;
        }
        else if (equipChangedEventArgs.type == EquipmentType.Armor)
        {
            beforeItem = _armorEquip;
            _armorEquip = equipChangedEventArgs.equipment;
        }
        else if (equipChangedEventArgs.type == EquipmentType.Shoes)
        {
            beforeItem = _shoesEquip;
            _shoesEquip = equipChangedEventArgs.equipment;
        }
        else if (equipChangedEventArgs.type == EquipmentType.Character)
        {
            beforeItem = _characterEquip;
            _characterEquip = equipChangedEventArgs.equipment;
        }

        if (beforeItem != null)
        {
            beforeItem.IsEquipped = false;
            _playerStat.ChangeStat(false, beforeItem.StatHp, beforeItem.StatAttack, beforeItem.StatDefense, beforeItem.AttackDistance);
            EventGameManager.CallStatChanged(_playerStat);
        }

        if (equipChangedEventArgs.equipment == null)
            equipChangedInfo.AfterItem = 0;
        else
        {
            equipChangedInfo.AfterItem = equipChangedEventArgs.equipment.ItemId;
            var afterItem = equipChangedEventArgs.equipment;
            _playerStat.ChangeStat(true, afterItem.StatHp, afterItem.StatAttack, afterItem.StatDefense, afterItem.AttackDistance);
            EventGameManager.CallStatChanged(_playerStat);
        }

        equipChangedInfo.ItemType = (ushort)equipChangedEventArgs.type;
        byte[] msg = Utilities.GetObjectToByte(equipChangedInfo);
        ServerManager.Instance.EventClientToServer.CallOnTcpSend(msg);

    }

    private void Event_Logout(LoginSceneEvents loginSceneEvents,
        LoginSceneEventLogoutArgs loginSceneEventLogoutArgs)
    {
        ResetPlayerInfo();
    }

    private void ResetPlayerInfo()
    {
        _nickname = "";
        _gold = 0;
        _credit = 0;
        _weaponEquip = null;
        _characterEquip = null;
        _helmetEquip = null;
        _armorEquip = null;
        _shoesEquip = null;
        _ownedItems = null;
    }

    public void InitializeLobbyScene()
    {
        LobbySceneManager.Instance.EventLobbyScene.CallLobbyInitialize(_nickname, _credit, _gold, _characterEquip,
            _weaponEquip, _helmetEquip, _armorEquip, _shoesEquip, _ownedItems);
    }



    private GameRoomType _roomType;
    public GameRoomType RoomType
    { get { return _roomType; } }


    public void SetGameRoomType(GameRoomType roomType)
    {
        this._roomType = roomType;
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        EventGameManager = GetComponent<MyGameManagerEvent>();
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, EventGameManager);
    }
#endif
}
