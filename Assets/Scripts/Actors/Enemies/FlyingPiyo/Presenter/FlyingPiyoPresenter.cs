using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class FlyingPiyoPresenter : MonoBehaviour
{
    void Awake()
    {
    }

    void Start()
    {
        DamagePresenter();
    }
}