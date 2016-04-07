using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class PlayerStandingGuard : MonoBehaviour
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
        PlayerState.IsStandingGuard
            .Where(x => x)
            .Subscribe(_ => StartCoroutine(StandingGuard()));
    }

    public IEnumerator StandingGuard()
    {
        BoxCollider2D.enabled = true;

        while (PlayerState.IsStandingGuard.Value)
        {
            yield return null;
        }

        BoxCollider2D.enabled = false;
    }
}
