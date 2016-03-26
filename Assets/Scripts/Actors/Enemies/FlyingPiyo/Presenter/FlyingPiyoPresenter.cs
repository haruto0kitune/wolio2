using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class FlyingPiyoPresenter : MonoBehaviour
{
    FlyingPiyoState FlyingPiyoState;
    FlyingPiyoConfig FlyingPiyoConfig;
    FlyingPiyoMotion FlyingPiyoMotion;

    void Awake()
    {
        FlyingPiyoState = GetComponent<FlyingPiyoState>();
        FlyingPiyoConfig = GetComponent<FlyingPiyoConfig>();
        FlyingPiyoMotion = GetComponent<FlyingPiyoMotion>();
    }

    void Start()
    {
        DamagePresenter();
    }
}