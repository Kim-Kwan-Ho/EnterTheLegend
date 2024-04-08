using StandardData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatsDisplay : BaseBehaviour
{
    [Header("Texts")]
    [SerializeField]
    private TextMeshProUGUI _healthText;
    [SerializeField]
    private TextMeshProUGUI _attackText;
    [SerializeField]
    private TextMeshProUGUI _defenseText;

    [Header("Sliders")]
    [SerializeField]
    private Slider _healthSlider;
    [SerializeField]
    private Slider _attackSlider;
    [SerializeField]
    private Slider _defenseSlider;


    private void Awake()
    {
        _healthSlider.maxValue = PlayerMaximumStats.MaxHp;
        _attackSlider.maxValue = PlayerMaximumStats.MaxAttack;
        _defenseSlider.maxValue = PlayerMaximumStats.MaxDefense;
    }
    private void OnEnable()
    {
        LobbySceneManager.Instance.EventLobbyScene.OnPlayerStatChanged += Event_OnPlayerStatChanged;
    }

    private void OnDisable()
    {
        LobbySceneManager.Instance.EventLobbyScene.OnPlayerStatChanged -= Event_OnPlayerStatChanged;
    }


    private void Event_OnPlayerStatChanged(LobbySceneEvent lobbySceneEvent,
        LobbyScenePlayerStatChangedEventArgs lobbyScenePlayerStatChangedEventArgs)
    {
        SetStats(lobbyScenePlayerStatChangedEventArgs.characterStat.Hp, lobbyScenePlayerStatChangedEventArgs.characterStat.Attack,
            lobbyScenePlayerStatChangedEventArgs.characterStat.Defense);
    }

    private void SetStats(ushort hp, ushort attack, ushort defense)
    {
        _healthText.text = hp.ToString();
        _attackText.text = attack.ToString();
        _defenseText.text = defense.ToString();

        _healthSlider.value = hp;
        _attackSlider.value = attack;
        _defenseSlider.value = defense;
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _healthText = FindGameObjectInChildren<TextMeshProUGUI>("HealthText");
        _attackText = FindGameObjectInChildren<TextMeshProUGUI>("AttackText");
        _defenseText = FindGameObjectInChildren<TextMeshProUGUI>("DefenseText");

        _healthSlider = FindGameObjectInChildren<Slider>("HealthSlider");
        _attackSlider = FindGameObjectInChildren<Slider>("AttackSlider");
        _defenseSlider = FindGameObjectInChildren<Slider>("DefenseSlider");
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _healthText);
        CheckNullValue(this.name, _attackText);
        CheckNullValue(this.name, _defenseText);
        CheckNullValue(this.name, _healthSlider);
        CheckNullValue(this.name, _attackSlider);
        CheckNullValue(this.name, _defenseSlider);
    }
#endif
}
