using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerPresenter : MonoBehaviour
{
    void JumpingLightAttackPresenter()
    {
        PlayerState.IsJumpingLightAttack
            .Where(x => x)
            .Subscribe(_ => StartCoroutine(PlayerMotion.JumpingLightAttack()));
    }
}
