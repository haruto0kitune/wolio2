using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class PlayerCreep : MonoBehaviour
{
    PlayerState PlayerState;
    BoxCollider2D BoxCollider2D;

    void Awake()
    {
        PlayerState = GameObject.Find("Test").GetComponent<PlayerState>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        PlayerState.IsCreeping
            .Where(x => x)
            .Subscribe(_ => BoxCollider2D.enabled = true);

        PlayerState.IsCreeping
            .Where(x => !x)
            .Subscribe(_ => BoxCollider2D.enabled = false);
    }
}
