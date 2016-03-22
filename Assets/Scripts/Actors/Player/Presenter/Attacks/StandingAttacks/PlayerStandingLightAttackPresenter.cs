using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerPresenter : MonoBehaviour
{
    void StandingLightAttackPresenter()
    {
        PlayerState.IsStandingLightAttack
            .Where(x => x)
            .Subscribe(_ => StartCoroutine(PlayerMotion.StandingLightAttack()));
    }
}
