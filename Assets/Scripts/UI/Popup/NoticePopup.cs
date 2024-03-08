using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoticePopup : UIPopup
{
    [SerializeField]
    private TextMeshProUGUI _infoText;
    [SerializeField]
    private Button _confirmButton;


    private void Start()
    {
        _confirmButton.onClick.AddListener(ClosePopup);
    }
    public void SetText(string text)
    {
        _infoText.text = text;
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _infoText = FindGameObjectInChildren<TextMeshProUGUI>("InfoText");
        _confirmButton = FindGameObjectInChildren<Button>("ConfirmButton");
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _infoText);
        CheckNullValue(this.name, _confirmButton);
    }
#endif
}
