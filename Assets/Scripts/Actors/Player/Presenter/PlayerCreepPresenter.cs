using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerPresenter : MonoBehaviour
{
    void CreepPresenter()
    {
        this.FixedUpdateAsObservable()
            .Where(x => PlayerState.IsCrouching.Value)
            .Where(x => !PlayerState.IsRunning.Value)
            .Where(x => Key.Horizontal.Value != 0 && Key.Vertical.Value == -1)
            .Subscribe(_ => PlayerMotion.Creep(Key.Horizontal.Value, PlayerConfig.CreepSpeed));

        this.FixedUpdateAsObservable()
            .Where(x => PlayerState.IsCreeping.Value)
            .Where(x => Key.Horizontal.Value == 0)
            .Subscribe(_ => PlayerMotion.ExitCreep());
    }
}
