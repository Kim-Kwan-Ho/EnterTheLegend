using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour, IPoolable
{
    public int PoolKey { get; set; }

    public void OnSpawn()
    {
        gameObject.SetActive(true);
        Invoke("OnDeSpawn",3);
    }

    public void OnDeSpawn()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        PoolManager.Instance.ReturnToPool(this.gameObject);
        gameObject.SetActive(false);
    }

}
