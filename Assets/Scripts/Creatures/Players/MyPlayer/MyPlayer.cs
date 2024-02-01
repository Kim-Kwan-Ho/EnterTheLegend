using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;


[RequireComponent(typeof(MovementEvent))]
[RequireComponent(typeof(AttackEvent))]
[RequireComponent(typeof(Rigidbody2D))]


public class MyPlayer : Creature
{

    public MovementEvent EventMovement;
    public AttackEvent EventAttack;

    [SerializeField] private Rigidbody2D _rigid;
    private float _moveSpeed = 3f;


    protected override void OnEnable()
    {
        base.OnEnable();
        EventMovement.OnVelocityMovement += Event_OnMovement;
        EventMovement.OnStopMovement += Event_OnMovementStop;
        EventAttack.OnAttack += Event_OnAttack;
        
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventMovement.OnVelocityMovement -= Event_OnMovement;
        EventMovement.OnStopMovement -= Event_OnMovementStop;
        EventAttack.OnAttack -= Event_OnAttack;

    }

    private void Start()
    {
        _animator.speed = AnimationSettings.MyPlayerAnimationSpeed;
    }


    private void Event_OnMovement(MovementEvent movementEvent, MovementEventArgs movementEventArgs)
    {
        if (_state != State.Attack)
        {
            EventState.CallStateChangedEvent(State.Move);
            if (movementEventArgs.direction != Direction.Previous)
                EventDirection.CallDirectionChanged(movementEventArgs.direction);

        }
        MoveRigidPlayer(movementEventArgs.velocity);
    }

    private void Event_OnMovementStop(MovementEvent movementEvent)
    {
        StopRigidPlayer();
        EventState.CallStateChangedEvent(State.Idle);
    }
    private void Event_OnAttack(AttackEvent attackEvent, AttackEventArgs attackEventArgs)
    {
        EventState.CallStateChangedEvent(State.Attack);
        EventDirection.CallDirectionChanged(attackEventArgs.direction);
    }

    private void MoveRigidPlayer(Vector2 velocity)
    {
        _rigid.velocity = velocity * _moveSpeed;
    }
    private void StopRigidPlayer()
    {
        _rigid.velocity = Vector2.zero;
    }

#if UNITY_EDITOR

    protected override void OnBindField()
    {
        base.OnBindField();
        EventMovement = GetComponent<MovementEvent>();
        EventAttack = GetComponent<AttackEvent>();
        _rigid = GetComponent<Rigidbody2D>();
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        CheckNullValue(this.name, EventMovement);
        CheckNullValue(this.name, EventAttack);
        CheckNullValue(this.name, _rigid);
    }


#endif


}
