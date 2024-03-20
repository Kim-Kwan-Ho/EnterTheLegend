using UnityEngine;
using UnityEngine.UI;

public class EquipmentItem : BaseBehaviour
{
    [SerializeField]
    private Image _itemImage;
    [SerializeField]
    private GameObject _equipedFrameGameObject;


    private EquipmentSO _equipment;
    
    public EquipmentSO EquipmentSo { get { return _equipment; } }

    private void OnEnable()
    {
        MyGameManager.Instance.EventGameManager.OnEquipChanged += Event_EquipChanged;
    }


    private void OnDisable()
    {
        MyGameManager.Instance.EventGameManager.OnEquipChanged -= Event_EquipChanged;
    }

    public void SetEquipmentItem(EquipmentSO equipment)
    {
        _equipment = equipment;
        _itemImage.sprite = _equipment.ItemSprite;
        _equipedFrameGameObject.SetActive(equipment.IsEquipped);
    }

    private void Event_EquipChanged(MyGameManagerEvent myGameManagerEvent,
        GameEquipChangedEventArgs equipChangedEventArgs)
    {

        if (_equipment == equipChangedEventArgs.equipment)
        {
            _equipedFrameGameObject.SetActive(true);
        }
        else if (_equipedFrameGameObject.activeSelf)
        {
            _equipedFrameGameObject.SetActive(false);
        }
        
    }

#if UNITY_EDITOR

    protected override void OnBindField()
    {
        base.OnBindField();
        _itemImage = GetComponentInChildrenExceptThis<Image>();
        _equipedFrameGameObject = GameObject.Find("EquipedFrameGameObject");
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _itemImage);
        CheckNullValue(this.name, _equipedFrameGameObject);
    }

#endif
}
