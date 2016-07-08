using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Basics
{
    public class PlayerCrouch : MonoBehaviour
    {
        Animator Animator;
        ObservableStateMachineTrigger ObservableStateMachineTrigger;
        PlayerState PlayerState;
        Key Key;
        BoxCollider2D BoxCollider2D;
        BoxCollider2D HurtBox;
        GameObject CeilingCheck;
        [SerializeField]
        LayerMask WhatIsGround;

        void Awake()
        {
            Animator = GameObject.Find("Test").GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            PlayerState = GameObject.Find("Test").GetComponent<PlayerState>();
            Key = GameObject.Find("Test").GetComponent<Key>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            HurtBox = GameObject.Find("CrouchHurtBox").GetComponent<BoxCollider2D>();
            CeilingCheck = GameObject.Find("CeilingCheck");
        }

        void Start()
        {
            //Animation
            #region Crouch->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Crouch"))
                .Where(x => Key.Vertical.Value == 0)
                .Where(x => Physics2D.OverlapCircle(CeilingCheck.transform.position, 0.1f, WhatIsGround) == null)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", true);
                    Animator.SetBool("IsCrouching", false);
                });
            #endregion
            #region Crouch->Creep
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Crouch"))
                .Where(x => PlayerState.canCreep.Value)
                .Where(x => Key.Horizontal.Value != 0 && Key.Vertical.Value == -1)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouching", false);
                    Animator.SetBool("IsCreeping", true);
                });
            #endregion
            #region Crouch->CrouchingLightAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Crouch"))
                .Where(x => PlayerState.canCrouchingLightAttack.Value)
                .Where(x => !PlayerState.hasInputedLightDragonPunchCommand.Value)
                .Where(x => Key.Z)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouching", false);
                    Animator.SetBool("IsCrouchingLightAttack", true);
                });
            #endregion
            #region Crouch->CrouchingMiddleAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Crouch"))
                .Where(x => Key.X)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouching", false);
                    Animator.SetBool("IsCrouchingMiddleAttack", true);
                });
            #endregion
            #region Crouch->CrouchingHighAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Crouch"))
                .Where(x => Key.C)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouching", false);
                    Animator.SetBool("IsCrouchingHighAttack", true);
                });
            #endregion
            #region Crouch->CrouchingGuard
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Crouch"))
                .Where(x => Key.LeftShift && (Key.Vertical.Value == -1))
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouching", false);
                    Animator.SetBool("IsCrouchingGuard", true);
                });
            #endregion
            #region Crouch->CrouchingDamage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Crouch"))
                .Where(x => PlayerState.WasAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouching", false);
                    Animator.SetBool("IsCrouchingDamage", true);
                });
            #endregion

            //Collision
            this.ObserveEveryValueChanged(x => Animator.GetBool("IsCrouching"))
                .Where(x => x)
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = true;
                    HurtBox.enabled = true;
                });

            this.ObserveEveryValueChanged(x => Animator.GetBool("IsCrouching"))
                .Where(x => !x)
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = false;
                    HurtBox.enabled = false;
                });
        }
    }
}