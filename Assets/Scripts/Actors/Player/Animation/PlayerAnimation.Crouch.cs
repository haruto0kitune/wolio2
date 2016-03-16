using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerAnimation : MonoBehaviour
{
    void Crouch()
    {
        ObservableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Crouch"))
            .SelectMany(x => Key.Vertical)
            .Where(x => x == 0)
            .Where(x => Physics2D.OverlapCircle(PlayerState.CeilingCheck.position, 0.1f, PlayerConfig.WhatIsGround) == null)
            .Subscribe(_ => Animator.SetBool("IsCrouching", false));

        ObservableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Crouch"))
            .Where(x => Key.Horizontal.Value != 0 && Key.Vertical.Value == -1)
            .Subscribe(_ => Animator.SetBool("IsCreeping", true));
    }
}
