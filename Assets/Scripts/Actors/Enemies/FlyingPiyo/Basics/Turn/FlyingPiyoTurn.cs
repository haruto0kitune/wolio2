using UnityEngine;
using System.Collections;
using System.Linq;
using UniRx;
using UniRx.Triggers;

public class FlyingPiyoTurn : MonoBehaviour
{
    [SerializeField]
    GameObject FlyingPiyo;
    [SerializeField]
    GameObject WallCheck;
    FlyingPiyoState FlyingPiyoState;
    SpriteRenderer FlyingPiyoSpriteRenderer;

    void Awake()
    {
        FlyingPiyoState = FlyingPiyo.GetComponent<FlyingPiyoState>();
        FlyingPiyoSpriteRenderer = FlyingPiyo.GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // Sprite flip.
        this.FixedUpdateAsObservable()
            .Where(x => FrontCheck())
            .Subscribe(_ => Turn());
        
        // WallCheck flip.
        var PublishFacingRight = FlyingPiyoState.FacingRight.Publish().RefCount();
    
        PublishFacingRight.Where(x => x).Subscribe(_ => WallCheck.transform.localPosition = new Vector2(0.2f, WallCheck.transform.localPosition.y));
        PublishFacingRight.Where(x => !x).Subscribe(_ => WallCheck.transform.localPosition = new Vector2(-0.2f, WallCheck.transform.localPosition.y));
    }

    void Turn()
    {
        Utility.Flip(FlyingPiyoSpriteRenderer);
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