using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

[RequireComponent(typeof(PlayerState))]
[RequireComponent(typeof(PlayerConfig))]
[RequireComponent(typeof(PlayerMotion))]
[RequireComponent(typeof(Key))]
public class PlayerPresenter : MonoBehaviour
{
    PlayerState PlayerState;
    PlayerConfig PlayerConfig;
    PlayerMotion PlayerMotion;
    Key Key;
    Animator Animator;
    Rigidbody2D Rigidbody2D;

    void Awake()
    {
        PlayerState = GetComponent<PlayerState>();
        PlayerConfig = GetComponent<PlayerConfig>();
        PlayerMotion = GetComponent<PlayerMotion>();
        Key = GetComponent<Key>();
        Animator = GetComponent<Animator>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        FixedUpdateAsObservables();
        OnTriggerStay2DAsObservables();
    }

    private void FixedUpdateAsObservables()
    {
        this.FixedUpdateAsObservable()
            .SelectMany(x => Key.Horizontal)
            .Where(x => (x > 0 & !(PlayerState.FacingRight.Value)) | (x < 0 & PlayerState.FacingRight.Value))
            .Subscribe(_ => PlayerMotion.Turn());

        this.FixedUpdateAsObservable()
            .Zip(Key.Vertical, (a, b) => b)
            .Where(x => x == 1) 
            .Where(x => PlayerState.IsGrounded.Value)
            .Where(x => !PlayerState.IsClimbable.Value)
            .Subscribe(_ => PlayerMotion.Jump(PlayerConfig.JumpForce));

        this.FixedUpdateAsObservable()
            .Zip(Key.Vertical, (a, b) => b)
            .Where(x => PlayerState.IsClimbable.Value)
            .Where(x => x == 1)
            .Subscribe(_ => 
            {
                Rigidbody2D.isKinematic = true;
                PlayerMotion.Climb(_, PlayerConfig.MaxSpeed);
            });

        this.FixedUpdateAsObservable()
            .Zip(Key.Vertical, (a, b) => b)
            .Where(x => PlayerState.IsClimbable.Value)
            .Where(x => !PlayerState.IsGrounded.Value)
            .Zip(Key.Vertical, (a, b) => b)
            .Where(x => x == -1)
            .Subscribe(_ => 
            {
                Rigidbody2D.isKinematic = true;
                PlayerMotion.Climb(_, PlayerConfig.MaxSpeed);
            });

        this.FixedUpdateAsObservable()
            .Zip(PlayerState.IsGrounded, (a, b) => b)
            .Where(x => x)
            .Subscribe(_ => Rigidbody2D.isKinematic = false);

        this.FixedUpdateAsObservable()
            .Zip(Key.Vertical, (a, b) => b)
            .Where(x => PlayerState.IsClimbable.Value)
            .Where(x => x != 1 & x != -1)
            .Subscribe(_ => Rigidbody2D.velocity = Vector2.zero);

        this.FixedUpdateAsObservable()
            .Where(x => Animator.GetBool("IsRunning"))
            .Subscribe(_ => PlayerMotion.Run(Key.Horizontal.Value, PlayerConfig.MaxSpeed));

        this.FixedUpdateAsObservable()
            .Where(x => !Animator.GetBool("IsRunning"))
            .Subscribe(_ => Rigidbody2D.velocity = new Vector2(0, Rigidbody2D.velocity.y));
    }

    private void OnTriggerStay2DAsObservables()
    {
        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Ladder")
            .Subscribe(_ =>
            {
                PlayerState.IsClimbable.Value = true;
            });

        this.OnTriggerExit2DAsObservable()
            .Where(x => x.gameObject.tag == "Ladder")
            .Subscribe(_ => 
            {
                Rigidbody2D.isKinematic = false;
                PlayerState.IsClimbable.Value = false;
            });
    }
}