using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

[RequireComponent(typeof(PlayerState))]
[RequireComponent(typeof(PlayerConfig))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Key))]
public class PlayerPresenter : MonoBehaviour
{
    PlayerState PlayerState;
    PlayerConfig PlayerConfig;
    PlayerMovement PlayerMovement;
    Key Key;

    void Awake()
    {
        PlayerState = GetComponent<PlayerState>();
        PlayerConfig = GetComponent<PlayerConfig>();
        PlayerMovement = GetComponent<PlayerMovement>();
        Key = GetComponent<Key>();
    }

    private void Start()
    {
        FixedUpdateAsObservables();
    }

    private void FixedUpdateAsObservables()
    {
        Key.Horizontal
            .Where(x => x != 0)
            .Subscribe(_ => GetComponent<Animator>().SetBool("IsRunning", true));

        Key.Horizontal
            .Where(x => x == 0)
            .Subscribe(_ => GetComponent<Animator>().SetBool("IsRunning", false));

        this.FixedUpdateAsObservable()
            .Where(x => GetComponent<Animator>().GetBool("IsRunning"))
            .Subscribe(_ => PlayerMovement.Run(Key.Horizontal.Value, PlayerConfig.MaxSpeed));

        this.FixedUpdateAsObservable()
            .Where(x => Key.Vertical == 1)
            .Subscribe(_ => PlayerMovement.Jump(PlayerConfig.JumpForce, PlayerConfig.WhatIsGround));
    }
}