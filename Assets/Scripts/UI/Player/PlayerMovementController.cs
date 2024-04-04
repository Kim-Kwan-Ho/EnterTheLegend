using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovementController : BaseBehaviour, IPointerDownHandler ,IPointerUpHandler, IDragHandler
{
    
    [Header("UI SETTINGS TEST")]
    [SerializeField] private float _movementRange = 25;

    private MyPlayer _player = null;

    private Vector3 _startPosition = Vector3.zero;
    private Vector2 _velocity = Vector2.zero;

    private void Start()
    {
        _startPosition = transform.position;
        _player = BattleSceneManager.Instance.Player;
    }


    private void Update()
    {
        if (!CanContinueUsing())
            return;

        if (_velocity != Vector2.zero)
        {
            _player.EventMovement.CallVelocityMovement(_velocity, Utilities.GetDirectionFromVector(_velocity));

        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {}
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.position = _startPosition;
        _velocity = Vector2.zero;

        if (CanContinueUsing())
        {
            _player.EventMovement.CallStopMovement();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        
        Vector3 pos = (Vector3)eventData.position - _startPosition;
        pos = Vector3.ClampMagnitude(pos, _movementRange);
        transform.position = _startPosition + pos;
        _velocity = new Vector2(pos.x / _movementRange, pos.y / _movementRange);
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
