using StandardData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfo : BaseBehaviour
{

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
    public void SetCharacterInfoUi(string nickname, bool isEnemy, bool isPlayer)
    {
        _nicknameText.text = nickname;
        if (isEnemy)
        {
            _nicknameText.color = Color.red;
            _hpGageImage.color = Color.red;
        }
        else
        {
            if (isPlayer)
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

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _hpSlider = FindGameObjectInChildren<Slider>("HpSlider");
        _hpGageImage = FindGameObjectInChildren<Image>("HpGageImage");
        _nicknameText = FindGameObjectInChildren<TextMeshProUGUI>("NicknameText");
        _hpText = FindGameObjectInChildren<TextMeshProUGUI>("HpText");
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _hpSlider);
        CheckNullValue(this.name, _hpGageImage);
        CheckNullValue(this.name, _nicknameText);
        CheckNullValue(this.name, _hpText);
    }


#endif

}
