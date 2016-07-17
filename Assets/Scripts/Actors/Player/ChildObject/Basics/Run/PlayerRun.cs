using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Basics
{
    public class PlayerRun : MonoBehaviour
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
        GameObject PlayerRunHurtBox;
        BoxCollider2D HurtBox;
        CircleCollider2D CircleCollider2D;
        [SerializeField]
        float MaxSpeed;

        void Awake()
        {
            Animator = Actor.GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            PlayerState = Actor.GetComponent<PlayerState>();
            PlayerRigidbody2D = Actor.GetComponent<Rigidbody2D>();
            Key = Actor.GetComponent<Key>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            HurtBox = PlayerRunHurtBox.GetComponent<BoxCollider2D>();
            CircleCollider2D = GetComponent<CircleCollider2D>();
        }

        void Start()
        {
            //Animation
            #region Run->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Run"))
                .Where(x => Key.Horizontal.Value == 0)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", true);
                    Animator.SetBool("IsRunning", false);
                });
            #endregion
            #region Run->ActionModeJump
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Run"))
                .Where(x => PlayerState.canActionModeJump.Value)
                .Where(x => Key.Vertical.Value == 1)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsRunning", false);
                    Animator.SetBool("IsActionModeJumping", true);
                });
            #endregion
            #region Run->FightingModeJump
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Run"))
                .Where(x => PlayerState.canFightingModeJump.Value)
                .Where(x => Key.Vertical.Value == 1)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsRunning", false);
                    Animator.SetBool("IsFightingModeJumping", true);
                });
            #endregion
            #region Run->Fall
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Run"))
                .Where(x => !PlayerState.IsGrounded.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsRunning", false);
                    Animator.SetBool("IsFalling", true);
                });
            #endregion

            //Motion
            this.FixedUpdateAsObservable()
                .Where(x => !PlayerState.hasInputedGrabCommand.Value)
                .Where(x => PlayerState.canRun.Value)
                .Subscribe(_ => this.Run(Key.Horizontal.Value, MaxSpeed));

            //Collision
            this.ObserveEveryValueChanged(x => Animator.GetBool("IsRunning"))
                .Where(x => x)
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = true;
                    CircleCollider2D.enabled = true;
                    HurtBox.enabled = true;
                });

            this.ObserveEveryValueChanged(x => Animator.GetBool("IsRunning"))
                .Where(x => !x)
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = false;
                    CircleCollider2D.enabled = false;
                    HurtBox.enabled = false;
                });
        }

        public void Run(float Horizontal, float MaxSpeed)
        {
            PlayerRigidbody2D.velocity = new Vector2(Horizontal * /*MaxSpeed*/Parameter.GetPlayerParameter().PlayerBasics.Run.MaxSpeed, PlayerRigidbody2D.velocity.y);
        }
    }
}