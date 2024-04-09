using System.Collections;
using System.Collections.Generic;
using StandardData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchPlayer : BaseBehaviour
{
    [SerializeField]
    private Image _playerIconImage;
    [SerializeField]
    private TextMeshProUGUI _playerNicknameText;



    public void SetMatchPlayer(bool isPlayer, bool isEnemy, string nickname)
    {
        // web에서 플레이어 이미지 가져옴 _playerIconImage.sprite = 
        _playerNicknameText.text = nickname;
        if (isEnemy)
        {
            _playerNicknameText.color = Color.red;
        }
        else if (isPlayer)
        {
            _playerNicknameText.color = Color.green;
        }
        else
        {
            _playerNicknameText.color = Color.blue;
        }
    }





#if UNITY_EDITOR

    protected override void OnBindField()
    {
        base.OnBindField();
        _playerIconImage = FindGameObjectInChildren<Image>("PlayerIconImage");
        _playerNicknameText = FindGameObjectInChildren<TextMeshProUGUI>("NickNameText");
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _playerIconImage);
        CheckNullValue(this.name, _playerIconImage);
    }

#endif
}

