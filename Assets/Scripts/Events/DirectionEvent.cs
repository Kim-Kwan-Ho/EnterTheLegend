using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionEvent : MonoBehaviour
{
    public Action<DirectionEvent, DirectionEventArgs> OnDirectionChanged;

    public void CallDirectionChanged(Direction direction)
    {
        OnDirectionChanged?.Invoke(this, new DirectionEventArgs() { direction = direction });
    }


}

public class DirectionEventArgs : EventArgs
{
    public Direction direction;
}
