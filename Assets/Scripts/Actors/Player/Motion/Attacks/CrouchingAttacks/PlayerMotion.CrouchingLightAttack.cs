using UnityEngine;
using System.Collections;

public partial class PlayerMotion : MonoBehaviour
{
    public IEnumerator CrouchingLightAttack()
    {
        var _GameObject = new GameObject();
        _GameObject.tag = "Attacks/CrouchingAttacks/CrouchingLightAttack";
        _GameObject.transform.parent = transform;
        _GameObject.transform.position = transform.position;

        var CrouchingLightAttackBounds = _GameObject.AddComponent<BoxCollider2D>();

        if (PlayerState.FacingRight.Value)
        {
            CrouchingLightAttackBounds.offset = new Vector2(0.2429385f, -0.1280639f);
            CrouchingLightAttackBounds.size = new Vector2(0.2515338f, 0.1315695f);
        }
        else
        {
            CrouchingLightAttackBounds.offset = new Vector2(-0.2429385f, -0.1280639f);
            CrouchingLightAttackBounds.size = new Vector2(0.2515338f, 0.1315695f);
        }

        CrouchingLightAttackBounds.isTrigger = true;

        for(var i = 0;i < 3;i++)
        {
            yield return null;
        }

        Destroy(_GameObject);
    }
}