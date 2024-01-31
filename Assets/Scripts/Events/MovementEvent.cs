using System;
using UnityEngine;

public class MovementEvent : MonoBehaviour
{
    public Action<MovementEvent, MovementEventArgs> OnVelocityMovement;
    public Action<MovementEvent, MovementEventArgs> OnPositionMovement;

    public void CallVelocityMovement(Vector2 velocity)
    {
        OnVelocityMovement?.Invoke(this, new MovementEventArgs() { velocity = velocity });
    }

    public void CallPositionMovement(Vector3 position)
    {
        OnPositionMovement?.Invoke(this, new MovementEventArgs(){position = position});
    }



}

public class MovementEventArgs : EventArgs
{
    public Vector2 velocity;
    public Vector3 position;
}
