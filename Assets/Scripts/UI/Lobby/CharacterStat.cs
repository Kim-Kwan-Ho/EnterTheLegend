using StandardData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStat : BaseBehaviour
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
        MyGameManager.Instance.EventGameManager.OnPlayerStatChanged += Event_OnPlayerStatChanged;
    }

    private void OnDisable()
    {
        MyGameManager.Instance.EventGameManager.OnPlayerStatChanged -= Event_OnPlayerStatChanged;
    }



    public void SetStat(LobbySceneInitializeArgs lobbySceneInitializeArgs)
    {
        ushort hp = 0;
        ushort attack = 0;
        ushort defense = 0;

        if (lobbySceneInitializeArgs.characterEquip != null)
        {
            hp += lobbySceneInitializeArgs.characterEquip.StatHp;
            attack += lobbySceneInitializeArgs.characterEquip.StatAttack;
            defense += lobbySceneInitializeArgs.characterEquip.StatDefense;
        }
        if (lobbySceneInitializeArgs.weaponEquip != null)
        {
            hp += lobbySceneInitializeArgs.weaponEquip.StatHp;
            attack += lobbySceneInitializeArgs.weaponEquip.StatAttack;
            defense += lobbySceneInitializeArgs.weaponEquip.StatDefense;
        }
        if (lobbySceneInitializeArgs.helmetEquip != null)
        {
            hp += lobbySceneInitializeArgs.helmetEquip.StatHp;
            attack += lobbySceneInitializeArgs.helmetEquip.StatAttack;
            defense += lobbySceneInitializeArgs.helmetEquip.StatDefense;
        }
        if (lobbySceneInitializeArgs.armorEquip != null)
        {
            hp += lobbySceneInitializeArgs.armorEquip.StatHp;
            attack += lobbySceneInitializeArgs.armorEquip.StatAttack;
            defense += lobbySceneInitializeArgs.armorEquip.StatDefense;
        }
        if (lobbySceneInitializeArgs.shoesEquip != null)
        {
            hp += lobbySceneInitializeArgs.shoesEquip.StatHp;
            attack += lobbySceneInitializeArgs.shoesEquip.StatAttack;
            defense += lobbySceneInitializeArgs.shoesEquip.StatDefense;
        }
        SetStats(hp,attack,defense);
    }

    private void Event_OnPlayerStatChanged(MyGameManagerEvent myGameManagerEvent,
        GamePlayerStatChangedEventArgs gamePlayerStatChangedEventArgs)
    {
        SetStats(gamePlayerStatChangedEventArgs.playerStat.Hp, gamePlayerStatChangedEventArgs.playerStat.Attack,
            gamePlayerStatChangedEventArgs.playerStat.Defense);
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
