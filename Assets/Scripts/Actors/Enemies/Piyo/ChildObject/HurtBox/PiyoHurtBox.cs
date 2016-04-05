using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class PiyoHurtBox : MonoBehaviour
{
    void Start()
    {
        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.layer == LayerMask.NameToLayer("Player/HitBox"))
            .Subscribe(_ => Destroy(GameObject.Find("Piyo")));
    }
}
