using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Enemy.Piyo.Basics
{
    public class PiyoRun : MonoBehaviour
    {
        [SerializeField]
        GameObject Piyo;
        Animator Animator;
        ObservableStateMachineTrigger ObservableStateMachineTrigger;
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
        [SerializeField]
        int damageValue;
        [SerializeField]
        int hitRecovery;

        void Awake()
        {
            Animator = Piyo.GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            PiyoRigidbody2D = Piyo.GetComponent<Rigidbody2D>();
            PiyoState = Piyo.GetComponent<PiyoState>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            HitBox = PiyoRunHitBox.GetComponent<BoxCollider2D>();
            HurtBox = PiyoRunHurtBox.GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            // Animation
            #region Run->Damage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.PiyoRun"))
                .Where(x => PiyoState.WasAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsRunning", false);
                    Animator.SetBool("IsDamaged", true);
                });
            #endregion

            // Motion
            this.FixedUpdateAsObservable()
                .Where(x => PiyoState.IsRunning.Value)
                .Subscribe(_ => Run(Speed, PiyoState.Direction.Value));

            // Damage
            this.OnTriggerEnter2DAsObservable()
                .Where(x => x.gameObject.tag == "HurtBox")
                .ThrottleFirstFrame(30)
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