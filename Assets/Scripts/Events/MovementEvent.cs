using System;
using UnityEngine;

public class MovementEvent : MonoBehaviour
{
    public Action<MovementEvent, MovementEventArgs> OnVelocityMovement;
    public Action<MovementEvent, MovementEventArgs> OnPositionMovement;
    public Action<MovementEvent> OnStopMovement;
    public void CallVelocityMovement(Vector2 velocity, Direction direction)
    {
        OnVelocityMovement?.Invoke(this, new MovementEventArgs() { velocity = velocity, direction = direction});
    }

    public void CallPositionMovement(Vector2 position)
    {
        OnPositionMovement?.Invoke(this, new MovementEventArgs(){position = position});
    }

    public void CallStopMovement()
    {
        OnStopMovement?.Invoke(this);
    }



}

public class MovementEventArgs : EventArgs
{
    public Vector2 velocity;
    public Vector3 position;
    public Direction direction;
}
