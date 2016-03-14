using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class FallingSplinter : MonoBehaviour
{
    void Start()
    {
        this.OnBecameInvisibleAsObservable()
            .Subscribe(_ => Destroy(this.gameObject));
    }
}
