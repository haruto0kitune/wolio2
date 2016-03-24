using UnityEngine;
using System.Collections;

public partial class PiyoMotion : MonoBehaviour
{
    public void Run(float speed, float direction)
    {
        Rigidbody2D.velocity = new Vector2(speed * direction, Rigidbody2D.velocity.y);
    }
}
