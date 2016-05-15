using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Attacks.NormalAttacks.CrouchingAttacks
{
    public class PlayerCrouchingHighAttack : MonoBehaviour
    {
        PlayerState PlayerState;
        BoxCollider2D BoxCollider2D;

        void Awake()
        {
            PlayerState = GameObject.Find("Test").GetComponent<PlayerState>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            PlayerState.IsCrouchingHighAttack
                .Where(x => x)
                .Subscribe(_ => StartCoroutine(Attack()));
        }

        public IEnumerator Attack()
        {
            BoxCollider2D.enabled = true;

            for (var i = 0; i < 3; i++)
            {
                yield return null;
            }

            BoxCollider2D.enabled = false;
        }
    }
}