using System;
using System.Collections.Generic;
using CustomizableCharacters;
using StandardData;
using UnityEngine;


[RequireComponent(typeof(StateEvent))]
[RequireComponent(typeof(DirectionEvent))]
[RequireComponent(typeof(MovementEvent))]

public class Character : BaseBehaviour
{
    [Header("UI")]
    [SerializeField]
    private CharacterInfo _characterInfo;



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
    public StateEvent EventState;
    public DirectionEvent EventDirection;
    public MovementEvent EventMovement;

    [Header("Equipments")]
    private EquipmentSO _characterEquip;
    private EquipmentSO _weaponEquip;
    private EquipmentSO _helmetEquip;
    private EquipmentSO _armorEquip;
    private EquipmentSO _shoesEquip;

    [Header("Customizer ")]
    [SerializeField]
    private Customizer _customizer;




    private void Awake()
    {
        _direction = Direction.Down;
        _state = State.Idle;
        _animator.speed = AnimationSettings.PlayerAnimationSpeed;
    }
    public virtual void Initialize(string nickname, bool isEnemy, bool isPlayer, int[] equipedItems)
    {
        _characterInfo.SetCharacterInfoUi(nickname, isEnemy, isPlayer);
        for (int i = 0; i < equipedItems.Length; i++)
        {
            SetEquipment((EquipmentType)i, equipedItems[i]);
        }

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
            _weaponEquip = equipment;
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

    protected virtual void OnEnable()
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
                break;
            case State.Move:
                _animator.SetFloat(AnimationSettings.Speed, 1);
                break;
            case State.Attack:
                break;
        }
    }



    protected virtual void Event_OnMovement(MovementEvent movementEvent, MovementEventArgs movementEventArgs)
    {
        transform.position = movementEventArgs.position;
    }
    protected virtual void InitializeState()
    {
        _animator.SetFloat(AnimationSettings.Speed, 0);
    }



    private void Event_OnDirectionChanged(DirectionEvent directionEvent, DirectionEventArgs directionEventArgs)
    {
        _direction = directionEventArgs.direction;
        UpdateDirection();
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



#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
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
