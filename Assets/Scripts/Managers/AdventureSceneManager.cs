using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureSceneManager : SingletonMonobehaviour<AdventureSceneManager>
{

    [HideInInspector] public AdventureSceneState SceneState;
    public MyPlayer Player;
    public GameObject PlayerPrefab;
    protected override void Awake()
    {
        base.Awake();
        InitializePlayer();
    }

    public void InitializePlayer()
    {
        Player = Instantiate(PlayerPrefab).GetComponent<MyPlayer>();
        Player.Initialize();
    }

    public MyPlayer GetMyPlayer()
    {
        return Player.GetComponent<MyPlayer>();
    }

#if UNITY_EDITOR


    private void OnValidate()
    {
        CheckNullValue(this.name, PlayerPrefab);
    }
#endif
}
