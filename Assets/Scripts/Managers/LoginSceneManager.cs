using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

[RequireComponent(typeof(LoginSceneEvents))]

public class LoginSceneManager : SingletonMonobehaviour<LoginSceneManager>, IPlayfabDataHandler
{
    public LoginSceneEvents EventLoginScene;


    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        EventLoginScene.OnLoginFailed += Event_LoginFailed;
        CheckPlayerInfo();
    }
    private void OnDisable()
    {
        EventLoginScene.OnLoginFailed -= Event_LoginFailed;
    }


    private void CheckPlayerInfo()
    {
        if (PlayerPrefs.HasKey("PlayerId"))
        {
            int platform = PlayerPrefs.GetInt("PlayerInfo");

            switch (platform)
            {
                case (int)LoginPlatform.Guest:
                    break;
                case (int)LoginPlatform.PlayFabAccount:
                    var request = new LoginWithPlayFabRequest
                    { Username = PlayerPrefs.GetString("PlayerId"), Password = PlayerPrefs.GetString("PlayerPassword") };
                    PlayFabClientAPI.LoginWithPlayFab(request, ((result) => PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), GetIpPortAddress, CallErrorReport)), CallErrorReport);
                    break;
            }
        }
        else
        {
            EventLoginScene.CallLoginFailed();
        }
    }


    public void GetIpPortAddress(GetTitleDataResult result)
    {
        string ip = result.Data["IP"];
        string port = result.Data["Port"];
        EventLoginScene.CallLoginSuccess(PlayerPrefs.GetString("PlayerId"), ip, Convert.ToInt32(port));
    }
    public void CallErrorReport(PlayFabError error)
    {
        PlayerPrefs.DeleteAll();
        CheckPlayerInfo();
    }

    private void Event_LoginFailed(LoginSceneEvents loginSceneEvents,
        LoginSceneEventLoginFailedArgs loginSceneEventLoginFailedArgs)
    {
        UIManager.Instance.OpenPopup<LoginPopup>();
    }



#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        EventLoginScene = GetComponent<LoginSceneEvents>();
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, EventLoginScene);
    }

#endif
}
