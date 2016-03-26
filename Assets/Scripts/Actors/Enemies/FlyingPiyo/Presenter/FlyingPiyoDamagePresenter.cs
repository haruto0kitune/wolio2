using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class FlyingPiyoPresenter : MonoBehaviour
{
    void DamagePresenter()
    {
        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Attacks/CrouchingAttacks/CrouchingLightAttack")
            .Subscribe(_ => Destroy(this.gameObject));

        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Attacks/CrouchingAttacks/CrouchingMiddleAttack")
            .Subscribe(_ => Destroy(this.gameObject));

        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Attacks/CrouchingAttacks/CrouchingHighAttack")
            .Subscribe(_ => Destroy(this.gameObject));

        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Attacks/StandingAttacks/StandingLightAttack")
            .Subscribe(_ => Destroy(this.gameObject));

        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Attacks/StandingAttacks/StandingMiddleAttack")
            .Subscribe(_ => Destroy(this.gameObject));

        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Attacks/StandingAttacks/StandingHighAttack")
            .Subscribe(_ => Destroy(this.gameObject));

        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Attacks/JumpingAttacks/JumpingLightAttack")
            .Subscribe(_ => Destroy(this.gameObject));

        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Attacks/JumpingAttacks/JumpingMiddleAttack")
            .Subscribe(_ => Destroy(this.gameObject));

        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Attacks/JumpingAttacks/JumpingHighAttack")
            .Subscribe(_ => Destroy(this.gameObject));
    }
}