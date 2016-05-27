using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using Wolio.Actor.Player;

namespace Wolio.Weapons
{
    public class Fireball : MonoBehaviour
    {
        Rigidbody2D Rigidbody2D;
        public Vector2 Speed;
        PlayerState PlayerState;

        void Awake()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
            PlayerState = GameObject.Find("Test").GetComponent<PlayerState>();
        }

        void Start()
        {
            this.FixedUpdateAsObservable()
                .Subscribe(_ => Rigidbody2D.velocity = Speed);

            this.OnBecameInvisibleAsObservable()
                .Subscribe(_ => Destroy(this.gameObject));
        }
    }
}
