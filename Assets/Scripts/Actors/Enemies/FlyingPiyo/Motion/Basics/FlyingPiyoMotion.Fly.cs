using UnityEngine;
using System.Collections;

public partial class FlyingPiyoMotion : MonoBehaviour
{
    public void Fly(float speed, float direction)
    {
        Rigidbody2D.velocity = new Vector2(speed * direction, Rigidbody2D.velocity.y);
    }
}
