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
    GameObject DamageObject;
    IDamage DamageComponent;
    
    void Awake()
    {
        Status = Actor.GetComponent<Status>();
        State = Actor.GetComponent<IState>();
        DamageComponent = DamageObject.GetComponent<IDamage>();
    }

    void Start()
    {
        
    }

    public void ApplyDamage(int damageValue, int recovery)
    {
        if(DamageComponent != null)
        {
            StartCoroutine(DamageComponent.Damage(damageValue, recovery));
        }
    }
}