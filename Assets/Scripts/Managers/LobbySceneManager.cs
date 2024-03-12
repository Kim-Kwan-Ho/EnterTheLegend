using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using StandardData;
using UnityEditor;
using UnityEngine;


[RequireComponent(typeof(EquipChangedEvent))]
public class LobbySceneManager : SingletonMonobehaviour<LobbySceneManager>
{

    [SerializeField] 
    private Inventory _inventory;


    [Header("Events")]
    public EquipChangedEvent EventEquipChanged;

    protected override void Awake()
    {
        base.Awake();
        GetPlayerData();
    }

    private void Start()
    {
        TestUpdate();
        EventEquipChanged.OnEquipChanged += Event_OnEquipChanged;
    }

    private void OnDestroy()
    {
        EventEquipChanged.OnEquipChanged -= Event_OnEquipChanged;
    }
    private void TestUpdate()
    {
        var items = Resources.LoadAll<EquipmentSO>("Items");
        _inventory.SetInventory(items);
    }

    private void GetPlayerData()
    {
        stRequestPlayerData request = new stRequestPlayerData();
        request.Header.MsgID = MessageIdTcp.RequestPlayerData;
        request.Header.PacketSize = (UInt16)Marshal.SizeOf(request);
        request.Id = ServerManager.Instance.ID;
        byte[] msg = Utilities.GetObjectToByte(request);
        ServerManager.Instance.EventClientToServer.CallOnTcpSend(msg);
    }


    public void Event_OnEquipChanged(EquipChangedEvent equipChangedEvent, EquipChangedEventArgs equipChangedEventArgs)
    {
        /*
         서버 통해서 DB에 전송
         */
    }

#if UNITY_EDITOR

    protected override void OnBindField()
    {
        base.OnBindField();
        _inventory = GameObject.FindObjectOfType<Inventory>();
        EventEquipChanged = GetComponent<EquipChangedEvent>();
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _inventory);
        CheckNullValue(this.name, EventEquipChanged);
    }

#endif
}


