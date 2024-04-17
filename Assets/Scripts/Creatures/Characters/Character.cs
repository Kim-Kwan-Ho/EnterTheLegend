using System;
using System.Collections;
using System.Collections.Generic;
using CustomizableCharacters;
using StandardData;
using UnityEngine;

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
    }
    protected virtual void OnEnable()
    {
        EventBattle.OnInitialize += Event_Initialize;
        EventBattle.OnTakeDamage += Event_TakeDamage;
        EventState.OnStateChanged += Event_OnStateChanged;
        EventDirection.OnDirectionChanged += Event_OnDirectionChanged;
        EventMovement.OnPositionMovement += Event_OnMovement;

    }

    protected virtual void OnDisable()
    {
        EventBattle.OnInitialize -= Event_Initialize;
        EventBattle.OnTakeDamage -= Event_TakeDamage;
        EventState.OnStateChanged -= Event_OnStateChanged;
        EventDirection.OnDirectionChanged -= Event_OnDirectionChanged;
        EventMovement.OnPositionMovement -= Event_OnMovement;

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
        _curHp = battleTakeDamageEventArgs.curHp;
        if (_curHp == 0)
        {
            EventState.CallStateChangedEvent(State.Death);
        }
    }
    private void Event_OnStateChanged(StateEvent stateEvent, StateEventArgs stateEventArgs)
    {
        _state = stateEventArgs.state;
        InitializeState();
        UpdateState();
    }

    protected virtual void Event_OnMovement(MovementEvent movementEvent, MovementEventArgs movementEventArgs)
    {
        transform.position = movementEventArgs.position;
    }
    private void Event_OnDirectionChanged(DirectionEvent directionEvent, DirectionEventArgs directionEventArgs)
    {
        _direction = directionEventArgs.direction;
        UpdateDirection();
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
    }


    private void ResetRig()
    {
        _downRig.SetActive(false);
        _upRig.SetActive(false);
        _sideRig.SetActive(false);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PoolManager.Instance.GetPool(_damageTextPrefab).OnSpawn();
        }
    }

    protected virtual void UpdateState()
    {
        switch (_state)
        {
            case State.Idle:
                break;
            case State.Move:
                _animator.SetFloat(AnimationSettings.Speed, 1);
                break;
            case State.Attack:
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

#endif
}
