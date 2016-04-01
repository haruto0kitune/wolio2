using UnityEngine;
using System.Collections;

public partial class PlayerMotion : MonoBehaviour
{
    public IEnumerator StandingGuard()
    {
        var _GameObject = new GameObject();
        _GameObject.name = "StandingGuard";
        _GameObject.tag = "Guards/StandingGuard";
        _GameObject.transform.parent = transform;
        _GameObject.transform.position = transform.position;

        var StandingGuardBounds = _GameObject.AddComponent<BoxCollider2D>();
        StandingGuardBounds.isTrigger = true;

        if (PlayerState.FacingRight.Value)
        {
            StandingGuardBounds.offset = new Vector2(-0.002461672f, 0.009845734f);
            StandingGuardBounds.size = new Vector2(0.3027933f, 0.8196913f);
        }
        else
        {
            StandingGuardBounds.offset = new Vector2(-0.002461672f, 0.009845734f);
            StandingGuardBounds.size = new Vector2(0.3027933f, 0.8196913f);
        }

        while (PlayerState.IsStandingGuard.Value)
        {
            yield return null;
        }

        Destroy(_GameObject);
    }
}
