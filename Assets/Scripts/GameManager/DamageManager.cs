using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class DamageManager : MonoBehaviour
{
    [SerializeField]
    GameObject Actor;
    IState State;
    [SerializeField]
    GameObject DamageObject;
    IDamage DamageComponent;
    
    void Awake()
    {
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
            DamageComponent.Damage(damageValue, recovery);
        }
        else
        {
            Debug.Log("DamageComponent is null");
        }
    }
}