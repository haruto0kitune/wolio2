using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Basics
{
    public class PlayerRun : MonoBehaviour
    {
        Animator Animator;
        ObservableStateMachineTrigger ObservableStateMachineTrigger;
        PlayerState PlayerState;
        Rigidbody2D PlayerRigidbody2D;
        Key Key;
        BoxCollider2D BoxCollider2D;
        BoxCollider2D HurtBox;
        CircleCollider2D CircleCollider2D;
        [SerializeField]
        float MaxSpeed;

        void Awake()
        {
            Animator = GameObject.Find("Test").GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            PlayerState = GameObject.Find("Test").GetComponent<PlayerState>();
            PlayerRigidbody2D = GameObject.Find("Test").GetComponent<Rigidbody2D>();
            Key = GameObject.Find("Test").GetComponent<Key>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            HurtBox = GameObject.Find("RunHurtBox").GetComponent<BoxCollider2D>();
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
            #region Run->Jump
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Run"))
                .SelectMany(x => Key.Vertical)
                .Where(x => x == 1)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsRunning", false);
                    Animator.SetBool("IsJumping", true);
                });
            #endregion

            //Motion
            this.FixedUpdateAsObservable()
                .Where(x => !Animator.GetBool("IsJumping"))
                .Subscribe(_ => this.Run(Key.Horizontal.Value, MaxSpeed));

            //Collision
            this.ObserveEveryValueChanged(x => Animator.GetBool("IsRunning"))
                .Where(x => x)
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = true;
                    HurtBox.enabled = true;
                    CircleCollider2D.enabled = true;
                });

            this.ObserveEveryValueChanged(x => Animator.GetBool("IsRunning"))
                .Where(x => !x)
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = false;
                    HurtBox.enabled = false;
                    CircleCollider2D.enabled = false;
                });
        }

        public void Run(float Horizontal, float MaxSpeed)
        {
            PlayerRigidbody2D.velocity = new Vector2(Horizontal * MaxSpeed, PlayerRigidbody2D.velocity.y);
        }
    }
}
