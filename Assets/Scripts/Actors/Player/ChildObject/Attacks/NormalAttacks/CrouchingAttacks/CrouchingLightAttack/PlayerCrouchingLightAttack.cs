using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Attacks.NormalAttacks.CrouchingAttacks
{
    public class PlayerCrouchingLightAttack : MonoBehaviour
    {
        [SerializeField]
        GameObject Player;
        Animator Animator;
        ObservableStateMachineTrigger ObservableStateMachineTrigger;
        PlayerState PlayerState;
        Rigidbody2D PlayerRigidbody2D;
        Key Key;
        BoxCollider2D BoxCollider2D;
        [SerializeField]
        GameObject PlayerCrouchingLightAttackHitBox;
        BoxCollider2D HitBox;
        [SerializeField]
        GameObject PlayerCrouchingLightAttackHurtBox;
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
            HitBox = PlayerCrouchingLightAttackHitBox.GetComponent<BoxCollider2D>();
            HurtBox = PlayerCrouchingLightAttackHurtBox.GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            // Animation
            #region EnterCrouchingLightAttack
            ObservableStateMachineTrigger
                 .OnStateEnterAsObservable()
                 .Where(x => x.StateInfo.IsName("Base Layer.CrouchingLightAttack"))
                 .Subscribe(_ => Animator.speed = 0);
            #endregion
            #region CrouchingLightAttack->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.CrouchingLightAttack"))
                .Where(x => wasFinished && Key.Vertical.Value == 0)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouchingLightAttack", false);
                    Animator.SetBool("IsStanding", true);
                    wasFinished = false;
                });
            #endregion
            #region CrouchingLightAttack->Crouch
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.CrouchingLightAttack"))
                .Where(x => wasFinished && Key.Vertical.Value == -1f)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouchingLightAttack", false);
                    Animator.SetBool("IsCrouching", true);
                    wasFinished = false;
                });
            #endregion
            #region CrouchingLightAttack->Jump
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.CrouchingLightAttack"))
                .Where(x => wasFinished && Key.Vertical.Value == 1f)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouchingLightAttack", false);
                    Animator.SetBool("IsJumping", true);
                    wasFinished = false;
                });
            #endregion
            #region CrouchingLightAttack->StandingLightAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.CrouchingLightAttack"))
                .Where(x => isCancelable)
                .Where(x => Key.Z && Key.Vertical.Value == 0)
                .Subscribe(_ =>
                {
                    Debug.Log("aha");
                    Animator.SetBool("IsCrouchingLightAttack", false);
                    Animator.SetBool("IsStandingLightAttack", true);
                    isCancelable = false;
                    StopCoroutine(coroutineStore);
                    wasCanceled = true;
                });
            #endregion
            #region CrouchingLightAttack->StandingMiddleAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.CrouchingLightAttack"))
                .Where(x => isCancelable)
                .Where(x => Key.X && Key.Vertical.Value == 0)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouchingLightAttack", false);
                    Animator.SetBool("IsStandingMiddleAttack", true);
                    isCancelable = false;
                    StopCoroutine(coroutineStore);
                    wasCanceled = true;
                });
            #endregion
            #region CrouchingLightAttack->StandingHighAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.CrouchingLightAttack"))
                .Where(x => isCancelable)
                .Where(x => Key.C && Key.Vertical.Value == 0)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouchingLightAttack", false);
                    Animator.SetBool("IsStandingHighAttack", true);
                    isCancelable = false;
                    StopCoroutine(coroutineStore);
                    wasCanceled = true;
                });
            #endregion
            #region CrouchingLightAttack->CrouchingLightAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.CrouchingLightAttack"))
                .Where(x => isCancelable)
                .Where(x => Key.Z && Key.Vertical.Value == -1f)
                .Subscribe(_ =>
                {
                    Animator.Play("CrouchingLightAttack", Animator.GetLayerIndex("Base Layer"), 0.0f);
                    isCancelable = false;
                    StopCoroutine(coroutineStore);
                    coroutineStore = StartCoroutine(Attack());
                });
            #endregion
            #region CrouchingLightAttack->CrouchingMiddleAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.CrouchingLightAttack"))
                .Where(x => isCancelable)
                .Where(x => Key.X && Key.Vertical.Value == -1f)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouchingLightAttack", false);
                    Animator.SetBool("IsCrouchingMiddleAttack", true);
                    isCancelable = false;
                    StopCoroutine(coroutineStore);
                    wasCanceled = true;
                });
            #endregion
            #region CrouchingLightAttack->CrouchingHighAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.CrouchingLightAttack"))
                .Where(x => isCancelable)
                .Where(x => Key.C && Key.Vertical.Value == -1f)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouchingLightAttack", false);
                    Animator.SetBool("IsCrouchingHighAttack", true);
                    isCancelable = false;
                    StopCoroutine(coroutineStore);
                    wasCanceled = true;
                });
            #endregion

            // Collision
            this.ObserveEveryValueChanged(x => Animator.GetBool("IsCrouchingLightAttack"))
                .Where(x => x)
                .Subscribe(_ => coroutineStore = StartCoroutine(Attack()));

            this.ObserveEveryValueChanged(x => wasCanceled)
                .Where(x => x)
                .Subscribe(_ => Cancel());

            // Damage
            PlayerCrouchingLightAttackHitBox.OnTriggerEnter2DAsObservable()
                .Where(x => x.gameObject.tag == "Enemy/HurtBox")
                .Subscribe(_ =>
                {
                    _.gameObject.GetComponent<DamageManager>().ApplyDamage(damageValue, hitRecovery, hitStop, isTechable, hasKnockdownAttribute, attackAttribute);
                    HitBox.enabled = false;
                    isCancelable = true;
                });
        }

        public IEnumerator Attack()
        {
            #region Startup
            BoxCollider2D.enabled = true;
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
            HurtBox.enabled = false;
            isCancelable = false;
            #endregion
        }

        void Cancel()
        {
            StopCoroutine(coroutineStore);

            // Collision disable
            BoxCollider2D.enabled = false;
            HurtBox.enabled = false;
            
            wasCanceled = false;
        }
    }
}