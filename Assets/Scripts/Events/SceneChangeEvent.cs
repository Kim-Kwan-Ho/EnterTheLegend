using System;
using UnityEngine;

public class SceneChangeEvent : MonoBehaviour
{
    public Action<SceneChangeEvent,  SceneChangeEventArgs> OnSceneChanged;

    public void CallSceneChanged(SceneType sceneType, object sceneInitialize ,bool hasDelay = true, float delayTime = 3)
    {
        OnSceneChanged?.Invoke(this,
            new SceneChangeEventArgs() { sceneType = sceneType, sceneInitialize = sceneInitialize, hasDealy = hasDelay, delayTime = delayTime });
    }
}

public class SceneChangeEventArgs : EventArgs
{
    public SceneType sceneType;
    public bool hasDealy;
    public float delayTime;
    public object sceneInitialize;

}
