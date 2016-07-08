using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Attacks.NormalAttacks.StandingAttacks
{
    public class PlayerStandingLightAttack : MonoBehaviour
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
        GameObject PlayerStandingLightAttackHitBox;
        BoxCollider2D HitBox;
        [SerializeField]
        GameObject PlayerStandingLightAttackHurtBox;
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
        IEnumerator _Attack;
        IEnumerator _HitStop;

        void Awake()
        {
            Animator = Player.GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            PlayerState = Player.GetComponent<PlayerState>();
            PlayerRigidbody2D = Player.GetComponent<Rigidbody2D>();
            Key = Player.GetComponent<Key>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            CircleCollider2D = GetComponent<CircleCollider2D>();
            HitBox = PlayerStandingLightAttackHitBox.GetComponent<BoxCollider2D>();
            HurtBox = PlayerStandingLightAttackHurtBox.GetComponent<BoxCollider2D>();
            _Attack = Attack();
            _HitStop = HitStop(hitStop);
        }

        void Start()
        {
            // Animation
            #region EnterStandingLightAttack
            ObservableStateMachineTrigger
                .OnStateEnterAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.StandingLightAttack"))
                .Subscribe(_ => Animator.speed = 0);
            #endregion
            #region StandingLightAttack->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.StandingLightAttack"))
                .Where(x => wasFinished)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStandingLightAttack", false);
                    Animator.SetBool("IsStanding", true);
                    wasFinished = false;
                });
            #endregion
            #region StandingLightAttack->Jump
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.StandingLightAttack"))
                .Where(x => isCancelable)
                .Where(x => Key.Vertical.Value == 1f)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStandingLightAttack", false);
                    Animator.SetBool("IsJumping", true);
                    isCancelable = false;
                    StopCoroutine(coroutineStore);
                    wasCanceled = true;
                });
            #endregion
            #region StandingLightAttack->StandingLightAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.StandingLightAttack"))
                .Where(x => PlayerState.canStandingLightAttack.Value)
                .Where(x => isCancelable)
                .Where(x => Key.Z && Key.Vertical.Value == 0)
                .Subscribe(_ =>
                {
                    Animator.Play("StandingLightAttack", Animator.GetLayerIndex("Base Layer"), 0.0f);
                    isCancelable = false;
                    StopCoroutine(coroutineStore);
                    coroutineStore = StartCoroutine(Attack());
                });
            #endregion
            #region StandingLightAttack->StandingMiddleAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.StandingLightAttack"))
                .Where(x => PlayerState.canStandingMiddleAttack.Value)
                .Where(x => isCancelable)
                .Where(x => Key.X && Key.Vertical.Value == 0)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStandingLightAttack", false);
                    Animator.SetBool("IsStandingMiddleAttack", true);
                    isCancelable = false;
                    StopCoroutine(coroutineStore);
                    wasCanceled = true;
                });
            #endregion
            #region StandingLightAttack->StandingHighAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.StandingLightAttack"))
                .Where(x => PlayerState.canStandingHighAttack.Value)
                .Where(x => isCancelable)
                .Where(x => Key.C && Key.Vertical.Value == 0)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStandingLightAttack", false);
                    Animator.SetBool("IsStandingHighAttack", true);
                    isCancelable = false;
                    StopCoroutine(coroutineStore);
                    wasCanceled = true;
                });
            #endregion
            #region StandingLightAttack->CrouchingLightAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.StandingLightAttack"))
                .Where(x => PlayerState.canCrouchingLightAttack.Value)
                .Where(x => isCancelable)
                .Where(x => Key.Z && Key.Vertical.Value == -1f)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStandingLightAttack", false);
                    Animator.SetBool("IsCrouchingLightAttack", true);
                    isCancelable = false;
                    StopCoroutine(coroutineStore);
                    wasCanceled = true;
                });
            #endregion
            #region StandingLightAttack->CrouchingMiddleAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.StandingLightAttack"))
                .Where(x => PlayerState.canCrouchingMiddleAttack.Value)
                .Where(x => isCancelable)
                .Where(x => Key.X && Key.Vertical.Value == -1f)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStandingLightAttack", false);
                    Animator.SetBool("IsCrouchingMiddleAttack", true);
                    isCancelable = false;
                    StopCoroutine(coroutineStore);
                    wasCanceled = true;
                });
            #endregion
            #region StandingLightAttack->CrouchingHighAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.StandingLightAttack"))
                .Where(x => PlayerState.canCrouchingHighAttack.Value)
                .Where(x => isCancelable)
                .Where(x => Key.C && Key.Vertical.Value == -1f)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStandingLightAttack", false);
                    Animator.SetBool("IsCrouchingHighAttack", true);
                    isCancelable = false;
                    StopCoroutine(coroutineStore);
                    wasCanceled = true;
                });
            #endregion
            #region StandingLightAttack->LightFireballMotion
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.StandingLightAttack"))
                .Where(x => PlayerState.canFireballMotion.Value)
                .Where(x => isCancelable)
                .Where(x => PlayerState.hasInputedLightFireballMotionCommand.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStandingLightAttack", false);
                    Animator.SetBool("IsLightFireballMotion", true);
                    isCancelable = false;
                    StopCoroutine(coroutineStore);
                    wasCanceled = true;
                });
            #endregion
            #region StandingLightAttack->MiddleFireballMotion
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.StandingLightAttack"))
                .Where(x => PlayerState.canFireballMotion.Value)
                .Where(x => isCancelable)
                .Where(x => PlayerState.hasInputedMiddleFireballMotionCommand.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStandingLightAttack", false);
                    Animator.SetBool("IsMiddleFireballMotion", true);
                    isCancelable = false;
                    StopCoroutine(coroutineStore);
                    wasCanceled = true;
                });
            #endregion
            #region StandingLightAttack->HighFireballMotion
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.StandingLightAttack"))
                .Where(x => PlayerState.canFireballMotion.Value)
                .Where(x => isCancelable)
                .Where(x => PlayerState.hasInputedHighFireballMotionCommand.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStandingLightAttack", false);
                    Animator.SetBool("IsHighFireballMotion", true);
                    isCancelable = false;
                    StopCoroutine(coroutineStore);
                    wasCanceled = true;
                });
            #endregion
            #region StandingLightAttack->LightDragonPunch
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.StandingLightAttack"))
                .Where(x => PlayerState.canFireballMotion.Value)
                .Where(x => isCancelable)
                .Where(x => PlayerState.hasInputedLightDragonPunchCommand.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStandingLightAttack", false);
                    Animator.SetBool("IsLightDragonPunch", true);
                    isCancelable = false;
                    StopCoroutine(coroutineStore);
                    wasCanceled = true;
                });
            #endregion
            #region StandingLightAttack->MiddleDragonPunch
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.StandingLightAttack"))
                .Where(x => PlayerState.canFireballMotion.Value)
                .Where(x => isCancelable)
                .Where(x => PlayerState.hasInputedMiddleDragonPunchCommand.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStandingLightAttack", false);
                    Animator.SetBool("IsMiddleDragonPunch", true);
                    isCancelable = false;
                    StopCoroutine(coroutineStore);
                    wasCanceled = true;
                });
            #endregion
            #region StandingLightAttack->HighDragonPunch
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.StandingLightAttack"))
                .Where(x => PlayerState.canFireballMotion.Value)
                .Where(x => isCancelable)
                .Where(x => PlayerState.hasInputedHighDragonPunchCommand.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStandingLightAttack", false);
                    Animator.SetBool("IsHighDragonPunch", true);
                    isCancelable = false;
                    StopCoroutine(coroutineStore);
                    wasCanceled = true;
                });
            #endregion

            // Collision
            this.ObserveEveryValueChanged(x => Animator.GetBool("IsStandingLightAttack"))
                .Where(x => x)
                .Subscribe(_ => coroutineStore = StartCoroutine(_Attack));

            this.ObserveEveryValueChanged(x => wasCanceled)
                .Where(x => x)
                .Subscribe(_ => Cancel());

            // Damage
            PlayerStandingLightAttackHitBox.OnTriggerEnter2DAsObservable()
                .Where(x => x.gameObject.tag == "Enemy/HurtBox")
                .Subscribe(_ =>
                {
                    _.gameObject.GetComponent<DamageManager>().ApplyDamage(damageValue, hitRecovery, hitStop, isTechable, hasKnockdownAttribute, attackAttribute, knockdownAttribute);
                    HitBox.enabled = false;
                    isCancelable = true;
                    StartCoroutine(HitStop(hitStop));
                });
        }

        IEnumerator Attack()
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
            _Attack = Attack();
            #endregion
        }

        IEnumerator HitStop(int hitStopFrame)
        {
            StopCoroutine(coroutineStore);

            for(var i = 0;i < hitStopFrame;i++)
            {
                yield return null;
            }

            coroutineStore = StartCoroutine(_Attack);
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