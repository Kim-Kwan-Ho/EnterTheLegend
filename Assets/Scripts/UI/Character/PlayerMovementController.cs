using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovementController : BaseBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{

    [Header("UI SETTINGS TEST")]
    [SerializeField] private float _movementRange = 25;

    [SerializeField]
    private MyCharacter _player;

    private Vector3 _startPosition = Vector3.zero;
    private Vector2 _velocity = Vector2.zero;

    private void Awake()
    {
        _startPosition = transform.position;
    }


    private void Update()
    {


        if (_velocity != Vector2.zero)
        {
            _player.EventMovement.CallVelocityMovement(_velocity, Utilities.GetDirectionFromVector(_velocity));

        }
    }
    public void OnPointerDown(PointerEventData eventData)
    { }
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.position = _startPosition;
        _velocity = Vector2.zero;

        _player.EventMovement.CallStopMovement();
    }

    public void OnDrag(PointerEventData eventData)
    {

        Vector3 pos = (Vector3)eventData.position - _startPosition;
        pos = Vector3.ClampMagnitude(pos, _movementRange);
        transform.position = _startPosition + pos;
        _velocity = new Vector2(pos.x / _movementRange, pos.y / _movementRange);
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
