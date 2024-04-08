using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : BaseBehaviour
{

    [Header("Texts")] 
    [SerializeField] 
    private TextMeshProUGUI _nameText;
    [SerializeField]
    private TextMeshProUGUI _descriptionText;


    [Header("Buttons")]
    [SerializeField]
    private Button _equipButton;
    [SerializeField] 
    private Button _unequipButton;
    [SerializeField]
    private Button _closeButton;


    private EquipmentInfoSO _equipmentInfo;


    private void Start()
    {
        _closeButton.onClick.AddListener(() => this.gameObject.SetActive(false));
        _equipButton.onClick.AddListener(OnEquip);
        _unequipButton.onClick.AddListener( OnUnequip);
        gameObject.SetActive(false);
    }


    public void SetItemInfo(Vector2 position, EquipmentInfoSO equipmentInfo)
    {
        _equipmentInfo = equipmentInfo;
        transform.position = position;
        _nameText.text = _equipmentInfo.Name;
        _descriptionText.text = _equipmentInfo.Description;
        SetEquipButton();
        gameObject.SetActive(true);
    }

    private void OnEquip()
    {
        _equipmentInfo.IsEquipped = true;
        LobbySceneManager.Instance.EventLobbyScene.CallOnEquipChanged(_equipmentInfo.Equipment.Type , _equipmentInfo);
        SetEquipButton();
    }

    private void OnUnequip()
    {
        _equipmentInfo.IsEquipped = false;
        LobbySceneManager.Instance.EventLobbyScene.CallOnEquipChanged(_equipmentInfo.Equipment.Type, null);
        SetEquipButton();
    }

    private void SetEquipButton()
    {
        _equipButton.gameObject.SetActive(!_equipmentInfo.IsEquipped);
        _unequipButton.gameObject.SetActive(_equipmentInfo.IsEquipped);
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _equipButton = GameObject.Find("EquipButton").GetComponent<Button>();
        _closeButton = GameObject.Find("InfoCloseButton").GetComponent<Button>();
        _unequipButton = GameObject.Find("UnequipButton").GetComponent<Button>();
        _nameText = GameObject.Find("ItemNameText").GetComponent<TextMeshProUGUI>();
        _descriptionText = GameObject.Find("DescriptionText").GetComponent<TextMeshProUGUI>();

    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _equipButton);
        CheckNullValue(this.name, _closeButton);
        CheckNullValue(this.name, _unequipButton);

    }
#endif

}
