using UnityEngine;
using System.Collections;

public partial class PlayerMotion : MonoBehaviour
{
    public IEnumerator StandingHighAttack()
    {
        var _GameObject = new GameObject();
        _GameObject.tag = "StandingHighAttack";
        _GameObject.transform.parent = transform;
        _GameObject.transform.position = transform.position;

        var StandingHighAttackBounds = _GameObject.AddComponent<BoxCollider2D>();

        if (PlayerState.FacingRight.Value)
        {
            StandingHighAttackBounds.offset = new Vector2(0.2463715f, -0.02989638f);
            StandingHighAttackBounds.size = new Vector2(0.3116956f, 0.1451947f);
        }
        else
        {
            StandingHighAttackBounds.offset = new Vector2(-0.2463715f, -0.02989638f);
            StandingHighAttackBounds.size = new Vector2(0.3116956f, 0.1451947f);
        }

        StandingHighAttackBounds.isTrigger = true;

        for(var i = 0;i < 3;i++)
        {
            if (PlayerState.FacingRight.Value)
            {
                Rigidbody2D.velocity = new Vector2(6f, Rigidbody2D.velocity.y);
            }
            else
            {
                Rigidbody2D.velocity = new Vector2(-6f, Rigidbody2D.velocity.y);
            }

            yield return null;
        }

        Rigidbody2D.velocity = new Vector2(0f, Rigidbody2D.velocity.y);
        Destroy(_GameObject);
    }
}
