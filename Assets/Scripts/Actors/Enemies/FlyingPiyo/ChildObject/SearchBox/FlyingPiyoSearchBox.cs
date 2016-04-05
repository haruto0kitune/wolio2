using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class FlyingPiyoSearchBox : MonoBehaviour
{
    public ReactiveProperty<bool> FoundPlayer;
    private BoxCollider2D BoxCollider2D;
    private FlyingPiyoState FlyingPiyoState;

    void Awake()
    {
        FoundPlayer = new ReactiveProperty<bool>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
        FlyingPiyoState = GameObject.Find("FlyingPiyo").GetComponent<FlyingPiyoState>();
    }

    void Start()
    {
        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Player")
            .ThrottleFirstFrame(1)
            .Subscribe(_ => FoundPlayer.Value = true);

        this.OnTriggerExit2DAsObservable()
            .Where(x => x.gameObject.tag == "Player")
            .Subscribe(_ => FoundPlayer.Value = false);

        FlyingPiyoState.FacingRight
            .Skip(1)
            .Subscribe(_ => BoxCollider2D.offset = new Vector2(BoxCollider2D.offset.x * -1f, BoxCollider2D.offset.y));
    }
}
