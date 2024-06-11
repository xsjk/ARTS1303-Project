using UnityEngine;

public static class Vector2Extensions
{
    public static Vector2 FromAngle(float angle)
    {
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }
}
