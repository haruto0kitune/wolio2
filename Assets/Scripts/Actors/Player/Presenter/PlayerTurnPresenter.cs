using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerPresenter : MonoBehaviour
{
    void TurnPresenter()
    {
        this.FixedUpdateAsObservable()
            .SelectMany(x => Key.Horizontal)
            .Where(x => (x > 0 & !(PlayerState.FacingRight.Value)) | (x < 0 & PlayerState.FacingRight.Value))
            .Subscribe(_ => PlayerMotion.Turn());
    }
}
