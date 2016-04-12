using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerPresenter : MonoBehaviour
{
    void RunPresenter()
    {
        this.FixedUpdateAsObservable()
            .Where(x => !PlayerState.IsCrouching.Value)
            .Where(x => !PlayerState.IsCreeping.Value)
            .Where(x => !PlayerState.IsJumping.Value)
            .Where(x => PlayerState.IsGrounded.Value)
            .Where(x => Key.Horizontal.Value != 0)
            .Subscribe(_ => PlayerMotion.Run(Key.Horizontal.Value, PlayerConfig.MaxSpeed));

        Key.Horizontal
            .Where(x => x == 0)
            .Subscribe(_ => PlayerMotion.ExitRun()); 
    }
}
