using UnityEngine;
using System.Collections;

public partial class PlayerMotion : MonoBehaviour
{
    public IEnumerator JumpingLightAttack()
    {
        var _GameObject = new GameObject();
        _GameObject.tag = "Attacks/JumpingAttacks/JumpingLightAttack";
        _GameObject.transform.parent = transform;
        _GameObject.transform.position = transform.position;

        var JumpingLightAttackBounds = _GameObject.AddComponent<BoxCollider2D>();

        if (PlayerState.FacingRight.Value)
        {
            JumpingLightAttackBounds.offset = new Vector2(0.2073791f, 0.1646836f);
            JumpingLightAttackBounds.size = new Vector2(0.1819281f, 0.2103918f);
        }
        else
        {
            JumpingLightAttackBounds.offset = new Vector2(-0.2073791f, 0.1646836f);
            JumpingLightAttackBounds.size = new Vector2(0.1819281f, 0.2103918f);
        }

        JumpingLightAttackBounds.isTrigger = true;

        for(var i = 0;i < 3;i++)
        {
            yield return null;
        }

        Destroy(_GameObject);
    }
}
