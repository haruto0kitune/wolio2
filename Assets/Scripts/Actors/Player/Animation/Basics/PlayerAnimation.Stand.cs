using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerAnimation : MonoBehaviour
{
    public void Stand()
    {
        ObservableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
            .Where(x => Key.Horizontal.Value != 0 && Key.Vertical.Value == 0)
            .Do(x => Animator.SetBool("IsStanding", false))
            .Subscribe(_ => Animator.SetBool("IsRunning", true));

        ObservableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
            .SelectMany(x => Key.Vertical)
            .Where(x => x == 1)
            .Where(x => !PlayerState.IsJumping.Value)
            .Do(x => Animator.SetBool("IsStanding", false))
            .Subscribe(_ => Animator.SetBool("IsJumping", true));

        ObservableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
            .Where(x => Key.Vertical.Value == -1)
            .Where(x => !PlayerState.IsJumping.Value)
            .Do(x => Animator.SetBool("IsStanding", false))
            .Subscribe(_ => Animator.SetBool("IsCrouching", true));

        ObservableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
            .Where(x => Key.Z)
            .Do(x => Animator.SetBool("IsStanding", false))
            .Subscribe(_ => Animator.SetBool("IsStandingLightAttack", true));

        ObservableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
            .Where(x => Key.X)
            .Do(x => Animator.SetBool("IsStanding", false))
            .Subscribe(_ => Animator.SetBool("IsStandingMiddleAttack", true));

        ObservableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
            .Where(x => Key.C)
            .Do(x => Animator.SetBool("IsStanding", false))
            .Subscribe(_ => Animator.SetBool("IsStandingHighAttack", true));

        ObservableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
            .Where(x => Key.LeftShift)
            .Do(x => Animator.SetBool("IsStanding", false))
            .Subscribe(_ => Animator.SetBool("IsStandingGuard", true));
    }
}
