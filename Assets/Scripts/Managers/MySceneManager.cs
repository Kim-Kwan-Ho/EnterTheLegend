using StandardData;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
[RequireComponent(typeof(SceneChangeEvent))]
public class MySceneManager : SingletonMonobehaviour<MySceneManager>
{
    public SceneChangeEvent EventSceneChanged;


    [Header("Fade")]
    [SerializeField]
    private Image _fadeImage;
    [SerializeField]
    private float _fadeTimer = 1.5f;


    [Header("Loading Gage")]
    [SerializeField]
    private GameObject _loadingGob;
    [SerializeField]
    private Slider _loadingSlider;
    [SerializeField]
    private TextMeshProUGUI _loadingGageText;
    private float _loadingGage = 0f;

    private object _sceneInitialize;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyGameObject();
    }

    private void OnEnable()
    {
        EventSceneChanged.OnSceneChanged += Event_LoadSceneAsync;
        SceneManager.sceneLoaded += InitializeScene;
    }
    private void OnDisable()
    {
        EventSceneChanged.OnSceneChanged -= Event_LoadSceneAsync;
        SceneManager.sceneLoaded -= InitializeScene;
    }



    private void Event_LoadSceneAsync(SceneChangeEvent sceneChangeEvent, SceneChangeEventArgs sceneChangeEventArgs)
    {

        _loadingGage = 0;
        _sceneInitialize = sceneChangeEventArgs.sceneInitialize;
        StartCoroutine(CoFadeOut(sceneChangeEventArgs));
    }

    private void InitializeScene(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "LoadingScene")
            return;

        if (scene.name == "TeamBattleScene")
        {
            stCreateTeamBattleRoom roomInfo = (stCreateTeamBattleRoom)_sceneInitialize;
            TeamBattleSceneManager.Instance.EventTeamBattleScene.CallRoomInitialize(roomInfo.RoomId,
                roomInfo.playersInfo);
        }
        else if (scene.name == "LobbyScene")
        {
            stResponsePlayerData playerData = (stResponsePlayerData)_sceneInitialize;
            LobbySceneManager.Instance.EventLobbyScene.CallLobbyInitialize(playerData);
        }

        _sceneInitialize = null;
    }

    private IEnumerator CoFadeIn()
    {
        float time = 0;
        Color targetColor = new Color(0, 0, 0, 0);
        Color curColor = _fadeImage.color;

        _loadingSlider.value = 0;
        _loadingGageText.text = "Loading... 0%";

        while (time < _fadeTimer)
        {
            _fadeImage.color = Color.Lerp(curColor, targetColor, time / _fadeTimer);
            time += Time.deltaTime;
            yield return null;
        }
        _fadeImage.color = targetColor;
    }

    private IEnumerator CoLoadScene(SceneType sceneType, bool hasDelay = true, float delayTime = 3)
    {
        _loadingGob.SetActive(true);
        float time = 0;
        AsyncOperation op = SceneManager.LoadSceneAsync(Enum.GetName(typeof(SceneType),sceneType), LoadSceneMode.Additive);
        op.allowSceneActivation = false;

        if (hasDelay)
        {
            while (time < delayTime)
            {
                _loadingGage = (time / delayTime) * 100;
                _loadingSlider.value = _loadingGage;
                _loadingGageText.text = $"Loading... {(int)(_loadingGage)}%";
                time += Time.deltaTime;
                yield return null;
            }
            while (!op.isDone)
                yield return null;
        }
        else
        {
            while (!op.isDone)
            {

                _loadingGage = op.progress * 100;
                _loadingSlider.value = _loadingGage;
                _loadingGageText.text = $"Loading... {(int)_loadingGage}%";
                yield return null;
            }
        }

        _loadingSlider.value = 1;
        _loadingGageText.text = "Loading... 100%";
        _loadingGob.SetActive(false);
        SceneManager.UnloadSceneAsync("LoadingScene");
        _loadingGage = 1;
        op.allowSceneActivation = true;
        StartCoroutine(CoFadeIn());
    }
    private IEnumerator CoFadeOut(SceneChangeEventArgs sceneChangeEventArgs = null)
    {
        float time = 0;
        Color targetColor = new Color(0, 0, 0, 1);
        Color curColor = _fadeImage.color;

        while (time < _fadeTimer)
        {
            _fadeImage.color = Color.Lerp(curColor, targetColor, time / _fadeTimer);
            time += Time.deltaTime;
            yield return null;
        }
        _fadeImage.color = targetColor;
        yield return StartCoroutine(CoUnloadScene(sceneChangeEventArgs));
    }
    private IEnumerator CoUnloadScene(SceneChangeEventArgs sceneChangeEventArgs)
    {
        SceneManager.LoadScene("LoadingScene");
        yield return StartCoroutine(CoLoadScene(sceneChangeEventArgs.sceneType,
            sceneChangeEventArgs.hasDealy,
            sceneChangeEventArgs.delayTime));
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        EventSceneChanged = GetComponent<SceneChangeEvent>();
        _fadeImage = FindGameObjectInChildren<Image>("FadeImage");
        _loadingGob = GameObject.Find("Loading");
        _loadingSlider = FindGameObjectInChildren<Slider>("LoadingSlider");
        _loadingGageText = FindGameObjectInChildren<TextMeshProUGUI>("LoadingGageText");
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _fadeImage);
        CheckNullValue(this.name, EventSceneChanged);
    }
#endif
}