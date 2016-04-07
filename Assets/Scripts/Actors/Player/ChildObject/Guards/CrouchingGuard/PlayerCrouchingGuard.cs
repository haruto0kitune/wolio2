using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class PlayerCrouchingGuard : MonoBehaviour
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
        PlayerState.IsCrouchingGuard
            .Where(x => x)
            .Subscribe(_ => StartCoroutine(CrouchingGuard()));

        PlayerState.FacingRight
            .Skip(1)
            .Subscribe(_ => BoxCollider2D.offset = new Vector2(BoxCollider2D.offset.x * -1f, BoxCollider2D.offset.y));
    }

    public IEnumerator CrouchingGuard()
    {
        BoxCollider2D.enabled = true;

        while (PlayerState.IsCrouchingGuard.Value)
        {
            yield return null;
        }

        BoxCollider2D.enabled = false;
    }
}