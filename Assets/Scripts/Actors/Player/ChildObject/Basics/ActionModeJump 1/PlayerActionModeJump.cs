using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Basics
{
    public class PlayerActionModeJump : MonoBehaviour
    {
        [SerializeField]
        GameObject Actor;
        Animator Animator;
        ObservableStateMachineTrigger ObservableStateMachineTrigger;
        PlayerState PlayerState;
        Rigidbody2D PlayerRigidbody2D;
        Key Key;
        BoxCollider2D BoxCollider2D;
        [SerializeField]
        GameObject PlayerActionModeJumpHurtBox;
        BoxCollider2D HurtBox;
        [SerializeField]
        float JumpForce;
        bool enterActionModeJumpState;

        void Awake()
        {
            Animator = Actor.GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            PlayerState = Actor.GetComponent<PlayerState>();
            PlayerRigidbody2D = Actor.GetComponent<Rigidbody2D>();
            Key = Actor.GetComponent<Key>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            HurtBox = PlayerActionModeJumpHurtBox.GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            //Animation
            #region EnterActionModeJump
            ObservableStateMachineTrigger
                .OnStateEnterAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.ActionModeJump"))
                .Subscribe(_ => enterActionModeJumpState = true);
                //.Subscribe(_ => Jump(JumpForce));
            #endregion
            #region ActionModeJump->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.ActionModeJump"))
                .Where(x => PlayerState.IsGrounded.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", true);
                    Animator.SetBool("IsActionModeJumping", false);
                    enterActionModeJumpState = false;
                });
            #endregion
            #region ActionModeJump->Run
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.ActionModeJump"))
                .Where(x => PlayerState.IsGrounded.Value && Key.Horizontal.Value != 0)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsRunning", true);
                    Animator.SetBool("IsActionModeJumping", false);
                    enterActionModeJumpState = false;
                });
            #endregion
            #region ActionModeJump->SupineJumpingDamage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.ActionModeJump"))
                .Where(x => PlayerState.WasSupineAttributeAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsActionModeJumping", false);
                    Animator.SetBool("IsSupineJumpingDamage", true);
                    enterActionModeJumpState = false;
                });
            #endregion
            #region ActionModeJump->ProneJumpingDamage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.ActionModeJump"))
                .Where(x => PlayerState.WasProneAttributeAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsActionModeJumping", false);
                    Animator.SetBool("IsProneJumpingDamage", true);
                    enterActionModeJumpState = false;
                });
            #endregion
            #region ActionModeJump->Fall
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.ActionModeJump"))
                .Where(x => !PlayerState.hasInputedAirDashCommand.Value)
                .Where(x => PlayerRigidbody2D.velocity.y < 0)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsActionModeJumping", false);
                    Animator.SetBool("IsFalling", true);
                    enterActionModeJumpState = false;
                });
            #endregion

            //Motion
            this.FixedUpdateAsObservable()
                .Where(x => enterActionModeJumpState)
                .Do(x => enterActionModeJumpState = false)
                .Subscribe(_ => this.Jump(/*JumpForce*/Parameter.GetPlayerParameter().PlayerBasics.ActionModeJump.JumpForce));

            //Collision
            this.ObserveEveryValueChanged(x => Animator.GetBool("IsActionModeJumping"))
                .Where(x => x)
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = true;
                    HurtBox.enabled = true;
                });

            this.ObserveEveryValueChanged(x => Animator.GetBool("IsActionModeJumping"))
                .Where(x => !x)
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = false;
                    HurtBox.enabled = false;
                });
        }

        public void Jump(float JumpForce)
        {
            PlayerRigidbody2D.velocity = new Vector2(PlayerRigidbody2D.velocity.x, JumpForce);
        }
    }
}