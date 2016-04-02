using UnityEngine;
using System.Collections;

public partial class PlayerMotion : MonoBehaviour
{
    public IEnumerator StandingGuardBack()
    {
        PlayerState.IsStandingGuardBack.Value = true;
        var StandingGuardBoxCollider2D = GameObject.Find("StandingGuard").GetComponent<BoxCollider2D>();
        StandingGuardBoxCollider2D.enabled = false;

        var VelocityStore = Rigidbody2D.velocity;

        if(PlayerState.FacingRight.Value)
        {
            Rigidbody2D.velocity = new Vector2(-2f, Rigidbody2D.velocity.y);
        }
        else
        {
            Rigidbody2D.velocity = new Vector2(2f, Rigidbody2D.velocity.y);
        }

        for (int i = 0; i < 3; i++)
        {
            yield return null;
        }

        Rigidbody2D.velocity = VelocityStore;
        StandingGuardBoxCollider2D.enabled = true;
        PlayerState.IsStandingGuardBack.Value = false;
    }
}
