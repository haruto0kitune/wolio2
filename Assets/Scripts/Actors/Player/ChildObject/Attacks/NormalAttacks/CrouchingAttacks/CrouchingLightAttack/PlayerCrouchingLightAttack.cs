using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class PlayerCrouchingLightAttack : MonoBehaviour
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
        PlayerState.IsCrouchingLightAttack
            .Where(x => x)
            .Subscribe(_ => StartCoroutine(Attack()));

        PlayerState.FacingRight
            .Subscribe(_ => BoxCollider2D.offset = new Vector2(BoxCollider2D.offset.x * -1f, BoxCollider2D.offset.y));
    }

    public IEnumerator Attack()
    {
        BoxCollider2D.enabled = true;

        for (var i = 0; i < 3; i++)
        {
            yield return null;
        }

        BoxCollider2D.enabled = false;
    }
}