using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Basics
{
    public class PlayerFall : MonoBehaviour
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
        GameObject PlayerFallHurtBox;
        BoxCollider2D HurtBox;

        void Awake()
        {
            Animator = Actor.GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            ActorRigidbody2D = Actor.GetComponent<Rigidbody2D>();
            PlayerState = Actor.GetComponent<PlayerState>();
            Key = Actor.GetComponent<Key>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            HurtBox = PlayerFallHurtBox.GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            // Animation
            #region Fall->AirDash
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Fall"))
                .Where(x => PlayerState.canAirDash.Value)
                .Where(x => !PlayerState.hasAirDashed.Value)
                .Where(x => PlayerState.hasInputedAirDashCommand.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsFalling", false);
                    Animator.SetBool("IsAirDashing", true);
                });
            #endregion
            #region Fall->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Fall"))
                .Where(x => PlayerState.IsGrounded.Value)
                .Where(x => PlayerState.IsSkipingLanding.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsFalling", false);
                    Animator.SetBool("IsStanding", true);
                });
            #endregion
            #region Fall->JumpingLightAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Fall"))
                .Where(x => PlayerState.canJumpingLightAttack.Value)
                .Where(x => Key.Z)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsFalling", false);
                    Animator.SetBool("IsJumpingLightAttack", true);
                });
            #endregion
            #region Fall->JumpingMiddleAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Fall"))
                .Where(x => PlayerState.canJumpingMiddleAttack.Value)
                .Where(x => Key.X)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsFalling", false);
                    Animator.SetBool("IsJumpingMiddleAttack", true);
                });
            #endregion
            #region Fall->JumpingHighAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Fall"))
                .Where(x => PlayerState.canJumpingHighAttack.Value)
                .Where(x => Key.C)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsFalling", false);
                    Animator.SetBool("IsJumpingHighAttack", true);
                });
            #endregion
            #region Fall->SupineJumpingDamage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Fall"))
                .Where(x => PlayerState.WasSupineAttributeAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsFalling", false);
                    Animator.SetBool("IsSupineJumpingDamage", true);
                });
            #endregion
            #region Fall->ProneJumpingDamage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Fall"))
                .Where(x => PlayerState.WasProneAttributeAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsFalling", false);
                    Animator.SetBool("IsProneJumpingDamage", true);
                });
            #endregion
            #region Fall->JumpingGuard
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Fall"))
                .Where(x => PlayerState.canJumpingGuard.Value)
                .Where(x => !PlayerState.IsGrounded.Value)
                .Where(x => Key.LeftShift)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsFalling", false);
                    Animator.SetBool("IsJumpingGuard", true);
                });
            #endregion
            #region Fall->Land
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Fall"))
                .Where(x => PlayerState.IsGrounded.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsFalling", false);
                    Animator.SetBool("IsLanding", true);
                });
            #endregion
            #region Fall->DoubleJump
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Fall"))
                .Where(x => !PlayerState.IsWallKickJumping.Value)
                .Where(x => !PlayerState.canWallKickJumping.Value)
                .Where(x => PlayerState.canDoubleJump.Value)
                .Where(x => ActorRigidbody2D.velocity.y < 2)
                .Where(x => Key.Vertical.Value == 1)
                .Where(x => Key.up.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsFalling", false);
                    Animator.SetBool("IsDoubleJumping", true);
                });
            #endregion
            #region Fall->AirDash
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Fall"))
                .Where(x => PlayerState.canAirDash.Value)
                .Where(x => !PlayerState.hasAirDashed.Value)
                .Where(x => PlayerState.hasInputedAirDashCommand.Value)
                .Subscribe(_ =>
                {
                    //Debug.Log("Fall->AirDash:hasAirDashed " + PlayerState.hasAirDashed.Value);
                    Animator.SetBool("IsFalling", false);
                    Animator.SetBool("IsAirDashing", true);
                });
            #endregion

            // Collision
            this.ObserveEveryValueChanged(x => Animator.GetBool("IsFalling"))
                .Where(x => x)
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = true;
                    HurtBox.enabled = true;
                });

            this.ObserveEveryValueChanged(x => Animator.GetBool("IsFalling"))
                .Where(x => !x)
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = false;
                    HurtBox.enabled = false;
                });
        }
    }
}
