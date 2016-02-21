using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

[RequireComponent(typeof(PlayerState))]
[RequireComponent(typeof(PlayerConfig))]
[RequireComponent(typeof(PlayerMotion))]
[RequireComponent(typeof(Key))]
public class PlayerPresenter : MonoBehaviour
{
    PlayerState PlayerState;
    PlayerConfig PlayerConfig;
    PlayerMotion PlayerMotion;
    Key Key;

    void Awake()
    {
        PlayerState = GetComponent<PlayerState>();
        PlayerConfig = GetComponent<PlayerConfig>();
        PlayerMotion = GetComponent<PlayerMotion>();
        Key = GetComponent<Key>();
    }

    private void Start()
    {
        FixedUpdateAsObservables();
    }

    private void FixedUpdateAsObservables()
    {
        this.FixedUpdateAsObservable()
            .Where(x => GetComponent<Animator>().GetBool("IsRunning"))
            .Subscribe(_ => PlayerMotion.Run(Key.Horizontal.Value, PlayerConfig.MaxSpeed));

        this.FixedUpdateAsObservable()
            .Where(x => !GetComponent<Animator>().GetBool("IsRunning"))
            .Subscribe(_ => GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y));

        this.FixedUpdateAsObservable()
            .Where(x => Key.Vertical == 1)
            .Subscribe(_ => PlayerMotion.Jump(PlayerConfig.JumpForce, PlayerConfig.WhatIsGround));
    }
}