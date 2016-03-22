using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerAnimation : MonoBehaviour
{
    void Creep()
    {
        ObservableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Creep"))
            .Subscribe(_ => Animator.SetBool("IsCrouching", false));

        ObservableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Creep"))
            .Where(x => Key.Horizontal.Value == 0 || Key.Vertical.Value == 0) 
            .Subscribe(_ => Animator.SetBool("IsCreeping", false));

        ObservableStateMachineTrigger
            .OnStateExitAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Creep"))
            .Subscribe(_ => Animator.SetBool("IsCrouching", true));

    }
}
