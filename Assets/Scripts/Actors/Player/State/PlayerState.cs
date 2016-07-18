using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

namespace Wolio.Actor.Player
{
    public class PlayerState : MonoBehaviour, IState
    {
        public Transform GroundCheck;
        public Transform CeilingCheck;
        private PlayerConfig PlayerConfig;
        private SpriteRenderer SpriteRenderer;
        private Rigidbody2D Rigidbody2D;
        private Animator Animator;
        private Status Status;
        private Key Key;

        public ControlMode controlMode;
        public ReactiveProperty<bool> IsDead;
        public ReactiveProperty<bool> IsGrounded;
        public ReactiveProperty<bool> IsDashing;
        public ReactiveProperty<bool> IsRunning;
        public ReactiveProperty<bool> canRun;
        public ReactiveProperty<bool> IsActionModeJumping;
        public ReactiveProperty<bool> canActionModeJump;
        public ReactiveProperty<bool> IsFightingModeJumping;
        public ReactiveProperty<bool> canFightingModeJump;
        public ReactiveProperty<bool> IsFightingModeDoubleJumping;
        public ReactiveProperty<bool> hasFightingModeDoubleJumped;
        public ReactiveProperty<bool> canFightingModeDoubleJump;
        public ReactiveProperty<bool> hasInputedFightingModeDoubleJumpCommand;
        public ReactiveProperty<bool> IsStanding;
        public ReactiveProperty<bool> IsCrouching;
        public ReactiveProperty<bool> canCrouch;
        public ReactiveProperty<bool> IsCreeping;
        public ReactiveProperty<bool> canCreep;
        public ReactiveProperty<bool> IsTouchingWall;
        public ReactiveProperty<bool> IsStandingLightAttack;
        public ReactiveProperty<bool> canStandingLightAttack;
        public ReactiveProperty<bool> IsStandingMiddleAttack;
        public ReactiveProperty<bool> canStandingMiddleAttack;
        public ReactiveProperty<bool> hitStandingMiddleAttack;
        public ReactiveProperty<bool> IsStandingHighAttack;
        public ReactiveProperty<bool> canStandingHighAttack;
        public ReactiveProperty<bool> IsCrouchingLightAttack;
        public ReactiveProperty<bool> canCrouchingLightAttack;
        public ReactiveProperty<bool> IsCrouchingMiddleAttack;
        public ReactiveProperty<bool> canCrouchingMiddleAttack;
        public ReactiveProperty<bool> IsCrouchingHighAttack;
        public ReactiveProperty<bool> canCrouchingHighAttack;
        public ReactiveProperty<bool> IsFightingModeJumpingLightAttack;
        public ReactiveProperty<bool> canFightingModeJumpingLightAttack;
        public ReactiveProperty<bool> IsFightingModeJumpingMiddleAttack;
        public ReactiveProperty<bool> canFightingModeJumpingMiddleAttack;
        public ReactiveProperty<bool> IsFightingModeJumpingHighAttack;
        public ReactiveProperty<bool> canFightingModeJumpingHighAttack;
        public ReactiveProperty<bool> IsStandingGuard;
        public ReactiveProperty<bool> canStandingGuard;
        public ReactiveProperty<bool> IsCrouchingGuard;
        public ReactiveProperty<bool> canCrouchingGuard;
        public ReactiveProperty<bool> IsFightingModeJumpingGuard;
        public ReactiveProperty<bool> canFightingModeJumpingGuard;
        public ReactiveProperty<bool> IsStandingDamage;
        public ReactiveProperty<bool> IsCrouchingDamage;
        public ReactiveProperty<bool> IsStandingHitBack;
        public ReactiveProperty<bool> IsCrouchingHitBack;
        public ReactiveProperty<bool> IsFightingModeJumpingHitBack;
        public ReactiveProperty<bool> IsStandingGuardBack;
        public ReactiveProperty<bool> IsCrouchingGuardBack;
        public ReactiveProperty<bool> IsFightingModeJumpingGuardBack;
        public ReactiveProperty<bool> IsWallKickJumping;
        public ReactiveProperty<bool> canWallKickJumping;
        public ReactiveProperty<bool> FacingRight;
        public ReactiveProperty<bool> IsInTheAir;
        public ReactiveProperty<bool> WasAttacked { get; set; }
        public ReactiveProperty<bool> IsAirTech;
        public ReactiveProperty<bool> canAirTech;
        public ReactiveProperty<bool> canTurn;
        public ReactiveProperty<bool> IsSupineKnockdown;
        public ReactiveProperty<bool> IsProneKnockdown;
        public ReactiveProperty<bool> IsStandingUpFromSupineKnockdown;
        public ReactiveProperty<bool> IsStandingUpFromProneKnockdown;
        public ReactiveProperty<bool> WasSupineAttributeAttacked;
        public ReactiveProperty<bool> WasProneAttributeAttacked;
        public ReactiveProperty<bool> WasKnockdownAttributeAttacked { get; set; }
        public ReactiveProperty<bool> hasInputedLightFireballMotionCommand;
        public ReactiveProperty<bool> hasInputedMiddleFireballMotionCommand;
        public ReactiveProperty<bool> hasInputedHighFireballMotionCommand;
        public ReactiveProperty<bool> hasInputedLightDragonPunchCommand;
        public ReactiveProperty<bool> hasInputedMiddleDragonPunchCommand;
        public ReactiveProperty<bool> hasInputedHighDragonPunchCommand;
        public ReactiveProperty<bool> hasInputedLightHurricaneKickCommand;
        public ReactiveProperty<bool> hasInputedMiddleHurricaneKickCommand;
        public ReactiveProperty<bool> hasInputedHighHurricaneKickCommand;
        public ReactiveProperty<bool> hasInputedGrabCommand;
        public ReactiveProperty<bool> IsGrabbing;
        public ReactiveProperty<bool> canGrab;
        public ReactiveProperty<bool> IsThrowing;
        public ReactiveProperty<bool> IsFireballMotion;
        public ReactiveProperty<bool> canFireballMotion;
        public ReactiveProperty<bool> IsDragonPunch;
        public ReactiveProperty<bool> canDragonPunch;
        public ReactiveProperty<bool> IsHurricaneKick;
        public ReactiveProperty<bool> canHurricaneKick;
        public ReactiveProperty<bool> IsFalling; 
        public ReactiveProperty<bool> canFall; 
        public ReactiveProperty<bool> IsLanding;
        public ReactiveProperty<bool> canLand;
        public ReactiveProperty<bool> IsAirDashing;
        public ReactiveProperty<bool> canAirDash;
        public ReactiveProperty<bool> hasInputedAirDashCommand;
        public ReactiveProperty<bool> hasAirDashed;
        public ReactiveProperty<bool> IsSkipingLanding;

