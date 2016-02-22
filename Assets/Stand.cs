using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class Stand : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Key>()
            .Horizontal
            .Where(x => x != 0)
            .Subscribe(_ => animator.SetBool("IsRunning", true));
    }
}
