using Assets.HeroEditor.InventorySystem.Scripts.Elements;
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


    private EquipmentSO _equipment;


    private void Start()
    {
        _closeButton.onClick.AddListener(() => this.gameObject.SetActive(false));
        _equipButton.onClick.AddListener(OnEquip);
        _unequipButton.onClick.AddListener( OnUnequip);
        gameObject.SetActive(false);
    }


    public void SetItemInfo(Vector2 position, EquipmentSO equipment)
    {
        _equipment = equipment;
        transform.position = position;
        _nameText.text = equipment.Name;
        _descriptionText.text = equipment.Description;
        SetEquipButton();
        gameObject.SetActive(true);
    }

    private void OnEquip()
    {
        _equipment.IsEquipped = true;
        LobbySceneManager.Instance.EventLobbyScene.CallOnEquipChanged(_equipment.Type , _equipment);
        SetEquipButton();
    }

    private void OnUnequip()
    {
        _equipment.IsEquipped = false;
        LobbySceneManager.Instance.EventLobbyScene.CallOnEquipChanged(_equipment.Type, null);
        SetEquipButton();
    }

    private void SetEquipButton()
    {
        _equipButton.gameObject.SetActive(!_equipment.IsEquipped);
        _unequipButton.gameObject.SetActive(_equipment.IsEquipped);
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
