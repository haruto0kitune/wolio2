using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerPresenter : MonoBehaviour
{
    void AirMovePresenter()
    {
        this.FixedUpdateAsObservable()
            .Where(x => PlayerState.IsJumping.Value)
            .Where(x => !PlayerState.IsGrounded.Value)
            .Where(x => Key.Horizontal.Value != 0)
            .Subscribe(_ => PlayerMotion.AirMove(Key.Horizontal.Value));

        this.FixedUpdateAsObservable()
            .Where(x => !PlayerState.IsJumping.Value)
            .Where(x => PlayerState.IsGrounded.Value)
            .Subscribe(_ => Rigidbody2D.AddForce(Vector2.zero));
    }
}
