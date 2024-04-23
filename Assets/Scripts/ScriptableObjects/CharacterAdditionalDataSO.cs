using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterAdditionalData",menuName = "Scriptable Objects/Character/CharacterData")]
public class CharacterAdditionalDataSO : ScriptableObject
{

    [Header("Damaged Popup")]
    public GameObject DamageTextPrefab;

}
