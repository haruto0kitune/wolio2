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
            .SelectMany(x => Key.Horizontal)
            .Where(x => x != 0)
            .Subscribe(_ => Animator.SetBool("IsRunning", true));
    }
}
