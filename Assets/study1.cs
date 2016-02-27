using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class study1 : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        var source1 = new Subject<int>();
        var source2 = new Subject<string>();

        source1.Zip(source2, (a, b) => b).Subscribe(_ => Debug.Log(_));

        source1.OnNext(1);
       // source2.OnNext("hello");
    }
}
