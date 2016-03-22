using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    [InspectorDisplay]
    public IntReactiveProperty Hp;

    public ReactiveProperty<bool> IsDead;
    public ReactiveProperty<bool> IsGrounded;
    public ReactiveProperty<bool> IsDashing;
    public ReactiveProperty<bool> IsRunning;
    public ReactiveProperty<bool> IsJumping;
    public ReactiveProperty<bool> IsCrouching;
    public ReactiveProperty<bool> IsCreeping;
    public ReactiveProperty<bool> IsTouchingWall;
    public ReactiveProperty<bool> IsClimbing;
    public ReactiveProperty<bool> IsClimbable;
    public ReactiveProperty<bool> IsCrouchingLightAttack;
    public ReactiveProperty<bool> IsCrouchingMiddleAttack;
    public ReactiveProperty<bool> IsCrouchingHighAttack;
    public ReactiveProperty<bool> IsStandingLightAttack;
    public ReactiveProperty<bool> IsStandingMiddleAttack;
    public ReactiveProperty<bool> IsStandingHighAttack;
    public ReactiveProperty<bool> FacingRight;

    public Transform GroundCheck;
    public Transform CeilingCheck;
    private PlayerConfig PlayerConfig;
    private Key Key;
    private SpriteRenderer SpriteRenderer;
    private Rigidbody2D Rigidbody2D;
    private BoxCollider2D BoxCollider2D;
    private Animator Animator;

    void Awake()
    {
        GroundCheck = transform.Find("GroundCheck");
        CeilingCheck = transform.Find("CeilingCheck");
        PlayerConfig = GetComponent<PlayerConfig>();
        Key = GetComponent<Key>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
        Animator = GetComponent<Animator>();

        IsDead = new ReactiveProperty<bool>(false);
        IsGrounded = new ReactiveProperty<bool>(false);
        IsDashing = new ReactiveProperty<bool>(false);
        IsRunning = new ReactiveProperty<bool>(false);
        IsJumping = new ReactiveProperty<bool>(false);
        IsCrouching = new ReactiveProperty<bool>(false);
        IsCreeping = new ReactiveProperty<bool>(false);
        IsTouchingWall = new ReactiveProperty<bool>(false);
        IsClimbing = new ReactiveProperty<bool>(false);
        IsClimbable = new ReactiveProperty<bool>(false);
        IsCrouchingLightAttack = new ReactiveProperty<bool>(false);
        IsCrouchingMiddleAttack = new ReactiveProperty<bool>(false);
        IsCrouchingHighAttack = new ReactiveProperty<bool>(false);
        IsStandingLightAttack = new ReactiveProperty<bool>(false);
        IsStandingMiddleAttack = new ReactiveProperty<bool>(false);
        IsStandingHighAttack = new ReactiveProperty<bool>(false);
        FacingRight = new ReactiveProperty<bool>(true);

        IsGrounded = this.ObserveEveryValueChanged(x => (bool)Physics2D.Linecast(GroundCheck.position, GroundCheck.position, PlayerConfig.WhatIsGround)).ToReactiveProperty();
        IsDead = Hp.Select(x => transform.position.y <= -5 || x <= 0).ToReactiveProperty();
        IsCrouchingLightAttack = this.ObserveEveryValueChanged(x => Animator.GetBool("IsCrouchingLightAttack")).ToReactiveProperty();
        IsCrouchingMiddleAttack = this.ObserveEveryValueChanged(x => Animator.GetBool("IsCrouchingMiddleAttack")).ToReactiveProperty();
        IsCrouchingHighAttack = this.ObserveEveryValueChanged(x => Animator.GetBool("IsCrouchingHighAttack")).ToReactiveProperty();
        IsStandingLightAttack = this.ObserveEveryValueChanged(x => Animator.GetBool("IsStandingLightAttack")).ToReactiveProperty();
        IsStandingMiddleAttack = this.ObserveEveryValueChanged(x => Animator.GetBool("IsStandingMiddleAttack")).ToReactiveProperty();
        IsStandingHighAttack = this.ObserveEveryValueChanged(x => Animator.GetBool("IsStandingHighAttack")).ToReactiveProperty();
        FacingRight = SpriteRenderer.ObserveEveryValueChanged(x => x.flipX).ToReactiveProperty();
    }

    void Start()
    {
        // Fall velocity limit
        this.ObserveEveryValueChanged(x => Rigidbody2D.velocity.y)
            .Where(x => x <= -6)
            .Subscribe(_ => Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, PlayerConfig.FallVelocityLimit));
    }
}