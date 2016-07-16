using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Basics
{
    public class PlayerAirDash : MonoBehaviour
    {
        [SerializeField]
        GameObject Actor;
        Animator Animator;
        ObservableStateMachineTrigger ObservableStateMachineTrigger;
        Rigidbody2D ActorRigidbody2D;
        PlayerState PlayerState;
        Key Key;
        BoxCollider2D BoxCollider2D;
        [SerializeField]
        GameObject PlayerAirDashHurtBox;
        BoxCollider2D HurtBox;
        bool hasFinished;
        [SerializeField]
        float speed;
        [SerializeField]
        int recovery;
        bool wasCanceled;
        Coroutine coroutineStore;

        void Awake()
        {
            Animator = Actor.GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            ActorRigidbody2D = Actor.GetComponent<Rigidbody2D>();
            PlayerState = Actor.GetComponent<PlayerState>();
            Key = Actor.GetComponent<Key>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            HurtBox = PlayerAirDashHurtBox.GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            // Animation
            #region AirDash->Fall
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.AirDash"))
                .Where(x => hasFinished)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsAirDashing", false);
                    Animator.SetBool("IsFalling", true);
                    hasFinished = false;
                });
            #endregion
            #region AirDash->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.AirDash"))
                .Where(x => PlayerState.IsGrounded.Value)
                .Where(x => PlayerState.IsSkipingLanding.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsAirDashing", false);
                    Animator.SetBool("IsStanding", true);
                    hasFinished = false;
                });
            #endregion
            #region AirDash->JumpingLightAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.AirDash"))
                .Where(x => PlayerState.canJumpingLightAttack.Value)
                .Where(x => Key.Z)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsAirDashing", false);
                    Animator.SetBool("IsJumpingLightAttack", true);
                    hasFinished = false;
                });
            #endregion
            #region AirDash->JumpingMiddleAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.AirDash"))
                .Where(x => PlayerState.canJumpingMiddleAttack.Value)
                .Where(x => Key.X)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsAirDashing", false);
                    Animator.SetBool("IsJumpingMiddleAttack", true);
                    hasFinished = false;
                });
            #endregion
            #region AirDash->JumpingHighAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.AirDash"))
                .Where(x => PlayerState.canJumpingHighAttack.Value)
                .Where(x => Key.C)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsAirDashing", false);
                    Animator.SetBool("IsJumpingHighAttack", true);
                    hasFinished = false;
                });
            #endregion
            #region AirDash->SupineJumpingDamage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.AirDash"))
                .Where(x => PlayerState.WasSupineAttributeAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsAirDashing", false);
                    Animator.SetBool("IsSupineJumpingDamage", true);
                    hasFinished = false;
                });
            #endregion
            #region AirDash->ProneJumpingDamage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.AirDash"))
                .Where(x => PlayerState.WasProneAttributeAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsAirDashing", false);
                    Animator.SetBool("IsProneJumpingDamage", true);
                    hasFinished = false;
                });
            #endregion

            // Motion
            ObservableStateMachineTrigger
                .OnStateEnterAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.AirDash"))
                .Subscribe(_ =>
                {
                    hasFinished = false;
                    coroutineStore = StartCoroutine(AirDash());
                });

            // Collision
            this.ObserveEveryValueChanged(x => Animator.GetBool("IsAirDashing"))
                .Where(x => x)
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = true;
                    HurtBox.enabled = true;
                });

            this.ObserveEveryValueChanged(x => Animator.GetBool("IsAirDashing"))
                .Where(x => !x)
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = false;
                    HurtBox.enabled = false;
                });

            // Cancel
            this.ObserveEveryValueChanged(x => wasCanceled)
                .Where(x => x)
                .Subscribe(_ => Cancel());

            this.ObserveEveryValueChanged(x => PlayerState.WasAttacked.Value)
                .Where(x => x)
                .Subscribe(_ => wasCanceled = _);
        }

        IEnumerator AirDash()
        {
            PlayerState.hasAirDashed.Value = true;
            ActorRigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            
            for (int i = 0; i < recovery; i++)
            {
                if (PlayerState.FacingRight.Value)
                {
                    ActorRigidbody2D.velocity = new Vector2(speed, 0);
                }
                else
                {
                    ActorRigidbody2D.velocity = new Vector2(-speed, 0);
                }

                yield return null;
            }

            ActorRigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            hasFinished = true;
        }

        void Cancel()
        {
            StopCoroutine(coroutineStore);

            // Collision disable
            BoxCollider2D.enabled = false;
            HurtBox.enabled = false;
            hasFinished = true;
            ActorRigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;

            wasCanceled = false;
        }
    }
}