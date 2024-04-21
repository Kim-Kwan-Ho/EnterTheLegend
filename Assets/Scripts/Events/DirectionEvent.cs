using System;
using UnityEngine;

public class DirectionEvent : MonoBehaviour
{
    public Action<DirectionEvent, DirectionEventArgs> OnDirectionChanged;

    public void CallDirectionChanged(Direction direction)
    {
        OnDirectionChanged?.Invoke(this, new DirectionEventArgs() { direction = direction });
        Debug.Log(direction);
    }


}

public class DirectionEventArgs : EventArgs
{
    public Direction direction;
}
