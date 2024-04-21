using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEvent : MonoBehaviour
{
    public Action<BattleEvent, BattleInitializeEventArgs> OnInitialize;
    public Action<BattleEvent, BattleTakeDamageEventArgs> OnTakeDamage;
    public Action<BattleEvent, BattleAttackEventArgs> OnAttack;
    public Action<BattleEvent> StopAttack;

    public void CallInitialize(string nickname, ushort hp, bool isEnemy, bool isPlayer, int[] equipedItems)
    {
        OnInitialize?.Invoke(this, new BattleInitializeEventArgs() { nickname = nickname, hp = hp, isPlayer = isPlayer, isEnemy = isEnemy, equipedItems = equipedItems });
    }


    public void CallOnAttack(Direction direction, float aimAngle)
    {
        OnAttack?.Invoke(this, new BattleAttackEventArgs() {direction = direction, aimAngle = aimAngle});
    }
    public void CallStopAttack()
    {
        StopAttack?.Invoke(this);
    }
    public void CallTakeDamage(ushort amount)
    {
        OnTakeDamage?.Invoke(this, new BattleTakeDamageEventArgs() {amount = amount });
    }
}


public class BattleInitializeEventArgs
{
    public string nickname;
    public ushort hp;
    public bool isEnemy;
    public bool isPlayer;
    public int[] equipedItems;
}
public class BattleTakeDamageEventArgs : EventArgs
{
    public ushort amount;
}
public class BattleAttackEventArgs : EventArgs
{
    public Direction direction;
    public float aimAngle;
}