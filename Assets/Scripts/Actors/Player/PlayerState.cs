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
    public ReactiveProperty<bool> FacingRight;

    void Awake()
    {
        IsDead = new ReactiveProperty<bool>(false);
        IsGrounded = new ReactiveProperty<bool>(false);
        IsDashing = new ReactiveProperty<bool>(false);
        IsTouchingWall = new ReactiveProperty<bool>(false);
        FacingRight = new ReactiveProperty<bool>(true);

        IsDead = this.Hp.Select(x => transform.position.y <= -5 || x <= 0).ToReactiveProperty();
    }
}