using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PiyoPresenter : MonoBehaviour
{
    PiyoState PiyoState;
    PiyoConfig PiyoConfig;
    PiyoMotion PiyoMotion;
    Animator Animator;
    Rigidbody2D Rigidbody2D;
    SpriteRenderer SpriteRenderer;

    void Awake()
    {
        PiyoState = GetComponent<PiyoState>();
        PiyoConfig = GetComponent<PiyoConfig>();
        PiyoMotion = GetComponent<PiyoMotion>();
        Animator = GetComponent<Animator>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        DamagePresenter();
        RunPresenter();
    }
}