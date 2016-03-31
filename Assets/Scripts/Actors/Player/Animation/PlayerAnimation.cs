using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerAnimation : MonoBehaviour
{
    private Animator Animator;
    private ObservableStateMachineTrigger ObservableStateMachineTrigger;
    private Key Key;
    private PlayerState PlayerState;
    private PlayerConfig PlayerConfig;

    void Awake()
    {
        Animator = GetComponent<Animator>();
        ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
        Key = GetComponent<Key>();
        PlayerState = GetComponent<PlayerState>();
        PlayerConfig = GetComponent<PlayerConfig>();
    }

    void Start()
    {
        Basics();
        StandingAttacks();
        CrouchingAttacks();
        JumpingAttacks();
        Guards();
    }

    void Basics()
    {
        Stand();
        Run();
        Jump();
        Crouch();
        Creep();
    }

    void StandingAttacks()
    {
        StandingLightAttack();
        StandingMiddleAttack();
        StandingHighAttack();
    }

    void CrouchingAttacks()
    {
        CrouchingLightAttack();
        CrouchingMiddleAttack();
        CrouchingHighAttack();
    }

    void JumpingAttacks()
    {
        JumpingLightAttack();
        JumpingMiddleAttack();
        JumpingHighAttack();
    }

    void Guards()
    {
        StandingGuard();
        CrouchingGuard();
        JumpingGuard();
    }
}
