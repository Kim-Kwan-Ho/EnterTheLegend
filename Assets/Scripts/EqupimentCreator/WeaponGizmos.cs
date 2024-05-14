using System.Collections;
using System.Collections.Generic;
using CustomizableCharacters;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class WeaponGizmos : MonoBehaviour
{
    [SerializeField]
    private List<WeaponEquipmentSO> _weaponList;
    private WeaponEquipmentSO _curWeapon;
    private int _weaponIndex;
    private Customizer _customizer;
    private void Awake()
    {
        _curWeapon = _weaponList[0];
        _customizer = GetComponent<Customizer>();
        _customizer.Add(_curWeapon.ItemCustomData);
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            _weaponIndex++;
            if (_weaponIndex >= _weaponList.Count)
                _weaponIndex = 0;
            _curWeapon = _weaponList[_weaponIndex];
            _customizer.Add(_curWeapon.ItemCustomData);
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;
        int segments = 20;
        float angle = _curWeapon.AttackAngle;
        float range = _curWeapon.AttackRange;

        Vector3 forward = transform.right; // 2D������ ���� right ���Ͱ� "��"�� ����ŵ�ϴ�.
        Vector3 startPoint = Quaternion.Euler(0, 0, -angle / 2) * forward;

        Gizmos.color = Color.red;
        for (int i = 0; i <= segments; i++)
        {
            Vector3 direction = Quaternion.Euler(0, 0, (angle / segments) * i) * startPoint;
            Vector3 from = transform.position;
            Vector3 to = transform.position + direction.normalized * range;
            if (i == 0 || i == segments)
            {
                Gizmos.DrawLine(from, to); // ��ä���� ���� ��輱�� �׸��ϴ�.
            }
            else
            {
                Gizmos.DrawLine(from + direction.normalized * range * ((i - 1) / (float)segments), to); // ��ä���� ���θ� �׸��ϴ�.
            }
        }
    }

#if UNITY_EDITOR 

#endif

}
