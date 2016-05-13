using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player
{
    public class PlayerJumpingGuard : MonoBehaviour
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
            PlayerState.IsJumpingGuard
                .Where(x => x)
                .Subscribe(_ => StartCoroutine(JumpingGuard()));
        }

        public IEnumerator JumpingGuard()
        {
            BoxCollider2D.enabled = true;

            while (PlayerState.IsJumpingGuard.Value)
            {
                yield return null;
            }

            BoxCollider2D.enabled = false;
        }
    }
}
