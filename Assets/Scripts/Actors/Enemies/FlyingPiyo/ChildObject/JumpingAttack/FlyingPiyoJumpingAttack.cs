using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class FlyingPiyoJumpingAttack : MonoBehaviour
{
    void Start()
    {
        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Player")
            .Subscribe(x => this.gameObject.SetActive(false));

        gameObject.SetActive(false);
    }
}