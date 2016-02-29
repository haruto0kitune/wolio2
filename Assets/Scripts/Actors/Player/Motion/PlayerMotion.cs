using UnityEngine;
using System.Collections;

public partial class PlayerMotion : MonoBehaviour
{
    Rigidbody2D Rigidbody2D;

    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }
    // This file exists for attaching to GameObject.
}
