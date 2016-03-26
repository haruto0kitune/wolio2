using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

[RequireComponent(typeof(FlyingPiyoConfig))]
[RequireComponent(typeof(FlyingPiyoMotion))]
[RequireComponent(typeof(FlyingPiyoState))]
[RequireComponent(typeof(FlyingPiyoPresenter))]
public class FlyingPiyoAI : MonoBehaviour
{
    FlyingPiyoState FlyingPiyoState;
    FlyingPiyoMotion FlyingPiyoMotion;
    FlyingPiyoConfig FlyingPiyoConfig;
 
    void Awake()
    {
        FlyingPiyoMotion = GetComponent<FlyingPiyoMotion>();
        FlyingPiyoState = GetComponent<FlyingPiyoState>();
        FlyingPiyoConfig = GetComponent<FlyingPiyoConfig>();
    }

    void Start()
    {
        this.FixedUpdateAsObservable()
            .Subscribe(_ => FlyingPiyoMotion.Fly(FlyingPiyoConfig.Speed, FlyingPiyoState.Direction.Value));
        this.FixedUpdateAsObservable()
            .Where(x => FrontCheck())
            .Subscribe(_ => FlyingPiyoMotion.Turn());
    }

    bool FrontCheck()
    {
        var isWall = default(bool);
        var Collider2D = new Collider2D[1][];
        Collider2D[0] = Physics2D.OverlapAreaAll(FlyingPiyoState.WallCheck.position, new Vector2(FlyingPiyoState.WallCheck.position.x + 0.02f, FlyingPiyoState.WallCheck.position.y - 0.12f));

        for (int i = 0; i < 1; i++)
        {
            foreach (var FrontCheck in Collider2D[i])
            {
                if ((FrontCheck.tag == "Gun" 
                    || FrontCheck.tag == "Block"
                    || FrontCheck.tag == "Wall") 
                    && !FrontCheck.isTrigger)
                {
                    isWall = true;
                }
            }
        }

        return isWall;
    }
}
