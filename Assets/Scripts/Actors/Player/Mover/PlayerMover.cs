using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerConfig))]
[RequireComponent(typeof(PlayerState))]
[RequireComponent(typeof(Key))]
public partial class PlayerMover : MonoBehaviour
{
    Rigidbody2D Rigidbody2D;
    PlayerConfig PlayerConfig;
    PlayerState PlayerState;
    Key Key;

    void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        PlayerConfig = GetComponent<PlayerConfig>();
        PlayerState = GetComponent<PlayerState>();
        Key = GetComponent<Key>();
    }

    private void Start()
    {
        #region Basics
        #region Turn
        this.FixedUpdateAsObservable()
            .SelectMany(x => Key.Horizontal)
            .Where(x => (x > 0 & !(PlayerState.FacingRight.Value)) | (x < 0 & PlayerState.FacingRight.Value))
            .Subscribe(_ => this.Turn());
        #endregion
        #region Run
        this.FixedUpdateAsObservable()
            .Subscribe(_ => this.Run(Key.Horizontal.Value, PlayerConfig.MaxSpeed));
        #endregion
        #region Jump
        this.FixedUpdateAsObservable()
            .Where(x => Key.Vertical.Value == 1)
            .Where(x => PlayerState.IsGrounded.Value)
            .Where(x => !PlayerState.IsClimbable.Value)
            .Where(x => !PlayerState.IsClimbing.Value)
            .Subscribe(_ => this.Jump(PlayerConfig.JumpForce));
        #endregion
        #region Creep
        this.FixedUpdateAsObservable()
            .Where(x => PlayerState.IsCrouching.Value)
            .Where(x => !PlayerState.IsRunning.Value)
            .Where(x => Key.Horizontal.Value != 0 && Key.Vertical.Value == -1)
            .Subscribe(_ => this.Creep(Key.Horizontal.Value, PlayerConfig.CreepSpeed));
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
                transform.position = new Vector2(transform.position.x, _.gameObject.transform.position.y + _.gameObject.GetComponent<SpriteRenderer>().bounds.extents.y + _.gameObject.GetComponent<SpriteRenderer>().bounds.extents.y);
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
            .Subscribe(_ => this.Climb(Key.Vertical.Value, PlayerConfig.MaxSpeed));
        #endregion
        #region AirMove
        this.FixedUpdateAsObservable()
            .Where(x => PlayerState.IsJumping.Value)
            .Where(x => !PlayerState.IsGrounded.Value)
            .Where(x => Key.Horizontal.Value != 0)
            .Subscribe(_ => this.AirMove(Key.Horizontal.Value));

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

    public void Turn()
    {
        Utility.Flip(transform);
    }

    public void Run(float Horizontal, float MaxSpeed)
    {
        Rigidbody2D.velocity = new Vector2(Horizontal * MaxSpeed, Rigidbody2D.velocity.y);
    }

    public void Jump(float JumpForce)
    {
        Rigidbody2D.velocity = new Vector2(0f, JumpForce);
    }

    public void Creep(float Horizontal, float CreepSpeed)
    {
        Rigidbody2D.velocity = new Vector2(Horizontal * CreepSpeed, Rigidbody2D.velocity.y);
    }

    public void Climb(float Vertical, float MaxSpeed)
    {
        Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, Vertical * (MaxSpeed - 3)); 
    }

    public void AirMove(float Horizontal)
    {
        if (Horizontal != 0)
        {
            Rigidbody2D.AddForce(new Vector2(50f * Horizontal, 0f));
        }
    }   
}