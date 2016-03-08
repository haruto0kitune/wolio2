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
    public ReactiveProperty<bool> IsTouchingWall;
    public ReactiveProperty<bool> IsClimbing;
    public ReactiveProperty<bool> IsClimbable;
    public ReactiveProperty<bool> FacingRight;

    private Transform GroundCheck;
    private PlayerConfig PlayerConfig;
    private Key Key;
    private SpriteRenderer SpriteRenderer;
    private Rigidbody2D Rigidbody2D;

    //debug
    [SerializeField]
    private Text Text1;
    [SerializeField]
    private Text Text2;
    //

    void Awake()
    {
        GroundCheck = transform.Find("GroundCheck");
        PlayerConfig = GetComponent<PlayerConfig>();
        Key = GetComponent<Key>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Rigidbody2D = GetComponent<Rigidbody2D>();

        IsDead = new ReactiveProperty<bool>(false);
        IsGrounded = new ReactiveProperty<bool>(false);
        IsDashing = new ReactiveProperty<bool>(false);
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
    }
}