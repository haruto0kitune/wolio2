using UnityEngine;
using System.Collections;

public class PlayerConfig : MonoBehaviour
{
    [SerializeField]
    private float MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [SerializeField]
    private float KnockBackSpeed = 3f;
    [SerializeField]
    private float DashSpeed = 10f;
    [SerializeField]
    private float JumpForce = 400f;                  // Amount of force added when the player jumps.
    [SerializeField]
    private bool AirControl = false;                 // Whether or not a player can steer while jumping;
    [SerializeField]
    private LayerMask WhatIsGround;                  // A mask determining what is ground to the character

    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    [SerializeField]
    private int shotwait = 0;
}
