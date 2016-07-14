using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Basics
{
    public class PlayerJump : MonoBehaviour
    {
        Animator Animator;
        ObservableStateMachineTrigger ObservableStateMachineTrigger;
        PlayerState PlayerState;
        Rigidbody2D PlayerRigidbody2D;
        Key Key;
        BoxCollider2D BoxCollider2D;
        BoxCollider2D HurtBox;
        [SerializeField]
        float JumpForce;

        void Awake()
        {
            Animator = GameObject.Find("Test").GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            PlayerState = GameObject.Find("Test").GetComponent<PlayerState>();
            PlayerRigidbody2D = GameObject.Find("Test").GetComponent<Rigidbody2D>();
            Key = GameObject.Find("Test").GetComponent<Key>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            HurtBox = GameObject.Find("JumpHurtBox").GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            //Animation
            #region Jump->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Jump"))
                .Where(x => PlayerState.IsGrounded.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", true);
                    Animator.SetBool("IsJumping", false);
                });
            #endregion
            #region Jump->JumpingLightAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Jump"))
                .Where(x => PlayerState.canJumpingLightAttack.Value)
                .Where(x => Key.Z)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsJumping", false);
                    Animator.SetBool("IsJumpingLightAttack", true);
                });
            #endregion
            #region Jump->JumpingMiddleAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Jump"))
                .Where(x => PlayerState.canJumpingMiddleAttack.Value)
                .Where(x => Key.X)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsJumping", false);
                    Animator.SetBool("IsJumpingMiddleAttack", true);
                });
            #endregion
            #region Jump->JumpingHighAttack
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Jump"))
                .Where(x => PlayerState.canJumpingHighAttack.Value)
                .Where(x => Key.C)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsJumping", false);
                    Animator.SetBool("IsJumpingHighAttack", true);
                });
            #endregion
            #region Jump->JumpingGuard
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Jump"))
                .Where(x => PlayerState.canJumpingGuard.Value)
                .Where(x => !PlayerState.IsGrounded.Value)
                .Where(x => Key.LeftShift)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsJumping", false);
                    Animator.SetBool("IsJumpingGuard", true);
                });
            #endregion
            #region Jump->SupineJumpingDamage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Jump"))
                .Where(x => PlayerState.WasSupineAttributeAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsJumping", false);
                    Animator.SetBool("IsSupineJumpingDamage", true);
                });
            #endregion
            #region Jump->ProneJumpingDamage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Jump"))
                .Where(x => PlayerState.WasProneAttributeAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsJumping", false);
                    Animator.SetBool("IsProneJumpingDamage", true);
                });
            #endregion
            #region Jump->DoubleJump
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Jump"))
                .Where(x => PlayerState.canDoubleJump.Value)
                .Where(x => !PlayerState.hasDoubleJumped.Value)
                .Where(x => PlayerRigidbody2D.velocity.y < 2)
                .Where(x => Key.Vertical.Value == 1)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsJumping", false);
                    Animator.SetBool("IsDoubleJumping", true);
                });
            #endregion
            #region Jump->Fall
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Jump"))
                .Where(x => PlayerRigidbody2D.velocity.y < 0)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsJumping", false);
                    Animator.SetBool("IsFalling", true);
                });
            #endregion
            #region Jump->AirDash
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Jump"))
                .Where(x => PlayerState.canAirDash.Value)
                .Where(x => !PlayerState.hasAirDashed.Value)
                .Where(x => PlayerState.hasInputedAirDashCommand.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsJumping", false);
                    Animator.SetBool("IsAirDashing", true);
                });
            #endregion

            //Motion
            this.FixedUpdateAsObservable()
                .Where(x => PlayerState.canJump.Value)
                .Where(x => Key.Vertical.Value == 1)
                .Subscribe(_ => this.Jump(JumpForce));

            //Collision
            this.ObserveEveryValueChanged(x => Animator.GetBool("IsJumping"))
                .Where(x => x)
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = true;
                    HurtBox.enabled = true;
                });

            this.ObserveEveryValueChanged(x => Animator.GetBool("IsJumping"))
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