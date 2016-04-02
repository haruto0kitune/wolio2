using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Utility))]
[RequireComponent(typeof(FlyingPiyoState))]
public partial class FlyingPiyoMotion : MonoBehaviour
{
    GameObject Player;
    Rigidbody2D Rigidbody2D;
    Utility Utility;
    FlyingPiyoState FlyingPiyoState;
    GameObject JumpingAttack;

    void Awake()
    {
        Player = GameObject.Find("Test");
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Utility = GetComponent<Utility>();
        FlyingPiyoState = GetComponent<FlyingPiyoState>();
        JumpingAttack = GameObject.Find("JumpingAttack");
    }
}