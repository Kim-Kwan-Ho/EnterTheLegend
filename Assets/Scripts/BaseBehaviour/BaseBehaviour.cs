using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;


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
            if (component.gameObject.GetInstanceID() == this.gameObject.GetInstanceID())
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
    protected GameObject FindGameObjectInChildren(string name)
    {
        var objects = GetComponentsInChildren<Transform>(true);
        foreach (var obj in objects)
        {
            if (obj.gameObject.name.Equals(name))
                return obj.gameObject;
        }
        return null;
    }
    protected T FindGameObjectInChildren<T>(string name) where T : Component
    {
        T[] objects = GetComponentsInChildren<T>(true);
        foreach (var obj in objects)
        {
            if (obj.gameObject.name.Equals(name))
                return obj;
        }
        return null;
    }
    protected T[] GetComponentsInGameObject<T>(string name) where T : Component
    {
        GameObject gob = GameObject.Find(name);
        return gob.GetComponentsInChildren<T>(true);
    }

    protected T GetComponentInChildrenExceptThis<T>() where T : Component
    {
        T[] components = GetComponentsInChildren<T>(true);
        foreach (T component in components)
        {
            if (component.gameObject.GetInstanceID() == this.gameObject.GetInstanceID())
            {
                continue;
            }
            else
            {
                return component;
            }
        }

        return null;
    }

#endif 
}

#if UNITY_EDITOR
[CustomEditor(typeof(BaseBehaviour), true)] 
[CanEditMultipleObjects]
public class BehaviourBaseEditor : Editor
{

    private MethodInfo _bindMethod = (typeof(BaseBehaviour)).GetMethod("OnBindField", BindingFlags.NonPublic | BindingFlags.Instance);
    public override void OnInspectorGUI()
    {
        
        if (GUILayout.Button("Bind Objects")) 
        {
            _bindMethod.Invoke(target ,new object[]{});
            EditorUtility.SetDirty(target);
        }
        base.OnInspectorGUI();


    }
}
#endif 