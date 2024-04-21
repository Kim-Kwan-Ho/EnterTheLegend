using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonMonobehaviour<PoolManager>
{

    [SerializeField]
    private Pool[] pools = null;

    private Dictionary<int, Queue<IPoolable>> poolDic = new Dictionary<int, Queue<IPoolable>>();

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        for (int i = 0; i < pools.Length; i++)
        {
            CreatePool(pools[i].PoolSize, pools[i].Prefab);
        }
    }

    private void CreatePool(int poolSize, GameObject prefab)
    {
        int poolKey = prefab.GetInstanceID();
        poolDic[poolKey] = new Queue<IPoolable>();

        GameObject parent = Instantiate(new GameObject(prefab.name + " Pool"), transform);

        for (int i = 0; i < poolSize; i++)
        {
            GameObject go = (Instantiate(prefab, parent.transform));
            go.SetActive(false);
            IPoolable poolable = go.GetComponent<IPoolable>();
            poolable.PoolKey = poolKey;
            poolDic[poolKey].Enqueue(poolable);
        }
    }

    public IPoolable GetPool(GameObject gameObject)
    {
        int poolKey = gameObject.GetInstanceID();
        if (poolDic[poolKey].Count == 0)
        {
            throw new Exception(("Need More Pool"));
        }
        else
        {
            return poolDic[poolKey].Dequeue();
        }
    }
    public void ReturnToPool(GameObject gameObject)
    {
        IPoolable poolable = gameObject.GetComponent<IPoolable>();
        poolDic[poolable.PoolKey].Enqueue(poolable);
    }

}

[System.Serializable]
public struct Pool
{
    public int PoolSize;
    public GameObject Prefab;
}
public interface IPoolable
{
    int PoolKey { get; set; }
    void OnSpawn(Vector2 position);
    void OnDeSpawn();
}