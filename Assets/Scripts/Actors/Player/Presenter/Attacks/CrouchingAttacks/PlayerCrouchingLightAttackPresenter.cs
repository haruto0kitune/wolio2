﻿using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerPresenter : MonoBehaviour
{
    void CrouchingLightAttackPresenter()
    {
        PlayerState.IsCrouchingLightAttack
            .Where(x => x)
            .Subscribe(_ => StartCoroutine(PlayerMotion.CrouchingLightAttack()));
    }
}