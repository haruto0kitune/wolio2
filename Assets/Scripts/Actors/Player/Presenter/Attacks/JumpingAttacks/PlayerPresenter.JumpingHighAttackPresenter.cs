using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerPresenter : MonoBehaviour
{
    void JumpingHighAttackPresenter()
    {
        PlayerState.IsJumpingHighAttack
            .Where(x => x)
            .Subscribe(_ => StartCoroutine(PlayerMotion.JumpingHighAttack()));
    }
}
