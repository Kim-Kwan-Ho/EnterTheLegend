using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureSceneManager : SingletonMonobehaviour<AdventureSceneManager>
{

    [HideInInspector] public AdventureSceneState SceneState;
    private MyPlayer _player;
    [SerializeField] private GameObject _playerPrefab;
    protected override void Awake()
    {
        base.Awake();
        InitializePlayer();
    }

    public void InitializePlayer()
    {
        _player = Instantiate(_playerPrefab).GetComponent<MyPlayer>();
        _player.Initialize();
    }

    public MyPlayer GetMyPlayer()
    {
        return _player;
    }

#if UNITY_EDITOR


    private void OnValidate()
    {
        CheckNullValue(this.name, _playerPrefab);
    }
#endif
}
