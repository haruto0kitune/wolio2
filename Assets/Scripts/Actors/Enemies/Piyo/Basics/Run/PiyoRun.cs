using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using Wolio.Actor.Player;

public class PiyoRun : MonoBehaviour
{
    [SerializeField]
    GameObject Piyo;
    Rigidbody2D PiyoRigidbody2D;
    PiyoState PiyoState;
    [SerializeField]
    GameObject RunHitBox;
    [SerializeField]
    GameObject RunHurtBox;
    BoxCollider2D BoxCollider2D;
    BoxCollider2D HitBox;
    BoxCollider2D HurtBox;
    [SerializeField]
    float Speed;

    void Awake()
    {
        PiyoRigidbody2D = Piyo.GetComponent<Rigidbody2D>();
        PiyoState = Piyo.GetComponent<PiyoState>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
        HitBox = RunHitBox.GetComponent<BoxCollider2D>();
        HurtBox = RunHurtBox.GetComponent<BoxCollider2D>();
    }
    void Start()
    {
        //Motion
        this.FixedUpdateAsObservable()
            .Subscribe(_ => Run(Speed, PiyoState.Direction.Value));

        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "HurtBox" && x.gameObject.layer == LayerMask.NameToLayer("Player/HurtBox"))
            .Subscribe(_ => GameObject.Find("Test").GetComponent<PlayerState>().Hp.Value--);
    }

    public void Run(float speed, float direction)
    {
        PiyoRigidbody2D.velocity = new Vector2(speed * direction, PiyoRigidbody2D.velocity.y);
    }
}