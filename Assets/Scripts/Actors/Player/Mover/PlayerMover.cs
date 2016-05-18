using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerConfig))]
    [RequireComponent(typeof(PlayerState))]
    [RequireComponent(typeof(Key))]
    public partial class PlayerMover : MonoBehaviour
    {
        Rigidbody2D Rigidbody2D;
        PlayerConfig PlayerConfig;
        PlayerState PlayerState;
        Key Key;

        void Awake()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
            PlayerConfig = GetComponent<PlayerConfig>();
            PlayerState = GetComponent<PlayerState>();
            Key = GetComponent<Key>();
        }

        private void Start()
        {
            #region Basics
            #region AirMove
            this.FixedUpdateAsObservable()
                .Where(x => PlayerState.IsJumping.Value)
                .Where(x => !PlayerState.IsGrounded.Value)
                .Where(x => Key.Horizontal.Value != 0)
                .Subscribe(_ => this.AirMove(Key.Horizontal.Value));

            this.FixedUpdateAsObservable()
                .Where(x => !PlayerState.IsJumping.Value)
                .Where(x => PlayerState.IsGrounded.Value)
                .Subscribe(_ => Rigidbody2D.AddForce(Vector2.zero));
            #endregion
            #region WallKickJump
            this.OnCollisionStay2DAsObservable()
                .Where(x => x.gameObject.layer == LayerMask.NameToLayer("Field"))
                .Where(x => PlayerState.IsJumping.Value)
                .Where(x => Key.Vertical.Value == 1f)
                .Subscribe(_ => WallKickJump(60, 10));
            #endregion
            #endregion
            #region Damages
            #region Damage
            this.OnTriggerEnter2DAsObservable()
                .Where(x => x.gameObject.tag == "FallingSplinter")
                .Subscribe(_ => PlayerState.Hp.Value--);

            this.OnCollisionEnter2DAsObservable()
                .Where(x => x.gameObject.tag == "Splinter")
                .Subscribe(_ => PlayerState.Hp.Value--);
            #endregion
            #endregion
        }

        public void AirMove(float Horizontal)
        {
            if (Horizontal != 0)
            {
                Rigidbody2D.AddForce(new Vector2(50f * Horizontal, 0f));
            }
        }

        public void WallKickJump(int Angle, float Radius)
        {
            Vector2 Vector;

            // Make velocity reset
            Rigidbody2D.velocity = Vector2.zero;

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

            Utility.Flip(GameObject.Find("Test").transform);
            Rigidbody2D.velocity = Vector;
        }
    }
}