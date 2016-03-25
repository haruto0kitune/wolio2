using UnityEngine;
using System.Collections;

public partial class PlayerMotion : MonoBehaviour
{
    public IEnumerator JumpingMiddleAttack()
    {
        var _GameObject = new GameObject();
        _GameObject.tag = "Attacks/JumpingAttacks/JumpingMiddleAttack";
        _GameObject.transform.parent = transform;
        _GameObject.transform.position = transform.position;

        var JumpingMiddleAttackBounds = _GameObject.AddComponent<BoxCollider2D>();

        if (PlayerState.FacingRight.Value)
        {
            JumpingMiddleAttackBounds.offset = new Vector2(0.271064f, 0.0415591f);
            JumpingMiddleAttackBounds.size = new Vector2(0.3092983f, 0.09151307f);
        }
        else
        {
            JumpingMiddleAttackBounds.offset = new Vector2(-0.271064f, 0.0415591f);
            JumpingMiddleAttackBounds.size = new Vector2(0.3092983f, 0.09151307f);
        }

        JumpingMiddleAttackBounds.isTrigger = true;

        for(var i = 0;i < 3;i++)
        {
            yield return null;
        }

        Destroy(_GameObject);
    }
}
