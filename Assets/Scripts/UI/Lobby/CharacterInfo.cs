using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.Scripts.CharacterScripts;
using UnityEngine;

public class CharacterInfo : BaseBehaviour
{
    [Header("Character Texture")]
    [SerializeField]
    private Character _character; // �ܺ� ��ũ��Ʈ


    private void Start()
    {

    }

    private void OnDestroy()
    {

    }

    private void Event_SceneInitialize(LobbySceneEvent lobbySceneEvent,
        LobbySceneInitializeArgs lobbySceneInitializeArgs)
    {
        //_character.
    }
#if UNITY_EDITOR

    protected override void OnBindField()
    {
        base.OnBindField();
        _character = GameObject.FindObjectOfType<Character>();
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _character);
    }

#endif
}
