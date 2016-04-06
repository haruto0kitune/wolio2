using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class FlyingPiyoState : MonoBehaviour
{
    public Transform WallCheck;
    private SpriteRenderer SpriteRenderer;

    public IntReactiveProperty Hp;

    public ReactiveProperty<bool> FacingRight;
    public ReactiveProperty<float> Direction;
    public ReactiveProperty<bool> IsAttacking;

    void Awake()
    {
        WallCheck = gameObject.transform.Find("WallCheck");
        SpriteRenderer = GetComponent<SpriteRenderer>();

        FacingRight = SpriteRenderer.ObserveEveryValueChanged(x => x.flipX).ToReactiveProperty();
        Direction = FacingRight.Select(x => x ? 1f : -1f).ToReactiveProperty(-1f);
        IsAttacking = new ReactiveProperty<bool>();
    }

    void Start()
    {
        var PublishFacingRight = FacingRight.Publish().RefCount();
    
        PublishFacingRight.Where(x => x).Subscribe(_ => WallCheck.localPosition = new Vector2(0.2f, WallCheck.localPosition.y));
        PublishFacingRight.Where(x => !x).Subscribe(_ => WallCheck.localPosition = new Vector2(-0.2f, WallCheck.localPosition.y));
    }
}
