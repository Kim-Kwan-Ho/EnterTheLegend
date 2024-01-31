using System.Collections;
using System.Collections.Generic;

using UnityEngine;


[RequireComponent(typeof(MovementEvent))]
[RequireComponent(typeof(Rigidbody2D))]


public class MyPlayer : Creature
{

    public MovementEvent EventMovement;
    [SerializeField] private Rigidbody2D _rigid;
    private float _moveSpeed = 3f;


    protected override void OnEnable()
    {
        base.OnEnable();
        EventMovement.OnVelocityMovement += Event_OnMovement;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventMovement.OnVelocityMovement -= Event_OnMovement;
    }

    private void Start()
    {
        _animator.speed = AnimationSettings.MyPlayerAnimationSpeed;
    }


    private void Event_OnMovement(MovementEvent movementEvent, MovementEventArgs movementEventArgs)
    {
        MoveRigidPlayer(movementEventArgs.velocity);
    }

    private void MoveRigidPlayer(Vector2 velocity)
    {
        _rigid.velocity = velocity * _moveSpeed;
    }


#if UNITY_EDITOR

    protected override void OnBindField()
    {
        base.OnBindField();
        EventMovement = GetComponent<MovementEvent>();
        _rigid = GetComponent<Rigidbody2D>();
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        CheckNullValue(this.name, EventMovement);
        CheckNullValue(this.name, _rigid);
    }


#endif


}
