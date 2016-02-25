using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class FragileBlock : MonoBehaviour
{
    Animator Animator;

    void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    void Start()
    {
        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Player")
            .DelayFrame(60)
            .Do(x => Animator.SetBool("Touched", true))
            .Do(x =>
            {
                var Colliders = GetComponents<BoxCollider2D>();
                foreach(var Col in Colliders)
                {
                    Destroy(Col);
                }
            })
            .DelayFrame(24)
            .Subscribe(_ => Destroy(gameObject))
            .AddTo(gameObject);
    }
}
