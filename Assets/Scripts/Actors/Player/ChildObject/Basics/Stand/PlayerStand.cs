using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Basics
{
    public class PlayerStand : MonoBehaviour
    {
        Animator Animator;
        ObservableStateMachineTrigger ObservableStateMachineTrigger;
        PlayerState PlayerState;
        Rigidbody2D PlayerRigidbody2D;
        Key Key;
        BoxCollider2D BoxCollider2D;
        BoxCollider2D HurtBox;
        CircleCollider2D CircleCollider2D;

        void Awake()
        {
            Animator = GameObject.Find("Test").GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            PlayerState = GameObject.Find("Test").GetComponent<PlayerState>();
            PlayerRigidbody2D = GameObject.Find("Test").GetComponent<Rigidbody2D>();
            Key = GameObject.Find("Test").GetComponent<Key>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            HurtBox = GameObject.Find("StandHurtBox").GetComponent<BoxCollider2D>();
            CircleCollider2D = GetComponent<CircleCollider2D>();
        }

        void Start()
        {
            //Animation
            #region Stand->Run
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
                .Where(x => Key.Horizontal.Value != 0 && Key.Vertical.Value == 0)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", false);
                    Animator.SetBool("IsRunning", true);
                });

            #endregion
            #region Stand->Jump
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
                .SelectMany(x => Key.Vertical)
                .Where(x => x == 1)
                .Where(x => !PlayerState.IsJumping.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", false);
                    Animator.SetBool("IsJumping", true);
                });
            #endregion
            #region Stand->Crouch
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
                .Where(x => Key.Vertical.Value == -1)
                .Where(x => !PlayerState.IsJumping.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", false);
                    Animator.SetBool("IsCrouching", true);
                });
            #endregion
            #region Stand->StandingLightAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
                .Where(x => Key.Z)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", false);
                    Animator.SetBool("IsStandingLightAttack", true);
                });
            #endregion
            #region Stand->StandingMiddleAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
                .Where(x => Key.X)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", false);
                    Animator.SetBool("IsStandingMiddleAttack", true);
                });
            #endregion
            #region Stand->StandingHighAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
                .Where(x => Key.C)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", false);
                    Animator.SetBool("IsStandingHighAttack", true);
                });
            #endregion
            #region Stand->StandingGuard
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
                .Where(x => Key.LeftShift)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", false);
                    Animator.SetBool("IsStandingGuard", true);
                });
            #endregion

            //Collision
            this.ObserveEveryValueChanged(x => Animator.GetBool("IsStanding"))
                .Where(x => x)
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = true;
                    HurtBox.enabled = true;
                    CircleCollider2D.enabled = true;
                });

            this.ObserveEveryValueChanged(x => Animator.GetBool("IsStanding"))
                .Where(x => !x)
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = false;
                    HurtBox.enabled = false;
                    CircleCollider2D.enabled = false;
                });
        }
    }
}
