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
    }

    private void FixedUpdateAsObservables()
    {
        Key.Horizontal
            .Where(x => (x > 0 & !(PlayerState.FacingRight.Value)) | (x < 0 & PlayerState.FacingRight.Value))
            .Subscribe(_ => PlayerMotion.Turn());

        this.FixedUpdateAsObservable()
            .Where(x => Animator.GetBool("IsRunning"))
            .Subscribe(_ => PlayerMotion.Run(Key.Horizontal.Value, PlayerConfig.MaxSpeed));

        this.FixedUpdateAsObservable()
            .Where(x => !Animator.GetBool("IsRunning"))
            .Subscribe(_ => Rigidbody2D.velocity = new Vector2(0, Rigidbody2D.velocity.y));

        this.FixedUpdateAsObservable()
            .Where(x => Key.Vertical == 1)
            .Where(x => PlayerState.IsGrounded.Value)
            .Subscribe(_ => PlayerMotion.Jump(PlayerConfig.JumpForce));
    }
}