using System;
using UnityEngine;

public class SceneChangeEvent : MonoBehaviour
{
    public Action<SceneChangeEvent,  SceneChangeEventArgs> OnSceneChanged;

    public void CallSceneChanged(string sceneName, object sceneInitialize ,bool hasDelay = true, float delayTime = 3)
    {
        OnSceneChanged?.Invoke(this,
            new SceneChangeEventArgs() { sceneName = sceneName, sceneInitialize = sceneInitialize, hasDealy = hasDelay, delayTime = delayTime });
    }
}

public class SceneChangeEventArgs : EventArgs
{
    public string sceneName;
    public bool hasDealy;
    public float delayTime;
    public object sceneInitialize;

}
