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

    void Awake()
    {
        Player = GameObject.Find("Test");
        Rigidbody2D = GameObject.Find("FlyingPiyo").GetComponent<Rigidbody2D>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
        FlyingPiyoState = GameObject.Find("FlyingPiyo").GetComponent<FlyingPiyoState>();
        FlyingPiyoSearchBox = GameObject.Find("FlyingPiyo").GetComponentInChildren<FlyingPiyoSearchBox>();
    }

    void Start()
    {
        FlyingPiyoSearchBox.FoundPlayer
            .Where(x => x)
            .Where(x => !FlyingPiyoState.IsAttacking.Value)
            .Do(x => FlyingPiyoState.IsAttacking.Value = true)
            .Subscribe(_ => StartCoroutine(Attack()));
    }

    public IEnumerator Attack()
    {
        // Start JumpingAttack
        BoxCollider2D.enabled = true;
        FlyingPiyoState.IsAttacking.Value = true;

        // Cache current coordinate of Y
        var FlyingPiyoInitialPositionY = transform.position.y;

        // Get unit vector of the direction of player 
        var Actor = Player.transform.parent;
        Player.transform.parent = transform;
        var UnitVectorInTheDirectionOfPlayer = Player.transform.localPosition.normalized;
        Player.transform.parent = Actor;

        // Charge Player
        Rigidbody2D.velocity = UnitVectorInTheDirectionOfPlayer * 3;

        while (transform.position.y >= Player.transform.position.y)
        {
            yield return null;
        }

        // Go back initial altitude
        float SpeedY = 1f;

        while (transform.position.y <= FlyingPiyoInitialPositionY)
        {
            Rigidbody2D.velocity = new Vector2(3f, SpeedY);
            SpeedY += 0.5f;
            yield return null;
        }

        // Finish Jumping Attack
        transform.position = new Vector3(transform.position.x, FlyingPiyoInitialPositionY);
        Rigidbody2D.velocity = new Vector2(3f, 0f);
        FlyingPiyoState.IsAttacking.Value = false;
        BoxCollider2D.enabled = false;
    }
}