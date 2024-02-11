using System.Collections;
using StandardData;
using UnityEngine;


[RequireComponent(typeof(AttackEvent))]
[RequireComponent(typeof(Rigidbody2D))]


public class MyPlayer : Creature
{
    public AttackEvent EventAttack;
    [SerializeField] private Rigidbody2D _rigid;
    private float _moveSpeed = 3f;

    private bool _canAttack = true;
    private float _attackCoolTime = 0.2f;
    
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
    }


    protected override void Event_OnMovement(MovementEvent movementEvent, MovementEventArgs movementEventArgs)
    {
        if (_state != State.Attack)
        {
            if (_state != State.Move)
                EventState.CallStateChangedEvent(State.Move);
            if (movementEventArgs.direction != _direction)
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
        if (_canAttack)
        {
            StartCoroutine(Event_OnAttackCoroutine(attackEventArgs));
        }
        
    }

    private IEnumerator Event_OnAttackCoroutine(AttackEventArgs attackEventArgs)
    {
        _canAttack = false;
        EventState.CallStateChangedEvent(State.Attack);
        EventDirection.CallDirectionChanged(attackEventArgs.direction);
        yield return new WaitForSeconds(_attackCoolTime);
        _canAttack = true;
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
        EventMovement = GetComponent<MovementEvent>();
        EventAttack = GetComponent<AttackEvent>();
        _rigid = GetComponent<Rigidbody2D>();
        base.OnBindField();
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
