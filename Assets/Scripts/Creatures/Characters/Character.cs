using System;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(StateEvent))]
[RequireComponent(typeof(DirectionEvent))]
[RequireComponent(typeof(MovementEvent))]

public class Character : BaseBehaviour
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

    [Header("Equipments")]
    private EquipmentSO _characterEquip;
    private EquipmentSO _weaponEquip;
    private EquipmentSO _helmetEquip;
    private EquipmentSO _armorEquip;
    private EquipmentSO _shoesEquip;

    [Header("Sprite Renderer")]
    [SerializeField]
    private SpriteRenderer _characterSpr;
    [SerializeField]
    private SpriteRenderer _weaponSpr;
    [SerializeField]
    private SpriteRenderer _helmetSpr;
    [SerializeField]
    private List<SpriteRenderer> _armorSprList;
    [SerializeField]
    private SpriteRenderer[] _shoesSprs = new SpriteRenderer[2];



    private void Awake()
    {
        _direction = Direction.Down;
        _state = State.Idle;
        _animator.speed = AnimationSettings.PlayerAnimationSpeed;
    }
    public virtual void Initialize(int[] equipedItems)
    {
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
            if (equipment == null)
            {
                _characterSpr.sprite = null;
            }
            else
            {
                _characterEquip = equipment;
                _characterSpr.sprite = equipment.CharacterEquipmentSprite[0];
            }
        }
        else if (type == EquipmentType.Weapon)
        {
            if (equipment == null)
            {
                _weaponSpr.sprite = null;
            }
            else
            {
                _weaponEquip = equipment;
                _weaponSpr.transform.localPosition = equipment.EquipmentSpriteOffSet;
                _weaponSpr.sprite = equipment.CharacterEquipmentSprite[0];
            }
        }
        else if (type == EquipmentType.Helmet)
        {
            if (equipment == null)
            {
                _helmetSpr.sprite = null;
            }
            else
            {
                _helmetEquip = equipment;
                _helmetSpr.sprite = equipment.CharacterEquipmentSprite[0];
            }
        }
        else if (type == EquipmentType.Armor)
        {
            if (equipment == null)
            {
                for (int i = 0; i < _armorSprList.Count; i++)
                {
                    _armorSprList[i].sprite = null;
                }
            }
            else
            {
                _armorEquip = equipment;
                for (int i = 0; i < equipment.CharacterEquipmentSprite.Count; i++)
                {
                    _armorSprList[i].sprite = equipment.CharacterEquipmentSprite[i];
                }
                _armorSprList[^1].sprite = equipment.CharacterEquipmentSprite[^1];
            }
        }
        else if (type == EquipmentType.Shoes)
        {
            if (equipment == null)
            {
                _shoesSprs[0].sprite = null;
                _shoesSprs[1].sprite = null;
            }
            else
            {
                _shoesEquip = equipment;
                _shoesSprs[0].sprite = equipment.CharacterEquipmentSprite[0];
                _shoesSprs[1].sprite = equipment.CharacterEquipmentSprite[0];
            }
        }
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
        _characterSpr = FindGameObjectInChildren<SpriteRenderer>("Hair");
        _helmetSpr = FindGameObjectInChildren<SpriteRenderer>("Helmet");
        _weaponSpr = FindGameObjectInChildren<SpriteRenderer>("Weapon");
        _shoesSprs[0] = FindGameObjectInChildren<SpriteRenderer>("ShinL[Armor]");
        _shoesSprs[1] = FindGameObjectInChildren<SpriteRenderer>("ShinR[Armor]");
    }

    protected virtual void OnValidate()
    {
        CheckNullValue(this.name, _animator);
        CheckNullValue(this.name, EventState);
        CheckNullValue(this.name, EventDirection);
        CheckNullValue(this.name, EventMovement);
        CheckNullValue(this.name, _characterSpr);
        CheckNullValue(this.name, _helmetSpr);
        CheckNullValue(this.name, _weaponSpr);
        CheckNullValue(this.name, _shoesSprs);
    }

#endif
}
