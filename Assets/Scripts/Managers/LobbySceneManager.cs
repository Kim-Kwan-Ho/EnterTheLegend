using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using StandardData;
using UnityEditor;
using UnityEngine;


[RequireComponent(typeof(LobbySceneEvent))]
public class LobbySceneManager : SingletonMonobehaviour<LobbySceneManager>
{
    [Header("Events")]
    public LobbySceneEvent EventLobbyScene;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        TestUpdate();
        EventLobbyScene.OnEquipChanged += Event_OnEquipChanged;
    }

    private void OnDestroy()
    {
        EventLobbyScene.OnEquipChanged -= Event_OnEquipChanged;
    }
    private void TestUpdate()
    {
        var items = Resources.LoadAll<EquipmentSO>("Items");
    }



    public void Event_OnEquipChanged(LobbySceneEvent lobbySceneEvent, LobbySceneEquipChangedArgs equipChangedEventArgs)
    {
        /*
         서버 통해서 DB에 전송
         */
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


