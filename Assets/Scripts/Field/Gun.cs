using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class Gun : MonoBehaviour
{
    [SerializeField]
    Vector2 Speed;
    [SerializeField]
    Vector2 Direction;

    void Start()
    {
        this.UpdateAsObservable()
            .ThrottleFirstFrame(300)
            .Subscribe(_ =>
            {
                var fireball = Instantiate(Resources.Load("Prefab/Weapons/Fireball"), transform.position, Quaternion.identity) as GameObject;
                fireball.GetComponent<Fireball>().Speed = new Vector2(Speed.x * Direction.x, Speed.y * Direction.y);
            });
    }
}
