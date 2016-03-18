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
    }
}
