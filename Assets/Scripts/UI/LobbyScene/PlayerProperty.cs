using TMPro;
using UnityEngine;

public class PlayerProperty : BaseBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _nicknameText;
    [SerializeField]
    private TextMeshProUGUI _creditText;
    [SerializeField]
    private TextMeshProUGUI _goldText;



    private void OnEnable()
    {
        LobbySceneManager.Instance.EventLobbyScene.OnLobbyInitialize += Event_SceneInitialize;
    }

    private void OnDisable()
    {
        LobbySceneManager.Instance.EventLobbyScene.OnLobbyInitialize -= Event_SceneInitialize;
    }

    private void Event_SceneInitialize(LobbySceneEvent lobbySceneEvent,
        LobbySceneInitializeArgs lobbySceneInitializeArgs)
    {
        _nicknameText.text = lobbySceneInitializeArgs.playerData.Nickname;
        _creditText.text = lobbySceneInitializeArgs.playerData.Credit.ToString();
        _goldText.text = lobbySceneInitializeArgs.playerData.Gold.ToString();

    }


#if UNITY_EDITOR
    protected override void OnBindField()
    {
        _nicknameText = FindGameObjectInChildren<TextMeshProUGUI>("NicknameText");
        _creditText = FindGameObjectInChildren<TextMeshProUGUI>("CreditText");
        _goldText = FindGameObjectInChildren<TextMeshProUGUI>("GoldText");
        base.OnBindField();
        
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _nicknameText);
        CheckNullValue(this.name, _creditText);
        CheckNullValue(this.name, _goldText);
    }
#endif

}
