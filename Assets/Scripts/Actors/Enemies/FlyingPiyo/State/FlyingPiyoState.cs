using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class FlyingPiyoState : MonoBehaviour
{
    private SpriteRenderer SpriteRenderer;

    public IntReactiveProperty Hp;

    public ReactiveProperty<bool> FacingRight;
    public ReactiveProperty<float> Direction;
    public ReactiveProperty<bool> IsAttacking;

    void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();

        FacingRight = SpriteRenderer.ObserveEveryValueChanged(x => x.flipX).ToReactiveProperty();
        Direction = FacingRight.Select(x => x ? 1f : -1f).ToReactiveProperty(-1f);
        IsAttacking = new ReactiveProperty<bool>();
    }

    void Start()
    {
        Hp.Where(x => x <= 0).Subscribe(_ => Destroy(this.gameObject));
    }
}
