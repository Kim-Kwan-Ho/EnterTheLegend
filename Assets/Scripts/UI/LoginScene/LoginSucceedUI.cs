using StandardData;
using System.Runtime.InteropServices;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LoginSucceedUI : BaseBehaviour
{
    [SerializeField]
    private Button _startButton;
    [SerializeField]
    private Button _logoutButton;

    private bool _isWaitingForRequest;
    private void Start()
    {
        _startButton.onClick.AddListener(GameStart);
        _logoutButton.onClick.AddListener(PlayerLogout);
    }

    private void OnEnable()
    {
        _isWaitingForRequest = false;
    }
    private void OnDisable()
    {
        _isWaitingForRequest = false;
    }
    private void GameStart()
    {
        //if (_isWaitingForRequest)
        //    return;

        _isWaitingForRequest = true;

        stRequestPlayerData request = new stRequestPlayerData();
        request.Header.MsgID = MessageIdTcp.RequestPlayerData;
        request.Header.PacketSize = (UInt16)Marshal.SizeOf(request);
        request.Id = ServerManager.Instance.ID;
        byte[] msg = Utilities.GetObjectToByte(request);
        ServerManager.Instance.EventClientToServer.CallOnTcpSend(msg);
    }

    private void PlayerLogout()
    {
        _isWaitingForRequest = true;
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
