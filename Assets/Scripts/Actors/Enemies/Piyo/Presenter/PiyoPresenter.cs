using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PiyoPresenter : MonoBehaviour
{
    PiyoState PiyoState;
    PiyoConfig PiyoConfig;
    PiyoMotion PiyoMotion;

    void Awake()
    {
        PiyoState = GetComponent<PiyoState>();
        PiyoConfig = GetComponent<PiyoConfig>();
        PiyoMotion = GetComponent<PiyoMotion>();
    }

    void Start()
    {
        RunPresenter();
    }
}