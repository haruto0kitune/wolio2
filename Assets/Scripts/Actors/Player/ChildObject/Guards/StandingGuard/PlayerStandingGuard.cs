using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Guards
{
    public class PlayerStandingGuard : MonoBehaviour
    {
        Animator Animator;
        ObservableStateMachineTrigger ObservableStateMachineTrigger;
        PlayerState PlayerState;
        Rigidbody2D PlayerRigidbody2D;
        Key Key;
        BoxCollider2D BoxCollider2D;
        CircleCollider2D CircleCollider2D;
        BoxCollider2D HurtBox;

        void Awake()
        {
            Animator = GameObject.Find("Test").GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            PlayerState = GameObject.Find("Test").GetComponent<PlayerState>();
            PlayerRigidbody2D = GameObject.Find("Test").GetComponent<Rigidbody2D>();
            Key = GameObject.Find("Test").GetComponent<Key>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            CircleCollider2D = GetComponent<CircleCollider2D>();
            HurtBox = GameObject.Find("StandingGuardHurtBox").GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            //Animation
            #region StandingGuard->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.StandingGuard"))
                .Where(x => !Key.LeftShift || !PlayerState.IsGrounded.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", true);
                    Animator.SetBool("IsStandingGuard", false);
                });
            #endregion
            #region StandingGuard->CrouchingGuard
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.StandingGuard"))
                .Where(x => Key.LeftShift && (Key.Vertical.Value == -1f))
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStandingGuard", false);
                    Animator.SetBool("IsCrouchingGuard", true);
                });
            #endregion

            //Collision
            this.ObserveEveryValueChanged(x => Animator.GetBool("IsStandingGuard"))
                .Where(x => x)
                .Subscribe(_ => StartCoroutine(StandingGuard()));

            this.OnTriggerEnter2DAsObservable()
                .Where(x => x.gameObject.tag == "AttackLevel/1")
                .Subscribe(_ => Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Enemy"), true));
        }

        public IEnumerator StandingGuard()
        {
            BoxCollider2D.enabled = true;
            CircleCollider2D.enabled = true;
            HurtBox.enabled = true;

            while (PlayerState.IsStandingGuard.Value)
            {
                yield return null;
            }

            BoxCollider2D.enabled = false;
            CircleCollider2D.enabled = false;
            HurtBox.enabled = false;
        }
    }
}
