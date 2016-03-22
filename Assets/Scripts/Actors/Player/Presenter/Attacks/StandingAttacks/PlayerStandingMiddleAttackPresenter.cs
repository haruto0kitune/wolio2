﻿using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerPresenter : MonoBehaviour
{
    void StandingMiddleAttackPresenter()
    {
        PlayerState.IsStandingMiddleAttack
            .Where(x => x)
            .Subscribe(_ => StartCoroutine(PlayerMotion.StandingMiddleAttack()));
    }
}
