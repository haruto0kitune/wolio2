﻿using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Basics
{
    public class PlayerCrouch : MonoBehaviour
    {
        PlayerState PlayerState;
        BoxCollider2D BoxCollider2D;

        void Awake()
        {
            PlayerState = GameObject.Find("Test").GetComponent<PlayerState>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            PlayerState.IsCrouching
                .Where(x => x)
                .Subscribe(_ => BoxCollider2D.enabled = true);

            PlayerState.IsCrouching
                .Where(x => !x)
                .Subscribe(_ => BoxCollider2D.enabled = false);
        }
    }
}