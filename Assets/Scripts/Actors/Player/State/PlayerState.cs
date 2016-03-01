using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

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

    void Awake()
    {
        GroundCheck = transform.Find("GroundCheck");
        PlayerConfig = GetComponent<PlayerConfig>();

        IsDead = new ReactiveProperty<bool>(false);
        IsGrounded = new ReactiveProperty<bool>(false);
        IsDashing = new ReactiveProperty<bool>(false);
        IsTouchingWall = new ReactiveProperty<bool>(false);
        IsClimbing = new ReactiveProperty<bool>(false);
        IsClimbable = new ReactiveProperty<bool>(false);
        FacingRight = new ReactiveProperty<bool>(true);

        this.UpdateAsObservable()
            .Subscribe(_ => Debug.Log(IsGrounded.Value));
        IsGrounded = this.ObserveEveryValueChanged(x => (bool)Physics2D.Linecast(transform.position, GroundCheck.position, PlayerConfig.WhatIsGround)).ToReactiveProperty();
        IsDead = Hp.Select(x => transform.position.y <= -5 || x <= 0).ToReactiveProperty();
        FacingRight = GetComponent<SpriteRenderer>().ObserveEveryValueChanged(x => x.flipX).ToReactiveProperty();
    }
}