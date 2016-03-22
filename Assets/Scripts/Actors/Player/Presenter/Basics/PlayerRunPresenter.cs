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
            .Where(x => Key.Horizontal.Value != 0)
            .Subscribe(_ => PlayerMotion.Run(Key.Horizontal.Value, PlayerConfig.MaxSpeed));

        this.FixedUpdateAsObservable()
            .Where(x => Key.Horizontal.Value == 0)
            .Subscribe(_ => Rigidbody2D.velocity = new Vector2(0, Rigidbody2D.velocity.y));

        this.UpdateAsObservable()
            .Where(x => PlayerState.IsRunning.Value)
            .Where(x => Key.Horizontal.Value == 0)
            .Subscribe(_ => PlayerMotion.ExitRun());
    }
}
