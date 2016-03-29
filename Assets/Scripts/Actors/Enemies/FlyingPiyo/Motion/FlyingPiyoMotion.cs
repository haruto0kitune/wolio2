using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Utility))]
[RequireComponent(typeof(FlyingPiyoState))]
public partial class FlyingPiyoMotion : MonoBehaviour
{
    Rigidbody2D Rigidbody2D;
    Utility Utility;
    FlyingPiyoState FlyingPiyoState;

    void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Utility = GetComponent<Utility>();
        FlyingPiyoState = GetComponent<FlyingPiyoState>();
    }
}