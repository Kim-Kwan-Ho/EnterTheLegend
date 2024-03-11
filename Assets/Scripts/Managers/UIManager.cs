using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonMonobehaviour<UIManager>
{
    [SerializeField]
    private GameObject _popupCanvas;
    private Stack<UIPopup> _popupStack = new Stack<UIPopup>();
    private Dictionary<Type, GameObject> _commonPopupDic = new Dictionary<Type, GameObject>();
    private Dictionary<Type, GameObject> _dynamicPopupDic = new Dictionary<Type, GameObject>();


    protected override void Awake()
    {
        base.Awake();
        DontDestroyGameObject();
    }

    private void Start()
    {
        MySceneManager.Instance.EventSceneChanged.OnSceneChanged += ClearOnSceneChange;
    }

    private void OnDestroy()
    {
        MySceneManager.Instance.EventSceneChanged.OnSceneChanged -= ClearOnSceneChange;
    }

    public void ClosePopup(UIPopup popup)
    {
        Destroy(popup.gameObject);
    }

    public void OpenPopup<T>() where T : UIPopup
    {
        Type type = typeof(T);
        if (!_dynamicPopupDic.ContainsKey(type))
        {
            _dynamicPopupDic[type] = Resources.Load<GameObject>($"UI/Popup/{type.Name}");
        }
        GameObject go = Instantiate(_dynamicPopupDic[type], _popupCanvas.transform) as GameObject;
        _popupStack.Push(go.GetComponent<T>());
    }

    public void OpenNoticePopup(string text)
    {

        if (!_commonPopupDic.ContainsKey(typeof(NoticePopup)))
        {
            _commonPopupDic[typeof(NoticePopup)] = Resources.Load<GameObject>($"UI/Popup/NoticePopup") as GameObject;
        }
        NoticePopup notice = Instantiate(_commonPopupDic[typeof(NoticePopup)], _popupCanvas.transform).GetComponent<NoticePopup>();
        notice.SetText(text);
        _popupStack.Push(notice);
    }


    public void CloseAllPopup()
    {
        while (_popupStack.Count > 0)
        {
            if (_popupStack.Peek() != null)
            {
                Destroy(_popupStack.Pop());
            }
            else
            {
                _popupStack.Pop();
            }
        }
    }

    public void ClearOnSceneChange(SceneChangeEvent sceneChangeEvent, SceneChangeEventArgs sceneChangeEventArgs)
    {
        CloseAllPopup();
        _dynamicPopupDic.Clear();
        Resources.UnloadUnusedAssets();
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _popupCanvas = transform.GetChild(0).gameObject;

    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _popupCanvas);
    }

#endif


}

