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
            .Subscribe(_ => PlayerState.Hp.Value--);

        this.OnCollisionEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Splinter")
            .Subscribe(_ => PlayerState.Hp.Value--);
    }
}
