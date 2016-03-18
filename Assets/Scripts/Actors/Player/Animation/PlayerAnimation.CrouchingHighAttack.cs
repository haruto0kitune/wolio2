using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerAnimation : MonoBehaviour
{
    void CrouchingHighAttack()
    {
        ObservableStateMachineTrigger
            .OnStateEnterAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.CrouchingHighAttack"))
            .Subscribe(_ => Animator.SetBool("IsCrouching", false));

        ObservableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.CrouchingHighAttack"))
            .Where(x => x.StateInfo.normalizedTime >= 1)
            .Subscribe(_ => Animator.SetBool("IsCrouchingHighAttack", false));

        ObservableStateMachineTrigger
            .OnStateExitAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.CrouchingHighAttack"))
            .Subscribe(_ => Animator.SetBool("IsCrouching", true));
    }
}
