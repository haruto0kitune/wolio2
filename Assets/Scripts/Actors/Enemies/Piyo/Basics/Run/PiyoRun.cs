using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using Wolio.Actor.Player;

namespace Wolio.Actor.Enemy.Piyo.Basics
{
    public class PiyoRun : MonoBehaviour
    {
        [SerializeField]
        GameObject Piyo;
        Rigidbody2D PiyoRigidbody2D;
        PiyoState PiyoState;
        [SerializeField]
        GameObject PiyoRunHitBox;
        [SerializeField]
        GameObject PiyoRunHurtBox;
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
            HitBox = PiyoRunHitBox.GetComponent<BoxCollider2D>();
            HurtBox = PiyoRunHurtBox.GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            // Motion
            this.FixedUpdateAsObservable()
                .Subscribe(_ => Run(Speed, PiyoState.Direction.Value));

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

        public void Run(float speed, float direction)
        {
            PiyoRigidbody2D.velocity = new Vector2(speed * direction, PiyoRigidbody2D.velocity.y);
        }
    }
}