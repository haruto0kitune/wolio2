using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class FlyingPiyoJumpingAttack : MonoBehaviour
{ 
    GameObject Player;
    Rigidbody2D Rigidbody2D;
    BoxCollider2D BoxCollider2D;
    FlyingPiyoState FlyingPiyoState;
    FlyingPiyoSearchBox FlyingPiyoSearchBox;

    Vector2 InitialPosition;
    float SpeedY;

    void Awake()
    {
        Player = GameObject.Find("Test");
        Rigidbody2D = GameObject.Find("FlyingPiyo").GetComponent<Rigidbody2D>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
        FlyingPiyoState = GameObject.Find("FlyingPiyo").GetComponent<FlyingPiyoState>();
        FlyingPiyoSearchBox = GameObject.Find("FlyingPiyo").GetComponentInChildren<FlyingPiyoSearchBox>();

        InitialPosition = GameObject.Find("FlyingPiyo").transform.position;
    }

    void Start()
    {
        this.FixedUpdateAsObservable()
            .ObserveEveryValueChanged(x => FlyingPiyoSearchBox.FoundPlayer.Value)
            .Where(x => x)
            .Where(x => !FlyingPiyoState.IsAttacking.Value)
            .Do(x => FlyingPiyoState.IsAttacking.Value = true)
            .Subscribe(_ => StartCoroutine(Attack()));
    }

    public IEnumerator Attack()
    {
        // Start JumpingAttack
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Enemy"), true);
        FlyingPiyoState.IsAttacking.Value = true;

        // Cache current coordinate of Y
        var FlyingPiyoInitialPositionY = transform.position.y;

        // Get unit vector of the direction of player 
        var UnitVector = Utility.GetUnitVector(gameObject, Player);

        // Charge Player
        Rigidbody2D.velocity = UnitVector * 3;

        while (transform.position.y >= Player.transform.position.y)
        {
            yield return null;
        }

        // Go back initial altitude
        float SpeedY = 1f;

        while (transform.position.y <= InitialPosition.y)
        {
            Rigidbody2D.velocity = new Vector2(3f, SpeedY);
            SpeedY += 0.5f;
            yield return null;
        }

        // Finish Jumping Attack
        Rigidbody2D.velocity = new Vector2(3f, 0f);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Enemy"), false);
        transform.position = new Vector2(transform.position.x, InitialPosition.y);
        FlyingPiyoState.IsAttacking.Value = false;
    }
}