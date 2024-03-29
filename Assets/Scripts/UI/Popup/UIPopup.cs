using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIPopup : BaseBehaviour
{

    public virtual void SetPopup<T>(T info) where T : struct
    {

    }
    public virtual void ClosePopup()
    {
        UIManager.Instance.ClosePopup(this);
    }


}
