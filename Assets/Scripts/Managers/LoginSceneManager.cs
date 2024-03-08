using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LoginSceneEvents))]

public class LoginSceneManager : SingletonMonobehaviour<LoginSceneManager>
{
    public LoginSceneEvents EventLoginScene;


    protected override void Awake()
    {
        base.Awake();

    }

    private void Start()
    {
        UIManager.Instance.OpenPopup<LoginPopup>();
        EventLoginScene.OnLoginSuccess += Event_LoginSuccess;
    }
    private void OnDestroy()
    {
        EventLoginScene.OnLoginSuccess -= Event_LoginSuccess;
    }



    private void Event_LoginSuccess(LoginSceneEvents loginSceneEvents,
        LoginSceneEventLoginSucceedArgs loginSceneEventLoginSucceedArgs)
    {
        
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
