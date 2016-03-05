using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerPresenter : MonoBehaviour
{
    void RunPresenter()
    {
        this.FixedUpdateAsObservable()
            .Where(x => Animator.GetBool("IsRunning"))
            .Subscribe(_ => PlayerMotion.Run(Key.Horizontal.Value, PlayerConfig.MaxSpeed));

        this.FixedUpdateAsObservable()
            .Where(x => !Animator.GetBool("IsRunning"))
            .Subscribe(_ => Rigidbody2D.velocity = new Vector2(0, Rigidbody2D.velocity.y));
    }
}
