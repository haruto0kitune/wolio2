using UnityEngine;
using System.Collections;

public partial class PlayerMotion : MonoBehaviour
{
    public IEnumerator JumpingHighAttack()
    {
        var _GameObject = new GameObject();
        _GameObject.tag = "Attacks/JumpingAttacks/JumpingHighAttack";
        _GameObject.transform.parent = transform;
        _GameObject.transform.position = transform.position;

        var JumpingHighAttackBounds = _GameObject.AddComponent<BoxCollider2D>();
        var JumpingHighAttackBounds2 = _GameObject.AddComponent<BoxCollider2D>();

        if (PlayerState.FacingRight.Value)
        {
            JumpingHighAttackBounds.offset = new Vector2(0.03010678f, -0.3378794f);
            JumpingHighAttackBounds.size = new Vector2(0.5696427f, 0.08597362f);
            JumpingHighAttackBounds2.offset = new Vector2(0.2714231f, -0.2464964f);
            JumpingHighAttackBounds2.size = new Vector2(0.1574482f, 0.1962221f);
        }
        else
        {
            JumpingHighAttackBounds.offset = new Vector2(-0.03010678f, -0.3378794f);
            JumpingHighAttackBounds.size = new Vector2(0.5696427f, 0.08597362f);
            JumpingHighAttackBounds2.offset = new Vector2(-0.2714231f, -0.2464964f);
            JumpingHighAttackBounds2.size = new Vector2(0.1574482f, 0.1962221f);
        }

        JumpingHighAttackBounds.isTrigger = true;

        for(var i = 0;i < 3;i++)
        {
            yield return null;
        }

        Destroy(_GameObject);
    }
}
