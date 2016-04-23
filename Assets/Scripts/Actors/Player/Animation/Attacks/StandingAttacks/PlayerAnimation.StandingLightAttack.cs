using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerAnimation : MonoBehaviour
{
    public void StandingLightAttack()
    {
        ObservableStateMachineTrigger
             .OnStateUpdateAsObservable()
             .Where(x => x.StateInfo.IsName("Base Layer.StandingLightAttack"))
             .Where(x => x.StateInfo.normalizedTime >= 1)
             .Do(x => Animator.SetBool("IsStanding", true))
             .Subscribe(_ =>
             {
                 Animator.SetBool("IsStandingLightAttack", false);
             });
    }
}