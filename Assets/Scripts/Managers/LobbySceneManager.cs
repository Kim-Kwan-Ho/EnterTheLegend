using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LobbySceneManager : SingletonMonobehaviour<LobbySceneManager>
{
    [SerializeField] 
    private Inventory _inventory;


    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        TestUpdate();
    }

    private void TestUpdate()
    {
        var items = Resources.LoadAll<EquipmentSO>("Items");
        _inventory.SetInventory(items);
    }


#if UNITY_EDITOR

    protected override void OnBindField()
    {
        base.OnBindField();
        _inventory = GameObject.FindObjectOfType<Inventory>();
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _inventory);
    }

#endif
}


