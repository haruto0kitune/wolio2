using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Guards
{
    public class PlayerCrouchingGuard : MonoBehaviour
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
            HurtBox = GameObject.Find("CrouchingGuardHurtBox").GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            //Animation
            #region CrouchingGuard->Crouch
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.CrouchingGuard"))
                .Where(x => !Key.LeftShift)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouching", true);
                    Animator.SetBool("IsCrouchingGuard", false);
                });
            #endregion
            #region CrouchingGuard->StandingGuard
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.CrouchingGuard"))
                .Where(x => Key.LeftShift && (Key.Vertical.Value == 0f))
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouchingGuard", false);
                    Animator.SetBool("IsStandingGuard", true);
                });
            #endregion
            
            //Collision
            this.ObserveEveryValueChanged(x => Animator.GetBool("IsCrouchingGuard"))
                .Where(x => x)
                .Subscribe(_ => StartCoroutine(CrouchingGuard()));

            this.OnTriggerEnter2DAsObservable()
                .Where(x => x.gameObject.tag == "AttackLevel/1")
                .Subscribe(_ => Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Enemy"), true));
        }

        public IEnumerator CrouchingGuard()
        {
            BoxCollider2D.enabled = true;
            HurtBox.enabled = true;

            while (PlayerState.IsCrouchingGuard.Value)
            {
                yield return null;
            }

            BoxCollider2D.enabled = false;
            HurtBox.enabled = false;
        }
    }
}