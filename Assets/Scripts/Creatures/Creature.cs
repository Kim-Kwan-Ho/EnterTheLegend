using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(StateEvent))]
[RequireComponent(typeof(DirectionEvent))]

[DisallowMultipleComponent]
public class Creature : BaseBehaviour
{
    [Header("State & LookPosition")]
    protected Direction _direction;
    protected State _state;

    [Header("Animation")]
    [SerializeField] protected Animator _animator;

    [Header("Events")]
    [SerializeField] public StateEvent EventState;
    [SerializeField] public DirectionEvent EventDirection;


    public virtual void Initialize()
    {
        _direction = Direction.Down;
        _state = State.Idle;
    }



    protected  virtual void OnEnable()
    {
        EventState.OnStateChanged += Event_OnStateChanged;
        EventDirection.OnDirectionChanged += Event_OnDirectionChanged;
    }

    protected virtual void OnDisable()
    {
        EventState.OnStateChanged -= Event_OnStateChanged;
        EventDirection.OnDirectionChanged -= Event_OnDirectionChanged;
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
        _animator = GetComponent<Animator>();
        EventState = GetComponent<StateEvent>();
        EventDirection = GetComponent<DirectionEvent>();
        
    }

    protected virtual void OnValidate()
    {
        CheckNullValue(this.name, _animator);
        CheckNullValue(this.name, EventState);
        CheckNullValue(this.name, EventDirection);
    }

#endif
}
