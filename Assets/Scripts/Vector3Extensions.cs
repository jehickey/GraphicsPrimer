using UnityEngine;

public static class Vector3Extensions
{
    public static Vector3 Clamp(this Vector3 v, float min, float max)
    {
        v.x = Mathf.Clamp(v.x, min, max);
        v.y = Mathf.Clamp(v.x, min, max);
        v.z = Mathf.Clamp(v.x, min, max);
        return v;
    }
}
