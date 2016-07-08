using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Attacks.SpecialAttacks
{
    public class PlayerLightHurricaneKick : MonoBehaviour
    {
        [SerializeField]
        GameObject Player;
        Rigidbody2D Rigidbody2D;
        Animator Animator;
        ObservableStateMachineTrigger ObservableStateMachineTrigger;
        BoxCollider2D BoxCollider2D;
        PlayerState PlayerState;
        [SerializeField]
        GameObject HurricaneKickHitBox;
        BoxCollider2D HitBox;
        [SerializeField]
        GameObject HurricaneKickHurtBox;
        BoxCollider2D HurtBox;
        bool hasFinished;
        Coroutine coroutineStore;
        [SerializeField]
        int startup;
        [SerializeField]
        int active;
        [SerializeField]
        int recovery;
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
        [SerializeField]
        KnockdownAttribute knockdownAttribute;

        bool wasCanceled;

        void Awake()
        {
            Rigidbody2D = Player.GetComponent<Rigidbody2D>();
            Animator = Player.GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            PlayerState = Player.GetComponent<PlayerState>();
            HitBox = HurricaneKickHitBox.GetComponent<BoxCollider2D>();
            HurtBox = HurricaneKickHurtBox.GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            // Animation
            #region EnterHurricaneKick
            ObservableStateMachineTrigger
                .OnStateEnterAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.LightHurricaneKick"))
                .Do(x => Debug.Log("Enter HurricaneKick"))
                .Subscribe(_ => coroutineStore = StartCoroutine(LightHurricaneKickCoroutine()));
            #endregion
            #region HurricaneKick->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.LightHurricaneKick"))
                .Where(x => hasFinished && PlayerState.IsGrounded.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsLightHurricaneKick", false);
                    Animator.SetBool("IsStanding", true);
                    hasFinished = false;
                    PlayerState.hasInputedLightHurricaneKickCommand.Value = false;
                });
            #endregion
            #region HurricaneKick->SupineJumpingDamage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.LightHurricaneKick"))
                .Where(x => PlayerState.WasSupineAttributeAttacked.Value && PlayerState.WasKnockdownAttributeAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsLightHurricaneKick", false);
                    Animator.SetBool("IsSupineJumpingDamage", true);
                });
            #endregion
            #region HurricaneKick->ProneJumpingDamage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.LightHurricaneKick"))
                .Where(x => PlayerState.WasProneAttributeAttacked.Value && PlayerState.WasKnockdownAttributeAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsLightHurricaneKick", false);
                    Animator.SetBool("IsProneJumpingDamage", true);
                });
            #endregion

            // Collision
            this.ObserveEveryValueChanged(x => wasCanceled)
                .Where(x => x)
                .Subscribe(_ => Cancel());

            this.ObserveEveryValueChanged(x => PlayerState.WasAttacked.Value)
                .Where(x => PlayerState.IsHurricaneKick.Value)
                .Subscribe(_ => wasCanceled = _);

            this.UpdateAsObservable()
                .Where(x => !gameObject.activeSelf)
                .Subscribe(x => Debug.Log("HurricaneKick Active: " + gameObject.activeSelf));

            
            // Damage
            HurricaneKickHitBox.OnTriggerEnter2DAsObservable()
                .Where(x => x.gameObject.tag == "Enemy/HurtBox")
                .Subscribe(_ =>
                {
                    _.gameObject.GetComponent<DamageManager>().ApplyDamage(damageValue, hitRecovery, hitStop, isTechable, hasKnockdownAttribute, attackAttribute, knockdownAttribute);
                    HitBox.enabled = false;
                });

        }

        IEnumerator LightHurricaneKickCoroutine()
        {
            // Startup
            Animator.speed = 1f;
            BoxCollider2D.enabled = true;
            HitBox.enabled = true;

            for (int i = 0; i < startup; i++)
            {
                yield return null;
            }

            // Active
            Rigidbody2D.isKinematic = true;

            if (PlayerState.FacingRight.Value)
            {
                Rigidbody2D.velocity = new Vector2(3f, 0f);
            }
            else
            {
                Rigidbody2D.velocity = new Vector2(-3f, 0f);
            }

            for (int i = 0; i < active; i++)
            {
                yield return null;
            }

            Rigidbody2D.isKinematic = false;

            // Recovery
            for (int i = 0; i < recovery; i++)
            {
                yield return null;
            }

            hasFinished = true;
            yield return null;

            BoxCollider2D.enabled = false;
            HitBox.enabled = false;
            HurtBox.enabled = false;
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