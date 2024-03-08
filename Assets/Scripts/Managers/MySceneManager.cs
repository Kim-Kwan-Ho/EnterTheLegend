using StandardData;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SceneChangeEvent))]
public class MySceneManager : SingletonMonobehaviour<MySceneManager>
{
    public SceneChangeEvent EventSceneChanged;
    private float _loadingGage = 0f;
    public float LoadingGage { get { return _loadingGage; } }
    private object _sceneInitialize;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyGameObject();
        Debug.Log("Qs");
    }

    private void Start()
    {
        EventSceneChanged.OnSceneChanged += Event_LoadSceneAsync;

    }
    private void OnDestroy()
    {
        EventSceneChanged.OnSceneChanged -= Event_LoadSceneAsync;
    }



    private void Event_LoadSceneAsync(SceneChangeEvent sceneChangeEvent, SceneChangeEventArgs sceneChangeEventArgs)
    {

        _loadingGage = 0;
        SceneManager.sceneLoaded += InitializeScene;
        //SceneManager.LoadScene("LoadingScene", LoadSceneMode.Additive);
        _sceneInitialize = sceneChangeEventArgs.sceneInitialize;
        StartCoroutine(LoadScene(sceneChangeEventArgs.sceneName,
            sceneChangeEventArgs.hasDealy,
            sceneChangeEventArgs.delayTime));
    }

    private void InitializeScene(Scene scene, LoadSceneMode arg1)
    {
        if (scene.name == "AdventureScene")
        {
            stCreateAdventureRoom roomInfo = (stCreateAdventureRoom)_sceneInitialize;
            AdventureSceneManager.Instance.EventAdventureScene.CallRoomInitialize(roomInfo.RoomId,
                roomInfo.playersInfo);
        }
        SceneManager.sceneLoaded -= InitializeScene;

    }

    private IEnumerator LoadScene(string sceneName ,bool hasDelay = true, float delayTime = 3)
    {
        float time = 0;
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;
        yield return new WaitForSeconds(2f);

        if (hasDelay)
        {
            while (time < delayTime)
            {
                _loadingGage = time / delayTime;
                time += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (!op.isDone)
            {
                if (op.progress >= 0.9f)
                    break;
                _loadingGage = op.progress;
                yield return null;
            }
        }
        
        _loadingGage = 1;
        op.allowSceneActivation = true;
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        EventSceneChanged = GetComponent<SceneChangeEvent>();
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, EventSceneChanged);
    }
#endif
}