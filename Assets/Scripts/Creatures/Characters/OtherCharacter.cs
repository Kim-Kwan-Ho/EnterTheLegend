
public class OtherCharacter : Character
{
    protected override void Event_OnAttack(BattleEvent battleEvent, BattleAttackEventArgs battleAttackEventArgs)
    {
        //EventState.CallStateChangedEvent(State.Attack);
        //EventDirection.CallDirectionChanged(battleAttackEventArgs.direction);
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();

    }

    protected override void OnValidate()
    {
        base.OnValidate();
    }
#endif

}
