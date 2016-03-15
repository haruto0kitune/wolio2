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
    public ReactiveProperty<bool> FacingRight;

    public Transform GroundCheck;
    public Transform CeilingCheck;
    private PlayerConfig PlayerConfig;
    private Key Key;
    private SpriteRenderer SpriteRenderer;
    private Rigidbody2D Rigidbody2D;
    private BoxCollider2D BoxCollider2D;

    //debug
    [SerializeField]
    private Text Text1;
    [SerializeField]
    private Text Text2;
    [SerializeField]
    private Text Text3;
    [SerializeField]
    private Text Text4;
    [SerializeField]
    private Text Text5;
    //

    void Awake()
    {
        GroundCheck = transform.Find("GroundCheck");
        CeilingCheck = transform.Find("CeilingCheck");
        PlayerConfig = GetComponent<PlayerConfig>();
        Key = GetComponent<Key>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        BoxCollider2D = GetComponent<BoxCollider2D>(); 

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
        FacingRight = new ReactiveProperty<bool>(true);

        IsGrounded = this.ObserveEveryValueChanged(x => (bool)Physics2D.Linecast(GroundCheck.position, GroundCheck.position, PlayerConfig.WhatIsGround)).ToReactiveProperty();
        IsDead = Hp.Select(x => transform.position.y <= -5 || x <= 0).ToReactiveProperty();
        FacingRight = SpriteRenderer.ObserveEveryValueChanged(x => x.flipX).ToReactiveProperty();
    }

    void Start()
    {
        IsClimbable.SubscribeToText(Text1);
        IsClimbing.SubscribeToText(Text2);
        this.ObserveEveryValueChanged(x => Rigidbody2D.velocity.x).SubscribeToText(Text3);
        this.ObserveEveryValueChanged(x => Rigidbody2D.velocity.y).SubscribeToText(Text4);
        IsCrouching.SubscribeToText(Text5);

        // Fall velocity limit
        this.ObserveEveryValueChanged(x => Rigidbody2D.velocity.y)
            .Where(x => x <= -6)
            .Subscribe(_ => Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, PlayerConfig.FallVelocityLimit));
    }
}