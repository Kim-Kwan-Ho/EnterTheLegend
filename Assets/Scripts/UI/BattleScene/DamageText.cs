using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour, IPoolable
{
    public int PoolKey { get; set; }

    public void OnSpawn(Vector2 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
        Invoke("OnDeSpawn", 2);
    }

    public void OnDeSpawn()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        PoolManager.Instance.ReturnToPool(this.gameObject);
        gameObject.SetActive(false);
    }

}
