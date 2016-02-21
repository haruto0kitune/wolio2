using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Collections;

public class Run : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Key>()
            .Horizontal
            .Where(x => x == 0)
            .Subscribe(_ => animator.SetBool("IsRunning", false));
    }
}
