using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeEvent : MonoBehaviour
{
    public Action<SceneChangeEvent,  SceneChangeEventArgs> OnSceneChanged;

    public void CallSceneChanged(string sceneName, bool hasDelay, float delayTime = 3)
    {
        OnSceneChanged?.Invoke(this,
            new SceneChangeEventArgs() { sceneName = sceneName, hasDealy = hasDelay, delayTime = delayTime });
    }
}

public class SceneChangeEventArgs : EventArgs
{
    public string sceneName;
    public bool hasDealy;
    public float delayTime;
}
