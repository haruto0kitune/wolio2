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

    void Awake()
    {
        Animator = GetComponent<Animator>();
        ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
        Key = GetComponent<Key>();
        PlayerState = GetComponent<PlayerState>();
    }

    void Start()
    {
        Stand();
        Run();
        Jump();
        Crouch();
        Creep();
    }
}
