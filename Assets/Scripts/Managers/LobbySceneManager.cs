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
    }

    private void OnDestroy()
    {
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


