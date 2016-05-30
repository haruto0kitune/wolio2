using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class FlyingPiyoFly : MonoBehaviour
{
    [SerializeField]
    GameObject FlyingPiyo;
    Rigidbody2D FlyingPiyoRigidbody2D;
    FlyingPiyoState FlyingPiyoState;
    [SerializeField]
    GameObject FlyingPiyoFlyHitBox;
    CircleCollider2D HitBox;
    [SerializeField]
    GameObject FlyingPiyoFlyHurtBox;
    CircleCollider2D HurtBox;
    [SerializeField]
    float speed;
    [SerializeField]
    int damageValue;
    [SerializeField]
    int hitRecovery;

    void Awake()
    {
        FlyingPiyoRigidbody2D = FlyingPiyo.GetComponent<Rigidbody2D>();
        FlyingPiyoState = FlyingPiyo.GetComponent<FlyingPiyoState>();
        HitBox = FlyingPiyoFlyHitBox.GetComponent<CircleCollider2D>();
        HurtBox = FlyingPiyoFlyHurtBox.GetComponent<CircleCollider2D>();
    }

    void Start()
    {
        // Motion
        this.FixedUpdateAsObservable()
            .Where(x => !FlyingPiyoState.IsAttacking.Value)
            .Subscribe(_ => Fly(speed, FlyingPiyoState.Direction.Value));

        // Damage
        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "HurtBox")
            .ThrottleFirstFrame(hitRecovery)
            .Subscribe(_ =>
            {
                _.gameObject.GetComponent<DamageManager>().ApplyDamage(damageValue, hitRecovery);
                HitBox.enabled = false;
            });
    }

    void Fly(float speed, float direction)
    {
        FlyingPiyoRigidbody2D.velocity = new Vector2(speed * direction, 0f);
    }
}