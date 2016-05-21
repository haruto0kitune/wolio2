using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class PiyoState : MonoBehaviour
{
    private SpriteRenderer SpriteRenderer;

    public IntReactiveProperty Hp;

    public ReactiveProperty<bool> FacingRight;
    public ReactiveProperty<float> Direction;

    void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();

        FacingRight = SpriteRenderer.ObserveEveryValueChanged(x => x.flipX).ToReactiveProperty();
        Direction = FacingRight.Select(x => x ? 1f : -1f).ToReactiveProperty(-1f);
    }

    void Start()
    {
    }
}