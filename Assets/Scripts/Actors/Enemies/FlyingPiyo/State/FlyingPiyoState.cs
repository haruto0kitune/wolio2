using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class FlyingPiyoState : MonoBehaviour, IState
{
    private Animator Animator;
    private SpriteRenderer SpriteRenderer;
    private FlyingPiyoStatus Status;

    public ReactiveProperty<bool> IsDead;
    public ReactiveProperty<bool> IsFlying;
    public ReactiveProperty<bool> IsDamaged;
    public ReactiveProperty<bool> FacingRight;
    public ReactiveProperty<float> Direction;
    public ReactiveProperty<bool> IsAttacking;
    public ReactiveProperty<bool> WasAttacked { get; set; }

    void Awake()
    {
        Animator = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Status = GetComponent<FlyingPiyoStatus>();

        IsDead = Status.Hp.Select(x => x <= 0).ToReactiveProperty();
        IsFlying = this.ObserveEveryValueChanged(x => Animator.GetBool("IsFlying")).ToReactiveProperty();
        IsDamaged = this.ObserveEveryValueChanged(x => Animator.GetBool("IsDamaged")).ToReactiveProperty();
        FacingRight = SpriteRenderer.ObserveEveryValueChanged(x => x.flipX).ToReactiveProperty();
        Direction = FacingRight.Select(x => x ? 1f : -1f).ToReactiveProperty(-1f);
        IsAttacking = new ReactiveProperty<bool>();
        WasAttacked = new ReactiveProperty<bool>();
    }

    void Start()
    {
        IsDead.Where(x => x).Subscribe(_ => Destroy(gameObject));
    }
}
