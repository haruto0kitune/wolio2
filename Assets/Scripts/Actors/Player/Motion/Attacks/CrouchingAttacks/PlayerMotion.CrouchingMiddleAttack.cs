using UnityEngine;
using System.Collections;

public partial class PlayerMotion : MonoBehaviour
{
    public IEnumerator CrouchingMiddleAttack()
    {
        var _GameObject = new GameObject();
        _GameObject.tag = "CrouchingMiddleAttack";
        _GameObject.transform.parent = transform;
        _GameObject.transform.position = transform.position;

        var CrouchingMiddleAttackBounds = _GameObject.AddComponent<BoxCollider2D>();

        if (PlayerState.FacingRight.Value)
        {
            CrouchingMiddleAttackBounds.offset = new Vector2(0.3111792f, -0.3366216f);
            CrouchingMiddleAttackBounds.size = new Vector2(0.279411f, 0.07978642f);
        }
        else
        {
            CrouchingMiddleAttackBounds.offset = new Vector2(-0.3111792f, -0.3366216f);
            CrouchingMiddleAttackBounds.size = new Vector2(0.279411f, 0.07978642f);
        }

        CrouchingMiddleAttackBounds.isTrigger = true;

        for(var i = 0;i < 3;i++)
        {
            yield return null;
        }

        Destroy(_GameObject);
    }
}
