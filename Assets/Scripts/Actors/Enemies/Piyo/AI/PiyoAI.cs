using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using System.Linq;

public class PiyoAI : MonoBehaviour
{
    PiyoState PiyoState;
    PiyoMotion PiyoMotion;
    PiyoConfig PiyoConfig;
 
    void Awake()
    {
        PiyoMotion = GetComponent<PiyoMotion>();
        PiyoState = GetComponent<PiyoState>();
        PiyoConfig = GetComponent<PiyoConfig>();
    }

    void Start()
    {
        this.FixedUpdateAsObservable()
            .Where(x => FrontCheck())
            .Subscribe(_ => PiyoMotion.Turn());
    }

    bool FrontCheck()
    {
        var isWall = default(bool);
        var Collider2D = new Collider2D[1][];
        Collider2D[0] = Physics2D.OverlapAreaAll(PiyoState.WallCheck.position, new Vector2(PiyoState.WallCheck.position.x + 0.02f, PiyoState.WallCheck.position.y - 0.12f));

        for (int i = 0; i < 1; i++)
        {
            foreach (var FrontCheck in Collider2D[i])
            {
                if ((FrontCheck.tag == "Gun" 
                    || FrontCheck.tag == "Block") 
                    && !FrontCheck.isTrigger)
                {
                    isWall = true;
                }
            }
        }

        return isWall;
    }
}
