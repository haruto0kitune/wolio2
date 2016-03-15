using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerPresenter : MonoBehaviour
{
    void CrouchPresenter()
    {
        this.FixedUpdateAsObservable()
            .Where(x => !PlayerState.IsClimbable.Value)
            .Where(x => !PlayerState.IsClimbing.Value)
            .Where(x => PlayerState.IsGrounded.Value)
            .Where(x => Key.Vertical.Value == -1)
            .Subscribe(_ => PlayerMotion.Crouch());

        this.FixedUpdateAsObservable()
            .Where(x => Key.Vertical.Value == 0)
            .Where(x => !Physics2D.OverlapCircle(PlayerState.CeilingCheck.position, 0.01f))
            .Subscribe(_ => PlayerMotion.ExitCrouch());
    }
}
