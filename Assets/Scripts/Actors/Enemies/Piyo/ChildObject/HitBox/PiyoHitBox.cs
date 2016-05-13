using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using Wolio.Actor.Player;

public class PiyoHitBox : MonoBehaviour
{
    GameObject Piyo;
    PlayerState PlayerState;

    void Awake()
    {
        Piyo = GameObject.Find("Piyo");
        PlayerState = GameObject.Find("Test").GetComponent<PlayerState>();
    }

    void Start()
    {
        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "HurtBox" && x.gameObject.layer == LayerMask.NameToLayer("Player/HurtBox"))
            .Subscribe(_ => PlayerState.Hp.Value--);
    }
}