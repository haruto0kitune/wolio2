using UnityEngine;
using System.Collections;

public static class Utility
{
    public static void Flip(Transform _Transform)
    {
        _Transform.localScale = new Vector2(_Transform.localScale.x * -1, _Transform.localScale.y);
        //_SpriteRenderer.flipX = !_SpriteRenderer.flipX;
    }

    public static Vector3 GetUnitVector(GameObject Source, GameObject Target)
    {
        var TargetParent = Target.transform.parent;
        Target.transform.parent = Source.transform;
        var UnitVector = Target.transform.localPosition.normalized;
        Target.transform.parent = TargetParent;

        return UnitVector;
    }

    public static float DegToRad(float Degree)
    {
        return Degree * Mathf.Deg2Rad;
    }

    public static int RadToDeg(float Radian)
    {
        return (int)(Radian * Mathf.Rad2Deg);
    }

    public static Vector2 PolarToRectangular2D(int Angle, float Radius)
    {
        var x = Mathf.Ceil(Radius * Mathf.Cos(DegToRad(Angle)));
        var y = Mathf.Ceil(Radius * Mathf.Sin(DegToRad(Angle)));

        return new Vector2(x, y);
    }

    public static Vector2 RectangularToPolar2D(float x, float y)
    {
        var r = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));
        var Theta = Mathf.Atan2(y, x) * Mathf.Rad2Deg; 

        return new Vector2(r, Theta);
    }
}