using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Basics
{
    public class PlayerStand : MonoBehaviour
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
        GameObject StandHurtBox;
        BoxCollider2D HurtBox;

        void Awake()
        {
            Animator = Player.GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            PlayerState = Player.GetComponent<PlayerState>();
            PlayerRigidbody2D = Player.GetComponent<Rigidbody2D>();
            Key = Player.GetComponent<Key>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            CircleCollider2D = GetComponent<CircleCollider2D>();
            HurtBox = StandHurtBox.GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            //Animation
            #region Stand->Run
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
                .Where(x => !PlayerState.hasInputedGrabCommand.Value)
                .Where(x => PlayerState.canRun.Value)
                .Where(x => Key.Horizontal.Value != 0 && Key.Vertical.Value == 0)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", false);
                    Animator.SetBool("IsRunning", true);
                });
            #endregion
            #region Stand->ActionModeJump
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
                .Where(x => PlayerState.canActionModeJump.Value)
                .Where(x => Key.Vertical.Value == 1)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", false);
                    Animator.SetBool("IsActionModeJumping", true);
                });
            #endregion
            #region Stand->FightingModeJump
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
                .Where(x => PlayerState.canFightingModeJump.Value)
                .Where(x => Key.Vertical.Value == 1)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", false);
                    Animator.SetBool("IsFightingModeJumping", true);
                });
            #endregion
            #region Stand->Crouch
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
                .Where(x => PlayerState.canCrouch.Value)
                .Where(x => Key.Vertical.Value == -1f)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", false);
                    Animator.SetBool("IsCrouching", true);
                });
            #endregion
            #region Stand->StandingLightAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
                .Where(x => PlayerState.canStandingLightAttack.Value)
                .Where(x => !PlayerState.hasInputedLightFireballMotionCommand.Value)
                .Where(x => Key.Z)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", false);
                    Animator.SetBool("IsStandingLightAttack", true);
                });
            #endregion
            #region Stand->StandingMiddleAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
                .Where(x => PlayerState.canStandingMiddleAttack.Value)
                .Where(x => Key.X)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", false);
                    Animator.SetBool("IsStandingMiddleAttack", true);
                });
            #endregion
            #region Stand->StandingHighAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
                .Where(x => !PlayerState.hasInputedGrabCommand.Value)
                .Where(x => PlayerState.canStandingHighAttack.Value)
                .Where(x => Key.C)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", false);
                    Animator.SetBool("IsStandingHighAttack", true);
                });
            #endregion
            #region Stand->StandingGuard
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
                .Where(x => PlayerState.canStandingGuard.Value)
                .Where(x => Key.LeftShift)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", false);
                    Animator.SetBool("IsStandingGuard", true);
                });
            #endregion
            #region Stand->StandingDamage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
                .Where(x => PlayerState.WasAttacked.Value && !PlayerState.WasKnockdownAttributeAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", false);
                    Animator.SetBool("IsStandingDamage", true);
                });
            #endregion
            #region Stand->SupineJumpingDamage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
                .Where(x => PlayerState.WasSupineAttributeAttacked.Value && PlayerState.WasKnockdownAttributeAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", false);
                    Animator.SetBool("IsSupineJumpingDamage", true);
                });
            #endregion
            #region Stand->ProneJumpingDamage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
                .Where(x => PlayerState.WasProneAttributeAttacked.Value && PlayerState.WasKnockdownAttributeAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", false);
                    Animator.SetBool("IsProneJumpingDamage", true);
                });
            #endregion
            #region Stand->LightFireballMotion
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
                .Where(x => PlayerState.canFireballMotion.Value)
                .Where(x => PlayerState.hasInputedLightFireballMotionCommand.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", false);
                    Animator.SetBool("IsLightFireballMotion", true);
                });
            #endregion
            #region Stand->MiddleFireballMotion
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
                .Where(x => PlayerState.canFireballMotion.Value)
                .Where(x => PlayerState.hasInputedMiddleFireballMotionCommand.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", false);
                    Animator.SetBool("IsMiddleFireballMotion", true);
                });
            #endregion
            #region Stand->HighFireballMotion
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
                .Where(x => PlayerState.canFireballMotion.Value)
                .Where(x => PlayerState.hasInputedHighFireballMotionCommand.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", false);
                    Animator.SetBool("IsHighFireballMotion", true);
                });
            #endregion
            #region Stand->LightHurricaneKick
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
                .Where(x => PlayerState.canHurricaneKick.Value)
                .Where(x => PlayerState.hasInputedLightHurricaneKickCommand.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", false);
                    Animator.SetBool("IsLightHurricaneKick", true);
                });
            #endregion
            #region Stand->MiddleHurricaneKick
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
                .Where(x => PlayerState.canHurricaneKick.Value)
                .Where(x => PlayerState.hasInputedMiddleHurricaneKickCommand.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", false);
                    Animator.SetBool("IsMiddleHurricaneKick", true);
                });
            #endregion
            #region Stand->HighHurricaneKick
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
                .Where(x => PlayerState.canHurricaneKick.Value)
                .Where(x => PlayerState.hasInputedHighHurricaneKickCommand.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", false);
                    Animator.SetBool("IsHighHurricaneKick", true);
                });
            #endregion
            #region Stand->Grab
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
                .Where(x => PlayerState.canGrab.Value)
                .Where(x => Key.Horizontal.Value != 0 && Key.C)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", false);
                    Animator.SetBool("IsGrabbing", true);
                });
            #endregion
            #region Stand->Fall
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
                .Where(x => !PlayerState.IsActionModeJumping.Value)
                .Where(x => !PlayerState.IsGrounded.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", false);
                    Animator.SetBool("IsFalling", true);
                });
            #endregion

            //Collision
            this.ObserveEveryValueChanged(x => Animator.GetBool("IsStanding"))
                .Where(x => x)
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = true;
                    CircleCollider2D.enabled = true;
                    HurtBox.enabled = true;
                });

            this.ObserveEveryValueChanged(x => Animator.GetBool("IsStanding"))
                .Where(x => !x)
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = false;
                    CircleCollider2D.enabled = false;
                    HurtBox.enabled = false;
                });
        }
    }
}
