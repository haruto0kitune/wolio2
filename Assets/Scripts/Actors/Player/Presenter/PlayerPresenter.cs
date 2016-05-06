using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

[RequireComponent(typeof(Key))]
[RequireComponent(typeof(PlayerState))]
[RequireComponent(typeof(PlayerConfig))]
[RequireComponent(typeof(PlayerMover))]
public partial class PlayerPresenter : MonoBehaviour
{
    PlayerState PlayerState;
    PlayerConfig PlayerConfig;
    PlayerMover PlayerMover;
    Key Key;
    Rigidbody2D Rigidbody2D;
    SpriteRenderer SpriteRenderer;

    void Awake()
    {
        PlayerState = GetComponent<PlayerState>();
        PlayerConfig = GetComponent<PlayerConfig>();
        PlayerMover = GetComponent<PlayerMover>();
        Key = GetComponent<Key>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        #region Basics
        #region Turn
        this.FixedUpdateAsObservable()
            .SelectMany(x => Key.Horizontal)
            .Where(x => (x > 0 & !(PlayerState.FacingRight.Value)) | (x < 0 & PlayerState.FacingRight.Value))
            .Subscribe(_ => PlayerMover.Turn());
        #endregion
        #region Run
        this.FixedUpdateAsObservable()
            .Subscribe(_ => PlayerMover.Run(Key.Horizontal.Value, PlayerConfig.MaxSpeed));
        #endregion
        #region Jump
        this.FixedUpdateAsObservable()
            .Where(x => Key.Vertical.Value == 1)
            .Where(x => PlayerState.IsGrounded.Value)
            .Where(x => !PlayerState.IsClimbable.Value)
            .Where(x => !PlayerState.IsClimbing.Value)
            .Subscribe(_ => PlayerMover.Jump(PlayerConfig.JumpForce));
        #endregion
        #region Creep
        this.FixedUpdateAsObservable()
            .Where(x => PlayerState.IsCrouching.Value)
            .Where(x => !PlayerState.IsRunning.Value)
            .Where(x => Key.Horizontal.Value != 0 && Key.Vertical.Value == -1)
            .Subscribe(_ => PlayerMover.Creep(Key.Horizontal.Value, PlayerConfig.CreepSpeed));
        #endregion
        #region Climb
        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Ladder")
            .Subscribe(_ => PlayerState.IsClimbable.Value = true);

        this.OnTriggerExit2DAsObservable()
            .Where(x => x.gameObject.tag == "Ladder")
            .Subscribe(_ =>
            {
                PlayerState.IsClimbable.Value = false;
                PlayerState.IsClimbing.Value = false;
            });

        this.OnTriggerEnter2DAsObservable()
            .Where(x => PlayerState.IsClimbing.Value)
            .Where(x => x.gameObject.tag == "Hard Platform")
            .Subscribe(_ =>
            {
                transform.position = new Vector2(transform.position.x, _.gameObject.transform.position.y + _.gameObject.GetComponent<SpriteRenderer>().bounds.extents.y + SpriteRenderer.bounds.extents.y);
            });

        this.OnTriggerStay2DAsObservable()
            .Where(x => x.gameObject.tag == "CanClimbDownLadder")
            .Where(x => Key.Vertical.Value == -1)
            .Subscribe(_ =>
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - 0.63f);
                PlayerState.IsClimbing.Value = true;
            });

        this.UpdateAsObservable()
            .Select(x => PlayerState.IsClimbable.Value && Key.Vertical.Value != 0)
            .Where(x => x)
            .Subscribe(_ => PlayerState.IsClimbing.Value = true);

        this.UpdateAsObservable()
            .Select(x => PlayerState.IsClimbing.Value && Key.Horizontal.Value != 0)
            .Where(x => x)
            .Subscribe(_ => PlayerState.IsClimbing.Value = false);

        this.FixedUpdateAsObservable()
            .Where(x => PlayerState.IsClimbing.Value)
            .Subscribe(_ => Rigidbody2D.gravityScale = 0f);

        this.FixedUpdateAsObservable()
            .Where(x => !PlayerState.IsClimbing.Value)
            .Subscribe(_ => Rigidbody2D.gravityScale = PlayerConfig.GravityScaleStore);

        this.FixedUpdateAsObservable()
            .Where(x => PlayerState.IsClimbing.Value)
            .Where(x => Key.Vertical.Value == 0)
            .Subscribe(_ => Rigidbody2D.velocity = Vector2.zero);

        this.FixedUpdateAsObservable()
            .Where(x => PlayerState.IsClimbing.Value)
            .Where(x => Key.Vertical.Value != 0)
            .Subscribe(_ => PlayerMover.Climb(Key.Vertical.Value, PlayerConfig.MaxSpeed));
        #endregion
        #region AirMove
        this.FixedUpdateAsObservable()
            .Where(x => PlayerState.IsJumping.Value)
            .Where(x => !PlayerState.IsGrounded.Value)
            .Where(x => Key.Horizontal.Value != 0)
            .Subscribe(_ => PlayerMover.AirMove(Key.Horizontal.Value));

        this.FixedUpdateAsObservable()
            .Where(x => !PlayerState.IsJumping.Value)
            .Where(x => PlayerState.IsGrounded.Value)
            .Subscribe(_ => Rigidbody2D.AddForce(Vector2.zero));
        #endregion
        #endregion
        #region Damages
        #region Damage
        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "FallingSplinter")
            .Subscribe(_ => PlayerState.Hp.Value--);

        this.OnCollisionEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Splinter")
            .Subscribe(_ => PlayerState.Hp.Value--);
        #endregion
        #endregion
    }
}