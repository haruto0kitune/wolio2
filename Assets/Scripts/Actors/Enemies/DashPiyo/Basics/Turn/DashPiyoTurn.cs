using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using System.Linq;

namespace Wolio.Actor.Enemy.DashPiyo
{
    public class DashPiyoTurn : MonoBehaviour
    {
        [SerializeField]
        GameObject DashPiyo;
        [SerializeField]
        GameObject WallCheck;
        SpriteRenderer SpriteRenderer;
        DashPiyoState DashPiyoState;
        BoxCollider2D[] BoxColliders2D;
        CircleCollider2D[] CircleColliders2D;

        void Awake()
        {
            SpriteRenderer = DashPiyo.GetComponent<SpriteRenderer>();
            DashPiyoState = DashPiyo.GetComponent<DashPiyoState>();
            BoxColliders2D = DashPiyo.GetComponentsInChildren<BoxCollider2D>();
            CircleColliders2D = DashPiyo.GetComponentsInChildren<CircleCollider2D>();
        }

        void Start()
        {
            this.FixedUpdateAsObservable()
                .Where(x => FrontCheck())
                .Subscribe(_ => Turn());
        }

        public void Turn()
        {
            // Turn Sprite
            Utility.Flip(SpriteRenderer);

            // Turn Collision
            foreach(var i in BoxColliders2D)
            {
                i.offset = new Vector2(i.offset.x * -1f, i.offset.y);
            }

            foreach(var i in CircleColliders2D)
            {
                i.offset = new Vector2(i.offset.x * -1f, i.offset.y);
            }

            // Turn FrontCheck
            WallCheck.transform.localPosition = new Vector2(WallCheck.transform.localPosition.x * -1f, WallCheck.transform.localPosition.y);
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
}