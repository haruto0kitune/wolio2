using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class Trampoline : MonoBehaviour
{
    [SerializeField]
    Vector2 velocity;

    bool onTop;
    Animator Animator;
    Rigidbody2D _Rigidbody2D;

    void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    void Start()
    {
        this.OnCollisionStay2DAsObservable()
            .Where(x => onTop)
            .Subscribe(_ => 
            {
                Animator.SetBool("IsRiding", true);
                _Rigidbody2D = _.gameObject.GetComponent<Rigidbody2D>();
            });

        this.OnTriggerEnter2DAsObservable()
            .Subscribe(_ => onTop = true);

        this.OnTriggerExit2DAsObservable()
            .Subscribe(_ => 
            {
                onTop = false;
                Animator.SetBool("IsRiding", false);
            });
        
    }

    void Jump()
    {
        _Rigidbody2D.velocity = velocity;
    }
}
