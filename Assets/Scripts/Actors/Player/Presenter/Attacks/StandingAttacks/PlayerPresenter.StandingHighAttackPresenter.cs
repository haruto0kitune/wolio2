using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerPresenter : MonoBehaviour
{
    void StandingHighAttackPresenter()
    {
        //PlayerState.IsStandingHighAttack
        this.FixedUpdateAsObservable()
            .Zip(PlayerState.IsStandingHighAttack, (a, b) => b)
            .Where(x => x)
            .Subscribe(_ => StartCoroutine(PlayerMotion.StandingHighAttack()));
    }
}
