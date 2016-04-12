using UnityEngine;
using System.Collections;

public partial class PlayerMotion : MonoBehaviour
{
    public void AirMove(float Horizontal)
    {
        if (Horizontal != 0)
        {
            Rigidbody2D.AddForce(new Vector2(50f * Horizontal, 0f));
        }
    }   
}
