using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class FlyingPiyoJumpingAttack : MonoBehaviour
{ 
    GameObject Player;
    GameObject FlyingPiyo;
    Rigidbody2D Rigidbody2D;
    BoxCollider2D BoxCollider2D;
    FlyingPiyoState FlyingPiyoState;
    FlyingPiyoSearchBox FlyingPiyoSearchBox;

    Vector2 InitialPosition;
    //float SpeedY;

    void Awake()
    {
        Player = GameObject.Find("Test");
        FlyingPiyo = transform.parent.gameObject;
        Rigidbody2D = FlyingPiyo.GetComponent<Rigidbody2D>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
        FlyingPiyoState = FlyingPiyo.GetComponent<FlyingPiyoState>();
        FlyingPiyoSearchBox = FlyingPiyo.GetComponentInChildren<FlyingPiyoSearchBox>();

        InitialPosition = FlyingPiyo.transform.position;
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
        BoxCollider2D.enabled = true;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Enemy"), true);
        FlyingPiyoState.IsAttacking.Value = true;

        // Cache current coordinate of Y
        var FlyingPiyoInitialPositionY = FlyingPiyo.transform.position.y;

        // Get unit vector of the direction of player 
        var UnitVector = Utility.GetUnitVector(FlyingPiyo, Player);

        // Charge Player
        Rigidbody2D.velocity = UnitVector * 3;

        while (FlyingPiyo.transform.position.y >= Player.transform.position.y)
        {
            yield return null;
        }

        // Go back initial altitude
        float SpeedY = 1f;

        while (FlyingPiyo.transform.position.y <= InitialPosition.y)
        {
            if (FlyingPiyoState.FacingRight.Value)
            {
                Rigidbody2D.velocity = new Vector2(3f, SpeedY);
            }
            else
            {
                Rigidbody2D.velocity = new Vector2(-3f, SpeedY);
            }

            if (SpeedY <= 3f)
            {
                SpeedY += 0.5f;
            }

            yield return null;
        }

        // Finish Jumping Attack
        Rigidbody2D.velocity = new Vector2(3f, 0f);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Enemy"), false);
        FlyingPiyo.transform.position = new Vector2(FlyingPiyo.transform.position.x, InitialPosition.y);
        FlyingPiyoState.IsAttacking.Value = false;
        BoxCollider2D.enabled = false;
    }
}