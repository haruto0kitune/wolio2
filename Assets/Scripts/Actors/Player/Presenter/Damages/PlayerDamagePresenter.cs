using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerPresenter : MonoBehaviour
{
    void DamagePresenter() 
    {
        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "FallingSplinter")
            .Subscribe(_ => Destroy(this.gameObject))
            .AddTo(this.gameObject);

        this.OnCollisionEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Splinter")
            .Subscribe(_ => Destroy(this.gameObject))
            .AddTo(this.gameObject);

        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Fireball")
            .Subscribe(_ => Destroy(this.gameObject))
            .AddTo(this.gameObject);
    }
}
