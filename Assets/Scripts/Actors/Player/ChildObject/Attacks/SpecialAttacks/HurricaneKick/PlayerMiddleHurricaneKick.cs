using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Attacks.SpecialAttacks
{
    public class PlayerMiddleHurricaneKick : MonoBehaviour
    {
        [SerializeField]
        GameObject Player;
        Rigidbody2D Rigidbody2D;
        Animator Animator;
        ObservableStateMachineTrigger ObservableStateMachineTrigger;
        BoxCollider2D BoxCollider2D;
        CircleCollider2D CircleCollider2D;
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
            CircleCollider2D = GetComponent<CircleCollider2D>();
            PlayerState = Player.GetComponent<PlayerState>();
            HitBox = HurricaneKickHitBox.GetComponent<BoxCollider2D>();
            HurtBox = HurricaneKickHurtBox.GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            // Animation
            #region EnterMiddleHurricaneKick
            ObservableStateMachineTrigger
                .OnStateEnterAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.MiddleHurricaneKick"))
                .Do(x => Debug.Log("Enter HurricaneKick"))
                .Subscribe(_ => coroutineStore = StartCoroutine(MiddleHurricaneKickCoroutine()));
            #endregion
            #region MiddleHurricaneKick->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.MiddleHurricaneKick"))
                .Where(x => hasFinished && PlayerState.IsGrounded.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsMiddleHurricaneKick", false);
                    Animator.SetBool("IsStanding", true);
                    hasFinished = false;
                    PlayerState.hasInputedMiddleHurricaneKickCommand.Value = false;
                });
            #endregion
            #region MiddleHurricaneKick->SupineJumpingDamage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.MiddleHurricaneKick"))
                .Where(x => PlayerState.WasSupineAttributeAttacked.Value && PlayerState.WasKnockdownAttributeAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsMiddleHurricaneKick", false);
                    Animator.SetBool("IsSupineJumpingDamage", true);
                });
            #endregion
            #region MiddleHurricaneKick->ProneJumpingDamage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.MiddleHurricaneKick"))
                .Where(x => PlayerState.WasProneAttributeAttacked.Value && PlayerState.WasKnockdownAttributeAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsMiddleHurricaneKick", false);
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

        IEnumerator MiddleHurricaneKickCoroutine()
        {
            // Startup
            Animator.speed = 1f;
            BoxCollider2D.enabled = true;
            CircleCollider2D.enabled = true;
            HitBox.enabled = true;

            var gracityScaleStore = Rigidbody2D.gravityScale;
            Rigidbody2D.gravityScale = 0f;

            for (int i = 0; i < startup; i++)
            {
                if (i < 2)
                {
                    if (PlayerState.FacingRight.Value)
                    {
                        Rigidbody2D.velocity = new Vector2(4f, 2f);
                    }
                    else
                    {
                        Rigidbody2D.velocity = new Vector2(-4f, 2f);
                    }
                }
                else
                {
                    if (PlayerState.FacingRight.Value)
                    {
                        Rigidbody2D.velocity = new Vector2(4f, 0f);
                    }
                    else
                    {
                        Rigidbody2D.velocity = new Vector2(-4f, 0f);
                    }
                }

                yield return null;
            }

            // Active

            for (int i = 0; i < active; i++)
            {
                if (PlayerState.FacingRight.Value)
                {
                    Rigidbody2D.velocity = new Vector2(4f, 0f);
                }
                else
                {
                    Rigidbody2D.velocity = new Vector2(-4f, 0f);
                }

                yield return null;
            }

            Rigidbody2D.gravityScale = gracityScaleStore;

            // Recovery
            for (int i = 0; i < recovery; i++)
            {
                yield return null;
            }

            hasFinished = true;
            yield return null;

            BoxCollider2D.enabled = false;
            CircleCollider2D.enabled = false;
            HitBox.enabled = false;
            HurtBox.enabled = false;
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