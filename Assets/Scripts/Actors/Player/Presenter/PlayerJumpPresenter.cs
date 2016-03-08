using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerPresenter : MonoBehaviour
{
    void JumpPresenter()
    {
        this.FixedUpdateAsObservable()
            .Zip(Key.Vertical, (a, b) => b)
            .Where(x => x == 1) 
            .Where(x => PlayerState.IsGrounded.Value)
            .Where(x => !PlayerState.IsClimbable.Value)
            .Where(x => !PlayerState.IsClimbing.Value)
            .Do(x => Debug.Log("Jump"))
            .Subscribe(_ => PlayerMotion.Jump(PlayerConfig.JumpForce));
    }
}
