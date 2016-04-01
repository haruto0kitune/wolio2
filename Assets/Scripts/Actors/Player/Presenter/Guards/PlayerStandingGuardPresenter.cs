using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerPresenter : MonoBehaviour
{
    void StandingGuardPresenter()
    {
        PlayerState.IsStandingGuard
            .Where(x => x)
            .Subscribe(_ => StartCoroutine(PlayerMotion.StandingGuard()));
    }
}
