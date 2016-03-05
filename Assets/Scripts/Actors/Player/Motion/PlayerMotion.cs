using UnityEngine;
using System.Collections;

public partial class PlayerMotion : MonoBehaviour
{
    Rigidbody2D Rigidbody2D;

    void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }
}
