using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerAnimation : MonoBehaviour
{
    public void CrouchingGuard()
    {
        ObservableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.CrouchingGuard"))
            .Where(x => !Key.LeftShift)
            .Subscribe(_ => Animator.SetBool("IsCrouchingGuard", false));

        ObservableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.CrouchingGuard"))
            .Where(x => Key.LeftShift && (Key.Vertical.Value == 0f))
            .Subscribe(_ => 
            {
                Animator.SetBool("IsCrouchingGuard", false);
                Animator.SetBool("IsStandingGuard", true);
            });
    }
}