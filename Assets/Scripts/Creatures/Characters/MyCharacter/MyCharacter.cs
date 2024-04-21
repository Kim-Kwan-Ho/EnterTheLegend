using System.Collections;
using StandardData;
using UnityEngine;


[RequireComponent(typeof(MyCharacterDataSender))]
[RequireComponent(typeof(Rigidbody2D))]
public class MyCharacter : Character
{
    [SerializeField] 
    private Rigidbody2D _rigid;


    [Header("SKill UI")]
    [SerializeField]
    private SkillController _skillController;
    
    
    private float _moveSpeed = 3f;
    private bool _canAttack = true;
    private bool _isAttacking = false;
    private float _attackCoolTime = 1.2f;

    [Header("Data Sender")]
    [SerializeField]
    private MyCharacterDataSender _dataSender;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        EventMovement.OnVelocityMovement += Event_PositionMovement;
        EventMovement.OnStopMovement += Event_MovementStop;
        EventBattle.StopAttack += Event_StopAttack;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventMovement.OnVelocityMovement -= Event_PositionMovement;
        EventMovement.OnStopMovement -= Event_MovementStop;
        EventBattle.StopAttack -= Event_StopAttack;
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

   



    protected override void Event_PositionMovement(MovementEvent movementEvent, MovementEventArgs movementEventArgs)
    {
        _animator.SetFloat(AnimationSettings.Speed, 1);
        if (!_isAttacking)
        {
            if (_state != State.Move)
                EventState.CallStateChangedEvent(State.Move);
            if (movementEventArgs.direction != _direction)
                EventDirection.CallDirectionChanged(movementEventArgs.direction);

        }
        MoveRigidPlayer(movementEventArgs.velocity * 3);
    }

    private void Event_MovementStop(MovementEvent movementEvent)
    {
        StopRigidPlayer();
        EventState.CallStateChangedEvent(State.Idle);
    }
    protected override void Event_OnAttack(BattleEvent battleEvent, BattleAttackEventArgs battleAttackEventArgs)
    {
        _isAttacking = true;
        if (battleAttackEventArgs.direction != _direction)
            EventDirection.CallDirectionChanged(battleAttackEventArgs.direction);
        if (_canAttack)
        {
            EventState.CallStateChangedEvent(State.Attack);
            _dataSender.SendPlayerOnAttack();
            StartCoroutine(CoAttackTimer());
        }
    }

    private void Event_StopAttack(BattleEvent battleEvent)
    {
        _isAttacking = false;
    }

    private IEnumerator CoAttackTimer()
    {
        _canAttack = false;
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
        _rigid = GetComponent<Rigidbody2D>();
        _skillController = FindAnyObjectByType<SkillController>();
        _dataSender = GetComponent<MyCharacterDataSender>();
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        CheckNullValue(this.name, _rigid);
        CheckNullValue(this.name, _skillController);
        CheckNullValue(this.name, _dataSender);
    }


#endif


}
