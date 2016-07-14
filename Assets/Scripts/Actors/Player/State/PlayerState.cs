﻿using UnityEngine;
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

        public ReactiveProperty<bool> IsDead;
        public ReactiveProperty<bool> IsGrounded;
        public ReactiveProperty<bool> IsDashing;
        public ReactiveProperty<bool> IsRunning;
        public ReactiveProperty<bool> canRun;
        public ReactiveProperty<bool> IsJumping;
        public ReactiveProperty<bool> canJump;
        public ReactiveProperty<bool> IsDoubleJumping;
        public ReactiveProperty<bool> hasDoubleJumped;
        public ReactiveProperty<bool> canDoubleJump;
        public ReactiveProperty<bool> hasInputedDoubleJumpCommand;
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
        public ReactiveProperty<bool> IsJumpingLightAttack;
        public ReactiveProperty<bool> canJumpingLightAttack;
        public ReactiveProperty<bool> IsJumpingMiddleAttack;
        public ReactiveProperty<bool> canJumpingMiddleAttack;
        public ReactiveProperty<bool> IsJumpingHighAttack;
        public ReactiveProperty<bool> canJumpingHighAttack;
        public ReactiveProperty<bool> IsStandingGuard;
        public ReactiveProperty<bool> canStandingGuard;
        public ReactiveProperty<bool> IsCrouchingGuard;
        public ReactiveProperty<bool> canCrouchingGuard;
        public ReactiveProperty<bool> IsJumpingGuard;
        public ReactiveProperty<bool> canJumpingGuard;
        public ReactiveProperty<bool> IsStandingDamage;
        public ReactiveProperty<bool> IsCrouchingDamage;
        public ReactiveProperty<bool> IsStandingHitBack;
        public ReactiveProperty<bool> IsCrouchingHitBack;
        public ReactiveProperty<bool> IsJumpingHitBack;
        public ReactiveProperty<bool> IsStandingGuardBack;
        public ReactiveProperty<bool> IsCrouchingGuardBack;
        public ReactiveProperty<bool> IsJumpingGuardBack;
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
            GroundCheck = transform.Find("GroundCheck");
            CeilingCheck = transform.Find("CeilingCheck");
            PlayerConfig = GetComponent<PlayerConfig>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            Rigidbody2D = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
            Status = GetComponent<Status>();
            Key = GetComponent<Key>();

            IsGrounded = this.ObserveEveryValueChanged(x => (bool)Physics2D.Linecast(GroundCheck.position, new Vector2(GroundCheck.position.x, GroundCheck.position.y - 0.05f), PlayerConfig.WhatIsGround))
                             .ToReactiveProperty();

            IsDead = Status.Hp
                           .Select(x => x <= 0)
                           .ToReactiveProperty();

            IsDashing = new ReactiveProperty<bool>();

            IsJumping = this.ObserveEveryValueChanged(x => Animator.GetBool("IsJumping"))
                            .ToReactiveProperty();

            canJump = this.ObserveEveryValueChanged(x => (IsStanding.Value || 
                                                          IsRunning.Value ||
                                                          (IsStandingMiddleAttack.Value && hitStandingMiddleAttack.Value)) &&
                                                          IsGrounded.Value).ToReactiveProperty();

            IsDoubleJumping = this.ObserveEveryValueChanged(x => Animator.GetBool("IsDoubleJumping"))
                                  .ToReactiveProperty();

            hasDoubleJumped = IsGrounded.Where(x => x).Select(x => !x).ToReactiveProperty();

            canDoubleJump = this.ObserveEveryValueChanged(x => (IsJumping.Value
                                                            || IsFalling.Value)
                                                            && !hasDoubleJumped.Value)
                                .ToReactiveProperty();

            hasInputedDoubleJumpCommand = this.ObserveEveryValueChanged(x => System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().Distinct().ToArray()), "58")
                                                                          || System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().Distinct().ToArray()), "57")
                                                                          || System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().Distinct().ToArray()), "59"))
                                              .ToReactiveProperty();

            IsStanding = this.ObserveEveryValueChanged(x => Animator.GetBool("IsStanding"))
                             .ToReactiveProperty();

            IsRunning = this.ObserveEveryValueChanged(x => Animator.GetBool("IsRunning"))
                            .ToReactiveProperty();

            canRun = this.ObserveEveryValueChanged(x => (IsStanding.Value || IsRunning.Value) && IsGrounded.Value).ToReactiveProperty();

            IsCrouching = this.ObserveEveryValueChanged(x => Animator.GetBool("IsCrouching"))
                              .ToReactiveProperty();

            canCrouch = this.ObserveEveryValueChanged(x => (IsStanding.Value || IsCrouching.Value) && IsGrounded.Value).ToReactiveProperty();

            IsCreeping = this.ObserveEveryValueChanged(x => Animator.GetBool("IsCreeping"))
                             .ToReactiveProperty();

            canCreep = this.ObserveEveryValueChanged(x => (IsCrouching.Value || IsCreeping.Value) && IsGrounded.Value).ToReactiveProperty();

            IsTouchingWall = new ReactiveProperty<bool>();

            IsStandingLightAttack = this.ObserveEveryValueChanged(x => Animator.GetBool("IsStandingLightAttack"))
                                        .ToReactiveProperty();
            
            canStandingLightAttack = this.ObserveEveryValueChanged(x => IsStanding.Value ||
                                                                        IsStandingLightAttack.Value ||
                                                                        IsCrouchingLightAttack.Value)
                                         .ToReactiveProperty();

            IsStandingMiddleAttack = this.ObserveEveryValueChanged(x => Animator.GetBool("IsStandingMiddleAttack"))
                                         .ToReactiveProperty();

            canStandingMiddleAttack = this.ObserveEveryValueChanged(x => IsStanding.Value ||
                                                                         IsStandingLightAttack.Value || 
                                                                         IsCrouchingLightAttack.Value)
                                          .ToReactiveProperty();

            hitStandingMiddleAttack = new ReactiveProperty<bool>();

            IsStandingHighAttack = this.ObserveEveryValueChanged(x => Animator.GetBool("IsStandingHighAttack"))
                                       .ToReactiveProperty();

            canStandingHighAttack = this.ObserveEveryValueChanged(x => IsStanding.Value || 
                                                                       IsStandingLightAttack.Value ||
                                                                       IsStandingMiddleAttack.Value ||
                                                                       IsCrouchingLightAttack.Value || 
                                                                       IsCrouchingMiddleAttack.Value)
                                        .ToReactiveProperty();

            IsCrouchingLightAttack = this.ObserveEveryValueChanged(x => Animator.GetBool("IsCrouchingLightAttack"))
                                         .ToReactiveProperty();

            canCrouchingLightAttack = this.ObserveEveryValueChanged(x => IsCrouching.Value ||
                                                                         IsStandingLightAttack.Value ||
                                                                         IsCrouchingLightAttack.Value)
                                          .ToReactiveProperty();

            IsCrouchingMiddleAttack = this.ObserveEveryValueChanged(x => Animator.GetBool("IsCrouchingMiddleAttack"))
                                          .ToReactiveProperty();

            canCrouchingMiddleAttack = this.ObserveEveryValueChanged(x => IsCrouching.Value ||
                                                                          IsCrouchingLightAttack.Value ||
                                                                          IsStandingLightAttack.Value)
                                           .ToReactiveProperty();
                                                                          
            IsCrouchingHighAttack = this.ObserveEveryValueChanged(x => Animator.GetBool("IsCrouchingHighAttack"))
                                        .ToReactiveProperty();

            canCrouchingHighAttack = this.ObserveEveryValueChanged(x => IsCrouching.Value ||
                                                                        IsCrouchingLightAttack.Value ||
                                                                        IsCrouchingMiddleAttack.Value ||
                                                                        IsStandingLightAttack.Value ||
                                                                        IsStandingMiddleAttack.Value)
                                         .ToReactiveProperty();

            IsJumpingLightAttack = this.ObserveEveryValueChanged(x => Animator.GetBool("IsJumpingLightAttack"))
                                       .ToReactiveProperty();

            canJumpingLightAttack = this.ObserveEveryValueChanged(x => IsJumping.Value
                                                                    || IsJumpingLightAttack.Value
                                                                    || IsAirDashing.Value)
                                        .ToReactiveProperty();

            IsJumpingMiddleAttack = this.ObserveEveryValueChanged(x => Animator.GetBool("IsJumpingMiddleAttack"))
                                        .ToReactiveProperty();

            canJumpingMiddleAttack = this.ObserveEveryValueChanged(x => IsJumping.Value 
                                                                     || IsJumpingLightAttack.Value
                                                                     || IsAirDashing.Value)
                                         .ToReactiveProperty();

            IsJumpingHighAttack = this.ObserveEveryValueChanged(x => Animator.GetBool("IsJumpingHighAttack"))
                                      .ToReactiveProperty();

            canJumpingHighAttack = this.ObserveEveryValueChanged(x => IsJumping.Value ||
                                                                      IsJumpingLightAttack.Value ||
                                                                      IsJumpingMiddleAttack.Value ||
                                                                      IsAirDashing.Value)
                                       .ToReactiveProperty();

            IsStandingGuard = this.ObserveEveryValueChanged(x => Animator.GetBool("IsStandingGuard"))
                                  .ToReactiveProperty();

            canStandingGuard = this.ObserveEveryValueChanged(x => (IsStanding.Value || IsStandingGuard.Value) && IsGrounded.Value)
                                   .ToReactiveProperty();

            IsCrouchingGuard = this.ObserveEveryValueChanged(x => Animator.GetBool("IsCrouchingGuard"))
                                   .ToReactiveProperty();

            canCrouchingGuard = this.ObserveEveryValueChanged(x => (IsCrouching.Value || IsCrouchingGuard.Value) && IsGrounded.Value)
                                    .ToReactiveProperty();

            IsJumpingGuard = this.ObserveEveryValueChanged(x => Animator.GetBool("IsJumpingGuard")) 
                                 .ToReactiveProperty();

            canJumpingGuard = this.ObserveEveryValueChanged(x => (IsJumping.Value || IsJumpingGuard.Value) && IsGrounded.Value)
                                  .ToReactiveProperty();

            IsStandingDamage = this.ObserveEveryValueChanged(x => Animator.GetBool("IsStandingDamage"))
                                   .ToReactiveProperty();

            IsCrouchingDamage = this.ObserveEveryValueChanged(x => Animator.GetBool("IsCrouchingDamage"))
                                    .ToReactiveProperty();

            IsStandingHitBack = new ReactiveProperty<bool>();
            IsCrouchingHitBack = new ReactiveProperty<bool>();
            IsJumpingHitBack = new ReactiveProperty<bool>();
            IsStandingGuardBack = new ReactiveProperty<bool>();
            IsCrouchingGuardBack = new ReactiveProperty<bool>();
            IsJumpingGuardBack = new ReactiveProperty<bool>();
            IsWallKickJumping = new ReactiveProperty<bool>();

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
                                                         IsJumping.Value ||
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

            canGrab = this.ObserveEveryValueChanged(x => IsStanding.Value
                                                      || IsRunning.Value)
                          .ToReactiveProperty();

            IsThrowing = this.ObserveEveryValueChanged(x => Animator.GetBool("IsThrowing"))
                             .ToReactiveProperty();

            IsFireballMotion = this.ObserveEveryValueChanged(x => Animator.GetBool("IsLightFireballMotion"))
                                   .ToReactiveProperty();

            canFireballMotion = this.ObserveEveryValueChanged(x => IsStanding.Value
                                                                || IsRunning.Value
                                                                || IsStandingLightAttack.Value
                                                                || IsStandingMiddleAttack.Value
                                                                || IsStandingHighAttack.Value
                                                                || IsCrouchingLightAttack.Value
                                                                || IsCrouchingMiddleAttack.Value
                                                                || IsCrouchingHighAttack.Value)
                                    .ToReactiveProperty();

            IsDragonPunch = this.ObserveEveryValueChanged(x => Animator.GetBool("IsLightDragonPunch"))
                                   .ToReactiveProperty();


            canDragonPunch = this.ObserveEveryValueChanged(x => IsStanding.Value
                                                             || IsCrouching.Value
                                                             || IsRunning.Value
                                                             || IsStandingLightAttack.Value
                                                             || IsStandingMiddleAttack.Value
                                                             || IsStandingHighAttack.Value
                                                             || IsCrouchingLightAttack.Value
                                                             || IsCrouchingMiddleAttack.Value
                                                             || IsCrouchingHighAttack.Value)
                                 .ToReactiveProperty();


            IsHurricaneKick = this.ObserveEveryValueChanged(x => Animator.GetBool("IsLightHurricaneKick"))
                                   .ToReactiveProperty();

            canHurricaneKick = this.ObserveEveryValueChanged(x => IsStanding.Value
                                                               || IsCrouching.Value
                                                               || IsRunning.Value
                                                               || IsStandingLightAttack.Value
                                                               || IsStandingMiddleAttack.Value
                                                               || IsStandingHighAttack.Value
                                                               || IsCrouchingLightAttack.Value
                                                               || IsCrouchingMiddleAttack.Value
                                                               || IsCrouchingHighAttack.Value)
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

            canFall = this.ObserveEveryValueChanged(x => IsJumping.Value || IsDoubleJumping.Value)
                          .ToReactiveProperty();

            IsLanding = this.ObserveEveryValueChanged(x => Animator.GetBool("IsLanding"))
                            .ToReactiveProperty();

            canLand = this.ObserveEveryValueChanged(x => IsJumping.Value
                                                      || IsDoubleJumping.Value
                                                      || IsFalling.Value)
                          .ToReactiveProperty();

            IsAirDashing = this.ObserveEveryValueChanged(x => Animator.GetBool("IsAirDashing"))
                               .ToReactiveProperty();

            canAirDash = this.ObserveEveryValueChanged(x => IsJumping.Value
                                                         || IsDoubleJumping.Value
                                                         || IsFalling.Value)
                             .ToReactiveProperty();

            hasInputedAirDashCommand = this.ObserveEveryValueChanged(x => System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().DistinctAdjacently().ToArray()), "5656")
                                                                       || System.Text.RegularExpressions.Regex.IsMatch(string.Concat(Key.inputHistory.ToArray().Reverse().DistinctAdjacently().ToArray()), "5454"))
                                           .ToReactiveProperty();

            //hasAirDashed = IsGrounded.Where(x => x).Select(x => !x).ToReactiveProperty();
            hasAirDashed = IsStanding.Where(x => x).Select(x => !x).ToReactiveProperty();

            hasAirDashed.Subscribe(_ => Debug.Log("hasAirDashed: " + _));

            IsSkipingLanding = new ReactiveProperty<bool>();


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
                .Where(x => IsJumping.Value)
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
        }
    }
}