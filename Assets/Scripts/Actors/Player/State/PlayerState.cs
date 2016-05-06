using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    public Transform GroundCheck;
    public Transform CeilingCheck;
    private PlayerConfig PlayerConfig;
    private SpriteRenderer SpriteRenderer;
    private Rigidbody2D Rigidbody2D;
    private Animator Animator;

    [InspectorDisplay]
    public IntReactiveProperty Hp;

    public ReactiveProperty<bool> IsDead;
    public ReactiveProperty<bool> IsGrounded;
    public ReactiveProperty<bool> IsDashing;
    public ReactiveProperty<bool> IsRunning;
    public ReactiveProperty<bool> IsJumping;
    public ReactiveProperty<bool> IsStanding;
    public ReactiveProperty<bool> IsCrouching;
    public ReactiveProperty<bool> IsCreeping;
    public ReactiveProperty<bool> IsTouchingWall;
    public ReactiveProperty<bool> IsClimbing;
    public ReactiveProperty<bool> IsClimbable;
    public ReactiveProperty<bool> IsStandingLightAttack;
    public ReactiveProperty<bool> IsStandingMiddleAttack;
    public ReactiveProperty<bool> IsStandingHighAttack;
    public ReactiveProperty<bool> IsCrouchingLightAttack;
    public ReactiveProperty<bool> IsCrouchingMiddleAttack;
    public ReactiveProperty<bool> IsCrouchingHighAttack;
    public ReactiveProperty<bool> IsJumpingLightAttack;
    public ReactiveProperty<bool> IsJumpingMiddleAttack;
    public ReactiveProperty<bool> IsJumpingHighAttack;
    public ReactiveProperty<bool> IsStandingGuard;
    public ReactiveProperty<bool> IsCrouchingGuard;
    public ReactiveProperty<bool> IsJumpingGuard;
    public ReactiveProperty<bool> IsStandingDamage;
    public ReactiveProperty<bool> IsCrouchingDamage;
    public ReactiveProperty<bool> IsJumpingDamage;
    public ReactiveProperty<bool> IsStandingHitBack; 
    public ReactiveProperty<bool> IsCrouchingHitBack; 
    public ReactiveProperty<bool> IsJumpingHitBack; 
    public ReactiveProperty<bool> IsStandingGuardBack; 
    public ReactiveProperty<bool> IsCrouchingGuardBack; 
    public ReactiveProperty<bool> IsJumpingGuardBack; 
    public ReactiveProperty<bool> FacingRight;

    void Awake()
    {
        GroundCheck = transform.Find("GroundCheck");
        CeilingCheck = transform.Find("CeilingCheck");
        PlayerConfig = GetComponent<PlayerConfig>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();

        //Hp = new IntReactiveProperty();
        IsGrounded = this.ObserveEveryValueChanged(x => (bool)Physics2D.Linecast(GroundCheck.position, GroundCheck.position, PlayerConfig.WhatIsGround)).ToReactiveProperty();
        IsDead = Hp.Select(x => x <= 0).ToReactiveProperty();
        IsDashing = new ReactiveProperty<bool>();
        IsRunning = this.ObserveEveryValueChanged(x => Animator.GetBool("IsRunning")).ToReactiveProperty();
        IsJumping = this.ObserveEveryValueChanged(x => Animator.GetBool("IsJumping")).ToReactiveProperty();
        IsStanding = this.ObserveEveryValueChanged(x => Animator.GetBool("IsStanding")).ToReactiveProperty();
        IsCrouching = this.ObserveEveryValueChanged(x => Animator.GetBool("IsCrouching")).ToReactiveProperty();
        IsCreeping = this.ObserveEveryValueChanged(x => Animator.GetBool("IsCreeping")).ToReactiveProperty();
        IsTouchingWall = new ReactiveProperty<bool>();
        IsClimbing = new ReactiveProperty<bool>();
        IsClimbable = new ReactiveProperty<bool>();
        IsStandingLightAttack = this.ObserveEveryValueChanged(x => Animator.GetBool("IsStandingLightAttack")).ToReactiveProperty();
        IsStandingMiddleAttack = this.ObserveEveryValueChanged(x => Animator.GetBool("IsStandingMiddleAttack")).ToReactiveProperty();
        IsStandingHighAttack = this.ObserveEveryValueChanged(x => Animator.GetBool("IsStandingHighAttack")).ToReactiveProperty();
        IsCrouchingLightAttack = this.ObserveEveryValueChanged(x => Animator.GetBool("IsCrouchingLightAttack")).ToReactiveProperty();
        IsCrouchingMiddleAttack = this.ObserveEveryValueChanged(x => Animator.GetBool("IsCrouchingMiddleAttack")).ToReactiveProperty();
        IsCrouchingHighAttack = this.ObserveEveryValueChanged(x => Animator.GetBool("IsCrouchingHighAttack")).ToReactiveProperty();
        IsJumpingLightAttack = this.ObserveEveryValueChanged(x => Animator.GetBool("IsJumpingLightAttack")).ToReactiveProperty();
        IsJumpingMiddleAttack = this.ObserveEveryValueChanged(x => Animator.GetBool("IsJumpingMiddleAttack")).ToReactiveProperty();
        IsJumpingHighAttack = this.ObserveEveryValueChanged(x => Animator.GetBool("IsJumpingHighAttack")).ToReactiveProperty();
        IsStandingGuard = this.ObserveEveryValueChanged(x => Animator.GetBool("IsStandingGuard")).ToReactiveProperty();
        IsCrouchingGuard = this.ObserveEveryValueChanged(x => Animator.GetBool("IsCrouchingGuard")).ToReactiveProperty();
        IsJumpingGuard = this.ObserveEveryValueChanged(x => Animator.GetBool("IsJumpingGuard")).ToReactiveProperty();
        IsStandingDamage = this.ObserveEveryValueChanged(x => Animator.GetBool("IsStandingDamage")).ToReactiveProperty();
        IsCrouchingDamage = this.ObserveEveryValueChanged(x => Animator.GetBool("IsCrouchingDamage")).ToReactiveProperty();
        IsJumpingDamage = this.ObserveEveryValueChanged(x => Animator.GetBool("IsJumpingDamage")).ToReactiveProperty();
        IsStandingHitBack = new ReactiveProperty<bool>();
        IsCrouchingHitBack = new ReactiveProperty<bool>();
        IsJumpingHitBack = new ReactiveProperty<bool>();
        IsStandingGuardBack = new ReactiveProperty<bool>();
        IsCrouchingGuardBack = new ReactiveProperty<bool>();
        IsJumpingGuardBack = new ReactiveProperty<bool>();
        //FacingRight = SpriteRenderer.ObserveEveryValueChanged(x => x.flipX).ToReactiveProperty();
        FacingRight = transform.ObserveEveryValueChanged(x => x.localScale.x).Select(x => x == -1).ToReactiveProperty();
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
    }
}