using UnityEngine;
using UnityEngine.UI;

public class CharacterChange : BaseBehaviour
{
    [Header("Buttons")]
    [SerializeField]
    private Button _characterChangeButton;
    [SerializeField]
    private Button _closeButton;


    [Header("Character & Stat &Equipment Change")]
    [SerializeField]
    private GameObject _characterChangePanel;

    private void Start()
    {
        _characterChangeButton.onClick.AddListener(() => _characterChangePanel.SetActive(true));
        _closeButton.onClick.AddListener(()=> _characterChangePanel.SetActive(false));
        _characterChangePanel.SetActive(false);
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _characterChangePanel = GameObject.Find("CharacterChangePanel");
        _characterChangeButton = FindGameObjectInChildren<Button>("CharacterChangeButton");
        _closeButton = FindGameObjectInChildren<Button>("CloseButton");
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _characterChangePanel);
        CheckNullValue(this.name, _characterChangeButton);
    }
#endif
}
