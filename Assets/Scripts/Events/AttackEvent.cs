using System;
using UnityEngine;

public class AttackEvent : MonoBehaviour
{
    public event Action<AttackEvent, AttackEventArgs> OnAttack;

    public void CallAttackEvent(Direction direction, float aimAngle)
    {
        OnAttack?.Invoke(this, new AttackEventArgs() { direction = direction, aimAngle = aimAngle });
    }

}

public class AttackEventArgs : EventArgs
{
    public Direction direction;
    public float aimAngle;
}
