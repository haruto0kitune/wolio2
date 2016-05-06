using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class ConstantVelocity2D : MonoBehaviour
{
    [SerializeField]
    private Vector2 velocity;
    private Rigidbody2D Rigidbody2D;

    void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Rigidbody2D.velocity = velocity;
    }
}
