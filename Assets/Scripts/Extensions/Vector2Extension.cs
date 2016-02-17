using UnityEngine;

// ReSharper disable once CheckNamespace
public static class Vector2Extension {
    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        var radians = degrees * Mathf.Deg2Rad;
        var sin = Mathf.Sin(radians);
        var cos = Mathf.Cos(radians);
        var tx = v.x;
        var ty = v.y;

        return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
    }

    public static Vector2 ShortenLength(this Vector2 vector, float reductionLength) {
        var temp = vector;
        temp *= (1 - reductionLength/vector.magnitude);
        return temp;
    }

    public static Vector2 SetLength(this Vector2 vector, float length) {
        return vector.normalized*length;
    }
}