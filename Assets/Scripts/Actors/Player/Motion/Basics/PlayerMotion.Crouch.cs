using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerMotion : MonoBehaviour
{
    public void Crouch()
    {
        PlayerState.IsCrouching.Value = true;

        if (SpriteRenderer.flipX)
        {
            BoxCollider2D.offset = PlayerConfig.RightCrouchColliderOffset;
            BoxCollider2D.size = PlayerConfig.RightCrouchColliderSize;
            CircleCollider2D.enabled = false;
        }
        else
        {
            BoxCollider2D.offset = PlayerConfig.LeftCrouchColliderOffset;
            BoxCollider2D.size = PlayerConfig.LeftCrouchColliderSize;
            CircleCollider2D.enabled = false;
        }
    }

    public void ExitCrouch()
    {
        PlayerState.IsCrouching.Value = false;

        BoxCollider2D.offset = PlayerConfig.BoxCollider2DInitialOffset;
        BoxCollider2D.size = PlayerConfig.BoxCollider2DInitialSize;
        CircleCollider2D.enabled = true;
    }
}
