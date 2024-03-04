using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[RequireComponent(typeof(EquipChangedEvent))]
public class LobbySceneManager : SingletonMonobehaviour<LobbySceneManager>
{

    [SerializeField] 
    private Inventory _inventory;


    [Header("Events")]
    public EquipChangedEvent EventEquipChanged;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        TestUpdate();
    }

    private void OnEnable()
    {
        EventEquipChanged.OnEquipChanged += Event_OnEquipChanged;
    }

    private void OnDisable()
    {
        EventEquipChanged.OnEquipChanged -= Event_OnEquipChanged;
    }
    private void TestUpdate()
    {
        var items = Resources.LoadAll<EquipmentSO>("Items");
        _inventory.SetInventory(items);
    }

    public void Event_OnEquipChanged(EquipChangedEvent equipChangedEvent, EquipChangedEventArgs equipChangedEventArgs)
    {
        /*
         ���� ���ؼ� DB�� ����
         */
    }

#if UNITY_EDITOR

    protected override void OnBindField()
    {
        base.OnBindField();
        _inventory = GameObject.FindObjectOfType<Inventory>();
        EventEquipChanged = GetComponent<EquipChangedEvent>();
    }

    private void OnValidate()
    {
        CheckNullValue(this.name, _inventory);
        CheckNullValue(this.name, EventEquipChanged);
    }

#endif
}


