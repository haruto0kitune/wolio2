using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class EnemyPresenter : MonoBehaviour
{
    void DamagePresenter()
    {
        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "CrouchingLightAttack")
            .Subscribe(_ => Destroy(this.gameObject));

        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "CrouchingMiddleAttack")
            .Subscribe(_ => Destroy(this.gameObject));

        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "CrouchingHighAttack")
            .Subscribe(_ => Destroy(this.gameObject));

        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "StandingLightAttack")
            .Subscribe(_ => Destroy(this.gameObject));

        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "StandingMiddleAttack")
            .Subscribe(_ => Destroy(this.gameObject));

        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "StandingHighAttack")
            .Subscribe(_ => Destroy(this.gameObject));

        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "JumpingLightAttack")
            .Subscribe(_ => Destroy(this.gameObject));

        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "JumpingMiddleAttack")
            .Subscribe(_ => Destroy(this.gameObject));

        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "JumpingHighAttack")
            .Subscribe(_ => Destroy(this.gameObject));
    }
}
