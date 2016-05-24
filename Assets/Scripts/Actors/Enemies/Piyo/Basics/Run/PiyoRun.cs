using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using Wolio.Actor.Player;

public class PiyoRun : MonoBehaviour
{
    [SerializeField]
    GameObject Piyo;
    Rigidbody2D PiyoRigidbody2D;
    PiyoState PiyoState;
    [SerializeField]
    GameObject RunHitBox;
    [SerializeField]
    GameObject RunHurtBox;
    BoxCollider2D BoxCollider2D;
    BoxCollider2D HitBox;
    BoxCollider2D HurtBox;
    [SerializeField]
    float Speed;
    public int damageValue;
    public int hitRecovery;

    void Awake()
    {
        PiyoRigidbody2D = Piyo.GetComponent<Rigidbody2D>();
        PiyoState = Piyo.GetComponent<PiyoState>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
        HitBox = RunHitBox.GetComponent<BoxCollider2D>();
        HurtBox = RunHurtBox.GetComponent<BoxCollider2D>();
    }
    void Start()
    {
        // Motion
        this.FixedUpdateAsObservable()
            .Subscribe(_ => Run(Speed, PiyoState.Direction.Value));

        // Damage
        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "HurtBox/StandingHurtBox")
            .ThrottleFirstFrame(1)
            .Subscribe(_ => 
            {
                _.gameObject.GetComponent<DamageManager>().ApplyDamage(damageValue, hitRecovery);
                HitBox.enabled = false;
            });

        this.OnTriggerExit2DAsObservable()
            .Where(x => x.gameObject.tag == "HurtBox/StandingHurtBox")
            .Subscribe(_ => HitBox.enabled = true);

        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "HurtBox/CrouchingHurtBox")
            .Subscribe(_ => Debug.Log("damage"));

        this.OnTriggerExit2DAsObservable()
            .Where(x => x.gameObject.tag == "HurtBox/CrouchingHurtBox")
            .Subscribe(_ => HitBox.enabled = true);

        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "HurtBox/JumpingHurtBox")
            .Subscribe(_ => Debug.Log("damage"));

        this.OnTriggerExit2DAsObservable()
            .Where(x => x.gameObject.tag == "HurtBox/JumpingHurtBox")
            .Subscribe(_ => HitBox.enabled = true);
    }

    public void Run(float speed, float direction)
    {
        PiyoRigidbody2D.velocity = new Vector2(speed * direction, PiyoRigidbody2D.velocity.y);
    }
}