using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class SplinterRange : MonoBehaviour
{
    GameObject Parent;
    Rigidbody2D Rigidbody2D;

    void Awake()
    {
        Parent = transform.root.gameObject;
        Rigidbody2D = Parent.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Player")
            .Subscribe(_ => Rigidbody2D.isKinematic = false);
    }
}
