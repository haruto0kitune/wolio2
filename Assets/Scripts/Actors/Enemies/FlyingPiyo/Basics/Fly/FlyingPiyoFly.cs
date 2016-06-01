using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Enemy.FlyingPiyo.Basics
{
    public class FlyingPiyoFly : MonoBehaviour
    {
        [SerializeField]
        GameObject FlyingPiyo;
        Animator Animator;
        ObservableStateMachineTrigger ObservableStateMachineTrigger;
        Rigidbody2D FlyingPiyoRigidbody2D;
        FlyingPiyoState FlyingPiyoState;
        CircleCollider2D CircleCollider2D;
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
            Animator = FlyingPiyo.GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            FlyingPiyoRigidbody2D = FlyingPiyo.GetComponent<Rigidbody2D>();
            FlyingPiyoState = FlyingPiyo.GetComponent<FlyingPiyoState>();
            CircleCollider2D = GetComponent<CircleCollider2D>();
            HitBox = FlyingPiyoFlyHitBox.GetComponent<CircleCollider2D>();
            HurtBox = FlyingPiyoFlyHurtBox.GetComponent<CircleCollider2D>();
        }

        void Start()
        {
            // Animation
            #region Fly->Damage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.FlyingPiyoFly"))
                .Where(x => FlyingPiyoState.WasAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsFlying", false);
                    Animator.SetBool("IsDamaged", true);
                });
            #endregion

            // Collision
            FlyingPiyoState
                .IsFlying
                .Subscribe(_ =>
                {
                    CircleCollider2D.enabled = _;
                    HitBox.enabled = _;
                    HurtBox.enabled = _;
                });

            // Motion
            this.FixedUpdateAsObservable()
                .Where(x => !FlyingPiyoState.IsAttacking.Value)
                .Where(x => !FlyingPiyoState.WasAttacked.Value)
                .Subscribe(_ => Fly(speed, FlyingPiyoState.Direction.Value));

            // Damage
            this.OnTriggerEnter2DAsObservable()
                .Where(x => x.gameObject.tag == "HurtBox")
                .ThrottleFirstFrame(hitRecovery)
                .Subscribe(_ =>
                {
                    _.gameObject.GetComponent<DamageManager>().ApplyDamage(damageValue, hitRecovery);
                });
        }

        void Fly(float speed, float direction)
        {
            FlyingPiyoRigidbody2D.velocity = new Vector2(speed * direction, 0f);
        }
    }
}