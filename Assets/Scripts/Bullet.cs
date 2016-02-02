using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class Bullet : MonoBehaviour
{
    private GameObject Camera;
    private Player Player;
    private ReactiveProperty<bool> FacingRight;
    
    void Awake()
    {
        Camera = GameObject.Find("Main Camera");
        Player = GameObject.Find("Player").GetComponent<Player>();
        FacingRight = new ReactiveProperty<bool>(Player.FacingRight.Value);
    }
    
    void Start()
    {
        this.UpdateAsObservable()
            .Where(x => !FacingRight.Value)
            .Subscribe(_ => LeftMove());

        this.UpdateAsObservable()
            .Where(x => FacingRight.Value)
            .Subscribe(_ => RightMove());

        this.UpdateAsObservable()
            .Where(x => (Camera.transform.position.x + 8.3) <= transform.position.x || (Camera.transform.position.x - 8.3) >= transform.position.x)
            .Subscribe(_ => Destroy(gameObject));

        this.OnTriggerEnter2DAsObservable()
            .Where(x => (x.gameObject.layer == LayerMask.NameToLayer("Enemy")) || (x.gameObject.tag == "Obstacle"))
            .Subscribe(_ => Destroy(gameObject));
    }

    void LeftMove()
    {
        transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z);
    }

    void RightMove()
    {
        transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z);
    }
}
