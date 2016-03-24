using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PiyoPresenter : MonoBehaviour
{
    void RunPresenter()
    {
        this.FixedUpdateAsObservable()
            .Subscribe(_ => PiyoMotion.Run);
    }
}
