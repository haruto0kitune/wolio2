using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class PlayerStandingLightAttack : MonoBehaviour
{
    PlayerState PlayerState;
    BoxCollider2D BoxCollider2D;
    [SerializeField]
    int Startup;
    [SerializeField]
    int Active;
    [SerializeField]
    int Recovery;

    void Awake()
    {
        PlayerState = GameObject.Find("Test").GetComponent<PlayerState>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        PlayerState.IsStandingLightAttack
            .Where(x => x)
            .Subscribe(_ => StartCoroutine(Attack()));
    }

    public IEnumerator Attack()
    {
        // Startup
        for (int i = 0; i < Startup; i++)
        {
            Debug.Log("Startup: " + i);
            yield return null;
        }

        // Active
        BoxCollider2D.enabled = true;

        for (var i = 0; i < Active ; i++)
        {
            Debug.Log("Active: " + i);
            yield return null;
        }

        BoxCollider2D.enabled = false;

        // Recovery
        for (int i = 0; i < Recovery; i++)
        {
            Debug.Log("Recovery: " + i);
            yield return null;
        }
    }
}