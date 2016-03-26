using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Utility))]
public partial class FlyingPiyoMotion : MonoBehaviour
{
    Rigidbody2D Rigidbody2D;
    Utility Utility;

    void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Utility = GetComponent<Utility>();
    }
}