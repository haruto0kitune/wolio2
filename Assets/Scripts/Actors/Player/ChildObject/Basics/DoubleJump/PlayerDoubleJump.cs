using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Basics
{
    public class PlayerDoubleJump : MonoBehaviour
    {
        [SerializeField]
        GameObject Actor;
        Animator Animator;
        ObservableStateMachineTrigger ObservableStateMachineTrigger;
        Rigidbody2D ActorRigidbody2D;
        PlayerState PlayerState;
        BoxCollider2D BoxCollider2D;
        [SerializeField]
        GameObject PlayerDoubleJumpHurtBox;
        BoxCollider2D HurtBox;
        Key Key;
        [SerializeField]
        float DoubleJumpForce;
        Coroutine coroutineStore;

        void Awake()
        {
            Animator = Actor.GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            ActorRigidbody2D = Actor.GetComponent<Rigidbody2D>();
            PlayerState = Actor.GetComponent<PlayerState>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            HurtBox = PlayerDoubleJumpHurtBox.GetComponent<BoxCollider2D>();
            Key = Actor.GetComponent<Key>();
        }

        void Start()
        {
            //Animation
            #region DoubleJump->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.DoubleJump"))
                .Where(x => PlayerState.IsGrounded.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", true);
                    Animator.SetBool("IsDoubleJumping", false);
                });
            #endregion
            #region DoubleJump->JumpingLightAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.DoubleJump"))
                .Where(x => PlayerState.canJumpingLightAttack.Value)
                .Where(x => Key.Z)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsDoubleJumping", false);
                    Animator.SetBool("IsJumpingLightAttack", true);
                });
            #endregion
            #region DoubleJump->JumpingMiddleAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.DoubleJump"))
                .Where(x => PlayerState.canJumpingMiddleAttack.Value)
                .Where(x => Key.X)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsDoubleJumping", false);
                    Animator.SetBool("IsJumpingMiddleAttack", true);
                });
            #endregion
            #region DoubleJump->JumpingHighAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.DoubleJump"))
                .Where(x => PlayerState.canJumpingHighAttack.Value)
                .Where(x => Key.C)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsDoubleJumping", false);
                    Animator.SetBool("IsJumpingHighAttack", true);
                });
            #endregion
            #region DoubleJump->JumpingGuard
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.DoubleJump"))
                .Where(x => PlayerState.canJumpingGuard.Value)
                .Where(x => !PlayerState.IsGrounded.Value)
                .Where(x => Key.LeftShift)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsDoubleJumping", false);
                    Animator.SetBool("IsJumpingGuard", true);
                });
            #endregion
            #region DoubleJump->SupineJumpingDamage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.DoubleJump"))
                .Where(x => PlayerState.WasSupineAttributeAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsDoubleJumping", false);
                    Animator.SetBool("IsSupineJumpingDamage", true);
                });
            #endregion
            #region DoubleJump->ProneJumpingDamage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.DoubleJump"))
                .Where(x => PlayerState.WasProneAttributeAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsDoubleJumping", false);
                    Animator.SetBool("IsProneJumpingDamage", true);
                });
            #endregion
            #region DoubleJump->Fall
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.DoubleJump"))
                .Where(x => ActorRigidbody2D.velocity.y < 0)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsDoubleJumping", false);
                    Animator.SetBool("IsFalling", true);
                });
            #endregion
            #region DoubleJump->AirDash
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.DoubleJump"))
                .Where(x => PlayerState.canAirDash.Value)
                .Where(x => !PlayerState.hasAirDashed.Value)
                .Where(x => PlayerState.hasInputedAirDashCommand.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsDoubleJumping", false);
                    Animator.SetBool("IsAirDashing", true);
                });
            #endregion

            // Motion
            this.FixedUpdateAsObservable()
                .Where(x => PlayerState.canDoubleJump.Value)
                .Where(x => !PlayerState.hasDoubleJumped.Value)
                .Where(x => Key.Vertical.Value == 1)
                .Where(x => ActorRigidbody2D.velocity.y < 2)
                .Where(x => coroutineStore == null)
                .Subscribe(_ => coroutineStore = StartCoroutine(DoubleJump()));

            //Collision
            this.ObserveEveryValueChanged(x => Animator.GetBool("IsDoubleJumping"))
                .Where(x => x)
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = true;
                    HurtBox.enabled = true;
                });

            this.ObserveEveryValueChanged(x => Animator.GetBool("IsDoubleJumping"))
                .Where(x => !x)
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = false;
                    HurtBox.enabled = false;
                });
        }

        IEnumerator DoubleJump()
        {
            // Reset velocity
            ActorRigidbody2D.velocity = Vector2.zero;
            yield return null;

            // Jump
            ActorRigidbody2D.velocity = new Vector2(6 * Key.Horizontal.Value, DoubleJumpForce);
            PlayerState.hasDoubleJumped.Value = true;
            coroutineStore = null;
        }
    }
}