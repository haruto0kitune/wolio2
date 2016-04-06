using UnityEngine;
using System.Collections;

public static class Utility
{
    public static void Flip(this SpriteRenderer _SpriteRenderer)
    {
        _SpriteRenderer.flipX = !_SpriteRenderer.flipX;
    }

    //public static Vector2 GetUnitVector2()
    //{

    //}
}