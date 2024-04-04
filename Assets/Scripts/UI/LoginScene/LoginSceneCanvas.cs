using UnityEngine;

public class LoginSceneCanvas : BaseBehaviour
{
    [SerializeField]
    private GameObject _loginSucceedUI;

    private void Start()
    {
        LoginSceneManager.Instance.EventLoginScene.OnLoginSuccess += Event_LoginSucceed;
    }

    private void OnDisable()
    {
        LoginSceneManager.Instance.EventLoginScene.OnLoginSuccess -= Event_LoginSucceed;
    }

    private void Event_LoginSucceed(LoginSceneEvents loginSceneEvents,
        LoginSceneEventLoginSucceedArgs loginSceneEventLoginSucceedArgs)
    {
        _loginSucceedUI.SetActive(true);
    }


#if UNITY_EDITOR

    protected override void OnBindField()
    {
        base.OnBindField();
        _loginSucceedUI = GameObject.Find("LoginSucceedUI");
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _loginSucceedUI);
    }


#endif
}
