using System.Collections;
using StandardData;
using UnityEngine;


[RequireComponent(typeof(MyCharacterDataSender))]
[RequireComponent(typeof(AttackEvent))]
[RequireComponent(typeof(Rigidbody2D))]
public class MyCharacter : Character
{
    public AttackEvent EventAttack;
    [SerializeField] 
    private Rigidbody2D _rigid;


    [Header("SKill UI")]
    [SerializeField]
    private SkillController _skillController;
    
    
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

    protected override void Event_Initialize(BattleEvent battleEvent, BattleInitializeEventArgs battleInitializeEventArgs)
    {
        base.Event_Initialize(battleEvent, battleInitializeEventArgs);
        _skillController.SetWeaponSkill(_weaponEquip?.WeaponSkill, UseWeaponSkill);
    }

    private void UseWeaponSkill()
    {
        Debug.Log("weaponSkill");
    }
    private void UseShoesSkill()
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
            StartCoroutine(CoOnAttack(attackEventArgs));
        }
        
    }

    private IEnumerator CoOnAttack(AttackEventArgs attackEventArgs)
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
        base.OnBindField();
        EventAttack = GetComponent<AttackEvent>();
        _rigid = GetComponent<Rigidbody2D>();
        _skillController = FindAnyObjectByType<SkillController>();
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        CheckNullValue(this.name, EventAttack);
        CheckNullValue(this.name, _rigid);
    }


#endif


}
