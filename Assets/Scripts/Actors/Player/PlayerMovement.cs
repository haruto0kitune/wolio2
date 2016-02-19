using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public void Run(float Horizontal, float MaxSpeed)
    {
        TurnAround(Horizontal);
        GetComponent<Rigidbody2D>().velocity = new Vector2(Horizontal * MaxSpeed, GetComponent<Rigidbody2D>().velocity.y);
    }

    public void Jump(float JumpForce, LayerMask WhatIsGround)
    {
        if ((bool)Physics2D.Linecast(transform.position, new Vector2(transform.position.x, transform.position.y - 0.539f), WhatIsGround))
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, JumpForce));
        }
    }

    public void TurnAround(float Horizontal)
    {
        if ((Horizontal > 0 && !(GetComponent<PlayerState>().FacingRight.Value)) || (Horizontal < 0 && GetComponent<PlayerState>().FacingRight.Value))
        {
            Flip();
        }
    }

    private void Flip()
    {
        GetComponent<PlayerState>().FacingRight.Value = !(GetComponent<PlayerState>().FacingRight.Value);
        GetComponent<SpriteRenderer>().flipX = !(GetComponent<SpriteRenderer>().flipX); 
    }
}
