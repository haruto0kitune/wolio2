using UnityEngine;
using System.Collections;

public partial class PiyoMotion : MonoBehaviour
{
    Rigidbody2D Rigidbody2D;
    Utility Utility;
    BoxCollider2D BoxCollider2D;
    CircleCollider2D CircleCollider2D;
    SpriteRenderer SpriteRenderer;
    PiyoConfig PiyoConfig;
    PiyoState PiyoState;

    void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Utility = GetComponent<Utility>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
        CircleCollider2D = GetComponent<CircleCollider2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        PiyoConfig = GetComponent<PiyoConfig>();
        PiyoState = GetComponent<PiyoState>();
    }
}