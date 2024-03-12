using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginSceneEvents : MonoBehaviour
{
    public Action<LoginSceneEvents, LoginSceneEventLoginSucceedArgs> OnLoginSuccess;
    public Action<LoginSceneEvents, LoginSceneEventLoginFailedArgs> OnLoginFailed;

    public void CallLoginSuccess(string id, string ip, int port)
    {
        OnLoginSuccess?.Invoke(this, new LoginSceneEventLoginSucceedArgs() { id = id, ip = ip, port = port });
    }
    public void CallLoginFailed()
    {
        OnLoginFailed?.Invoke(this, new LoginSceneEventLoginFailedArgs());
    }



}

public class LoginSceneEventLoginSucceedArgs : EventArgs
{
    public string id;
    public string ip;
    public int port;
}

public class LoginSceneEventLoginFailedArgs : EventArgs
{

}