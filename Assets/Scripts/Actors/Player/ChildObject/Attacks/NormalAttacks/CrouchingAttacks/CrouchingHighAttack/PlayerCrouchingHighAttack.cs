using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Attacks.NormalAttacks.CrouchingAttacks
{
    public class PlayerCrouchingHighAttack : MonoBehaviour
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
        GameObject PlayerCrouchingHighAttackHitBox;
        BoxCollider2D HitBox;
        [SerializeField]
        GameObject PlayerCrouchingHighAttackHurtBox;
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
            HitBox = PlayerCrouchingHighAttackHitBox.GetComponent<BoxCollider2D>();
            HurtBox = PlayerCrouchingHighAttackHurtBox.GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            //Animation
            #region EnterCrouchingHighAttack
            ObservableStateMachineTrigger
                 .OnStateEnterAsObservable()
                 .Where(x => x.StateInfo.IsName("Base Layer.CrouchingHighAttack"))
                 .Subscribe(_ => Animator.speed = 0);
            #endregion
            #region CrouchingHighAttack->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.CrouchingHighAttack"))
                .Where(x => wasFinished && Key.Vertical.Value == 0)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouchingHighAttack", false);
                    Animator.SetBool("IsStanding", true);
                    wasFinished = false;
                });
            #endregion
            #region CrouchingHighAttack->Crouch
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.CrouchingHighAttack"))
                .Where(x => wasFinished && Key.Vertical.Value == -1f)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouchingHighAttack", false);
                    Animator.SetBool("IsCrouching", true);
                    wasFinished = false;
                });
            #endregion
            #region CrouchingHighAttack->Jump
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.CrouchingHighAttack"))
                .Where(x => wasFinished && Key.Vertical.Value == 1f)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouchingHighAttack", false);
                    Animator.SetBool("IsFightingModeJumping", true);
                    wasFinished = false;
                });
            #endregion
            #region CrouchingHighAttack->LightFireballMotion
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.CrouchingHighAttack"))
                .Where(x => PlayerState.canFireballMotion.Value)
                .Where(x => isCancelable)
                .Where(x => PlayerState.hasInputedLightFireballMotionCommand.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouchingHighAttack", false);
                    Animator.SetBool("IsLightFireballMotion", true);
                    isCancelable = false;
                    StopCoroutine(coroutineStore);
                    wasCanceled = true;
                });
            #endregion
            #region CrouchingHighAttack->MiddleFireballMotion
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.CrouchingHighAttack"))
                .Where(x => PlayerState.canFireballMotion.Value)
                .Where(x => isCancelable)
                .Where(x => PlayerState.hasInputedMiddleFireballMotionCommand.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouchingHighAttack", false);
                    Animator.SetBool("IsMiddleFireballMotion", true);
                    isCancelable = false;
                    StopCoroutine(coroutineStore);
                    wasCanceled = true;
                });
            #endregion
            #region CrouchingHighAttack->HighFireballMotion
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.CrouchingHighAttack"))
                .Where(x => PlayerState.canFireballMotion.Value)
                .Where(x => isCancelable)
                .Where(x => PlayerState.hasInputedHighFireballMotionCommand.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouchingHighAttack", false);
                    Animator.SetBool("IsHighFireballMotion", true);
                    isCancelable = false;
                    StopCoroutine(coroutineStore);
                    wasCanceled = true;
                });
            #endregion
            #region CrouchingHighAttack->LightDragonPunch
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.CrouchingHighAttack"))
                .Where(x => PlayerState.canFireballMotion.Value)
                .Where(x => isCancelable)
                .Where(x => PlayerState.hasInputedLightDragonPunchCommand.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouchingHighAttack", false);
                    Animator.SetBool("IsLightDragonPunch", true);
                    isCancelable = false;
                    StopCoroutine(coroutineStore);
                    wasCanceled = true;
                });
            #endregion
            #region CrouchingHighAttack->MiddleDragonPunch
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.CrouchingHighAttack"))
                .Where(x => PlayerState.canFireballMotion.Value)
                .Where(x => isCancelable)
                .Where(x => PlayerState.hasInputedMiddleDragonPunchCommand.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouchingHighAttack", false);
                    Animator.SetBool("IsMiddleDragonPunch", true);
                    isCancelable = false;
                    StopCoroutine(coroutineStore);
                    wasCanceled = true;
                });
            #endregion
            #region CrouchingHighAttack->HighDragonPunch
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.CrouchingHighAttack"))
                .Where(x => PlayerState.canFireballMotion.Value)
                .Where(x => isCancelable)
                .Where(x => PlayerState.hasInputedHighDragonPunchCommand.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouchingHighAttack", false);
                    Animator.SetBool("IsHighDragonPunch", true);
                    isCancelable = false;
                    StopCoroutine(coroutineStore);
                    wasCanceled = true;
                });
            #endregion

            //Collision
            this.ObserveEveryValueChanged(x => Animator.GetBool("IsCrouchingHighAttack"))
                .Where(x => x)
                .Subscribe(_ => coroutineStore = StartCoroutine(Attack()));
            
            this.ObserveEveryValueChanged(x => wasCanceled)
                .Where(x => x)
                .Subscribe(_ => Cancel());

            // Damage
            PlayerCrouchingHighAttackHitBox.OnTriggerEnter2DAsObservable()
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