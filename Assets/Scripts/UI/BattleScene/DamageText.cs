using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : BaseBehaviour, IPoolable
{
    [SerializeField]
    private TextMeshPro _text;



    [Header("Spawn Settings")]
    [SerializeField]
    private Vector2 _offSet;
    [SerializeField]
    private float _lifeTime = 1.0f;
    [SerializeField]
    private float _moveSpeed = 0.5f;
    public int PoolKey { get; set; }


    public void SetDamage(ushort damage)
    {
        _text.text = damage.ToString();
    }

    public void OnSpawn(Vector2 position)
    {
        transform.position = position + _offSet;
        gameObject.SetActive(true);

        StartCoroutine(CoVisualize());
    }

    public void OnDeSpawn()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        PoolManager.Instance.ReturnToPool(this.gameObject);
        gameObject.SetActive(false);
        _text.color = Color.red;
    }


    private IEnumerator CoVisualize()
    {
        float curTime = 0;
        while (curTime < _lifeTime)
        {
            transform.position += Vector3.up * _moveSpeed * Time.deltaTime;
            _text.color = new Color(255, 0, 0,  _lifeTime - curTime);
            curTime += Time.deltaTime;
            yield return null;
        }
        _text.color = new Color(255, 0, 0, 0);
        OnDeSpawn();
    }
#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _text = GetComponent<TextMeshPro>();
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _text);
    }

#endif
}
