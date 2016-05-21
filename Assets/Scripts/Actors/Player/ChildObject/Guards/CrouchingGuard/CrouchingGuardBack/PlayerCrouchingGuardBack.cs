using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.KnockBacks.GuardBacks
{
    public class PlayerCrouchingGuardBack : MonoBehaviour
    {
        PlayerState PlayerState;
        Rigidbody2D Rigidbody2D;
        BoxCollider2D BoxCollider2D;

        void Awake()
        {
            PlayerState = GameObject.Find("Test").GetComponent<PlayerState>();
            Rigidbody2D = GameObject.Find("Test").GetComponent<Rigidbody2D>();
            BoxCollider2D = transform.parent.GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            transform.parent.OnTriggerEnter2DAsObservable()
                .Where(x => PlayerState.IsCrouchingGuard.Value)
                .Where(x => !PlayerState.IsCrouchingGuardBack.Value)
                .Where(x => x.gameObject.tag == "AttackLevel/1")
                .Subscribe(_ => StartCoroutine(CrouchingGuardBack()));
        }

        public IEnumerator CrouchingGuardBack()
        {
            PlayerState.IsCrouchingGuardBack.Value = true;
            BoxCollider2D.enabled = false;

            var VelocityStore = Rigidbody2D.velocity;

            if (PlayerState.FacingRight.Value)
            {
                Rigidbody2D.velocity = new Vector2(-2f, Rigidbody2D.velocity.y);
            }
            else
            {
                Rigidbody2D.velocity = new Vector2(2f, Rigidbody2D.velocity.y);
            }

            for (int i = 0; i < 3; i++)
            {
                yield return null;
            }

            Rigidbody2D.velocity = VelocityStore;
            BoxCollider2D.enabled = true;
            PlayerState.IsCrouchingGuardBack.Value = false;
        }
    }
}