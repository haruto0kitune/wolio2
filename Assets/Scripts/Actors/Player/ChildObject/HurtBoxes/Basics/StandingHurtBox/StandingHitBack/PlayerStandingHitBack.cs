using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class PlayerStandingHitBack : MonoBehaviour
{
    PlayerState PlayerState;
    BoxCollider2D BoxCollider2D;
    Rigidbody2D Rigidbody2D;

    void Awake()
    {
        PlayerState = GameObject.Find("Test").GetComponent<PlayerState>();
        BoxCollider2D = transform.parent.GetComponent<BoxCollider2D>();
        Rigidbody2D = GameObject.Find("Test").GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        transform.parent.OnTriggerEnter2DAsObservable()
            .Where(x => PlayerState.IsStanding.Value)
            .Where(x => !PlayerState.IsStandingHitBack.Value)
            .Where(x => x.gameObject.tag == "AttackLevel/1")
            .Subscribe(_ => StartCoroutine(StandingHitBack()));
    }

    IEnumerator StandingHitBack()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Enemy"), true);
        Rigidbody2D.velocity = new Vector2(2f, Rigidbody2D.velocity.y);
        
        if (PlayerState.FacingRight.Value)
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

        Rigidbody2D.velocity = new Vector2(0f, Rigidbody2D.velocity.y);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Enemy"), false);
    }
}
