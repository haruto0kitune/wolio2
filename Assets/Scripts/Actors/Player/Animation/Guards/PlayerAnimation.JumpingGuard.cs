using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerAnimation : MonoBehaviour
{
    public void JumpingGuard()
    {
        ObservableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.JumpingGuard"))
            .Where(x => !Key.LeftShift || PlayerState.IsGrounded.Value)
            .Subscribe(_ => Animator.SetBool("IsJumpingGuard", false));
    }
}