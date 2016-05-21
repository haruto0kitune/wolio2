using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using System.Linq;

public class PiyoTurn : MonoBehaviour
{
    [SerializeField]
    GameObject Piyo;
    [SerializeField]
    GameObject WallCheck;
    SpriteRenderer SpriteRenderer;
    PiyoState PiyoState;

    void Awake()
    {
        SpriteRenderer = Piyo.GetComponent<SpriteRenderer>();
        PiyoState = Piyo.GetComponent<PiyoState>();
    }

    void Start()
    {
        var PublishFacingRight = PiyoState.FacingRight.Publish().RefCount();

        PublishFacingRight
            .Where(x => x)
            .Subscribe(_ => WallCheck.transform.localPosition = new Vector2(0.2f, WallCheck.transform.localPosition.y));

        PublishFacingRight
            .Where(x => !x)
            .Subscribe(_ => WallCheck.transform.localPosition = new Vector2(-0.2f, WallCheck.transform.localPosition.y));

        this.FixedUpdateAsObservable()
            .Where(x => FrontCheck())
            .Subscribe(_ => Turn());
    }

    public void Turn()
    {
        Utility.Flip(SpriteRenderer);
    }

    bool FrontCheck()
    {
        var isWall = default(bool);
        var Collider2D = new Collider2D[1][];
        Collider2D[0] = Physics2D.OverlapAreaAll(WallCheck.transform.position, new Vector2(WallCheck.transform.position.x + 0.02f, WallCheck.transform.position.y - 0.12f));

        Collider2D
            .SelectMany(x => x)
            .ToObservable()
            .Where(x => (x.tag == "Gun" || x.tag == "Block" || x.tag == "Wall") && !x.isTrigger)
            .Subscribe(_ => isWall = true);

        return isWall;
    }
}