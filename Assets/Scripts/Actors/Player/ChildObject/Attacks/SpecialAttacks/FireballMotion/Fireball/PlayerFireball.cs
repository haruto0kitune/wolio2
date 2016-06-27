using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class PlayerFireball : MonoBehaviour
{
    Rigidbody2D Rigidbody2D;
    [SerializeField]
    GameObject FireballHitBox;
    BoxCollider2D HitBox;
    [SerializeField]
    float speed;
    [SerializeField]
    float direction;
    [SerializeField]
    int damageValue;
    [SerializeField]
    int hitRecovery;
    [SerializeField]
    int hitStop;
    [SerializeField]
    bool isTechable;
    [SerializeField]
    bool hasKnockdownAttribute;
    [SerializeField]
    AttackAttribute attackAttribute;
    [SerializeField]
    KnockdownAttribute knockdownAttribute;

    void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        HitBox = FireballHitBox.GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        // Motion
        this.FixedUpdateAsObservable()
            .Subscribe(_ => Move(speed, direction));

        // Damage
        FireballHitBox.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Enemy/HurtBox")
            .Subscribe(_ =>
            {
                _.gameObject.GetComponent<DamageManager>().ApplyDamage(damageValue, hitRecovery, hitStop, isTechable, hasKnockdownAttribute, attackAttribute, knockdownAttribute);
                HitBox.enabled = false;
                Destroy(this.gameObject);
            });

        // Destroy
        this.OnBecameInvisibleAsObservable()
            .Subscribe(_ => Destroy(this.gameObject));
    }

    public void Initialize(Vector2 position, float speed, float direction)
    {
        this.transform.position = position;
        this.speed = speed;
        this.direction = direction;
        HitBox.enabled = true;
    }

    void Move(float speed, float direction)
    {
        Rigidbody2D.velocity = new Vector2(speed * direction, 0f);
    }
}