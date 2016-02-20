using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class ConstantVelocity2D : MonoBehaviour
{
    [SerializeField]
    private Vector2 velocity;

    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = velocity;
    }
}
