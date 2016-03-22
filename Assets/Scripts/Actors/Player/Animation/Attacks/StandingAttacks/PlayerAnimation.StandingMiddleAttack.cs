using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerAnimation : MonoBehaviour
{
    public void StandingMiddleAttack()
    {
        ObservableStateMachineTrigger
             .OnStateUpdateAsObservable()
             .Where(x => x.StateInfo.IsName("Base Layer.StandingMiddleAttack"))
             .Where(x => x.StateInfo.normalizedTime >= 1)
             .Subscribe(_ => Animator.SetBool("IsStandingMiddleAttack", false));
    }
}
