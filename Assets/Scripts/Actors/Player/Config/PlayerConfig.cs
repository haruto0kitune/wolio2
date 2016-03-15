using UnityEngine;
using System.Collections;

public class PlayerConfig : MonoBehaviour
{
    public float MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    public float KnockBackSpeed = 3f;
    public float DashSpeed = 10f;
    public float CreepSpeed = 4f;
    public float JumpForce = 400f;                  // Amount of force added when the player jumps.
    public bool AirControl = false;                 // Whether or not a player can steer while jumping;
    public LayerMask WhatIsGround;                  // A mask determining what is ground to the character
    public int shotwait = 0;
    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    public float GravityScaleStore = 5f;
    public const float FallVelocityLimit = -10f;
    public Vector2 RightCrouchColliderOffset;
    public Vector2 RightCrouchColliderSize;
    public Vector2 LeftCrouchColliderOffset;
    public Vector2 LeftCrouchColliderSize;
    public Vector2 RightCreepColliderOffset;
    public Vector2 RightCreepColliderSize;
    public Vector2 LeftCreepColliderOffset;
    public Vector2 LeftCreepColliderSize;
    public Vector2 BoxCollider2DInitialOffset { get; set; }
    public Vector2 BoxCollider2DInitialSize { get; set; }

    void Start()
    {
        BoxCollider2DInitialOffset = GetComponent<BoxCollider2D>().offset;
        BoxCollider2DInitialSize = GetComponent<BoxCollider2D>().size;
    }
}
