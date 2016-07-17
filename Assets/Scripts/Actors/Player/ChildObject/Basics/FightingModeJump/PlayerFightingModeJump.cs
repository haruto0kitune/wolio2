using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Basics
{
    public class PlayerFightingModeJump : MonoBehaviour
    {
        [SerializeField]
        GameObject Actor;
        Animator Animator;
        ObservableStateMachineTrigger ObservableStateMachineTrigger;
        PlayerState PlayerState;
        Rigidbody2D PlayerRigidbody2D;
        Key Key;
        BoxCollider2D BoxCollider2D;
        [SerializeField]
        GameObject PlayerFightingModeJumpHurtBox;
        BoxCollider2D HurtBox;
        [SerializeField]
        float JumpForce;

        void Awake()
        {
            Animator = Actor.GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            PlayerState = Actor.GetComponent<PlayerState>();
            PlayerRigidbody2D = Actor.GetComponent<Rigidbody2D>();
            Key = Actor.GetComponent<Key>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            HurtBox = PlayerFightingModeJumpHurtBox.GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            //Animation
            #region Jump->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.FightingModeJump"))
                .Where(x => PlayerState.IsGrounded.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", true);
                    Animator.SetBool("IsFightingModeJumping", false);
                });
            #endregion
            #region Jump->JumpingLightAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.FightingModeJump"))
                .Where(x => PlayerState.canFightingModeJumpingLightAttack.Value)
                .Where(x => Key.Z)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsFightingModeJumping", false);
                    Animator.SetBool("IsFightingModeJumpingLightAttack", true);
                });
            #endregion
            #region Jump->JumpingMiddleAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.FightingModeJump"))
                .Where(x => PlayerState.canFightingModeJumpingMiddleAttack.Value)
                .Where(x => Key.X)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsFightingModeJumping", false);
                    Animator.SetBool("IsFightingModeJumpingMiddleAttack", true);
                });
            #endregion
            #region Jump->JumpingHighAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.FightingModeJump"))
                .Where(x => PlayerState.canFightingModeJumpingHighAttack.Value)
                .Where(x => Key.C)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsFightingModeJumping", false);
                    Animator.SetBool("IsFightingModeJumpingHighAttack", true);
                });
            #endregion
            #region Jump->JumpingGuard
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.FightingModeJump"))
                .Where(x => PlayerState.canFightingModeJumpingGuard.Value)
                .Where(x => !PlayerState.IsGrounded.Value)
                .Where(x => Key.LeftShift)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsFightingModeJumping", false);
                    Animator.SetBool("IsFightingModeJumpingGuard", true);
                });
            #endregion
            #region Jump->SupineJumpingDamage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.FightingModeJump"))
                .Where(x => PlayerState.WasSupineAttributeAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsFightingModeJumping", false);
                    Animator.SetBool("IsSupineJumpingDamage", true);
                });
            #endregion
            #region Jump->ProneJumpingDamage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.FightingModeJump"))
                .Where(x => PlayerState.WasProneAttributeAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsFightingModeJumping", false);
                    Animator.SetBool("IsProneJumpingDamage", true);
                });
            #endregion
            #region Jump->DoubleJump
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.FightingModeJump"))
                .Where(x => !PlayerState.IsWallKickJumping.Value)
                .Where(x => !PlayerState.canWallKickJumping.Value)
                .Where(x => PlayerState.canFightingModeDoubleJump.Value)
                .Where(x => PlayerRigidbody2D.velocity.y < 2)
                .Where(x => Key.Vertical.Value == 1)
                .Where(x => Key.up.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsFightingModeJumping", false);
                    Animator.SetBool("IsFightingModeDoubleJumping", true);
                });
            #endregion
            #region Jump->Fall
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.FightingModeJump"))
                .Where(x => !PlayerState.hasInputedAirDashCommand.Value)
                .Where(x => PlayerRigidbody2D.velocity.y < 0)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsFightingModeJumping", false);
                    Animator.SetBool("IsFalling", true);
                });
            #endregion
            #region Jump->AirDash
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.FightingModeJump"))
                .Where(x => PlayerState.canAirDash.Value)
                .Where(x => !PlayerState.hasAirDashed.Value)
                .Where(x => PlayerState.hasInputedAirDashCommand.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsFightingModeJumping", false);
                    Animator.SetBool("IsAirDashing", true);
                });
            #endregion

            //Motion
            this.FixedUpdateAsObservable()
                .Where(x => PlayerState.canFightingModeJump.Value)
                .Where(x => Key.Vertical.Value == 1)
                .Subscribe(_ => this.Jump(/*JumpForce*/Parameter.GetPlayerParameter().PlayerBasics.FightingModeJump.JumpForce));

            //Collision
            this.ObserveEveryValueChanged(x => Animator.GetBool("IsFightingModeJumping"))
                .Where(x => x)
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = true;
                    HurtBox.enabled = true;
                });

            this.ObserveEveryValueChanged(x => Animator.GetBool("IsFightingModeJumping"))
                .Where(x => !x)
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = false;
                    HurtBox.enabled = false;
                });
        }

        public void Jump(float JumpForce)
        {
            PlayerRigidbody2D.velocity = new Vector2(PlayerRigidbody2D.velocity.x, JumpForce);
        }
    }
}