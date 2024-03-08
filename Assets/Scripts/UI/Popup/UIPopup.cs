using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SortingLayer))]
public class UIPopup : BaseBehaviour
{

    public virtual void ClosePopup()
    {
        UIManager.Instance.ClosePopup(this);
    }


}
