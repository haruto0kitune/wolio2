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
                .Where(x => PlayerState.canFightingModeJumpingLightAttack.Value)
                .Where(x => Key.Z)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsFalling", false);
                    Animator.SetBool("IsFightingModeJumpingLightAttack", true);
                });
            #endregion
            #region Fall->JumpingMiddleAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Fall"))
                .Where(x => PlayerState.canFightingModeJumpingMiddleAttack.Value)
                .Where(x => Key.X)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsFalling", false);
                    Animator.SetBool("IsFightingModeJumpingMiddleAttack", true);
                });
            #endregion
            #region Fall->JumpingHighAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Fall"))
                .Where(x => PlayerState.canFightingModeJumpingHighAttack.Value)
                .Where(x => Key.C)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsFalling", false);
                    Animator.SetBool("IsFightingModeJumpingHighAttack", true);
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
                .Where(x => PlayerState.canFightingModeJumpingGuard.Value)
                .Where(x => !PlayerState.IsGrounded.Value)
                .Where(x => Key.LeftShift)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsFalling", false);
                    Animator.SetBool("IsFightingModeJumpingGuard", true);
                });
            #endregion
            #region Fall->Land
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Fall"))
                .Where(x => !PlayerState.IsSkipingLanding.Value)
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
                .Where(x => PlayerState.canFightingModeDoubleJump.Value)
                .Where(x => ActorRigidbody2D.velocity.y < 2)
                .Where(x => Key.Vertical.Value == 1)
                .Where(x => Key.up.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsFalling", false);
                    Animator.SetBool("IsFightingModeDoubleJumping", true);
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
                    Animator.SetBool("IsFalling", false);
                    Animator.SetBool("IsAirDashing", true);
                });
            #endregion
            #region Fall->WallKickJump
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Fall"))
                .Where(x => PlayerState.canWallKickJumping.Value)
                .DistinctUntilChanged(x => Key.Vertical.Value)
                .Where(x => PlayerState.IsInTheAir.Value)
                .Where(x => Key.Vertical.Value == 1f)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsFalling", false);
                    Animator.SetBool("IsWallKickJumping", true);
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
