using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerPresenter : MonoBehaviour
{
    void StandingGuardBackPresenter()
    {
        this.OnTriggerEnter2DAsObservable()
            .Where(x => PlayerState.IsStandingGuard.Value)
            .Where(x => !PlayerState.IsStandingGuardBack.Value)
            .Where(x => x.gameObject.tag == "AttackLevel/1")
            .Subscribe(_ => StartCoroutine(PlayerMotion.StandingGuardBack()));
    }
}
