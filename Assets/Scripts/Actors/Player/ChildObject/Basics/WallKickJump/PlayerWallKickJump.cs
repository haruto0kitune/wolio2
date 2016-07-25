using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Basics
{
    public class PlayerWallKickJump : MonoBehaviour
    {
        [SerializeField]
        GameObject Player;
        Animator Animator;
        ObservableStateMachineTrigger ObservableStateMachineTrigger;
        PlayerState PlayerState;
        Key Key;
        Rigidbody2D PlayerRigidbody2D;
        SpriteRenderer SpriteRenderer;
        BoxCollider2D BoxCollider2D;
        [SerializeField]
        GameObject PlayerWallKickJumpHurtBox;
        BoxCollider2D HurtBox;
        [SerializeField]
        GameObject PlayerWallKickJumpableCheckBox;
        BoxCollider2D CheckBox;
        [SerializeField]
        int Angle;
        [SerializeField]
        float JumpForce;
        [SerializeField]
        int Recovery;

        void Awake()
        {
            Animator = Player.GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            PlayerState = Player.GetComponent<PlayerState>();
            Key = Player.GetComponent<Key>();
            PlayerRigidbody2D = Player.GetComponent<Rigidbody2D>();
            SpriteRenderer = Player.GetComponent<SpriteRenderer>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            HurtBox = PlayerWallKickJumpHurtBox.GetComponent<BoxCollider2D>();
            CheckBox = PlayerWallKickJumpableCheckBox.GetComponent<BoxCollider2D>();

        }

        void Start()
        {
            // Animation
            #region EnterWallKickJump
            ObservableStateMachineTrigger
                .OnStateEnterAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.WallKickJump"))
                .Subscribe(_ => StartCoroutine(WallKickJump(Parameter.GetPlayerParameter().PlayerBasics.WallKickJump.Angle, Parameter.GetPlayerParameter().PlayerBasics.WallKickJump.JumpForce)));
            #endregion
            #region WallKickJump->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.WallKickJump"))
                .Where(x => PlayerState.IsGrounded.Value)
                .Where(x => Key.Vertical.Value != 1)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", true);
                    Animator.SetBool("IsWallKickJumping", false);
                });
            #endregion
            #region WallKickJump->Run
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.WallKickJump"))
                .Where(x => PlayerState.IsGrounded.Value && Key.Horizontal.Value != 0)
                .Where(x => Key.Vertical.Value != 1)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsRunning", true);
                    Animator.SetBool("IsWallKickJumping", false);
                });
            #endregion
            #region WallKickJump->SupineJumpingDamage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.WallKickJump"))
                .Where(x => PlayerState.WasSupineAttributeAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsWallKickJumping", false);
                    Animator.SetBool("IsSupineJumpingDamage", true);
                });
            #endregion
            #region WallKickJump->ProneJumpingDamage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.WallKickJump"))
                .Where(x => PlayerState.WasProneAttributeAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsWallKickJumping", false);
                    Animator.SetBool("IsProneJumpingDamage", true);
                });
            #endregion
            #region WallKickJump->Fall
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.WallKickJump"))
                .Where(x => PlayerRigidbody2D.velocity.y < 0) /*|| (Key.Vertical.Value != 1 || frameCount == Parameter.GetPlayerParameter().PlayerBasics.WallKickJump.Active))*/
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsWallKickJumping", false);
                    Animator.SetBool("IsFalling", true);
                });
            #endregion
            #region WallKickJump->WallKickJump
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.WallKickJump"))
                .Where(x => PlayerState.canWallKickJumping.Value)
                .Where(x => Key.Vertical.Value == 1f)
                .Subscribe(_ =>
                {
                    Animator.Play("WallKickJump", Animator.GetLayerIndex("Base Layer"), 0.0f);
                });
            #endregion


            // Motion

            //this.FixedUpdateAsObservable()
            //    .Where(x => PlayerState.canWallKickJumping.Value)
            //    .DistinctUntilChanged(x => Key.Vertical.Value)
            //    .Where(x => PlayerState.IsInTheAir.Value)
            //    .Where(x => Key.Vertical.Value == 1f)
            //    .Subscribe(_ => StartCoroutine(WallKickJump(/*Angle*/Parameter.GetPlayerParameter().PlayerBasics.WallKickJump.Angle, /*JumpForce*/Parameter.GetPlayerParameter().PlayerBasics.WallKickJump.JumpForce)));

            // Collision
            this.ObserveEveryValueChanged(x => Animator.GetBool("IsWallKickJumping"))
                .Where(x => x)
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = true;
                    HurtBox.enabled = true;
                });

            this.ObserveEveryValueChanged(x => Animator.GetBool("IsWallKickJumping"))
                .Where(x => !x)
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = false;
                    HurtBox.enabled = false;
                });

            // Flag
            CheckBox.OnTriggerStay2DAsObservable()
                .Where(x => x.gameObject.layer == LayerMask.NameToLayer("Field")
                         || x.gameObject.layer == LayerMask.NameToLayer("Wall"))
                .Subscribe(_ => PlayerState.canWallKickJumping.Value = true);

            //this.OnTriggerEnter2DAsObservable()
            //    .Where(x => x.gameObject.layer == LayerMask.NameToLayer("Field")
            //             || x.gameObject.layer == LayerMask.NameToLayer("Wall"))
            //    .Subscribe(_ => PlayerState.canWallKickJumping.Value = true);

            CheckBox.OnTriggerExit2DAsObservable()
                .Where(x => x.gameObject.layer == LayerMask.NameToLayer("Field")
                         || x.gameObject.layer == LayerMask.NameToLayer("Wall"))
                .Subscribe(_ => PlayerState.canWallKickJumping.Value = false);

            // Flag
            //this.ObserveEveryValueChanged(x => PlayerRigidbody2D.velocity.y)
            //    .Where(x => x < 2)
            //    .Subscribe(_ => PlayerState.IsWallKickJumping.Value = false);
        }

        public IEnumerator WallKickJump(int Angle, float Radius)
        {
            PlayerState.canAirMove.Value = false;
            PlayerState.canTurn.Value = false;
            Vector2 Vector;

            // Make velocity reset
            PlayerRigidbody2D.velocity = Vector2.zero;

            // Convert Polar to Rectangular
            if (PlayerState.FacingRight.Value)
            {
                Vector = Utility.PolarToRectangular2D(Angle, Radius);
                Vector = new Vector2(Vector.x * -1, Vector.y);
            }
            else
            {
                Vector = Utility.PolarToRectangular2D(Angle, Radius);
            }

            // Turn Sprite
            SpriteRenderer.flipX = !SpriteRenderer.flipX;

            // Turn Collision
            BoxCollider2D.offset = new Vector2(BoxCollider2D.offset.x * -1f, BoxCollider2D.offset.y);
            HurtBox.offset = new Vector2(HurtBox.offset.x * -1f, HurtBox.offset.y);
            CheckBox.offset = new Vector2(CheckBox.offset.x * -1f, CheckBox.offset.y);

            // WallKickJump
            PlayerRigidbody2D.velocity = Vector;

            // Recovery
            for (var i = 0; i < /*Recovery*/Parameter.GetPlayerParameter().PlayerBasics.WallKickJump.Recovery; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            PlayerState.canAirMove.Value = true;
            PlayerState.canTurn.Value = true;
        }
    }
}