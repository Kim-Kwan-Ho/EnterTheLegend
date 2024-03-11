using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginSceneEvents : MonoBehaviour
{
    public Action<LoginSceneEvents,LoginSceneEventLoginSucceedArgs> OnLoginSuccess;
    public Action<LoginSceneEvents, LoginSceneEventLoginFailedArgs> OnLoginFailed;

    public void CallLoginSuccess(string Id, string Ip, int Port)
    {
        OnLoginSuccess?.Invoke(this, new LoginSceneEventLoginSucceedArgs() { Ip = Ip, Port = Port });
    }
    public void CallLoginFailed()
    {
        OnLoginFailed?.Invoke(this, new LoginSceneEventLoginFailedArgs());
    }



}

public class LoginSceneEventLoginSucceedArgs : EventArgs
{
    public string Id;
    public string Ip;
    public int Port;
}

public class LoginSceneEventLoginFailedArgs : EventArgs
{

}
