using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class study1 : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        var source = new Subject<int>();

        source.Subscribe(_ => Debug.Log("one:"+_));
        source.Subscribe(_ => Debug.Log("two:"+_));

        source.OnNext(1);

        source.Dispose();

        source.OnNext(2);
        source.OnNext(3);
    }
}
