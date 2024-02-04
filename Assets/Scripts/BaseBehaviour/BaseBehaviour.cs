using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public class BaseBehaviour : MonoBehaviour
{
#if UNITY_EDITOR // 에디터에서만 작동되게끔
    protected virtual void OnBindField() { } // 자식 클래스에서 바인딩을 처리할 부분

    // 유효값 검즘
    protected void CheckNullValue(string objectName, UnityEngine.Object obj) 
    {
        if (obj == null)
        {
            Debug.Log(objectName + " has null value");
        }
    }
    protected void CheckNullValue(string objectName, IEnumerable objs) 
    {
        if (objs == null)
        {
            Debug.Log(objectName + "has null value");
            return;
        }
        foreach (var obj in objs)
        {
            if (obj == null)
            {
                Debug.Log(objectName + "has null value");
            }
        }
    }
    protected List<T> GetComponentsInChildrenExceptThis<T>() where T : Component
    {
        T[] components = GetComponentsInChildren<T>();
        List<T> list = new List<T>();
        foreach (T component in components)
        {
            if (component == this)
            {
                continue;
            }
            else
            {
                list.Add(component);
            }
        }
        return list;
    }

#endif 
}

#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(BaseBehaviour), true)] 
[UnityEditor.CanEditMultipleObjects]
public class BehaviourBaseEditor : UnityEditor.Editor
{

    private MethodInfo _bindMethod = (typeof(BaseBehaviour)).GetMethod("OnBindField", BindingFlags.NonPublic | BindingFlags.Instance);
    public override void OnInspectorGUI()
    {
        
        if (GUILayout.Button("Bind Objects")) 
        {
            _bindMethod.Invoke(target ,new object[]{}); 
        }
        base.OnInspectorGUI();


    }
}
#endif 