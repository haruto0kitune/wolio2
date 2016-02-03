using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public void Run(float Horizontal)
    {
        Rigidbody2D.velocity = new Vector2(Horizontal * MaxSpeed, Rigidbody2D.velocity.y);
    }

    public void Dash(float Horizontal, bool shift)
    {
        if ((shift && ((Horizontal < 0) || (Horizontal > 0))) && !IsDashing.Value)
        {
            StartCoroutine(DashCoroutine(Horizontal, shift));
        }
    }

    public IEnumerator DashCoroutine(float Horizontal, bool shift)
    {
        IsDashing.Value = true;

        //right backdash
        if (FacingRight.Value && ((Horizontal < 0) && shift))
        {
            for (int i = 0; i < 5; i++)
            {
                Rigidbody2D.velocity = new Vector2(Horizontal * (DashSpeed - i * 2), Rigidbody2D.velocity.y);
                yield return null;
            }
        }
        //right frontdash
        else if (FacingRight.Value && ((Horizontal > 0) && shift))
        {
            for (int i = 0; i < 5; i++)
            {
                Rigidbody2D.velocity = new Vector2(Horizontal * (DashSpeed - i * 2), Rigidbody2D.velocity.y);
                yield return null;
            }
        }
        //left frontdash
        else if (!FacingRight.Value && ((Horizontal < 0) && shift))
        {
            for (int i = 0; i < 5; i++)
            {
                Rigidbody2D.velocity = new Vector2(Horizontal * (DashSpeed - i * 2), Rigidbody2D.velocity.y);
                yield return null;
            }
        }
        //left backdash
        else if (!FacingRight.Value && ((Horizontal > 0) && shift))
        {
            for (int i = 0; i < 5; i++)
            {
                Rigidbody2D.velocity = new Vector2(Horizontal * (DashSpeed - i * 2), Rigidbody2D.velocity.y);
                yield return null;
            }
        }

        // wait for 5 frames.
        for (int i = 0; i < 23; i++)
        {
            yield return null;
        }

        IsDashing.Value = false;
        yield return null;
    }

    public void TurnAround(float Horizontal)
    {
        if ((Horizontal > 0 && !FacingRight.Value) || (Horizontal < 0 && FacingRight.Value))
        {
            Flip();
        }
    }

    public void Jump()
    {
        // If the player should jump...
        if ((bool)Physics2D.Linecast(this.transform.position, new Vector2(this.transform.position.x, this.transform.position.y - 0.539f), /*ground*/WhatIsGround))
        {
            // Add a vertical force to the player.
            Rigidbody2D.AddForce(new Vector2(0f, JumpForce));
        }
    }

    public void Die()
    {
        if ((transform.position.y <= -5 || Hp.Value <= 0))
        {
            Debug.Log(this.IsDead.Value);
        }
    }

    public void Guard()
    {
        int left = -1;
        int right = 1;

        if (FacingRight.Value)
        {
            Rigidbody2D.velocity = new Vector2(left * KnockBackSpeed, Rigidbody2D.velocity.y);
        }
        else if (!FacingRight.Value)
        {
            Rigidbody2D.velocity = new Vector2(right * KnockBackSpeed, Rigidbody2D.velocity.y);
        }

    }

    public IEnumerator WallKickJump()
    {

        Flip();

        Rigidbody2D.velocity = new Vector2(0.0f, 0.0f);
        Rigidbody2D.AddForce(new Vector2(500.0f, JumpForce));

        for (int i = 0; i < 15; i++)
        {
            Rigidbody2D.AddForce(new Vector2(500.0f, 0.0f));
            yield return null;
        }
    }

    public IEnumerator BecomeInvincible()
    {
        // Player become invincible.
        gameObject.layer = LayerMask.NameToLayer("Invincible");

        // Invincible frames.
        for (int i = 0; i < 36; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                yield return null;
            }

            GetComponent<SpriteRenderer>().enabled = !(GetComponent<SpriteRenderer>().enabled);
        }

        // Player become normal state.
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        FacingRight.Value = !FacingRight.Value;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
