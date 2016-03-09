using UnityEngine;
using System.Collections;

public class PlayerConfig : MonoBehaviour
{
    public float MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    public float KnockBackSpeed = 3f;
    public float DashSpeed = 10f;
    public float JumpForce = 400f;                  // Amount of force added when the player jumps.
    public bool AirControl = false;                 // Whether or not a player can steer while jumping;
    public LayerMask WhatIsGround;                  // A mask determining what is ground to the character
    public int shotwait = 0;
    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    public float GravityScaleStore = 5f;
}
