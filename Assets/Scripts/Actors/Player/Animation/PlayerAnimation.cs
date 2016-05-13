using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator Animator;
        private ObservableStateMachineTrigger ObservableStateMachineTrigger;
        private Key Key;
        private PlayerState PlayerState;
        private PlayerConfig PlayerConfig;

        void Awake()
        {
            Animator = GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            Key = GetComponent<Key>();
            PlayerState = GetComponent<PlayerState>();
            PlayerConfig = GetComponent<PlayerConfig>();
        }

        void Start()
        {
            #region Basics
            #region Stand
            #region Stand->Run
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
                .Where(x => Key.Horizontal.Value != 0 && Key.Vertical.Value == 0)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", false);
                    Animator.SetBool("IsRunning", true);
                });
            #endregion
            #region Stand->Jump
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
                .SelectMany(x => Key.Vertical)
                .Where(x => x == 1)
                .Where(x => !PlayerState.IsJumping.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", false);
                    Animator.SetBool("IsJumping", true);
                });
            #endregion
            #region Stand->Crouch
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
                .Where(x => Key.Vertical.Value == -1)
                .Where(x => !PlayerState.IsJumping.Value)
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
                .Where(x => !x.StateInfo.IsName("Base Layer.StandingLightAttack"))
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
                .Where(x => Key.LeftShift)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", false);
                    Animator.SetBool("IsStandingGuard", true);
                });
            #endregion
            #endregion
            #region Run
            #region Run->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Run"))
                .Where(x => Key.Horizontal.Value == 0)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", true);
                    Animator.SetBool("IsRunning", false);
                });
            #endregion
            #region Run->Jump
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Run"))
                .SelectMany(x => Key.Vertical)
                .Where(x => x == 1)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsRunning", false);
                    Animator.SetBool("IsJumping", true);
                });
            #endregion
            #endregion
            #region Crouch
            #region Crouch->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Crouch"))
                .Where(x => Key.Vertical.Value == 0)
                .Where(x => Physics2D.OverlapCircle(PlayerState.CeilingCheck.position, 0.1f, PlayerConfig.WhatIsGround) == null)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", true);
                    Animator.SetBool("IsCrouching", false);
                });
            #endregion
            #region Crouch->Creep
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Crouch"))
                .Where(x => Key.Horizontal.Value != 0 && Key.Vertical.Value == -1)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouching", false);
                    Animator.SetBool("IsCreeping", true);
                });
            #endregion
            #region Crouch->CrouchingLightAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Crouch"))
                .Where(x => Key.Z)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouching", false);
                    Animator.SetBool("IsCrouchingLightAttack", true);
                });
            #endregion
            #region Crouch->CrouchingMiddleAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Crouch"))
                .Where(x => Key.X)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouching", false);
                    Animator.SetBool("IsCrouchingMiddleAttack", true);
                });
            #endregion
            #region Crouch->CrouchingHighAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Crouch"))
                .Where(x => Key.C)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouching", false);
                    Animator.SetBool("IsCrouchingHighAttack", true);
                });
            #endregion
            #region Crouch->CrouchingGuard
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Crouch"))
                .Where(x => Key.LeftShift && (Key.Vertical.Value == -1))
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouching", false);
                    Animator.SetBool("IsCrouchingGuard", true);
                });
            #endregion
            #endregion
            #region Jump
            #region Jump->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Jump"))
                .SelectMany(x => PlayerState.IsGrounded)
                .Where(x => x)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", true);
                    Animator.SetBool("IsJumping", false);
                });
            #endregion
            #region Jump->JumpingLightAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Jump"))
                .Where(x => Key.Z)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsJumping", false);
                    Animator.SetBool("IsJumpingLightAttack", true);
                });
            #endregion
            #region Jump->JumpingMiddleAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Jump"))
                .Where(x => Key.X)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsJumping", false);
                    Animator.SetBool("IsJumpingMiddleAttack", true);
                });
            #endregion
            #region Jump->JumpingHighAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Jump"))
                .Where(x => Key.C)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsJumping", false);
                    Animator.SetBool("IsJumpingHighAttack", true);
                });
            #endregion
            #region Jump->JumpingGuard
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Jump"))
                .Where(x => !PlayerState.IsGrounded.Value)
                .Where(x => Key.LeftShift)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsJumping", false);
                    Animator.SetBool("IsJumpingGuard", true);
                });
            #endregion
            #endregion
            #region Creep
            #region Creep->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Creep"))
                .Where(x => Key.Horizontal.Value == 0 && Key.Vertical.Value == 0)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", true);
                    Animator.SetBool("IsCreeping", false);
                });
            #endregion
            #region Creep->Crouch 
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Creep"))
                .Where(x => Key.Horizontal.Value == 0 && Key.Vertical.Value == -1)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouching", true);
                    Animator.SetBool("IsCreeping", false);
                });
            #endregion
            #endregion
            #endregion
            #region Attacks
            #region StandingAttacks
            #region StandingLightAttack
            #region StandingLightAttack->Stand
            ObservableStateMachineTrigger
                 .OnStateUpdateAsObservable()
                 .Where(x => x.StateInfo.IsName("Base Layer.StandingLightAttack"))
                 .Where(x => x.StateInfo.normalizedTime >= 1)
                 .Subscribe(_ =>
                 {
                     Animator.SetBool("IsStanding", true);
                     Animator.SetBool("IsStandingLightAttack", false);
                 });
            #endregion
            #endregion
            #region StandingMiddleAttack
            #region StandingMiddleAttack->Stand
            ObservableStateMachineTrigger
                 .OnStateUpdateAsObservable()
                 .Where(x => x.StateInfo.IsName("Base Layer.StandingMiddleAttack"))
                 .Where(x => x.StateInfo.normalizedTime >= 1)
                 .Subscribe(_ =>
                 {
                     Animator.SetBool("IsStanding", true);
                     Animator.SetBool("IsStandingMiddleAttack", false);
                 });
            #endregion
            #endregion
            #region StandingHighAttack
            #region StandingHighAttack->Stand
            ObservableStateMachineTrigger
                 .OnStateUpdateAsObservable()
                 .Where(x => x.StateInfo.IsName("Base Layer.StandingHighAttack"))
                 .Where(x => x.StateInfo.normalizedTime >= 1)
                 .Subscribe(_ =>
                 {
                     Animator.SetBool("IsStanding", true);
                     Animator.SetBool("IsStandingHighAttack", false);
                 });
            #endregion
            #endregion
            #endregion
            #region CrouchingAttacks
            #region CrouchingLightAttack
            #region CrouchingLightAttack->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.CrouchingLightAttack"))
                .Where(x => x.StateInfo.normalizedTime >= 1)
                .Where(x => Key.Vertical.Value == 0)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", true);
                    Animator.SetBool("IsCrouchingLightAttack", false);
                });
            #endregion
            #region CrouchingLightAttack->Crouch
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.CrouchingLightAttack"))
                .Where(x => x.StateInfo.normalizedTime >= 1)
                .Where(x => Key.Vertical.Value == -1)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouching", true);
                    Animator.SetBool("IsCrouchingLightAttack", false);
                });
            #endregion
            #endregion
            #region CrouchingMiddleAttack
            #region CrouchingMiddleAttack->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.CrouchingMiddleAttack"))
                .Where(x => x.StateInfo.normalizedTime >= 1)
                .Where(x => Key.Vertical.Value == 0)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", true);
                    Animator.SetBool("IsCrouchingMiddleAttack", false);
                });
            #endregion
            #region CrouchingMiddleAttack->Crouch
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.CrouchingMiddleAttack"))
                .Where(x => x.StateInfo.normalizedTime >= 1)
                .Where(x => Key.Vertical.Value == -1)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouching", true);
                    Animator.SetBool("IsCrouchingMiddleAttack", false);
                });
            #endregion
            #endregion
            #region CrouchingHighAttack
            #region CrouchingHighAttack->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.CrouchingHighAttack"))
                .Where(x => x.StateInfo.normalizedTime >= 1)
                .Where(x => Key.Vertical.Value == 0)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", true);
                    Animator.SetBool("IsCrouchingHighAttack", false);
                });
            #endregion
            #region CrouchingHighAttack->Crouch
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.CrouchingHighAttack"))
                .Where(x => x.StateInfo.normalizedTime >= 1)
                .Where(x => Key.Vertical.Value == -1)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouching", true);
                    Animator.SetBool("IsCrouchingHighAttack", false);
                });
            #endregion
            #endregion
            #endregion
            #region JumpingAttacks
            #region JumpingLightAttack->Jump
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.JumpingLightAttack"))
                .Where(x => x.StateInfo.normalizedTime >= 1)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsJumping", true);
                    Animator.SetBool("IsJumpingLightAttack", false);
                });
            #endregion
            #region JumpingMiddleAttack->Jump
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.JumpingMiddleAttack"))
                .Where(x => x.StateInfo.normalizedTime >= 1)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsJumping", true);
                    Animator.SetBool("IsJumpingMiddleAttack", false);
                });
            #endregion
            #region JumpingHighAttack->Jump
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.JumpingHighAttack"))
                .Where(x => x.StateInfo.normalizedTime >= 1)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsJumping", true);
                    Animator.SetBool("IsJumpingHighAttack", false);
                });
            #endregion
            #endregion
            #endregion
            #region Guards
            #region StandingGuard
            #region StandingGuard->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.StandingGuard"))
                .Where(x => !Key.LeftShift || !PlayerState.IsGrounded.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", true);
                    Animator.SetBool("IsStandingGuard", false);
                });
            #endregion
            #region StandingGuard->CrouchingGuard
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.StandingGuard"))
                .Where(x => Key.LeftShift && (Key.Vertical.Value == -1f))
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStandingGuard", false);
                    Animator.SetBool("IsCrouchingGuard", true);
                });
            #endregion
            #endregion
            #region CrouchingGuard
            #region CrouchingGuard->Crouch
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.CrouchingGuard"))
                .Where(x => !Key.LeftShift)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouching", true);
                    Animator.SetBool("IsCrouchingGuard", false);
                });
            #endregion
            #region CrouchingGuard->StandingGuard
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.CrouchingGuard"))
                .Where(x => Key.LeftShift && (Key.Vertical.Value == 0f))
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouchingGuard", false);
                    Animator.SetBool("IsStandingGuard", true);
                });
            #endregion
            #endregion
            #region JumpingGuard
            #region JumpingGuard->Jump
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.JumpingGuard"))
                .Where(x => !Key.LeftShift)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsJumping", true);
                    Animator.SetBool("IsJumpingGuard", false);
                });
            #endregion
            #region JumpingGuard->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.JumpingGuard"))
                .Where(x => PlayerState.IsGrounded.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", true);
                    Animator.SetBool("IsJumpingGuard", false);
                });
            #endregion
            #endregion
            #endregion
        }
    }
}