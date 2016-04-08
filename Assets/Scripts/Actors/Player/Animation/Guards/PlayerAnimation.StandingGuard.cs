using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerAnimation : MonoBehaviour
{
    public void StandingGuard()
    {
        ObservableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.StandingGuard"))
            .Where(x => !Key.LeftShift || !PlayerState.IsGrounded.Value)
            .Do(x => Animator.SetBool("IsStanding", true))
            .Subscribe(_ => Animator.SetBool("IsStandingGuard", false));

        ObservableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.StandingGuard"))
            .Where(x => Key.LeftShift && (Key.Vertical.Value == -1f))
            .Subscribe(_ =>
            {
                Animator.SetBool("IsStandingGuard", false);
                Animator.SetBool("IsCrouchingGuard", true);
            });
    }
}
