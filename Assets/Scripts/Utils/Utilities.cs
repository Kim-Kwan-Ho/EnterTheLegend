using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public static class Utilities 
{
    private static Direction GetDirection(float degrees)
    {
        if ((degrees >= -45f && degrees < 45f))
            return Direction.Right;
        else if ((degrees >= 45f && degrees < 135f))
            return Direction.Up;
        else if ((degrees >= -135f && degrees < -45f))
            return Direction.Down;
        else 
            return Direction.Left;
    }

    public static Direction GetDirectionFromVector(Vector2 vector)
    {
        float degrees = GetAngleFromVector(vector);
        return GetDirection(degrees);
    }

    public static float GetAngleFromVector(Vector2 vector)
    {
        float radians = Mathf.Atan2(vector.y, vector.x);
        float degrees = radians * Mathf.Rad2Deg;
        return degrees;
    }


    public static T ResourceLoader<T>(string path, int itemId) where T : ScriptableObject
    {
        return Resources.Load<T>(path + "/" + itemId.ToString());
    }
    public static List<T> ResourcesLoader<T>(string path, int[] itemId) where T : ScriptableObject
    {
        T[] resources = new T[itemId.Length];

        for (int i = 0; i < itemId.Length; i++)
        {
            resources[i] = Resources.Load<T>(path + "/" + itemId[i].ToString());
        }
        return resources.ToList();
    }
    public static List<T> ResourcesLoader<T>(string path) where T : ScriptableObject
    {
        var resources = Resources.LoadAll<T>(path);
        return resources.ToList();
    }

    public static byte[] GetObjectToByte<T>(T str) where T : struct
    {
        int size = Marshal.SizeOf(str);
        byte[] arr = new byte[size];
        IntPtr ptr = Marshal.AllocHGlobal(size);
        try
        {
            Marshal.StructureToPtr(str, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
        return arr;
    }
    public static T GetObjectFromByte<T>(byte[] arr) where T : struct
    {
        int size = Marshal.SizeOf<T>();
        IntPtr ptr = Marshal.AllocHGlobal(size);
        try
        {
            Marshal.Copy(arr, 0, ptr, size);
            return Marshal.PtrToStructure<T>(ptr);
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }

}
