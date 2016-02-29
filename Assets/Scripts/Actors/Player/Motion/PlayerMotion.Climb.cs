using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerMotion : MonoBehaviour
{
    public void Climb(float Vertical, float MaxSpeed)
    {
        Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, Vertical * MaxSpeed); 
    }
}
