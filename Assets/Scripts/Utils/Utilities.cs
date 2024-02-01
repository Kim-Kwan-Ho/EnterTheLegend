using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities 
{
    private static Direction GetDirection(float degrees)
    {
        if (degrees >= -22.5f && degrees < 22.5f)
            return Direction.Right;
        else if (degrees >= 22.5f && degrees < 67.5f)
            return Direction.UpRight;
        else if (degrees >= 67.5f && degrees < 112.5f)
            return Direction.Up;
        else if (degrees >= 112.5f && degrees < 157.5f)
            return Direction.UpLeft;
        else if ((degrees >= 157.5f && degrees <= 180) || (degrees >= -180 && degrees < -157.5f))
            return Direction.Left;
        else if (degrees >= -157.5f && degrees < -112.5f)
            return Direction.DownLeft;
        else if (degrees >= -112.5f && degrees < -67.5f)
            return Direction.Down;
        else 
            return Direction.DownRight;
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

    
}
