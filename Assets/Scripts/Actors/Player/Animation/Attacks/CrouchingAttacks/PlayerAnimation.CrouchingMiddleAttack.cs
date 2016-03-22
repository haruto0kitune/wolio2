using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerAnimation : MonoBehaviour
{
    void CrouchingMiddleAttack()
    {
        ObservableStateMachineTrigger
            .OnStateEnterAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.CrouchingMiddleAttack"))
            .Subscribe(_ => Animator.SetBool("IsCrouching", false));

        ObservableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.CrouchingMiddleAttack"))
            .Where(x => x.StateInfo.normalizedTime >= 1)
            .Subscribe(_ => Animator.SetBool("IsCrouchingMiddleAttack", false));

        ObservableStateMachineTrigger
            .OnStateExitAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.CrouchingMiddleAttack"))
            .Subscribe(_ => Animator.SetBool("IsCrouching", true));
    }
}
