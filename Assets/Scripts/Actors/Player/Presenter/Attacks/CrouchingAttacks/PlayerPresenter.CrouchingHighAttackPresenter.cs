using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerPresenter : MonoBehaviour
{
    void CrouchingHighAttackPresenter()
    {
        PlayerState.IsCrouchingHighAttack
            .Where(x => x)
            .Subscribe(_ => StartCoroutine(PlayerMotion.CrouchingHighAttack()));
    }
}
