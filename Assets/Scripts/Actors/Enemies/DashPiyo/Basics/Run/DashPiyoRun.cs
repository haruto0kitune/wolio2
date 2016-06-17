using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Enemy.DashPiyo
{
    public class DashPiyoRun : MonoBehaviour
    {
        [SerializeField]
        GameObject DashPiyo;
        BoxCollider2D BoxCollider2D;
        Animator Animator;
        ObservableStateMachineTrigger ObservableStateMachineTrigger;
        Rigidbody2D DashPiyoRigidbody2D;
        DashPiyoState DashPiyoState;
        [SerializeField]
        GameObject DashPiyoRunHitBox;
        BoxCollider2D HitBox;
        [SerializeField]
        GameObject DashPiyoRunHurtBox;
        BoxCollider2D HurtBox;
        [SerializeField]
        GameObject DashPiyoRunDashTriggerBox;
        BoxCollider2D DashTriggerBox;
        [SerializeField]
        float Speed;
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
        bool isDashTrigger;

        void Awake()
        {
            Animator = DashPiyo.GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            DashPiyoRigidbody2D = DashPiyo.GetComponent<Rigidbody2D>();
            DashPiyoState = DashPiyo.GetComponent<DashPiyoState>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            HitBox = DashPiyoRunHitBox.GetComponent<BoxCollider2D>();
            HurtBox = DashPiyoRunHurtBox.GetComponent<BoxCollider2D>();
            DashTriggerBox = DashPiyoRunDashTriggerBox.GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            // Animation
            #region Run->Damage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Run"))
                .Where(x => DashPiyoState.WasAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsRunning", false);
                    Animator.SetBool("IsDamaged", true);
                });
            #endregion
            #region Run->Dash
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Run"))
                .Where(x => isDashTrigger)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsRunning", false);
                    Animator.SetBool("IsDashing", true);
                    isDashTrigger = false;
                });
            #endregion

            // Collision
            ObservableStateMachineTrigger
                .OnStateEnterAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Run"))
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = true;
                    HitBox.enabled = true;
                    HurtBox.enabled = true;
                    DashTriggerBox.enabled = true;
                });

            ObservableStateMachineTrigger
                .OnStateExitAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Run"))
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = false;
                    HitBox.enabled = false;
                    HurtBox.enabled = false;
                    DashTriggerBox.enabled = false;
                });

            // Motion
            this.FixedUpdateAsObservable()
                .Where(x => DashPiyoState.IsRunning.Value)
                .Subscribe(_ => Run(Speed, DashPiyoState.Direction.Value));

            // Trigger
            DashTriggerBox.OnTriggerEnter2DAsObservable()
                .Where(x => x.gameObject.tag == "HurtBox")
                .ThrottleFirstFrame(30)
                .Subscribe(_ => isDashTrigger = true);

            // Damage
            this.OnTriggerEnter2DAsObservable()
                .Where(x => x.gameObject.tag == "HurtBox")
                .ThrottleFirstFrame(30)
                .Subscribe(_ =>
                {
                    _.gameObject.GetComponent<DamageManager>().ApplyDamage(damageValue, hitRecovery, hitStop, isTechable, hasKnockdownAttribute, attackAttribute);
                    HitBox.enabled = false;
                });
        }

        public void Run(float speed, float direction)
        {
            DashPiyoRigidbody2D.velocity = new Vector2(speed * direction, DashPiyoRigidbody2D.velocity.y);
        }
    }
}