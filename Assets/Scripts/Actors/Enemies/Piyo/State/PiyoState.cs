using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class PiyoState : MonoBehaviour
{
    private Animator Animator;
    private SpriteRenderer SpriteRenderer;
    private PiyoStatus Status;

    public ReactiveProperty<bool> IsDead;
    public ReactiveProperty<bool> IsRunning;
    public ReactiveProperty<bool> IsDamaged;
    public ReactiveProperty<bool> FacingRight;
    public ReactiveProperty<float> Direction;
    public ReactiveProperty<bool> WasAttacked;

    void Awake()
    {
        Animator = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Status = GetComponent<PiyoStatus>();
        
        IsDead = Status.Hp.Select(x => x <= 0).ToReactiveProperty();
        IsRunning = this.ObserveEveryValueChanged(x => Animator.GetBool("IsRunning")).ToReactiveProperty(); 
        IsDamaged = this.ObserveEveryValueChanged(x => Animator.GetBool("IsDamaged")).ToReactiveProperty(); 
        FacingRight = SpriteRenderer.ObserveEveryValueChanged(x => x.flipX).ToReactiveProperty();
        Direction = FacingRight.Select(x => x ? 1f : -1f).ToReactiveProperty(-1f);
        WasAttacked = new ReactiveProperty<bool>();
    }

    void Start()
    {
        IsDead.Where(x => x).Subscribe(_ => Destroy(gameObject));
    }
}