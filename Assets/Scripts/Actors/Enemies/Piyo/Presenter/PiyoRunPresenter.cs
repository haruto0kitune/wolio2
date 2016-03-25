using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PiyoPresenter : MonoBehaviour
{
    void RunPresenter()
    {
        this.FixedUpdateAsObservable()
            .Do(x => Debug.Log("PiyoState.Direction: "+PiyoState.Direction.Value))
            .Subscribe(_ => PiyoMotion.Run(PiyoConfig.Speed, PiyoState.Direction.Value));
    }
}
