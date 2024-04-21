using System;
using System.Collections;
using System.Collections.Generic;
using CustomizableCharacters;
using StandardData;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(BattleEvent))]
[RequireComponent(typeof(StateEvent))]
[RequireComponent(typeof(DirectionEvent))]
[RequireComponent(typeof(MovementEvent))]

public class Character : BaseBehaviour
{
    [Header("UI")]
    [SerializeField]
    private CharacterInfo _characterInfo;
    [SerializeField]
    private GameObject _damageTextPrefab;


    [Header("Stats")]
    private ushort _curHp;

    [Header("Rig")]
    [SerializeField]
    protected GameObject _downRig;
    [SerializeField]
    protected GameObject _upRig;
    [SerializeField]
    protected GameObject _sideRig;


    [Header("State & LookPosition")]
    protected Direction _direction;
    protected State _state;

    [Header("Animation")]
    [SerializeField] protected Animator _animator;

    [Header("Events")]
    public BattleEvent EventBattle;
    public StateEvent EventState;
    public DirectionEvent EventDirection;
    public MovementEvent EventMovement;

    [Header("Equipments")]
    protected EquipmentSO _characterEquip;
    [SerializeField]
    protected WeaponEquipmentSO _weaponEquip;
    protected EquipmentSO _helmetEquip;
    protected EquipmentSO _armorEquip;
    protected EquipmentSO _shoesEquip;

    [Header("Customizer ")]
    [SerializeField]
    private Customizer _customizer;




    private void Awake()
    {
        _direction = Direction.Down;
        _state = State.Idle;
        _animator.speed = AnimationSettings.PlayerAnimationSpeed;
        InitializeState();
    }
    protected virtual void OnEnable()
    {
        EventBattle.OnInitialize += Event_Initialize;
        EventBattle.OnTakeDamage += Event_TakeDamage;
        EventBattle.OnAttack += Event_OnAttack;

        EventState.OnStateChanged += Event_StateChanged;
        EventDirection.OnDirectionChanged += Event_DirectionChanged;
        EventMovement.OnPositionMovement += Event_PositionMovement;


    }

    protected virtual void OnDisable()
    {
        EventBattle.OnInitialize -= Event_Initialize;
        EventBattle.OnTakeDamage -= Event_TakeDamage;
        EventBattle.OnAttack -= Event_OnAttack;

        EventState.OnStateChanged -= Event_StateChanged;
        EventDirection.OnDirectionChanged -= Event_DirectionChanged;
        EventMovement.OnPositionMovement -= Event_PositionMovement;

    }
    protected virtual void Event_Initialize(BattleEvent battleEvent, BattleInitializeEventArgs battleInitializeEventArgs)
    {
        _curHp = battleInitializeEventArgs.hp;
        for (int i = 0; i < battleInitializeEventArgs.equipedItems.Length; i++)
        {
            SetEquipment((EquipmentType)i, battleInitializeEventArgs.equipedItems[i]);
        }
    }

    protected virtual void Event_TakeDamage(BattleEvent battleEvent,
        BattleTakeDamageEventArgs battleTakeDamageEventArgs)
    {
        _curHp -= battleTakeDamageEventArgs.amount;

        IPoolable pool = PoolManager.Instance.GetPool(_damageTextPrefab);
        pool.OnSpawn((Vector2)transform.position + Vector2.up * 3);
        //if (_curHp == 0)
        //{
        //    EventState.CallStateChangedEvent(State.Death);
        //}
    }
    private void Event_StateChanged(StateEvent stateEvent, StateEventArgs stateEventArgs)
    {
        _state = stateEventArgs.state;
        InitializeState();
        UpdateState();
    }

    protected virtual void Event_PositionMovement(MovementEvent movementEvent, MovementEventArgs movementEventArgs)
    {
        bool movement = (movementEventArgs.position - transform.position).magnitude > 0.05f;
        if (movement)
        {
            transform.position = movementEventArgs.position;
            _animator.SetFloat(AnimationSettings.Speed, 1);
        }
    }
    private void Event_DirectionChanged(DirectionEvent directionEvent, DirectionEventArgs directionEventArgs)
    {
        _direction = directionEventArgs.direction;
        UpdateDirection();
    }

