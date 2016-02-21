using UnityEngine;
using System.Collections;

public partial class PlayerMotion : MonoBehaviour
{
    public void Jump(float JumpForce, LayerMask WhatIsGround)
    {
        if ((bool)Physics2D.Linecast(transform.position, new Vector2(transform.position.x, transform.position.y - 0.539f), WhatIsGround))
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, JumpForce));
        }
    }
}
