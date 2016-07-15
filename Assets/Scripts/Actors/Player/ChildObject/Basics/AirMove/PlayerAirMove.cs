using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Basics
{
    public class PlayerAirMove : MonoBehaviour
    {
        PlayerState PlayerState;
        Key Key;
        Rigidbody2D PlayerRigidbody2D;
        [SerializeField]
        float MaxSpeed;
        [SerializeField]
        float Force;

        void Awake()
        {
            PlayerState = GameObject.Find("Test").GetComponent<PlayerState>();
            Key = GameObject.Find("Test").GetComponent<Key>();
            PlayerRigidbody2D = GameObject.Find("Test").GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            //Motion
            this.FixedUpdateAsObservable()
                .Where(x => PlayerState.IsJumping.Value)
                .Where(x => !PlayerState.IsGrounded.Value)
                .Where(x => !PlayerState.IsWallKickJumping.Value)
                .Where(x => Key.Horizontal.Value != 0)
                .Subscribe(_ => this.AirMove(Key.Horizontal.Value));
        }

        public void AirMove(float Horizontal)
        {
            PlayerRigidbody2D.AddForce(new Vector2(Force * Horizontal, 0f));

            if (PlayerRigidbody2D.velocity.x > MaxSpeed || PlayerRigidbody2D.velocity.x < -MaxSpeed)
            {
                PlayerRigidbody2D.velocity = new Vector2(MaxSpeed * Horizontal, PlayerRigidbody2D.velocity.y);
            }
        }
    }
}