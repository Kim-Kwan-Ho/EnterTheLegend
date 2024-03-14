using StandardData;
using UnityEngine;
using System.Collections.Generic;

public class MyGameManager : SingletonMonobehaviour<MyGameManager>
{
    [SerializeField] private string _nickname = "Client_1";
    public string Nickname { get { return _nickname; } }

    [SerializeField] private int _credit;

    public int Credit { get { return _credit; } }
    [SerializeField] private int _gold;
    public int Gold { get { return _gold; } }

    [SerializeField]
    private EquipmentSO _characterEquip;
    [SerializeField]
    private EquipmentSO _weaponEquip;
    [SerializeField]
    private EquipmentSO _helmetEquip;
    [SerializeField]
    private EquipmentSO _armorEquip;
    [SerializeField]
    private EquipmentSO _shoesEquip;

    private Dictionary<int, EquipmentSO> _items = new Dictionary<int, EquipmentSO>();
    private List<int> _ownedItems;
    


    protected override void Awake()
    {
        base.Awake();
        DontDestroyGameObject();
    }

    private void Start()
    {
        var items = Resources.LoadAll<EquipmentSO>("Items");
        foreach (var item in items)
        {
            _items[item.ItemId] = item;
        }
        LobbySceneManager.Instance.EventLobbyScene.OnLobbyInitialize += Event_LobbySceneInitialize;
    }

    private void OnDestroy()
    {
        LobbySceneManager.Instance.EventLobbyScene.OnLobbyInitialize -= Event_LobbySceneInitialize;
    }
    private void Event_LobbySceneInitialize(LobbySceneEvent lobbySceneEvent,
        LobbySceneInitializeArgs lobbySceneInitializeArgs)
    {
        _nickname = lobbySceneInitializeArgs.data.Nickname;
        _credit = lobbySceneInitializeArgs.data.Credit;
        _gold = lobbySceneInitializeArgs.data.Gold;
        _ownedItems = new List<int>(lobbySceneInitializeArgs.data.ItemCount);
        for (int i = 0; i < lobbySceneInitializeArgs.data.ItemCount; i++)
        {
            _ownedItems.Add(lobbySceneInitializeArgs.data.Items[i]);
        }

        for (int i = 0; i < lobbySceneInitializeArgs.data.EquipedItems.Length; i++)
        {
            switch (lobbySceneInitializeArgs.data.EquipedItems[i])
            {
                case 0:
                    break;
                case < 20000:
                    _characterEquip = _items[lobbySceneInitializeArgs.data.EquipedItems[i]];
                    break;
                case < 30000:
                    _weaponEquip = _items[lobbySceneInitializeArgs.data.EquipedItems[i]];
                    break;
                case < 40000:
                    _helmetEquip = _items[lobbySceneInitializeArgs.data.EquipedItems[i]];
                    break;
                case < 50000:
                    _armorEquip = _items[lobbySceneInitializeArgs.data.EquipedItems[i]];
                    break;
                case < 60000:
                    _shoesEquip = _items[lobbySceneInitializeArgs.data.EquipedItems[i]];
                    break;

            }
        }
    }
    private GameRoomType _roomType;
    public GameRoomType RoomType
    { get { return _roomType; } }


    public void SetGameRoomType(GameRoomType roomType)
    {
        this._roomType = roomType;
    }


}
