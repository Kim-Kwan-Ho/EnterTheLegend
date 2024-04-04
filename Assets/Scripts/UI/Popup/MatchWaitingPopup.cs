using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchWaitingPopup : UIPopup
{

    [Header("Team Players")]
    [SerializeField]
    private MatchPlayer[] _blueTeamPlayers;
    [SerializeField]
    private MatchPlayer[] _redTeamPlayers;


    [Header("Timer")]
    [SerializeField]
    private TextMeshProUGUI _timerText;
    private int _timerTime = 3;


    private void OnEnable()
    {
        BattleSceneManager.Instance.EventBattleScene.OnRoomInitialize += Event_InitializeTeamBattleRoom;
        BattleSceneManager.Instance.EventBattleScene.OnGameStart += Event_OnGameStart;
    }

    private void OnDisable()
    {
        BattleSceneManager.Instance.EventBattleScene.OnRoomInitialize -= Event_InitializeTeamBattleRoom;
        BattleSceneManager.Instance.EventBattleScene.OnGameStart -= Event_OnGameStart;
    }



    private void Event_InitializeTeamBattleRoom(BattleSceneEvent battleSceneEvent,
        BattleSceneEventRoomInfoArgs teamBattleSceneEventArgs)
    {
        stBattleRoomInfo roomInfo = teamBattleSceneEventArgs.roomInfo;
        bool isBlueTeam = teamBattleSceneEventArgs.roomInfo.playerIndex < _blueTeamPlayers.Length;
        
        
        for (int i = 0; i < _blueTeamPlayers.Length; i++)
        {
            if (roomInfo.playerIndex == i)
            {
                _blueTeamPlayers[i].SetMatchPlayer(true, false, roomInfo.playersInfo[i].Nickname);
            }
            else
            {
                _blueTeamPlayers[i].SetMatchPlayer(false, !isBlueTeam, roomInfo.playersInfo[i].Nickname);
            }
        }
        for (int i = 0; i < _redTeamPlayers.Length; i++)
        {
            if (roomInfo.playerIndex == i + _blueTeamPlayers.Length)
            {
                _redTeamPlayers[i].SetMatchPlayer(true, false, roomInfo.playersInfo[i + _blueTeamPlayers.Length].Nickname);
            }
            else
            {
                _redTeamPlayers[i].SetMatchPlayer(false, isBlueTeam, roomInfo.playersInfo[i + _blueTeamPlayers.Length].Nickname);
            }
        }

    }

    private void Event_OnGameStart(BattleSceneEvent battleSceneEvent, bool loadInfo)
    {
        StartCoroutine(CoGameTimer());
    }
    private IEnumerator CoGameTimer()
    {
        _timerText.text = _timerTime.ToString();
        float curTime = _timerTime;
        while (curTime > 0)
        {
            curTime -= Time.deltaTime;
            _timerText.text = curTime.ToString("0");
            yield return null;
        }
        Destroy(this.gameObject);
    }



#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _blueTeamPlayers = GetComponentsInGameObject<MatchPlayer>("BlueTeam");
        _redTeamPlayers = GetComponentsInGameObject<MatchPlayer>("RedTeam");
        _timerText = FindGameObjectInChildren<TextMeshProUGUI>("TimerText");
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _blueTeamPlayers);
        CheckNullValue(this.name, _redTeamPlayers);
        CheckNullValue(this.name, _timerText);
    }


#endif
}
