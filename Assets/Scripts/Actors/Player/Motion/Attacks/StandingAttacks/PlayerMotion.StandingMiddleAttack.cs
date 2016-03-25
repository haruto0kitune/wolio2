using UnityEngine;
using System.Collections;

public partial class PlayerMotion : MonoBehaviour
{
    public IEnumerator StandingMiddleAttack()
    {
        var _GameObject = new GameObject();
        _GameObject.tag = "Attacks/StandingAttacks/StandingMiddleAttack";
        _GameObject.transform.parent = transform;
        _GameObject.transform.position = transform.position;

        var StandingMiddleAttackBounds = _GameObject.AddComponent<BoxCollider2D>();

        if (PlayerState.FacingRight.Value)
        {
            StandingMiddleAttackBounds.offset = new Vector2(0.1529124f, -0.05826771f);
            StandingMiddleAttackBounds.size = new Vector2(0.291669f, 0.1685592f);
        }
        else
        {
            StandingMiddleAttackBounds.offset = new Vector2(-0.1529124f, -0.05826771f);
            StandingMiddleAttackBounds.size = new Vector2(0.291669f, 0.1685592f);
        }

        StandingMiddleAttackBounds.isTrigger = true;

        for(var i = 0;i < 3;i++)
        {
            yield return null;
        }

        Destroy(_GameObject);
    }
}
