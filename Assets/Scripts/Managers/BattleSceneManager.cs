using UnityEngine;



[RequireComponent(typeof(BattleSceneEvent))]
public class BattleSceneManager : SingletonMonobehaviour<BattleSceneManager>
{
    [Header("Events")]
    [SerializeField]
    public BattleSceneEvent EventBattleScene;


    protected GameSceneState _sceneState;
    public GameSceneState SceneState { get { return _sceneState; } }
    protected ushort _roomId;
    public ushort RoomId { get { return _roomId; } }
    protected ushort _playerIndex;
    public ushort PlayerIndex { get { return _playerIndex; } }

    [Header("My Character")]
    [SerializeField]
    protected MyCharacter _player;
    public MyCharacter Player { get { return _player; } }



#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        EventBattleScene = GetComponent<BattleSceneEvent>();
        _player = FindObjectOfType<MyCharacter>();
    }

    protected virtual void OnValidate()
    {
        CheckNullValue(this.name, EventBattleScene);
        CheckNullValue(this.name, _player);
    }

#endif
}
