using UnityEngine;


[RequireComponent(typeof(StateEvent))]
[RequireComponent(typeof(DirectionEvent))]
[RequireComponent(typeof(MovementEvent))]

public class Creature : BaseBehaviour
{
    [Header("State & LookPosition")]
    protected Direction _direction;
    protected State _state;

    [Header("Animation")]
    [SerializeField] protected Animator _animator;

    [Header("Events")]
    public StateEvent EventState;
    public DirectionEvent EventDirection;
    public MovementEvent EventMovement;


    public virtual void Initialize()
    {
        _direction = Direction.Down;
        _state = State.Idle;
        _animator.speed = AnimationSettings.PlayerAnimationSpeed;
    }



    protected  virtual void OnEnable()
    {
        EventState.OnStateChanged += Event_OnStateChanged;
        EventDirection.OnDirectionChanged += Event_OnDirectionChanged;
        EventMovement.OnPositionMovement += Event_OnMovement;

    }

    protected virtual void OnDisable()
    {
        EventState.OnStateChanged -= Event_OnStateChanged;
        EventDirection.OnDirectionChanged -= Event_OnDirectionChanged;
        EventMovement.OnPositionMovement -= Event_OnMovement;

    }


    private void Event_OnStateChanged(StateEvent stateEvent, StateEventArgs stateEventArgs)
    {
        _state = stateEventArgs.state;
        InitializeState();
        UpdateState();
    }
    protected virtual void UpdateState()
    {
        switch (_state)
        {
            case State.Idle:
                _animator.SetBool(AnimationSettings.IsIdle, true);
                break;
            case State.Move:
                _animator.SetBool(AnimationSettings.IsMoving, true);
                break;
            case State.Attack:
                _animator.SetBool(AnimationSettings.IsAttacking, true);
                break;
        }
    }



    protected virtual void Event_OnMovement(MovementEvent movementEvent, MovementEventArgs movementEventArgs)
    {
        transform.position = movementEventArgs.position;
    }
    protected virtual void InitializeState()
    {
        _animator.SetBool(AnimationSettings.IsMoving, false);
        _animator.SetBool(AnimationSettings.IsIdle, false);
        _animator.SetBool(AnimationSettings.IsAttacking, false);

    }



    private void Event_OnDirectionChanged(DirectionEvent directionEvent, DirectionEventArgs directionEventArgs)
    {

        _direction = directionEventArgs.direction;
        InitializeDirection();
        UpdateDirection();
    }
    protected virtual void UpdateDirection()
    {
        switch (_direction)
        {
            case Direction.Down:
                _animator.SetFloat(AnimationSettings.Vertical, -1);
                _animator.SetFloat(AnimationSettings.Horizontal, 0);
                break;
            case Direction.DownLeft:
                _animator.SetFloat(AnimationSettings.Vertical, -1);
                _animator.SetFloat(AnimationSettings.Horizontal, -1);
                break;
            case Direction.DownRight:
                _animator.SetFloat(AnimationSettings.Vertical, -1);
                _animator.SetFloat(AnimationSettings.Horizontal, 1);
                break;
            case Direction.Up:
                _animator.SetFloat(AnimationSettings.Vertical, 1);
                _animator.SetFloat(AnimationSettings.Horizontal, 0);
                break;
            case Direction.UpRight:
                _animator.SetFloat(AnimationSettings.Vertical, 1);
                _animator.SetFloat(AnimationSettings.Horizontal, 1);
                break;
            case Direction.UpLeft:
                _animator.SetFloat(AnimationSettings.Vertical, 1);
                _animator.SetFloat(AnimationSettings.Horizontal, -1);
                break;
            case Direction.Left:
                _animator.SetFloat(AnimationSettings.Vertical, 0);
                _animator.SetFloat(AnimationSettings.Horizontal, -1);
                break;
            case Direction.Right:
                _animator.SetFloat(AnimationSettings.Vertical, 0);
                _animator.SetFloat(AnimationSettings.Horizontal, 1);
                break;
        }
    }

    protected  virtual void InitializeDirection()
    {
        _animator.SetFloat(AnimationSettings.Vertical, 0);
        _animator.SetFloat(AnimationSettings.Horizontal, 0);
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _animator = GetComponentInChildren<Animator>();
        EventState = GetComponent<StateEvent>();
        EventDirection = GetComponent<DirectionEvent>();
        EventMovement = GetComponent<MovementEvent>();
    }

    protected virtual void OnValidate()
    {
        CheckNullValue(this.name, _animator);
        CheckNullValue(this.name, EventState);
        CheckNullValue(this.name, EventDirection);
        CheckNullValue(this.name, EventMovement);
    }

#endif
}
