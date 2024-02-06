using UnityEngine;


[CreateAssetMenu(fileName = "PlayerDetails_", menuName = "Scriptable Object/Player/PlayerDetails")]
public class PlayerDetailSO : ScriptableObject
{
    public GameObject PlayerPrefab;
    public float Hp;
    public float AttackDistance;



}
