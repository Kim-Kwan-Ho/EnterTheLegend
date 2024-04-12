using System.Collections;
using System.Collections.Generic;
using CustomizableCharacters;
using UnityEngine;

public class NewCharacterDisplay : MonoBehaviour
{
    public Customizer Customizer;
    public EquipmentSO item;



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Customizer.Add(item.ItemCustomData);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Customizer.Remove(item.ItemCustomData);
        }
    }




}
