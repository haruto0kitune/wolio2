using UnityEngine;
using System.Collections;

namespace Wolio.Actor.Player
{
    public class PlayerConfig : MonoBehaviour
    {
        public LayerMask WhatIsGround;                  // A mask determining what is ground to the character
        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        public float GravityScaleStore = 5f;
        public const float FallVelocityLimit = -6f;

        void Start()
        {
        }
    }
}
