using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using System.Linq;

[RequireComponent(typeof(FlyingPiyoConfig))]
[RequireComponent(typeof(FlyingPiyoMotion))]
[RequireComponent(typeof(FlyingPiyoState))]
[RequireComponent(typeof(FlyingPiyoPresenter))]
public class FlyingPiyoAI : MonoBehaviour
{
    FlyingPiyoState FlyingPiyoState;
    FlyingPiyoMotion FlyingPiyoMotion;
    FlyingPiyoConfig FlyingPiyoConfig;

    Vector2 InitialPoisiton;
 
    void Awake()
    {
        FlyingPiyoMotion = GetComponent<FlyingPiyoMotion>();
        FlyingPiyoState = GetComponent<FlyingPiyoState>();
        FlyingPiyoConfig = GetComponent<FlyingPiyoConfig>();

        InitialPoisiton = transform.position;
    }

    void Start()
    {
        this.UpdateAsObservable()
            .Where(x => !FlyingPiyoState.IsAttacking.Value)
            .Subscribe(_ => transform.position = new Vector2(transform.position.x, InitialPoisiton.y));

        this.FixedUpdateAsObservable()
            .Where(x => !FlyingPiyoState.IsAttacking.Value)
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

        Collider2D
            .SelectMany(x => x)
            .ToObservable()
            .Where(x => (x.tag == "Gun" || x.tag == "Block" || x.tag == "Wall") && !x.isTrigger)
            .Subscribe(_ => isWall = true);

        return isWall;
    }
}
