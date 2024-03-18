using System;
using StandardData;

public class PlayerStat
{
    private ushort _hp = 0;
    public ushort Hp { get { return _hp; } }
    private ushort _attack = 0;
    public ushort Attack { get { return _attack; } }
    private ushort _defense = 0;
    public ushort Defense { get { return _defense; } }

    private float _attackDistance = 0;
    public float AttackDistance { get { return _attackDistance; } }



    public void ChangeStat(bool equip, ushort hp, ushort attack, ushort defense, float attackDistance)
    {
        if (equip)
        {
            _hp = (ushort)Math.Clamp(_hp + hp, 0, PlayerMaximumStats.MaxHp);
            _attack = (ushort)Math.Clamp(_attack + attack, 0, PlayerMaximumStats.MaxAttack);
            _defense = (ushort)Math.Clamp(_defense + defense, 0, PlayerMaximumStats.MaxDefense);
            _attackDistance = (ushort)Math.Clamp(_attackDistance + attackDistance, 0, PlayerMaximumStats.MaxAttackDistance);
        }
        else
        {
            _hp = (ushort)Math.Clamp(_hp - hp, 0, PlayerMaximumStats.MaxHp);
            _attack = (ushort)Math.Clamp(_attack - attack, 0, PlayerMaximumStats.MaxAttack);
            _defense = (ushort)Math.Clamp(_defense - defense, 0, PlayerMaximumStats.MaxDefense);
            _attackDistance = (ushort)Math.Clamp(_attackDistance - attackDistance, 0, PlayerMaximumStats.MaxAttackDistance);
        }

    }

}
