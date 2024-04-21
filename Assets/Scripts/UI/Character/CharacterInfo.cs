using System.Collections;
using StandardData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfo : BaseBehaviour
{
    [Header("Character")]
    [SerializeField]
    private Character _character;

    [Header("Nickname")]
    [SerializeField]
    private TextMeshProUGUI _nicknameText;


    [Header("Hp FillGage")]
    [SerializeField]
    private Slider _hpSlider;
    [SerializeField]
    private Image _hpGageImage;
    [SerializeField]
    private TextMeshProUGUI _hpText;
    private ushort _maxHp;
    private ushort _curHp;

    private void OnEnable()
    {
        _character.EventBattle.OnInitialize += Event_Initialize;
        _character.EventBattle.OnTakeDamage += Event_TakeDamage;
    }

    private void OnDisable()
    {
        _character.EventBattle.OnInitialize -= Event_Initialize;
        _character.EventBattle.OnTakeDamage -= Event_TakeDamage;
    }

    private void Event_Initialize(BattleEvent battleEvent, BattleInitializeEventArgs battleInitializeEventArgs)
    {
        _nicknameText.text = battleInitializeEventArgs.nickname;
        _hpText.text = battleInitializeEventArgs.hp.ToString();
        _maxHp = battleInitializeEventArgs.hp;
        _curHp = _maxHp;
        if (battleInitializeEventArgs.isEnemy)
        {
            _nicknameText.color = Color.red;
            _hpGageImage.color = Color.red;
        }
        else
        {
            if (battleInitializeEventArgs.isPlayer)
            {
                _nicknameText.color = Color.green;
            }
            else
            {
                _nicknameText.color = Color.blue;
            }
            _hpGageImage.color = Color.green;
        }
        _hpSlider.value = 1;
    }

    private void Event_TakeDamage(BattleEvent battleEvent, BattleTakeDamageEventArgs battleTakeDamageEventArgs)
    {
        _curHp -= battleTakeDamageEventArgs.amount;
        _hpSlider.value = _curHp / (float)_maxHp;
    }

    private IEnumerator CoTakeDamaged(ushort amount)
    {

        yield return null;
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _character = GetComponentInParent<Character>();
        _hpSlider = FindGameObjectInChildren<Slider>("HpSlider");
        _hpGageImage = FindGameObjectInChildren<Image>("HpGageImage");
        _nicknameText = FindGameObjectInChildren<TextMeshProUGUI>("NicknameText");
        _hpText = FindGameObjectInChildren<TextMeshProUGUI>("HpText");
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _character);
        CheckNullValue(this.name, _hpSlider);
        CheckNullValue(this.name, _hpGageImage);
        CheckNullValue(this.name, _nicknameText);
        CheckNullValue(this.name, _hpText);
    }


#endif

}
