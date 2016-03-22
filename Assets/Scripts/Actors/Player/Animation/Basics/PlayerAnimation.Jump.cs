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
            .Subscribe(_ => Animator.SetBool("IsJumping", false));        
    }
}
