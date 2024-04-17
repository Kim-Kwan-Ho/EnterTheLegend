using UnityEngine;

public abstract class EquipmentSkillSO : ScriptableObject
{
    public Sprite SkillIcon;
    public float CoolTime;

    public abstract void ActiveSkill();

}
