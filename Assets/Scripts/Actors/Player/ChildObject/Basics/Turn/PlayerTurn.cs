using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using MyUtility;

namespace Wolio.Actor.Player.Basics
{
    public class PlayerTurn : MonoBehaviour
    {
        GameObject Player;
        Key Key;
        PlayerState PlayerState;
        SpriteRenderer SpriteRenderer;
        BoxCollider2D[] BoxColliders2D;
        CircleCollider2D[] CircleColliders2D;

        void Awake()
        {
            Player = GameObject.Find("Test");
            Key = Player.GetComponent<Key>();
            PlayerState = Player.GetComponent<PlayerState>();
            SpriteRenderer = Player.GetComponent<SpriteRenderer>();
            BoxColliders2D = Player.GetComponentsInChildren<BoxCollider2D>();
            CircleColliders2D = Player.GetComponentsInChildren<CircleCollider2D>();
        }

        void Start()
        {
            this.FixedUpdateAsObservable()
                .SelectMany(x => Key.Horizontal)
                .Where(x => (x > 0 & !(PlayerState.FacingRight.Value)) | (x < 0 & PlayerState.FacingRight.Value))
                .Subscribe(_ => this.Turn());
        }

        public void Turn()
        {
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
        }
    }
}