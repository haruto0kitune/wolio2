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
            .SelectMany(x => Key.Horizontal)
            .Where(x => x == 0)
            .Subscribe(_ => Animator.SetBool("IsRunning", false));
    }
}
