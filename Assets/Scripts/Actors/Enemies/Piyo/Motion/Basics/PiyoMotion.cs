using UnityEngine;
using System.Collections;

public partial class PiyoMotion : MonoBehaviour
{
    Rigidbody2D Rigidbody2D;
    Utility Utility;
    SpriteRenderer SpriteRenderer;
    PiyoConfig PiyoConfig;
    PiyoState PiyoState;

    void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Utility = GetComponent<Utility>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        PiyoConfig = GetComponent<PiyoConfig>();
        PiyoState = GetComponent<PiyoState>();
    }
}