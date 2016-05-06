using UnityEngine;
using System.Collections;

public partial class PlayerMover : MonoBehaviour
{
    Rigidbody2D Rigidbody2D;
    BoxCollider2D BoxCollider2D;
    CircleCollider2D CircleCollider2D;
    SpriteRenderer SpriteRenderer;
    PlayerConfig PlayerConfig;
    PlayerState PlayerState;

    void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
        CircleCollider2D = GetComponent<CircleCollider2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        PlayerConfig = GetComponent<PlayerConfig>();
        PlayerState = GetComponent<PlayerState>();
    }

    public void Turn()
    {
        Utility.Flip(transform);
    }

    public void Run(float Horizontal, float MaxSpeed)
    {
        Rigidbody2D.velocity = new Vector2(Horizontal * MaxSpeed, Rigidbody2D.velocity.y);
    }

    public void Jump(float JumpForce)
    {
        Rigidbody2D.velocity = new Vector2(0f, JumpForce);
    }

    public void Creep(float Horizontal, float CreepSpeed)
    {
        Rigidbody2D.velocity = new Vector2(Horizontal * CreepSpeed, Rigidbody2D.velocity.y);
    }

    public void Climb(float Vertical, float MaxSpeed)
    {
        Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, Vertical * (MaxSpeed - 3)); 
    }

    public void AirMove(float Horizontal)
    {
        if (Horizontal != 0)
        {
            Rigidbody2D.AddForce(new Vector2(50f * Horizontal, 0f));
        }
    }   
}