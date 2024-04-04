using StandardData;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class GameMode : BaseBehaviour
{


    [Header("Buttons")]
    [SerializeField]
    private Button _leftButton;
    [SerializeField]
    private Button _rightButton;
    [SerializeField]
    private Button _matchButton;
    [SerializeField]
    private Button _cancelMatchButton;

    [Header("Scriptable Object")]
    [SerializeField]
    private GameRoomSO _gameRoomSO;
    private Transform[] _roomTypeTrs;


    [Header("Swipe Settings")]
    [SerializeField]
    private Transform _gameModeParent;
    [SerializeField]
    private float _swipeTime = 0.5f;
    [SerializeField]
    private float _startPosX = 250;
    private bool _isSwiping = false;
    private bool _isMatching = false;

    private int _curIndex = 0;
    private void Awake()
    {
        _roomTypeTrs = new Transform[_gameRoomSO.RoomTypeList.Count];
        _roomTypeTrs[0] = Instantiate(_gameRoomSO.RoomTypePrefabList[0], _gameModeParent).GetComponent<Transform>();
        _roomTypeTrs[0].localPosition = Vector2.zero;
        for (int i = 1; i < _roomTypeTrs.Length; i++)
        {
            _roomTypeTrs[i] = Instantiate(_gameRoomSO.RoomTypePrefabList[i], _gameModeParent).GetComponent<Transform>();
            _roomTypeTrs[i].localPosition = new Vector2(_startPosX, 0);
        }

        _leftButton.onClick.AddListener(() => SwipeGameMode(false));
        _rightButton.onClick.AddListener(()=> SwipeGameMode(true));
        _matchButton.onClick.AddListener(MatchStart);
        _cancelMatchButton.onClick.AddListener(CancelMatch);

        _cancelMatchButton.gameObject.SetActive(false);
    }


    private void SwipeGameMode(bool isUpper)
    {
        if (_isMatching || _isSwiping || _roomTypeTrs.Length == 1)
            return;

        _isSwiping = true;
        int targetIndex;
        if (isUpper)
        {
            targetIndex = _curIndex + 1;
            if (targetIndex == _roomTypeTrs.Length)
                targetIndex = 0;
        }
        else
        {
            targetIndex = _curIndex - 1;
            if (targetIndex == -1)
                targetIndex = _roomTypeTrs.Length - 1;
        }


        StartCoroutine(CoSwipe(_curIndex, targetIndex, isUpper));
        _curIndex = targetIndex;
    }

    private IEnumerator CoSwipe(int curIndex, int targetIndex, bool isUpper)
    {
        Vector2 curTargetVec;
        Vector2 afterStartVec;

        if (isUpper)
        {
            _roomTypeTrs[targetIndex].localPosition = new Vector2(_startPosX, 0);
            curTargetVec = new Vector2(-_startPosX, 0);
        }
        else
        {
            _roomTypeTrs[targetIndex].localPosition = new Vector2(-_startPosX, 0);
            curTargetVec = new Vector2(_startPosX, 0);
        }
        afterStartVec = _roomTypeTrs[targetIndex].localPosition;
        float time = 0;

        while (time < _swipeTime)
        {
            _roomTypeTrs[curIndex].localPosition = Vector2.Lerp(Vector2.zero, curTargetVec, time / _swipeTime);
            _roomTypeTrs[targetIndex].localPosition = Vector2.Lerp(afterStartVec, Vector2.zero, time / _swipeTime);
            time += Time.deltaTime;
            yield return null;
        }

        _roomTypeTrs[curIndex].localPosition = curTargetVec;
        _roomTypeTrs[targetIndex].localPosition = Vector2.zero;
        _isSwiping = false;
    }

    private void MatchStart()
    {
        if (_isSwiping)
            return;

        _isMatching = true;
        _cancelMatchButton.gameObject.SetActive(true);
        _matchButton.gameObject.SetActive(false);
        
        stRequestForMatch requestMatch = new stRequestForMatch();
        requestMatch.Header.MsgID = MessageIdTcp.RequestForMatch;
        requestMatch.Header.PacketSize = (ushort)Marshal.SizeOf(requestMatch);
        requestMatch.MatchType = (ushort)_gameRoomSO.RoomTypeList[_curIndex];
        byte[] msg = Utilities.GetObjectToByte(requestMatch);
        ServerManager.Instance.EventClientToServer.CallOnTcpSend(msg);
    }

    private void CancelMatch()
    {
        if (_isSwiping)
            return;

        _isMatching = false;
        _cancelMatchButton.gameObject.SetActive(false);
        _matchButton.gameObject.SetActive(true);
        Debug.Log("match canceled");

    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _leftButton = FindGameObjectInChildren<Button>("LeftButton");
        _rightButton = FindGameObjectInChildren<Button>("RightButton");
        _matchButton = FindGameObjectInChildren<Button>("MatchButton");
        _cancelMatchButton = FindGameObjectInChildren<Button>("CancelMatchButton");
        _gameModeParent = FindGameObjectInChildren<Transform>("GameModeParent");
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _leftButton);
        CheckNullValue(this.name, _rightButton);
        CheckNullValue(this.name, _matchButton);
        CheckNullValue(this.name, _cancelMatchButton);
        CheckNullValue(this.name, (Object)_gameModeParent);
        CheckNullValue(this.name, _gameRoomSO);


    }

#endif
}
