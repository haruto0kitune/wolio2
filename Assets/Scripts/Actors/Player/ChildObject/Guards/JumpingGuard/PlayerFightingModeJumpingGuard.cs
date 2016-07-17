using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Guards
{
    public class PlayerFightingModeJumpingGuard : MonoBehaviour
    {
        Animator Animator;
        ObservableStateMachineTrigger ObservableStateMachineTrigger;
        PlayerState PlayerState;
        Rigidbody2D PlayerRigidbody2D;
        Key Key;
        BoxCollider2D BoxCollider2D;
        BoxCollider2D HurtBox;

        void Awake()
        {
            Animator = GameObject.Find("Test").GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            PlayerState = GameObject.Find("Test").GetComponent<PlayerState>();
            PlayerRigidbody2D = GameObject.Find("Test").GetComponent<Rigidbody2D>();
            Key = GameObject.Find("Test").GetComponent<Key>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            HurtBox = GameObject.Find("JumpingGuardHurtBox").GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            //Animation
            #region JumpingGuard->Jump
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.JumpingGuard"))
                .Where(x => !Key.LeftShift)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsFightingModeJumping", true);
                    Animator.SetBool("IsFightingModeJumpingGuard", false);
                });
            #endregion
            #region JumpingGuard->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.JumpingGuard"))
                .Where(x => PlayerState.IsGrounded.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", true);
                    Animator.SetBool("IsFightingModeJumpingGuard", false);
                });
            #endregion

            //Collision
            this.ObserveEveryValueChanged(x => Animator.GetBool("IsFightingModeJumpingGuard"))
                .Where(x => x)
                .Subscribe(_ => StartCoroutine(JumpingGuard()));

            this.OnTriggerEnter2DAsObservable()
                .Where(x => x.gameObject.tag == "AttackLevel/1")
                .Subscribe(_ => Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Enemy"), true));
        }

        public IEnumerator JumpingGuard()
        {
            BoxCollider2D.enabled = true;
            HurtBox.enabled = true;

            while (PlayerState.IsFightingModeJumpingGuard.Value)
            {
                yield return null;
            }

            BoxCollider2D.enabled = false;
            HurtBox.enabled = false;
        }
    }
}
