
using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerAnimation : MonoBehaviour
{
    void JumpingMiddleAttack()
    {
        ObservableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.JumpingMiddleAttack"))
            .Where(x => x.StateInfo.normalizedTime >= 1)
            .Subscribe(_ => Animator.SetBool("IsJumpingMiddleAttack", false));
    }
}
