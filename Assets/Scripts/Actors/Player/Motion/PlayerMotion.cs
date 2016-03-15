using UnityEngine;
using System.Collections;

public partial class PlayerMotion : MonoBehaviour
{
    Rigidbody2D Rigidbody2D;
    Utility Utility;
    BoxCollider2D BoxCollider2D;
    CircleCollider2D CircleCollider2D;
    SpriteRenderer SpriteRenderer;
    PlayerConfig PlayerConfig;
    PlayerState PlayerState;

    void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Utility = GetComponent<Utility>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
        CircleCollider2D = GetComponent<CircleCollider2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        PlayerConfig = GetComponent<PlayerConfig>();
        PlayerState = GetComponent<PlayerState>();
    }
}
