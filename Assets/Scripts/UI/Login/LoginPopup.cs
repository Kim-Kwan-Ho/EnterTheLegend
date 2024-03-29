using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using Assets.FantasyMonsters.Scripts.EditorScripts;

public class LoginPopup : UIPopup, IPlayfabDataHandler
{
    [Header("InputFields")]
    [SerializeField]
    private TMP_InputField _idInputField;
    [SerializeField]
    private TMP_InputField _passwordInputField;

    [Header("Buttons")]
    [SerializeField]
    private Button _loginButton;
    [SerializeField]
    private Button _registerButton;
    [SerializeField]
    private Button _guestButton;
    [SerializeField]
    private Button _facebookButton;
    [SerializeField]
    private Button _googleButton;
    [SerializeField]
    private Button _closeButton;


    [Header("Player Info")]
    private string _id;
    private string _password;


    private bool _waitingResponse = false;

    private void Awake()
    {
        _closeButton.onClick.AddListener(ClosePopup);
        _loginButton.onClick.AddListener(OnLogin);
        _registerButton.onClick.AddListener(OnRegister);
        _guestButton.onClick.AddListener(OnGuestLogin);
    }


    private void OnLogin()
    {
        if (_waitingResponse)
            return;

        _waitingResponse = true;
        _id = _idInputField.text;
        _password = _passwordInputField.text;
        var request = new LoginWithPlayFabRequest
        { Username = _id, Password = _password };
        PlayerPrefs.SetInt("PlayerInfo", (int)LoginPlatform.PlayFabAccount);
        PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSucceed, CallErrorReport);

    }

    private void OnLoginSucceed(LoginResult result)
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), GetIpPortAddress, CallErrorReport);
        PlayerPrefs.SetString("PlayerId", _id);
        PlayerPrefs.SetString("PlayerPassword", _password);
        _waitingResponse = false;
    }

    private void OnRegister()
    {
        if (_waitingResponse)
            return;


        _waitingResponse = true;
        var request = new RegisterPlayFabUserRequest()
        { Email = $"{_idInputField.text}@testexampletest.com", Username = _idInputField.text, Password = _passwordInputField.text };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSucceed, CallErrorReport);
    }


    private void OnRegisterSucceed(RegisterPlayFabUserResult result)
    {
        UIManager.Instance.OpenNoticePopup("RegisterSucceed");
        _waitingResponse = false;
    }

    private void OnGuestLogin()
    {
        if (_waitingResponse)
            return;

        _waitingResponse = true;
        var request = new LoginWithCustomIDRequest()
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };

        PlayerPrefs.SetInt("PlayerInfo", (int)LoginPlatform.Guest);
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSucceed, CallErrorReport);
    }

    public void GetIpPortAddress(GetTitleDataResult result)
    {
        string ip = result.Data["IP"];
        string port = result.Data["Port"];
        LoginSceneManager.Instance.EventLoginScene.CallLoginSuccess(_id, ip, Convert.ToInt32(port));
        ClosePopup();
    }

    public void CallErrorReport(PlayFabError error)
    {
        UIManager.Instance.OpenNoticePopup(error.ToString());
        _waitingResponse = false;
    }


#if UNITY_EDITOR

    protected override void OnBindField()
    {
        base.OnBindField();
        _idInputField = FindGameObjectInChildren<TMP_InputField>("IdInputField");
        _passwordInputField = FindGameObjectInChildren<TMP_InputField>("PasswordInputField");
        _registerButton = FindGameObjectInChildren<Button>("RegisterButton");
        _loginButton = FindGameObjectInChildren<Button>("LoginButton");
        _guestButton = FindGameObjectInChildren<Button>("GuestButton");
        _facebookButton = FindGameObjectInChildren<Button>("FacebookButton");
        _googleButton = FindGameObjectInChildren<Button>("GoogleButton");
        _closeButton = FindGameObjectInChildren<Button>("CloseButton");
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _idInputField);
        CheckNullValue(this.name, _passwordInputField);
        CheckNullValue(this.name, _loginButton);
        CheckNullValue(this.name, _registerButton);
        CheckNullValue(this.name, _guestButton);
        CheckNullValue(this.name, _facebookButton);
        CheckNullValue(this.name, _googleButton);
        CheckNullValue(this.name, _closeButton);
    }

#endif
}


public interface IPlayfabDataHandler
{
    void GetIpPortAddress(GetTitleDataResult result);
    void CallErrorReport(PlayFabError error);
}
