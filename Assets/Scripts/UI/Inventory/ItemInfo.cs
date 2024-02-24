using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : BaseBehaviour
{

    [Header("Texts")] 
    [SerializeField] 
    private Text _nameText;
    [SerializeField]
    private Text _descriptionText;


    [Header("Buttons")]
    [SerializeField]
    private Button _equipButton;
    [SerializeField] 
    private Button _unequipButton;
    [SerializeField]
    private Button _closeButton;




    private void Start()
    {
        _closeButton.onClick.AddListener(() => this.gameObject.SetActive(false));
        _equipButton.onClick.AddListener(OnEquip);
        _unequipButton.onClick.AddListener(OnUnequip);
    }


    public void SetItemInfo(Vector2 position, EquipmentSO equipment = null)
    {
        transform.position = position;
        _equipButton.gameObject.SetActive(!equipment.IsEquipped);
        _unequipButton.gameObject.SetActive(equipment.IsEquipped);
        _nameText.text = equipment.Name;
        _descriptionText.text = equipment.Description;
        gameObject.SetActive(true);
    }

    private void OnEquip()
    {

    }

    private void OnUnequip()
    {

    }



#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _equipButton = GameObject.Find("EquipButton").GetComponent<Button>();
        _closeButton = GameObject.Find("InfoCloseButton").GetComponent<Button>();
        _unequipButton = GameObject.Find("UnequipButton").GetComponent<Button>();
        _nameText = GameObject.Find("ItemNameText").GetComponent<Text>();
        _descriptionText = GameObject.Find("DescriptionText").GetComponent<Text>();

    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _equipButton);
        CheckNullValue(this.name, _closeButton);
        CheckNullValue(this.name, _unequipButton);

    }
#endif

}
