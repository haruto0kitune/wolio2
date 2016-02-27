using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class Trampoline : MonoBehaviour
{
    [SerializeField]
    GameObject Player;
    [SerializeField]
    Vector2 velocity;

    BoxCollider2D BoxCollider2D;
    Animator Animator;
    ObservableStateMachineTrigger ObservableStateMachineTrigger;
    Rigidbody2D _Rigidbody2D;

    void Awake()
    {
        Player = GameObject.Find("Player");
        BoxCollider2D = GetComponent<BoxCollider2D>();
        Animator = GetComponent<Animator>();
        ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
        _Rigidbody2D = Player.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        //ObservableStateMachineTrigger
        //    .OnStateEnterAsObservable()
        //    .Where(x => x.StateInfo.IsName("Base Layer.Idle"))
        //    .Zip(this.OnTriggerEnter2DAsObservable().Where(x => x.gameObject.tag == "Player"), (a, b) => b)
        //    .DelayFrame(5)
        //    .Subscribe(_ =>
        //    {
        //        Animator.SetBool("IsRiding", true);
        //        jvalue\
        //    });

        //ObservableStateMachineTrigger
        //    .OnStateUpdateAsObservable()
        //    .Where(x => x.StateInfo.IsName("Base Layer.Trampoline"));
        
        this.UpdateAsObservable()
            .Subscribe(_ => Debug.Log(_Rigidbody2D.velocity));
        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Player")
            .DelayFrame(1)
            .Subscribe(_ => 
            {
                _Rigidbody2D.AddForce(new Vector2(0, 600));
                Animator.SetBool("IsRiding", true);
            });

        ObservableStateMachineTrigger
            .OnStateExitAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Trampoline"))
            .Subscribe(_ => Animator.SetBool("IsRiding", false));
    }
}
