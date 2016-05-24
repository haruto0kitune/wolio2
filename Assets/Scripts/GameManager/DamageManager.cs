using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class DamageManager : MonoBehaviour
{
    [SerializeField]
    GameObject Actor;
    Status Status;
    IState State;
    [SerializeField]
    GameObject StandingDamage;
    IDamage StandingDamageComponent;
    [SerializeField]
    GameObject CrouchingDamage;
    IDamage CrouchingDamageComponent;
    [SerializeField]
    GameObject JumpingDamage;
    IDamage JumpingDamageComponent;
    
    void Awake()
    {
        Status = Actor.GetComponent<Status>();
        State = Actor.GetComponent<IState>();
        StandingDamageComponent = StandingDamage.GetComponent<IDamage>();
        CrouchingDamageComponent = CrouchingDamage.GetComponent<IDamage>();
        JumpingDamageComponent = JumpingDamage.GetComponent<IDamage>();
    }

    void Start()
    {
        
    }

    public void ApplyDamage(int damageValue, int recovery)
    {
        if(StandingDamageComponent != null)
        {
            StartCoroutine(StandingDamageComponent.Damage(damageValue, recovery));
        }
    }
}
