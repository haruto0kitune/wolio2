using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class PlayerStand : MonoBehaviour
{
    PlayerState PlayerState;
    BoxCollider2D BoxCollider2D;
    CircleCollider2D CircleCollider2D;

    void Awake()
    {
        PlayerState = GameObject.Find("Test").GetComponent<PlayerState>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
        CircleCollider2D = GetComponent<CircleCollider2D>();
    }

    void Start()
    {
        PlayerState.IsStanding
            .Where(x => x)
            .Subscribe(_ =>
            {
                BoxCollider2D.enabled = true;
                CircleCollider2D.enabled = true;
            });

        PlayerState.IsStanding
            .Where(x => !x)
            .Subscribe(_ =>
            {
                BoxCollider2D.enabled = false;
                CircleCollider2D.enabled = false;
            });
    }
}
