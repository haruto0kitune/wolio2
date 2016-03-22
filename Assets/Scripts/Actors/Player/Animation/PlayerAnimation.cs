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
        Stand();
        Run();
        Jump();
        Crouch();
        Creep();
        CrouchingLightAttack();
        CrouchingMiddleAttack();
        CrouchingHighAttack();
        StandingLightAttack();
        StandingMiddleAttack();
        StandingHighAttack();
    }
}
