using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerAnimation : MonoBehaviour
{
    public void Run()
    {
        ObservableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Run"))
            .Where(x => Key.Horizontal.Value == 0)
            .Subscribe(_ => Animator.SetBool("IsRunning", false));


        ObservableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Run"))
            .SelectMany(x => Key.Vertical)
            .Where(x => x == 1)
            .Subscribe(_ => Animator.SetBool("IsJumping", true));
    }
}