using UnityEngine;
using System.Collections;

public partial class PlayerMotion : MonoBehaviour
{
    public void Jump(float JumpForce)
    {
        Rigidbody2D.velocity = new Vector2(0f, JumpForce);
    }
}
