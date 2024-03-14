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
        stRequestPlayerData request = new stRequestPlayerData();
        request.Header.MsgID = MessageIdTcp.RequestPlayerData;
        request.Header.PacketSize = (UInt16)Marshal.SizeOf(request);
        request.Id = ServerManager.Instance.ID;
        byte[] msg = Utilities.GetObjectToByte(request);
        ServerManager.Instance.EventClientToServer.CallOnTcpSend(msg);
    }

    private void PlayerLogout()
    {
        PlayerPrefs.DeleteAll();
        LoginSceneManager.Instance.EventLoginScene.CallLoginFailed();
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
