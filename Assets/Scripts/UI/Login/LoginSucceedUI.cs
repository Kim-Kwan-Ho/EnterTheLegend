using StandardData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class LoginSucceedUI : BaseBehaviour
{
    [SerializeField]
    private Button _startButton;
    [SerializeField]
    private Button _logoutButton;

    private void Start()
    {
        _startButton.onClick.AddListener(GameStart);
        _logoutButton.onClick.AddListener(PlayerLogout);
    }
    private void GameStart()
    {
        MySceneManager.Instance.EventSceneChanged.CallSceneChanged("LobbyScene", null, true, 3f);
    }

    private void PlayerLogout()
    {
        PlayerPrefs.DeleteAll();
        LoginSceneManager.Instance.EventLoginScene.CallLogout();
        gameObject.SetActive(false);
    }
    



#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _startButton = FindGameObjectInChildren<Button>("StartButton");
        _logoutButton = FindGameObjectInChildren<Button>("LogoutButton");
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _startButton);
        CheckNullValue(this.name, _logoutButton);
    }

#endif


}
