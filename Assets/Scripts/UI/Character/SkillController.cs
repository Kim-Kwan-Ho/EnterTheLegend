using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillController : BaseBehaviour
{

    [Header("Weapon Skill")]
    [SerializeField]
    private Button _weaponSkillButton;
    [SerializeField]
    private Image _weaponSkillImage;

    [Header("Shoes Skill")]
    [SerializeField]
    private Button _shoesSkillButton;
    [SerializeField]
    private Image _shoesSkillImage;



    public void SetWeaponSkill(WeaponSkillSO skillSO, Action action)
    {
        if (skillSO == null)
            return;

        _weaponSkillImage.sprite = skillSO.SkillIcon;
        _weaponSkillButton.onClick.AddListener(() => action?.Invoke());
        _weaponSkillButton.onClick.AddListener(() => StartCoroutine(CoSkillCoolTime(_weaponSkillButton, _weaponSkillImage, skillSO.CoolTime)));
    }
    public void SetShoesSkill(ShoesSkillSO skillSO, Action action)
    {
        if (skillSO == null)
            return;


        _weaponSkillImage.sprite = skillSO.SkillIcon;
        _weaponSkillButton.onClick.AddListener(() => action?.Invoke());
    }

    private IEnumerator CoSkillCoolTime(Button button, Image image, float time)
    {
        _weaponSkillButton.interactable = false;
        image.fillAmount = 0;
        float curTime = 0;
        while (curTime < time)
        {
            image.fillAmount = curTime / time;
            curTime += Time.deltaTime;
            yield return null;
        }
        image.fillAmount = 1;
        _weaponSkillButton.interactable = true;
    }


#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _weaponSkillButton = FindGameObjectInChildren<Button>("WeaponSkillButton");
        _weaponSkillImage = FindGameObjectInChildren<Image>("WeaponSkillImage");
        _shoesSkillButton = FindGameObjectInChildren<Button>("ShoesSkillButton");
        _shoesSkillImage = FindGameObjectInChildren<Image>("ShoesSkillImage");


    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _weaponSkillButton);
        CheckNullValue(this.name, _weaponSkillImage);
        CheckNullValue(this.name, _shoesSkillButton);
        CheckNullValue(this.name, _shoesSkillImage);
    }

#endif
}
