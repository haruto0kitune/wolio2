using UnityEngine;
using System.Collections;

public partial class PlayerMotion : MonoBehaviour
{
    public IEnumerator StandingLightAttack()
    {
        var _GameObject = new GameObject();
        _GameObject.tag = "Attacks/StandingAttacks/StandingLightAttack";
        _GameObject.transform.parent = transform;
        _GameObject.transform.position = transform.position;

        var StandingLightAttackBounds = _GameObject.AddComponent<BoxCollider2D>();
        StandingLightAttackBounds.isTrigger = true;

        if (PlayerState.FacingRight.Value)
        {
            StandingLightAttackBounds.offset = new Vector2(0.1095207f, 0.03352177f);
            StandingLightAttackBounds.size = new Vector2(0.2048863f, 0.04506044f);
        }
        else
        {
            StandingLightAttackBounds.offset = new Vector2(-0.1095207f, 0.03352177f);
            StandingLightAttackBounds.size = new Vector2(0.2048863f, 0.04506044f);
        }

        StandingLightAttackBounds.isTrigger = true;

        for(var i = 0;i < 3;i++)
        {
            yield return null;
        }

        Destroy(_GameObject);
    }
}
