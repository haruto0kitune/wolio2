using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerPresenter : MonoBehaviour
{
    void CrouchingLightAttackPresenter()
    {
        //this.ObserveEveryValueChanged(x => Animator.GetBool("IsCrouchingLightAttack"))
        PlayerState.IsCrouchingLightAttack
            .Where(x => x)
            .Subscribe(_ => StartCoroutine(PlayerMotion.CrouchingLightAttack()));
    }
}
