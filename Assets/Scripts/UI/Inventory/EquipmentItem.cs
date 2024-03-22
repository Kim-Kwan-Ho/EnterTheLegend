using UnityEngine;
using UnityEngine.UI;

public class EquipmentItem : BaseBehaviour
{
    [SerializeField]
    private Image _itemImage;
    [SerializeField]
    private GameObject _equipedFrameGameObject;

    private EquipmentInfoSO _equipmentInfo;
    

    private void OnEnable()
    {
        LobbySceneManager.Instance.EventLobbyScene.OnEquipChanged += Event_EquipChanged;
    }


    private void OnDisable()
    {
        LobbySceneManager.Instance.EventLobbyScene.OnEquipChanged -= Event_EquipChanged;
    }

    public void SetEquipmentItem(EquipmentInfoSO equipmentInfo)
    {
        _equipmentInfo = equipmentInfo;
        _itemImage.sprite = _equipmentInfo.ItemIcon;
        _equipedFrameGameObject.SetActive(_equipmentInfo.IsEquipped);
    }

    private void Event_EquipChanged(LobbySceneEvent lobbySceneEvent,
        LobbySceneEquipChangedEventArgs lobbySceneEquipChangedEventArgs)
    {
        if (_equipmentInfo == lobbySceneEquipChangedEventArgs.equipmentInfo)
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
