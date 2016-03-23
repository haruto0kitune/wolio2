using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerAnimation : MonoBehaviour
{
    void JumpingLightAttack()
    {
        ObservableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.JumpingLightAttack"))
            .Where(x => x.StateInfo.normalizedTime >= 1)
            .Subscribe(_ => Animator.SetBool("IsJumpingLightAttack", false));
    }
}
