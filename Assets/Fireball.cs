using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class Fireball : MonoBehaviour
{
    Rigidbody2D Rigidbody2D;
    public Vector2 Speed { get; set; }

    void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        this.FixedUpdateAsObservable()
            .Subscribe(_ => Rigidbody2D.velocity = Speed);
    }
}
