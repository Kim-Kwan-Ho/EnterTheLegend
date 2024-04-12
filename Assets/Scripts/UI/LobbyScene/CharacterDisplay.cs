using System.Collections.Generic;
using CustomizableCharacters;
using UnityEngine;

public class CharacterDisplay : BaseBehaviour
{
    [SerializeField]
    private Customizer _customizer;

    private void OnEnable()
    {
        LobbySceneManager.Instance.EventLobbyScene.OnEquipChanged += Event_EquipChanged;
    }

    private void OnDisable()
    {
        LobbySceneManager.Instance.EventLobbyScene.OnEquipChanged -= Event_EquipChanged;
    }

    private void Event_EquipChanged(LobbySceneEvent lobbySceneEvent, LobbySceneEquipChangedEventArgs lobbySceneEquipChangedEventArgs)
    {
        if (lobbySceneEquipChangedEventArgs.equipmentInfo == null)
        {
            _customizer.RemoveAllInCategory(lobbySceneEquipChangedEventArgs.category);
            return;
        }

        if (lobbySceneEquipChangedEventArgs.equipmentInfo.Equipment.ItemCustomData == null)
            return;

        _customizer.Add(lobbySceneEquipChangedEventArgs.equipmentInfo.Equipment.ItemCustomData);
    }


#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _customizer = GetComponent<Customizer>();
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _customizer);
    }

#endif
}
