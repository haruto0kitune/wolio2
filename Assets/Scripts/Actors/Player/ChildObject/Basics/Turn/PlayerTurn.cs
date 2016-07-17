using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using MyUtility;

namespace Wolio.Actor.Player.Basics
{
    public class PlayerTurn : MonoBehaviour
    {
        [SerializeField]
        GameObject Player;
        Animator Animator;
        Key Key;
        PlayerState PlayerState;
        SpriteRenderer SpriteRenderer;
        BoxCollider2D[] BoxColliders2D;
        CircleCollider2D[] CircleColliders2D;

        void Awake()
        {
            Animator = Player.GetComponent<Animator>();
            Key = Player.GetComponent<Key>();
            PlayerState = Player.GetComponent<PlayerState>();
            SpriteRenderer = Player.GetComponent<SpriteRenderer>();
            BoxColliders2D = Player.GetComponentsInChildren<BoxCollider2D>();
            CircleColliders2D = Player.GetComponentsInChildren<CircleCollider2D>();
        }

        void Start()
        {
            this.FixedUpdateAsObservable()
                .Where(x => PlayerState.canTurn.Value)
                .Where(x => (Key.Horizontal.Value > 0 & !(PlayerState.FacingRight.Value)) | (Key.Horizontal.Value < 0 & PlayerState.FacingRight.Value))
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