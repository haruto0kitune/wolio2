using UnityEngine;
using System.Collections;

public partial class PlayerMotion : MonoBehaviour
{
    public void Creep(float Horizontal, float CreepSpeed)
    {
        PlayerState.IsCreeping.Value = true;

        if (Horizontal == 1)
        {
            BoxCollider2D.offset = PlayerConfig.RightCreepColliderOffset;
            BoxCollider2D.size = PlayerConfig.RightCreepColliderSize;
        }
        else if (Horizontal == -1)
        {
            BoxCollider2D.offset = PlayerConfig.LeftCreepColliderOffset;
            BoxCollider2D.size = PlayerConfig.LeftCreepColliderSize;
        }

        Rigidbody2D.velocity = new Vector2(Horizontal * CreepSpeed, Rigidbody2D.velocity.y);
    }

    public void ExitCreep()
    {
        PlayerState.IsCreeping.Value = false;

        if (SpriteRenderer.flipX)
        {
            BoxCollider2D.offset = PlayerConfig.RightCrouchColliderOffset;
            BoxCollider2D.size = PlayerConfig.RightCrouchColliderSize;
        }
        else
        {
            BoxCollider2D.offset = PlayerConfig.LeftCrouchColliderOffset;
            BoxCollider2D.size = PlayerConfig.LeftCrouchColliderSize;
        }
    }
}