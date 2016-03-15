using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

[RequireComponent(typeof(Key))]
[RequireComponent(typeof(PlayerState))]
[RequireComponent(typeof(PlayerConfig))]
[RequireComponent(typeof(PlayerMotion))]
public partial class PlayerPresenter : MonoBehaviour
{
    PlayerState PlayerState;
    PlayerConfig PlayerConfig;
    PlayerMotion PlayerMotion;
    Key Key;
    Animator Animator;
    Rigidbody2D Rigidbody2D;
    SpriteRenderer SpriteRenderer;

    void Awake()
    {
        PlayerState = GetComponent<PlayerState>();
        PlayerConfig = GetComponent<PlayerConfig>();
        PlayerMotion = GetComponent<PlayerMotion>();
        Key = GetComponent<Key>();
        Animator = GetComponent<Animator>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        TurnPresenter();
        RunPresenter();
        JumpPresenter();
        ClimbPresenter();
        DamagePresenter();
        CrouchPresenter();
        CreepPresenter();
    }
}