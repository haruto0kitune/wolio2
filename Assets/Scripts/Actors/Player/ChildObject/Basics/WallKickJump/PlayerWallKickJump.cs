using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Basics
{
    public class PlayerWallKickJump : MonoBehaviour
    {
        GameObject Player;
        PlayerState PlayerState;
        Key Key;
        Rigidbody2D PlayerRigidbody2D;
        SpriteRenderer SpriteRenderer;
        BoxCollider2D[] BoxColliders2D;
        CircleCollider2D[] CircleColliders2D;
        [SerializeField]
        int Angle;
        [SerializeField]
        float JumpForce;
        [SerializeField]
        int Recovery;

        void Awake()
        {
            Player = GameObject.Find("Test");
            PlayerState = Player.GetComponent<PlayerState>();
            Key = Player.GetComponent<Key>();
            PlayerRigidbody2D = Player.GetComponent<Rigidbody2D>();
            SpriteRenderer = Player.GetComponent<SpriteRenderer>();
            BoxColliders2D = Player.GetComponentsInChildren<BoxCollider2D>();
            CircleColliders2D = Player.GetComponentsInChildren<CircleCollider2D>();
        }

        void Start()
        {
            // Motion
            this.FixedUpdateAsObservable()
                .Where(x => PlayerState.canWallKickJumping.Value)
                .DistinctUntilChanged(x => Key.Vertical.Value)
                .Where(x => PlayerState.IsInTheAir.Value)
                .Where(x => Key.Vertical.Value == 1f)
                .Subscribe(_ => StartCoroutine(WallKickJump(Angle, JumpForce)));

            // Flag
            this.OnTriggerEnter2DAsObservable()
                .Where(x => x.gameObject.layer == LayerMask.NameToLayer("Field")
                         || x.gameObject.layer == LayerMask.NameToLayer("Wall"))
                .Subscribe(_ => PlayerState.canWallKickJumping.Value = true);

            this.OnTriggerExit2DAsObservable()
                .Where(x => x.gameObject.layer == LayerMask.NameToLayer("Field")
                         || x.gameObject.layer == LayerMask.NameToLayer("Wall"))
                .Subscribe(_ => PlayerState.canWallKickJumping.Value = false);

            // Flag
            //this.ObserveEveryValueChanged(x => PlayerRigidbody2D.velocity.y)
            //    .Where(x => x < 2)
            //    .Subscribe(_ => PlayerState.IsWallKickJumping.Value = false);
        }

        public IEnumerator WallKickJump(int Angle, float Radius)
        {
            PlayerState.IsWallKickJumping.Value = true;
            Vector2 Vector;

            // Make velocity reset
            PlayerRigidbody2D.velocity = Vector2.zero;

            // Convert Polar to Rectangular
            if (PlayerState.FacingRight.Value)
            {
                Vector = Utility.PolarToRectangular2D(Angle, Radius);
                Vector = new Vector2(Vector.x * -1, Vector.y);
            }
            else
            {
                Vector = Utility.PolarToRectangular2D(Angle, Radius);
            }

            // Turn Sprite
            SpriteRenderer.flipX = !SpriteRenderer.flipX;

            // Turn Collision
            foreach(var i in BoxColliders2D)
            {
                i.offset = new Vector2(i.offset.x * -1f, i.offset.y);
            }

            foreach(var i in CircleColliders2D)
            {
                i.offset = new Vector2(i.offset.x * -1f, i.offset.y);
            }

            // WallKickJump
            PlayerRigidbody2D.velocity = Vector;

            // Recovery
            for(var i = 0;i < Recovery; i++)
            {
                yield return null;
            }

            PlayerState.IsWallKickJumping.Value = false;
        }
    }
}