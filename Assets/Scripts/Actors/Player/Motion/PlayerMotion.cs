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
    GameObject _StandingLightAttack;
    GameObject _StandingMiddleAttack;
    GameObject _StandingHighAttack;
    GameObject _CrouchingLightAttack;
    GameObject _CrouchingMiddleAttack;
    GameObject _CrouchingHighAttack;
    GameObject _JumpingLightAttack;
    GameObject _JumpingMiddleAttack;
    GameObject _JumpingHighAttack;

    void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Utility = GetComponent<Utility>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
        CircleCollider2D = GetComponent<CircleCollider2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        PlayerConfig = GetComponent<PlayerConfig>();
        PlayerState = GetComponent<PlayerState>();
        _StandingLightAttack = GameObject.Find("StandingLightAttack");
        _StandingMiddleAttack = GameObject.Find("StandingMiddleAttack");
        _StandingHighAttack = GameObject.Find("StandingHighAttack");
        _CrouchingLightAttack = GameObject.Find("CrouchingLightAttack");
        _CrouchingMiddleAttack = GameObject.Find("CrouchingMiddleAttack");
        _CrouchingHighAttack = GameObject.Find("CrouchingHighAttack");
        _JumpingLightAttack = GameObject.Find("JumpingLightAttack");
        _JumpingMiddleAttack = GameObject.Find("JumpingMiddleAttack");
        _JumpingHighAttack = GameObject.Find("JumpingHighAttack");
    }
}
