using UnityEngine;
using System.Collections;

public partial class PiyoMotion : MonoBehaviour
{
    Rigidbody2D Rigidbody2D;
    SpriteRenderer SpriteRenderer;

    void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }
}