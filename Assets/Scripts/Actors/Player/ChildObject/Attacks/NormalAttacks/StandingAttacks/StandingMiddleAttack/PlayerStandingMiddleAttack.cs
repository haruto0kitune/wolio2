using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Attacks.NormalAttacks.StandingAttacks
{
    public class PlayerStandingMiddleAttack : MonoBehaviour
    {
        [SerializeField]
        GameObject Player;
        Animator Animator;
        ObservableStateMachineTrigger ObservableStateMachineTrigger;
        PlayerState PlayerState;
        Rigidbody2D PlayerRigidbody2D;
        Key Key;
        BoxCollider2D BoxCollider2D;
        CircleCollider2D CircleCollider2D;
        [SerializeField]
        GameObject PlayerStandingMiddleAttackHitBox;
        BoxCollider2D HitBox;
        [SerializeField]
        GameObject PlayerStandingMiddleAttackHurtBox;
        BoxCollider2D HurtBox;
        [SerializeField]
        int damageValue;
        [SerializeField]
        int hitRecovery;
        [SerializeField]
        int hitStop;
        [SerializeField]
        int Startup;
        [SerializeField]
        int Active;
        [SerializeField]
        int Recovery;
        [SerializeField]
        bool isTechable;
        [SerializeField]
        bool hasKnockdownAttribute;
        [SerializeField]
        AttackAttribute attackAttribute;
        [SerializeField]
        KnockdownAttribute knockdownAttribute;
        bool wasFinished;
        bool isCancelable;
        bool wasCanceled;
        Coroutine coroutineStore;

        void Awake()
        {
            Animator = Player.GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            PlayerState = Player.GetComponent<PlayerState>();
            PlayerRigidbody2D = Player.GetComponent<Rigidbody2D>();
            Key = Player.GetComponent<Key>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            CircleCollider2D = GetComponent<CircleCollider2D>();
            HitBox = PlayerStandingMiddleAttackHitBox.GetComponent<BoxCollider2D>();
            HurtBox = PlayerStandingMiddleAttackHurtBox.GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            // Animation
            #region EnterStandingMiddleAttack
            ObservableStateMachineTrigger
                .OnStateEnterAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.StandingMiddleAttack"))
                .Subscribe(_ => Animator.speed = 0);
            #endregion
            #region StandingMiddleAttack->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.StandingMiddleAttack"))
                .Where(x => wasFinished)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStandingMiddleAttack", false);
                    Animator.SetBool("IsStanding", true);
                    wasFinished = false;
                });
            #endregion
            #region StandingMiddleAttack->Jump
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.StandingMiddleAttack"))
                .Where(x => PlayerState.canJump.Value)
                .Where(x => isCancelable)
                .Where(x => Key.Vertical.Value == 1f)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStandingMiddleAttack", false);
                    Animator.SetBool("IsJumping", true);
                    isCancelable = false;
                    StopCoroutine(coroutineStore);
                    wasCanceled = true;
                });
            #endregion
            #region StandingMiddleAttack->StandingHighAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.StandingMiddleAttack"))
                .Where(x => PlayerState.canStandingHighAttack.Value)
                .Where(x => isCancelable)
                .Where(x => Key.C && Key.Vertical.Value == 0)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStandingMiddleAttack", false);
                    Animator.SetBool("IsStandingHighAttack", true);
                    isCancelable = false;
                    StopCoroutine(coroutineStore);
                    wasCanceled = true;
                });
            #endregion
            #region StandingMiddleAttack->CrouchingHighAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.StandingMiddleAttack"))
                .Where(x => PlayerState.canCrouchingHighAttack.Value)
                .Where(x => isCancelable)
                .Where(x => Key.C && Key.Vertical.Value == -1f)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStandingMiddleAttack", false);
                    Animator.SetBool("IsCrouchingHighAttack", true);
                    isCancelable = false;
                    StopCoroutine(coroutineStore);
                    wasCanceled = true;
                });
            #endregion

            // Collision
            this.ObserveEveryValueChanged(x => Animator.GetBool("IsStandingMiddleAttack"))
                .Where(x => x)
                .Subscribe(_ => coroutineStore = StartCoroutine(Attack()));

            this.ObserveEveryValueChanged(x => wasCanceled)
                .Where(x => x)
                .Subscribe(_ => Cancel());

            // Damage
            PlayerStandingMiddleAttackHitBox.OnTriggerEnter2DAsObservable()
                .Where(x => x.gameObject.tag == "Enemy/HurtBox")
                .Subscribe(_ =>
                {
                    _.gameObject.GetComponent<DamageManager>().ApplyDamage(damageValue, hitRecovery, hitStop, isTechable, hasKnockdownAttribute, attackAttribute, knockdownAttribute);
                    HitBox.enabled = false;
                    isCancelable = true;
                });
        }

        public IEnumerator Attack()
        {
            #region Startup
            BoxCollider2D.enabled = true;
            CircleCollider2D.enabled = true;
            HurtBox.enabled = true;

            for (int i = 0; i < Startup; i++)
            {
                yield return null;
            }
            #endregion
            #region Active
            Animator.speed = 1;
            HitBox.enabled = true;

            for (var i = 0; i < Active; i++)
            {
                yield return null;
            }

            HitBox.enabled = false;
            #endregion
            #region Recovery
            for (int i = 0; i < Recovery; i++)
            {
                yield return null;
            }

            // This needs to enable collision of next state.
            // First of all, It should enable to collision of next state.
            // Otherwise, Player become strange motion.
            wasFinished = true;
            yield return null;

            BoxCollider2D.enabled = false;
            CircleCollider2D.enabled = false;
            HurtBox.enabled = false;
            isCancelable = false;
            #endregion
        }

        void Cancel()
        {
            StopCoroutine(coroutineStore);

            // Collision disable
            BoxCollider2D.enabled = false;
            CircleCollider2D.enabled = false;
            HurtBox.enabled = false;
            
            wasCanceled = false;
        }
    }
}