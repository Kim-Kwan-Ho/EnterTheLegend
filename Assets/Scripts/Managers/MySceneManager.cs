using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : SingletonMonobehaviour<MySceneManager>
{
    public SceneChangeEvent EventSceneChanged;
    private float _loadingGage = 0f;
    public float LoadingGage { get { return _loadingGage; } }

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        EventSceneChanged.OnSceneChanged += LoadSceneAsync;
    }


     
    public void LoadSceneAsync(SceneChangeEvent sceneChangeEvent, SceneChangeEventArgs sceneChangeEventArgs)
    {
        _loadingGage = 0;
        SceneManager.LoadScene("LoadingScene");
        StartCoroutine(LoadScene(sceneChangeEventArgs.sceneName, sceneChangeEventArgs.hasDealy,
            sceneChangeEventArgs.delayTime));
    }

    private IEnumerator LoadScene(string sceneName, bool hasDelay = false, float delayTime = 3)
    {
        float time = 0;
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;
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
        
        yield return new WaitForSeconds(1f);
        _loadingGage = 1;
        op.allowSceneActivation = true;

    }


}