        void Awake()
        {
            Debug.Log(Parameter.GetPlayerParameter().PlayerBasics.WallKickJump.Recovery);
            Debug.Log(Parameter.GetPlayerParameter().PlayerBasics.ActionModeJump.Active);
            GroundCheck = transform.Find("GroundCheck");
            CeilingCheck = transform.Find("CeilingCheck");
            PlayerConfig = GetComponent<PlayerConfig>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            Rigidbody2D = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
            Status = GetComponent<Status>();
            Key = GetComponent<Key>();
            controlMode = ControlMode.ActionMode;
 
            IsGrounded = this.ObserveEveryValueChanged(x => (bool)Physics2D.Linecast(GroundCheck.position, new Vector2(GroundCheck.position.x, GroundCheck.position.y - 0.03f), PlayerConfig.WhatIsGround))
                             .ToReactiveProperty();

            IsDead = Status.Hp
                           .Select(x => x <= 0)
                           .ToReactiveProperty();

            IsDashing = new ReactiveProperty<bool>();


            IsStanding = this.ObserveEveryValueChanged(x => Animator.GetBool("IsStanding"))
                             .ToReactiveProperty();

            IsRunning = this.ObserveEveryValueChanged(x => Animator.GetBool("IsRunning"))
                            .ToReactiveProperty();

            canRun = this.ObserveEveryValueChanged(x => (IsStanding.Value || IsRunning.Value) && IsGrounded.Value).ToReactiveProperty();

            IsActionModeJumping = this.ObserveEveryValueChanged(x => Animator.GetBool("IsActionModeJumping"))
                                      .ToReactiveProperty();

            canActionModeJump = this.ObserveEveryValueChanged(x => (IsStanding.Value || 
                                                                   IsRunning.Value   ||
                                                                   IsActionModeJumping.Value)  &&
                                                                   (controlMode == ControlMode.ActionMode))
                                    .ToReactiveProperty();

            IsFightingModeJumping = this.ObserveEveryValueChanged(x => Animator.GetBool("IsFightingModeJumping"))
                            .ToReactiveProperty();

            canFightingModeJump = this.ObserveEveryValueChanged(x => (IsStanding.Value || 
                                                                      IsRunning.Value  ||
                                                                     (IsStandingMiddleAttack.Value && hitStandingMiddleAttack.Value)) &&
                                                                     (controlMode == ControlMode.FightingMode))
                                      .ToReactiveProperty();

            IsFightingModeDoubleJumping = this.ObserveEveryValueChanged(x => Animator.GetBool("IsFightingModeDoubleJumping"))
                                              .ToReactiveProperty();

            hasFightingModeDoubleJumped = IsStanding.Where(x => x).Select(x => !x).ToReactiveProperty();

            canFightingModeDoubleJump = this.ObserveEveryValueChanged(x => ((IsFightingModeJumping.Value
                                                                        || IsFalling.Value)
                                                                        && !hasFightingModeDoubleJumped.Value)
                                                                        && (controlMode == ControlMode.FightingMode))
                                            .ToReactiveProperty();

            hasInputedFightingModeDoubleJumpCommand = this.ObserveEveryValueChanged(x => System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().Distinct().ToArray()), "58")
                                                                                      || System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().Distinct().ToArray()), "57")
                                                                                      || System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().Distinct().ToArray()), "59"))
                                                          .ToReactiveProperty();

            IsCrouching = this.ObserveEveryValueChanged(x => Animator.GetBool("IsCrouching"))
                              .ToReactiveProperty();

            canCrouch = this.ObserveEveryValueChanged(x => (IsStanding.Value || IsCrouching.Value) && IsGrounded.Value).ToReactiveProperty();

            IsCreeping = this.ObserveEveryValueChanged(x => Animator.GetBool("IsCreeping"))
                             .ToReactiveProperty();

            canCreep = this.ObserveEveryValueChanged(x => (IsCrouching.Value || IsCreeping.Value) && IsGrounded.Value).ToReactiveProperty();

            IsTouchingWall = new ReactiveProperty<bool>();

            IsStandingLightAttack = this.ObserveEveryValueChanged(x => Animator.GetBool("IsStandingLightAttack"))
                                        .ToReactiveProperty();
            
            canStandingLightAttack = this.ObserveEveryValueChanged(x => (IsStanding.Value ||
                                                                        IsStandingLightAttack.Value ||
                                                                        IsCrouchingLightAttack.Value) &&
                                                                        (controlMode == ControlMode.FightingMode))
                                         .ToReactiveProperty();

            IsStandingMiddleAttack = this.ObserveEveryValueChanged(x => Animator.GetBool("IsStandingMiddleAttack"))
                                         .ToReactiveProperty();

            canStandingMiddleAttack = this.ObserveEveryValueChanged(x => (IsStanding.Value ||
                                                                         IsStandingLightAttack.Value || 
                                                                         IsCrouchingLightAttack.Value) &&
                                                                         (controlMode == ControlMode.FightingMode))
                                          .ToReactiveProperty();

            hitStandingMiddleAttack = new ReactiveProperty<bool>();

            IsStandingHighAttack = this.ObserveEveryValueChanged(x => Animator.GetBool("IsStandingHighAttack"))
                                       .ToReactiveProperty();

            canStandingHighAttack = this.ObserveEveryValueChanged(x => (IsStanding.Value || 
                                                                       IsStandingLightAttack.Value ||
                                                                       IsStandingMiddleAttack.Value ||
                                                                       IsCrouchingLightAttack.Value || 
                                                                       IsCrouchingMiddleAttack.Value) &&
                                                                       (controlMode == ControlMode.FightingMode))
                                        .ToReactiveProperty();

            IsCrouchingLightAttack = this.ObserveEveryValueChanged(x => Animator.GetBool("IsCrouchingLightAttack"))
                                         .ToReactiveProperty();

            canCrouchingLightAttack = this.ObserveEveryValueChanged(x => (IsCrouching.Value ||
                                                                         IsStandingLightAttack.Value ||
                                                                         IsCrouchingLightAttack.Value) &&
                                                                         (controlMode == ControlMode.FightingMode))
                                          .ToReactiveProperty();

            IsCrouchingMiddleAttack = this.ObserveEveryValueChanged(x => Animator.GetBool("IsCrouchingMiddleAttack"))
                                          .ToReactiveProperty();

            canCrouchingMiddleAttack = this.ObserveEveryValueChanged(x => (IsCrouching.Value ||
                                                                          IsCrouchingLightAttack.Value ||
                                                                          IsStandingLightAttack.Value) &&
                                                                          (controlMode == ControlMode.FightingMode))
                                           .ToReactiveProperty();
                                                                          
            IsCrouchingHighAttack = this.ObserveEveryValueChanged(x => Animator.GetBool("IsCrouchingHighAttack"))
                                        .ToReactiveProperty();

            canCrouchingHighAttack = this.ObserveEveryValueChanged(x => (IsCrouching.Value ||
                                                                        IsCrouchingLightAttack.Value ||
                                                                        IsCrouchingMiddleAttack.Value ||
                                                                        IsStandingLightAttack.Value ||
                                                                        IsStandingMiddleAttack.Value) &&
                                                                        (controlMode == ControlMode.FightingMode))
                                         .ToReactiveProperty();

            IsFightingModeJumpingLightAttack = this.ObserveEveryValueChanged(x => Animator.GetBool("IsFightingModeJumpingLightAttack"))
                                       .ToReactiveProperty();

            canFightingModeJumpingLightAttack = this.ObserveEveryValueChanged(x => (IsFightingModeJumping.Value
                                                                                || IsFightingModeJumpingLightAttack.Value
                                                                                || IsAirDashing.Value
                                                                                || IsFightingModeDoubleJumping.Value
                                                                                || IsFalling.Value) 
                                                                                && (controlMode == ControlMode.FightingMode))
                                        .ToReactiveProperty();

            IsFightingModeJumpingMiddleAttack = this.ObserveEveryValueChanged(x => Animator.GetBool("IsFightingModeJumpingMiddleAttack"))
                                        .ToReactiveProperty();

            canFightingModeJumpingMiddleAttack = this.ObserveEveryValueChanged(x => (IsFightingModeJumping.Value 
                                                                                 || IsFightingModeJumpingLightAttack.Value
                                                                                 || IsAirDashing.Value
                                                                                 || IsFightingModeDoubleJumping.Value
                                                                                 || IsFalling.Value)
                                                                                 && (controlMode == ControlMode.FightingMode))
                                         .ToReactiveProperty();

            IsFightingModeJumpingHighAttack = this.ObserveEveryValueChanged(x => Animator.GetBool("IsFightingModeJumpingHighAttack"))
                                      .ToReactiveProperty();

            canFightingModeJumpingHighAttack = this.ObserveEveryValueChanged(x => (IsFightingModeJumping.Value ||
                                                                                  IsFightingModeJumpingLightAttack.Value ||
                                                                                  IsFightingModeJumpingMiddleAttack.Value ||
                                                                                  IsAirDashing.Value ||
                                                                                  IsFightingModeDoubleJumping.Value ||
                                                                                  IsFalling.Value) &&
                                                                                  (controlMode == ControlMode.FightingMode))
                                       .ToReactiveProperty();

            IsStandingGuard = this.ObserveEveryValueChanged(x => Animator.GetBool("IsStandingGuard"))
                                  .ToReactiveProperty();

            canStandingGuard = this.ObserveEveryValueChanged(x => ((IsStanding.Value 
                                                               || IsStandingGuard.Value)
                                                               && IsGrounded.Value)
                                                               && (controlMode == ControlMode.FightingMode))
                                   .ToReactiveProperty();

            IsCrouchingGuard = this.ObserveEveryValueChanged(x => Animator.GetBool("IsCrouchingGuard"))
                                   .ToReactiveProperty();

            canCrouchingGuard = this.ObserveEveryValueChanged(x => ((IsCrouching.Value
                                                                || IsCrouchingGuard.Value)
                                                                && IsGrounded.Value)
                                                                && (controlMode == ControlMode.FightingMode))
                                    .ToReactiveProperty();

            IsFightingModeJumpingGuard = this.ObserveEveryValueChanged(x => Animator.GetBool("IsFightingModeJumpingGuard")) 
                                 .ToReactiveProperty();

            canFightingModeJumpingGuard = this.ObserveEveryValueChanged(x => ((IsFightingModeJumping.Value 
                                                                          || IsFightingModeJumpingGuard.Value)
                                                                          && IsGrounded.Value)
                                                                          && (controlMode == ControlMode.FightingMode))
                                  .ToReactiveProperty();

            IsStandingDamage = this.ObserveEveryValueChanged(x => Animator.GetBool("IsStandingDamage"))
                                   .ToReactiveProperty();

            IsCrouchingDamage = this.ObserveEveryValueChanged(x => Animator.GetBool("IsCrouchingDamage"))
                                    .ToReactiveProperty();

            IsStandingHitBack = new ReactiveProperty<bool>();
            IsCrouchingHitBack = new ReactiveProperty<bool>();
            IsFightingModeJumpingHitBack = new ReactiveProperty<bool>();
            IsStandingGuardBack = new ReactiveProperty<bool>();
            IsCrouchingGuardBack = new ReactiveProperty<bool>();
            IsFightingModeJumpingGuardBack = new ReactiveProperty<bool>();
            IsWallKickJumping = new ReactiveProperty<bool>();

            canWallKickJumping = new ReactiveProperty<bool>();

            FacingRight = SpriteRenderer.ObserveEveryValueChanged(x => x.flipX)
                                        .ToReactiveProperty();

            IsInTheAir = IsGrounded.Select(x => !x)
                                   .ToReactiveProperty();

            WasAttacked = new ReactiveProperty<bool>();

            IsAirTech = this.ObserveEveryValueChanged(x => Animator.GetBool("IsAirTech"))
                            .ToReactiveProperty();

            canAirTech = new ReactiveProperty<bool>();

            canTurn = this.ObserveEveryValueChanged(x => (IsStanding.Value ||
                                                         IsRunning.Value ||
                                                         IsFightingModeJumping.Value ||
                                                         IsActionModeJumping.Value || 
                                                         (controlMode == ControlMode.ActionMode && IsFalling.Value) ||
                                                         IsCreeping.Value))
                          .ToReactiveProperty();

            IsSupineKnockdown = this.ObserveEveryValueChanged(x => Animator.GetBool("IsSupineKnockdown"))
                                    .ToReactiveProperty();

            IsProneKnockdown = this.ObserveEveryValueChanged(x => Animator.GetBool("IsProneKnockdown"))
                                   .ToReactiveProperty();

            IsStandingUpFromSupineKnockdown = this.ObserveEveryValueChanged(x => Animator.GetBool("IsStandingUpFromSupineKnockdown"))
                                                  .ToReactiveProperty();

            IsStandingUpFromProneKnockdown = this.ObserveEveryValueChanged(x => Animator.GetBool("IsStandingUpFromProneKnockdown"))
                                                 .ToReactiveProperty();

            WasSupineAttributeAttacked = new ReactiveProperty<bool>();
            WasProneAttributeAttacked = new ReactiveProperty<bool>();
            WasKnockdownAttributeAttacked = new ReactiveProperty<bool>();

            IsGrabbing = this.ObserveEveryValueChanged(x => Animator.GetBool("IsGrabbing"))
                             .ToReactiveProperty();

            canGrab = this.ObserveEveryValueChanged(x => (IsStanding.Value
                                                      || IsRunning.Value)
                                                      && (controlMode == ControlMode.FightingMode))
                          .ToReactiveProperty();

            IsThrowing = this.ObserveEveryValueChanged(x => Animator.GetBool("IsThrowing"))
                             .ToReactiveProperty();

            IsFireballMotion = this.ObserveEveryValueChanged(x => Animator.GetBool("IsLightFireballMotion"))
                                   .ToReactiveProperty();

            canFireballMotion = this.ObserveEveryValueChanged(x => (IsStanding.Value
                                                                || IsRunning.Value
                                                                || IsStandingLightAttack.Value
                                                                || IsStandingMiddleAttack.Value
                                                                || IsStandingHighAttack.Value
                                                                || IsCrouchingLightAttack.Value
                                                                || IsCrouchingMiddleAttack.Value
                                                                || IsCrouchingHighAttack.Value)
                                                                && (controlMode == ControlMode.FightingMode))
                                    .ToReactiveProperty();

            IsDragonPunch = this.ObserveEveryValueChanged(x => Animator.GetBool("IsLightDragonPunch"))
                                   .ToReactiveProperty();


            canDragonPunch = this.ObserveEveryValueChanged(x => (IsStanding.Value
                                                             || IsCrouching.Value
                                                             || IsRunning.Value
                                                             || IsStandingLightAttack.Value
                                                             || IsStandingMiddleAttack.Value
                                                             || IsStandingHighAttack.Value
                                                             || IsCrouchingLightAttack.Value
                                                             || IsCrouchingMiddleAttack.Value
                                                             || IsCrouchingHighAttack.Value)
                                                             && (controlMode == ControlMode.FightingMode))
                                 .ToReactiveProperty();


            IsHurricaneKick = this.ObserveEveryValueChanged(x => Animator.GetBool("IsLightHurricaneKick"))
                                   .ToReactiveProperty();

            canHurricaneKick = this.ObserveEveryValueChanged(x => (IsStanding.Value
                                                               || IsCrouching.Value
                                                               || IsRunning.Value
                                                               || IsStandingLightAttack.Value
                                                               || IsStandingMiddleAttack.Value
                                                               || IsStandingHighAttack.Value
                                                               || IsCrouchingLightAttack.Value
                                                               || IsCrouchingMiddleAttack.Value
                                                               || IsCrouchingHighAttack.Value)
                                                               && (controlMode == ControlMode.FightingMode))
                                   .ToReactiveProperty();


            hasInputedLightFireballMotionCommand = this.ObserveEveryValueChanged(x => System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().Distinct().ToArray()), "26Z")
                                                                                   || System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().Distinct().ToArray()), "24Z")
                                                                                   || System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().Distinct().ToArray()), "236Z")
                                                                                   || System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().Distinct().ToArray()), "214Z"))
                                                  .ToReactiveProperty();

            hasInputedMiddleFireballMotionCommand = this.ObserveEveryValueChanged(x => System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().Distinct().ToArray()), "26X")
                                                                                    || System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().Distinct().ToArray()), "24X")
                                                                                    || System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().Distinct().ToArray()), "236X")
                                                                                    || System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().Distinct().ToArray()), "214X"))
                                                  .ToReactiveProperty();

            hasInputedHighFireballMotionCommand = this.ObserveEveryValueChanged(x => System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().Distinct().ToArray()), "26C")
                                                                                  || System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().Distinct().ToArray()), "24C")
                                                                                  || System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().Distinct().ToArray()), "236C")
                                                                                  || System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().Distinct().ToArray()), "214C"))
                                                  .ToReactiveProperty();

            hasInputedLightDragonPunchCommand = this.ObserveEveryValueChanged(x => System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().Distinct().ToArray()), "623Z")
                                                                                || System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().Distinct().ToArray()), "421Z"))
                                        .ToReactiveProperty();

            hasInputedMiddleDragonPunchCommand = this.ObserveEveryValueChanged(x => System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().Distinct().ToArray()), "623X")
                                                                                 || System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().Distinct().ToArray()), "421X"))
                                        .ToReactiveProperty();

            hasInputedHighDragonPunchCommand = this.ObserveEveryValueChanged(x => System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().Distinct().ToArray()), "623C")
                                                                               || System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().Distinct().ToArray()), "421C"))
                                        .ToReactiveProperty();

            hasInputedLightHurricaneKickCommand = this.ObserveEveryValueChanged(x => System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().DistinctAdjacently().ToArray()), "252Z")
                                                                                  || System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().DistinctAdjacently().ToArray()), "2525Z"))
                                          .ToReactiveProperty();

            hasInputedMiddleHurricaneKickCommand = this.ObserveEveryValueChanged(x => System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().Distinct().ToArray()), "252X")
                                                                                   || System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().DistinctAdjacently().ToArray()), "2525X"))
                                          .ToReactiveProperty();

            hasInputedHighHurricaneKickCommand = this.ObserveEveryValueChanged(x => System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().Distinct().ToArray()), "252C")
                                                                                 || System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().DistinctAdjacently().ToArray()), "2525C"))
                                          .ToReactiveProperty();

            hasInputedGrabCommand = this.ObserveEveryValueChanged(x => System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().Distinct().ToArray()), "56C")
                                                                    || System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().Distinct().ToArray()), "54C"))
                                        .ToReactiveProperty();

            IsFalling = this.ObserveEveryValueChanged(x => Animator.GetBool("IsFalling"))
                            .ToReactiveProperty();

            canFall = this.ObserveEveryValueChanged(x => IsFightingModeJumping.Value || IsFightingModeDoubleJumping.Value)
                          .ToReactiveProperty();

            IsLanding = this.ObserveEveryValueChanged(x => Animator.GetBool("IsLanding"))
                            .ToReactiveProperty();

            canLand = this.ObserveEveryValueChanged(x => IsFightingModeJumping.Value
                                                      || IsFightingModeDoubleJumping.Value
                                                      || IsFalling.Value)
                          .ToReactiveProperty();

            IsAirDashing = this.ObserveEveryValueChanged(x => Animator.GetBool("IsAirDashing"))
                               .ToReactiveProperty();

            canAirDash = this.ObserveEveryValueChanged(x => (IsFightingModeJumping.Value
                                                         || IsFightingModeDoubleJumping.Value
                                                         || IsFalling.Value)
                                                         && (controlMode == ControlMode.FightingMode))
                             .ToReactiveProperty();

            hasInputedAirDashCommand = this.ObserveEveryValueChanged(x => System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().DistinctAdjacently().ToArray()), "5656")
                                                                       || System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().DistinctAdjacently().ToArray()), "5454"))
                                           .ToReactiveProperty();

            hasAirDashed = IsStanding.Where(x => x).Select(x => !x).ToReactiveProperty();

            IsSkipingLanding = this.ObserveEveryValueChanged(x => controlMode == ControlMode.ActionMode)
                                   .ToReactiveProperty();

            //this.UpdateAsObservable()
            //    .Subscribe(_ => Debug.Log(string.Concat(Key.inputHistory.ToArray().Reverse().DistinctAdjacently().ToArray())));

            //this.UpdateAsObservable()
            //    .Subscribe(_ => Debug.Log("HurricaneCommand: " + (System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().DistinctAdjacently().ToArray()), "252Z")
            //                                                   || System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().DistinctAdjacently().ToArray()), "5252Z"))));
        }

        void Start()
        {
            // Fall velocity limit
            this.ObserveEveryValueChanged(x => Rigidbody2D.velocity.y)
                .Where(x => x <= -6)
                .Subscribe(_ => Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, PlayerConfig.FallVelocityLimit));

            // Speed limit
            this.ObserveEveryValueChanged(x => Rigidbody2D.velocity.x)
                .Where(x => IsFightingModeJumping.Value)
                .Where(x => x >= 5 | x <= -5)
                .Subscribe(_ =>
                {
                    if (Rigidbody2D.velocity.x >= 5)
                    {
                        Rigidbody2D.velocity = new Vector2(5f, Rigidbody2D.velocity.y);
                    }
                    else if (Rigidbody2D.velocity.x <= -5)
                    {
                        Rigidbody2D.velocity = new Vector2(-5f, Rigidbody2D.velocity.y);
                    }
                });

            // Kill Player
            IsDead.Where(x => x).Subscribe(_ => Destroy(gameObject));

            // When Player land, velocity reset.
            IsGrounded.Where(x => x).Subscribe(_ => Rigidbody2D.velocity = Vector2.zero);

            // GroundCheck position
            //IsAirDashing.Where(x => x).Subscribe(_ => GroundCheck.localPosition = new Vector2(GroundCheck.localPosition.x, -0.159f));

            // Change control mode
            this.ObserveEveryValueChanged(x => Key.A)
                .Where(x => x)
                .Subscribe(_ =>
                {
                    if (controlMode == ControlMode.ActionMode)
                    {
                        controlMode = ControlMode.FightingMode;
                    }
                    else if (controlMode == ControlMode.FightingMode)
                    {
                        controlMode = ControlMode.ActionMode;
                    }
                });
        }
    }
}