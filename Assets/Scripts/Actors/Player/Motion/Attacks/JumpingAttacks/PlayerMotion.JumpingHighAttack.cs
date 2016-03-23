using UnityEngine;
using System.Collections;

public partial class PlayerMotion : MonoBehaviour
{
    public IEnumerator JumpingHighAttack()
    {
        var _GameObject = new GameObject();
        _GameObject.tag = "JumpingHighAttack";
        _GameObject.transform.parent = transform;
        _GameObject.transform.position = transform.position;

        var JumpingHighAttackBounds = _GameObject.AddComponent<BoxCollider2D>();

        if (PlayerState.FacingRight.Value)
        {
            JumpingHighAttackBounds.offset = new Vector2(0.436434f, -0.364021f);
            JumpingHighAttackBounds.size = new Vector2(0.5299196f, 0.02498758f);
        }
        else
        {
            JumpingHighAttackBounds.offset = new Vector2(-0.436434f, -0.364021f);
            JumpingHighAttackBounds.size = new Vector2(0.5299196f, 0.02498758f);
        }

        JumpingHighAttackBounds.isTrigger = true;

        for(var i = 0;i < 3;i++)
        {
            yield return null;
        }

        Destroy(_GameObject);
    }
}
