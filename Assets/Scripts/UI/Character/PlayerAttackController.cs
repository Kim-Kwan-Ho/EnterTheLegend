using UnityEngine;
using UnityEngine.EventSystems;
public class PlayerAttackController : BaseBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [Header("UI SETTINGS")]
    [SerializeField] private float _movementRange = 25;

    [SerializeField] private float _attackDistanceRange = 0.4f;

    private Vector3 _startPosition = Vector3.zero;
    private bool _onAttack = false;
    private Vector2 _distance = Vector2.zero;
    
    [SerializeField]
    private MyCharacter _player;


    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        if (_onAttack)
        {
            _player.EventBattle.CallOnAttack(Utilities.GetDirectionFromVector(_distance), 0);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 pos = (Vector3)eventData.position - _startPosition;
        pos = Vector3.ClampMagnitude(pos, _movementRange);
        _distance = new Vector2(pos.x / _movementRange, pos.y / _movementRange);
        transform.position = _startPosition + pos;

        _onAttack = _distance.magnitude > _attackDistanceRange;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _onAttack = false;
        transform.position = _startPosition;
        _player.EventBattle.CallStopAttack();
        //_player.EventMovement.CallStopMovement();
    }




#if UNITY_EDITOR

    protected override void OnBindField()
    {
        base.OnBindField();
        _player = FindAnyObjectByType<MyCharacter>();
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _player);
    }

#endif
}
