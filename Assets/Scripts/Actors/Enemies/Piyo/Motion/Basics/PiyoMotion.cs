using UnityEngine;
using System.Collections;

public partial class PiyoMotion : MonoBehaviour
{
    Rigidbody2D Rigidbody2D;
    Utility Utility;

    void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Utility = GetComponent<Utility>();
    }
}