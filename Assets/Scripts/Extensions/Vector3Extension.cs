using UnityEngine;
using System.Collections;

public static class Vector3Extension  {

    public static Vector3 Rotate(this Vector3 v, float degrees)
    {
        var radians = degrees * Mathf.Deg2Rad;
        var sin = Mathf.Sin(radians);
        var cos = Mathf.Cos(radians);
        var tx = v.x;
        var ty = v.y;

        return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
    }
}
