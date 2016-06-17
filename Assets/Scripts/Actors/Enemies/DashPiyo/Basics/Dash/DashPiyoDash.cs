using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Enemy.DashPiyo
{
    public class DashPiyoDash : MonoBehaviour
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
        float Speed;
        [SerializeField]
        int damageValue;
        [SerializeField]
        int hitRecovery;
        [SerializeField]
        int hitStop;
        [SerializeField]
        int startup;
        [SerializeField]
        int active;
        [SerializeField]
        int recovery;
        [SerializeField]
        bool isTechable;
        [SerializeField]
        bool hasKnockdownAttribute;
        [SerializeField]
        AttackAttribute attackAttribute;
        bool isFinished;
        bool hasHit;
        Coroutine coroutineStore;

        void Awake()
        {
            Animator = DashPiyo.GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            DashPiyoRigidbody2D = DashPiyo.GetComponent<Rigidbody2D>();
            DashPiyoState = DashPiyo.GetComponent<DashPiyoState>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            HitBox = DashPiyoRunHitBox.GetComponent<BoxCollider2D>();
            HurtBox = DashPiyoRunHurtBox.GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            // Animation
            #region Dash->Damage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Dash"))
                .Where(x => DashPiyoState.WasAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsDashing", false);
                    Animator.SetBool("IsDamaged", true);
                });
            #endregion
            #region Dash->Run
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Dash"))
                .Where(x => isFinished)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsRunning", true);
                    Animator.SetBool("IsDashing", false);
                    isFinished = false;
                });
            #endregion

            // Collision
            ObservableStateMachineTrigger
                .OnStateEnterAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Dash"))
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = true;
                    HitBox.enabled = true;
                    HurtBox.enabled = true;
                });

            ObservableStateMachineTrigger
                .OnStateExitAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Dash"))
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = false;
                    HitBox.enabled = false;
                    HurtBox.enabled = false;
                });

            // Motion
            this.FixedUpdateAsObservable()
                .ObserveEveryValueChanged(x => DashPiyoState.IsDashing.Value)
                .Where(x => x)
                .Subscribe(_ => coroutineStore = StartCoroutine(Dash(Speed, DashPiyoState.Direction.Value)));

            // Damage
            this.OnTriggerEnter2DAsObservable()
                .Where(x => x.gameObject.tag == "HurtBox")
                .ThrottleFirstFrame(30)
                .Subscribe(_ =>
                {
                    _.gameObject.GetComponent<DamageManager>().ApplyDamage(damageValue, hitRecovery, hitStop, isTechable, hasKnockdownAttribute, attackAttribute);
                    HitBox.enabled = false;
                    hasHit = true;
                });
        }

        public IEnumerator Dash(float speed, float direction)
        {
            // Startup
            for (int i = 0; i < startup; i++)
            {
                yield return null;
            }

            // Active
            for (int i = 0; i < active; i++)
            {
                DashPiyoRigidbody2D.velocity = new Vector2(speed * direction, DashPiyoRigidbody2D.velocity.y);
                yield return null;
            }

            // Recovery
            for (int i = 0; i < recovery; i++)
            {
                DashPiyoRigidbody2D.velocity = Vector2.zero;
                yield return null;
            }

            isFinished = true;
        }
    }
}