    protected virtual void Event_OnAttack(BattleEvent battleEvent, BattleAttackEventArgs battleAttackEventArgs)
    {

    }

    private void SetEquipment(EquipmentType type, int itemId)
    {
        EquipmentSO equipment =
            Utilities.ResourceLoader<EquipmentSO>($"Items/Item_{type.ToString()}", itemId);
        if (type == EquipmentType.Character)
        {
            _characterEquip = equipment;
        }
        else if (type == EquipmentType.Weapon)
        {
            _weaponEquip = (WeaponEquipmentSO)equipment;
        }
        else if (type == EquipmentType.Helmet)
        {
            _helmetEquip = equipment;
        }
        else if (type == EquipmentType.Armor)
        {
            _armorEquip = equipment;
        }
        else if (type == EquipmentType.Shoes)
        {
            _shoesEquip = equipment;
        }

        if (equipment != null && equipment.ItemCustomData != null)
            _customizer.Add(equipment.ItemCustomData);
    }


    private void ResetRig()
    {
        _downRig.SetActive(false);
        _upRig.SetActive(false);
        _sideRig.SetActive(false);
    }



    protected virtual void UpdateState()
    {
        switch (_state)
        {
            case State.Idle:
                break;
            case State.Move:
                break;
            case State.Attack:
                if (_weaponEquip?.WeaponType == WeaponType.Bow)
                {
                    _animator.SetTrigger(AnimationSettings.BowLoad);
                    _animator.SetTrigger(AnimationSettings.BowRelease);
                }
                else
                {
                    _animator.SetTrigger(AnimationSettings.Attack);
                }
                break;
            case State.Death:
                _animator.SetTrigger(AnimationSettings.Die);
                StartCoroutine(CoDeath());
                break;
        }
    }



    protected virtual void InitializeState()
    {
        _animator.SetFloat(AnimationSettings.Speed, 0);
    }




    protected virtual void UpdateDirection()
    {
        switch (_direction)
        {
            case Direction.Down:
                ResetRig();
                _downRig.SetActive(true);
                _animator.SetFloat(AnimationSettings.Direction, 2);
                break;
            case Direction.Right:
                ResetRig();
                _sideRig.transform.localScale = new Vector3(1, 1, 1);
                _sideRig.SetActive(true);
                _animator.SetFloat(AnimationSettings.Direction, 1);
                break;
            case Direction.Left:
                ResetRig();
                _sideRig.transform.localScale = new Vector3(-1, 1, 1);
                _sideRig.SetActive(true);
                _animator.SetFloat(AnimationSettings.Direction, 1);
                break;
            case Direction.Up:
                ResetRig();
                _upRig.SetActive(true);
                _animator.SetFloat(AnimationSettings.Direction, 0);
                break;
        }
    }

    protected virtual IEnumerator CoDeath()
    {
        Debug.Log("Death");
        yield return null;
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        EventBattle = GetComponent<BattleEvent>();
        _characterInfo = GetComponentInChildren<CharacterInfo>();
        _downRig = FindGameObjectInChildren("Down");
        _upRig = FindGameObjectInChildren("Up");
        _sideRig = FindGameObjectInChildren("Side");
        _animator = GetComponentInChildren<Animator>();
        EventState = GetComponent<StateEvent>();
        EventDirection = GetComponent<DirectionEvent>();
        EventMovement = GetComponent<MovementEvent>();
        _customizer = GetComponent<Customizer>();
    }

    protected virtual void OnValidate()
    {
        CheckNullValue(this.name, EventBattle);
        CheckNullValue(this.name, _characterInfo);
        CheckNullValue(this.name, _upRig);
        CheckNullValue(this.name, _downRig);
        CheckNullValue(this.name, _sideRig);
        CheckNullValue(this.name, _animator);
        CheckNullValue(this.name, EventState);
        CheckNullValue(this.name, EventDirection);
        CheckNullValue(this.name, EventMovement);
        CheckNullValue(this.name, _customizer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 3);

        Gizmos.DrawRay(transform.position, (Vector2.up + Vector2.right) * 3);
        Gizmos.DrawRay(transform.position, (Vector2.down + Vector2.right) * 3);
    }

#endif
}
