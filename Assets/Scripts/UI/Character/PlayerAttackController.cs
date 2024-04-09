using UnityEngine;
using UnityEngine.EventSystems;
public class PlayerAttackController : BaseBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [Header("UI SETTINGS TEST")]
    [SerializeField] private float _movementRange = 25;

    [SerializeField] private float _distanceRange = 0.3f;

    private Vector3 _startPosition = Vector3.zero;
    private bool _isDragging = false;
    private Vector2 _distance = Vector2.zero;

    private MyCharacter _player = null;

    

    private void Start()
    {
        _startPosition = transform.position;
        _player = BattleSceneManager.Instance.Player;
    }

    private void Update()
    {
        if (_isDragging)
        {
            _player.EventAttack.CallAttackEvent(Utilities.GetDirectionFromVector(_distance), 0);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 pos = (Vector3)eventData.position - _startPosition;
        pos = Vector3.ClampMagnitude(pos, _movementRange);
        _distance = new Vector2(pos.x / _movementRange, pos.y / _movementRange);
        transform.position = _startPosition + pos;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isDragging = false;
        _player.EventState.CallStateChangedEvent(State.Idle);

        transform.position = _startPosition;

        if (CanContinueUsing())
        {
            _player.EventMovement.CallStopMovement();
        }
    }

    private bool CanContinueUsing()
    {
        return TeamBattleSceneManager.Instance.SceneState != GameSceneState.MyPlayerDeath &&
               TeamBattleSceneManager.Instance.SceneState != GameSceneState.ClearFailed;
    }


#if UNITY_EDITOR

    protected override void OnBindField()
    {
        base.OnBindField();
    }
    private void OnValidate()
    {
    }   
#endif
}