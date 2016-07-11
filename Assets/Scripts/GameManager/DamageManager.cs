using UnityEngine;
using System;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class DamageManager : MonoBehaviour
{
    public GameObject Actor;
    IState State;
    [SerializeField]
    GameObject DamageObject;
    IDamage DamageComponent;
    [SerializeField]
    GameObject SupineDamageObject;
    IDamage SupineDamageComponent;
    [SerializeField]
    GameObject ProneDamageObject;
    IDamage ProneDamageComponent;
    
    void Awake()
    {
        State = Actor.GetComponent<IState>();
        DamageComponent = DamageObject.GetComponent<IDamage>();

        try
        {
            SupineDamageComponent = SupineDamageObject.GetComponent<IDamage>();
            ProneDamageComponent = ProneDamageObject.GetComponent<IDamage>();
        }
        catch(UnassignedReferenceException ex)
        {
            SupineDamageComponent = null;
            ProneDamageComponent = null;
        }
    }

    void Start()
    {
        
    }

    public void ApplyDamage(int damageValue, int recovery, int hitStop, bool isTechable, bool hasKnockdownAttribute, AttackAttribute attackAttribute, KnockdownAttribute knockdownAttribute)
    {
        if(DamageComponent != null)
        {
            if (hasKnockdownAttribute)
            {
                if(knockdownAttribute == KnockdownAttribute.supineKnockdown)
                {
                    if (SupineDamageComponent != null) SupineDamageComponent.Damage(damageValue, recovery, hitStop, isTechable, hasKnockdownAttribute, attackAttribute, knockdownAttribute);
                }
                else if(knockdownAttribute == KnockdownAttribute.proneKnockdown)
                {
                    if (ProneDamageComponent != null) ProneDamageComponent.Damage(damageValue, recovery, hitStop, isTechable, hasKnockdownAttribute, attackAttribute, knockdownAttribute);
                }
            }
            else
            {
                DamageComponent.Damage(damageValue, recovery, hitStop, isTechable, hasKnockdownAttribute, attackAttribute, knockdownAttribute);
            }
        }
        else
        {
            Debug.Log("DamageComponent is null");
        }
    }
}