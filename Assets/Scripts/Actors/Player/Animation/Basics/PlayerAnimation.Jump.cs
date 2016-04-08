using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerAnimation : MonoBehaviour
{
    void Jump()
    {
        ObservableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Jump"))
            .SelectMany(x => PlayerState.IsGrounded)
            .Where(x => x)
            .Do(x => Animator.SetBool("IsStanding", true))
            .Subscribe(_ => Animator.SetBool("IsJumping", false));

        ObservableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Jump"))
            .Where(x => Key.Z)
            .Subscribe(_ => Animator.SetBool("IsJumpingLightAttack", true));

        ObservableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Jump"))
            .Where(x => Key.X)
            .Subscribe(_ => Animator.SetBool("IsJumpingMiddleAttack", true));

        ObservableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Jump"))
            .Where(x => Key.C)
            .Subscribe(_ => Animator.SetBool("IsJumpingHighAttack", true));

        ObservableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Jump"))
            .SelectMany(x => PlayerState.IsGrounded)
            .Where(x => !x)
            .Where(x => Key.LeftShift)
            .Subscribe(_ => Animator.SetBool("IsJumpingGuard", true));
    }
}
