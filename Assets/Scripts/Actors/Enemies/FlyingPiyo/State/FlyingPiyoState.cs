using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class FlyingPiyoState : MonoBehaviour
{
    public Transform WallCheck;
    private SpriteRenderer SpriteRenderer;
    private BoxCollider2D BoxCollider2D;

    public IntReactiveProperty Hp;

    public ReactiveProperty<bool> FacingRight;
    public ReactiveProperty<float> Direction;
    public ReactiveProperty<bool> IsAttacking;

    void Awake()
    {
        WallCheck = gameObject.transform.Find("WallCheck");
        SpriteRenderer = GetComponent<SpriteRenderer>();
        BoxCollider2D = GetComponent<BoxCollider2D>();

        FacingRight = SpriteRenderer.ObserveEveryValueChanged(x => x.flipX).ToReactiveProperty();
        Direction = FacingRight.Select(x => x ? 1f : -1f).ToReactiveProperty(-1f);
        IsAttacking = new ReactiveProperty<bool>();
    }

    void Start()
    {
        var PublishFacingRight = FacingRight.Publish().RefCount();
        PublishFacingRight.Where(x => x).Subscribe(_ => WallCheck.localPosition = new Vector2(0.2f, WallCheck.localPosition.y));
        PublishFacingRight.Where(x => !x).Subscribe(_ => WallCheck.localPosition = new Vector2(-0.2f, WallCheck.localPosition.y));
        PublishFacingRight.Subscribe(_ => BoxCollider2D.offset = new Vector2(BoxCollider2D.offset.x * -1, BoxCollider2D.offset.y));
    }
}
