using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FlyingPiyoState))]
public partial class FlyingPiyoMotion : MonoBehaviour
{
    Rigidbody2D Rigidbody2D;
    SpriteRenderer SpriteRenderer;

    void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }
}