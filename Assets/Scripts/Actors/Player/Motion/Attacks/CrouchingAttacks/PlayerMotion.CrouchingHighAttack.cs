using UnityEngine;
using System.Collections;

public partial class PlayerMotion : MonoBehaviour
{
    public IEnumerator CrouchingHighAttack()
    {
        var _GameObject = new GameObject();
        _GameObject.tag = "Attacks/CrouchingAttacks/CrouchingHighAttack";
        _GameObject.transform.parent = transform;
        _GameObject.transform.position = transform.position;

        var CrouchingHighAttackBounds = _GameObject.AddComponent<BoxCollider2D>();

        if (PlayerState.FacingRight.Value)
        {
            CrouchingHighAttackBounds.offset = new Vector2(0.436434f, -0.364021f);
            CrouchingHighAttackBounds.size = new Vector2(0.5299196f, 0.02498758f);
        }
        else
        {
            CrouchingHighAttackBounds.offset = new Vector2(-0.436434f, -0.364021f);
            CrouchingHighAttackBounds.size = new Vector2(0.5299196f, 0.02498758f);
        }

        CrouchingHighAttackBounds.isTrigger = true;

        for(var i = 0;i < 3;i++)
        {
            yield return null;
        }

        Destroy(_GameObject);
    }
}
