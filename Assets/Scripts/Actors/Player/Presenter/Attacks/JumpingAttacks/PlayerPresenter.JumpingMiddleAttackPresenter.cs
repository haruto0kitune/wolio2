using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerPresenter : MonoBehaviour
{
    void JumpingMiddleAttackPresenter()
    {
        PlayerState.IsJumpingMiddleAttack
            .Where(x => x)
            .Subscribe(_ => StartCoroutine(PlayerMotion.JumpingMiddleAttack()));
    }
}
