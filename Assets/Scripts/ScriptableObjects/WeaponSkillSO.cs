using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Objects/Skills/WeaponSkill")]
public class WeaponSkillSO : EquipmentSkillSO
{
    public GameObject SkillPrefab;
    public int SkillRange;
    public WeaponSkillType SkillType;

    public override void ActiveSkill()
    {
        
    }
}
