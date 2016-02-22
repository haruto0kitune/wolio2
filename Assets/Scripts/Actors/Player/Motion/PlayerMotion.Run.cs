using UnityEngine;
using System.Collections;

public partial class PlayerMotion : MonoBehaviour
{
    public void Run(float Horizontal, float MaxSpeed)
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(Horizontal * MaxSpeed, GetComponent<Rigidbody2D>().velocity.y);
    }
}
