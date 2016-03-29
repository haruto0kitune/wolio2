using UnityEngine;
using System.Collections;

public partial class FlyingPiyoMotion : MonoBehaviour
{
    public IEnumerator Attacks()
    {
        var FlyingPiyoInitialPositionY = transform.position.y;
        var Player = GameObject.Find("Test");
        var Actor = Player.transform.parent;
        Player.transform.parent = transform;
        var UnitVectorInTheDirectionOfPlayer = Player.transform.localPosition.normalized;
        Player.transform.parent = Actor;

        // 11 = Player 12 = Enemy
        Physics2D.IgnoreLayerCollision(11, 12, true);

        Rigidbody2D.velocity = UnitVectorInTheDirectionOfPlayer * 3;

        while (transform.position.y >= Player.transform.position.y)
        {
            yield return null;
        }


        float y = 1f;

        while (transform.position.y <= FlyingPiyoInitialPositionY)
        {
            Rigidbody2D.velocity = new Vector2(3f, y);
            y += 0.5f;
            yield return null;
        }

        Rigidbody2D.velocity = new Vector2(3f, 0f);

        // 11 = Player 12 = Enemy
        Physics2D.IgnoreLayerCollision(11, 12, false);
        FlyingPiyoState.IsAttacking.Value = false;
    }
}
