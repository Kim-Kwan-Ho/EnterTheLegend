using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StateEvent : MonoBehaviour
{
    public event Action<StateEvent, StateEventArgs> OnStateChanged;
    public void CallStateChangedEvent(State state)
    {
        OnStateChanged?.Invoke(this, new StateEventArgs() { state = state });
    }
}

public class StateEventArgs : EventArgs
{
    private bool isEnemy = false;
    public State state;
}
