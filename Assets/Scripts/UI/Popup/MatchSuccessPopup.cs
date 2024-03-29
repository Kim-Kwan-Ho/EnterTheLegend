using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchSuccessPopup : UIPopup
{


    [Header("Prefabs")]
    [SerializeField]
    private GameObject _playerBluePrefab;
    [SerializeField]
    private GameObject _playerRedPrefab;

    [Header("Team Transform")]
    [SerializeField]
    private Transform _teamBlue;
    [SerializeField]
    private Transform _teamRed;


    [Header("Timer")]
    [SerializeField]
    private TextMeshProUGUI _timerText;
    private int _time = 3;

    public override void SetPopup<T>(T info) where T : struct
    {

        StartCoroutine(CoTimer());
    }

    private IEnumerator CoTimer()
    {
        float curTime = _time;
        while (curTime > 0)
        {
            curTime -= Time.deltaTime;
            _timerText.text = _time.ToString("0");
            yield return null;
        }
    }



#if UNITY_EDITOR

    protected override void OnBindField()
    {
        base.OnBindField();
        _teamBlue = FindGameObjectInChildren<Transform>("TeamBlue");
        _teamRed = FindGameObjectInChildren<Transform>("TeamRed");
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, (Object)_teamBlue);
        CheckNullValue(this.name, (Object)_teamRed);
    }


#endif
}
