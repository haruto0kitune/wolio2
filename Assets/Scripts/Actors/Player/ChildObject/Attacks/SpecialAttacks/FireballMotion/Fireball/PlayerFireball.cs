using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class PlayerFireball : MonoBehaviour
{
    [SerializeField]
    float speed;
    float direction;
    Rigidbody2D Rigidbody2D;

    void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        this.FixedUpdateAsObservable()
            .Subscribe(_ => Move(speed, direction));
    }

    public void Initialize(float direction)
    {
        this.direction = direction;
    }

    void Move(float speed, float direction)
    {
        Rigidbody2D.velocity = new Vector2(speed * direction, 0f);
    }
}