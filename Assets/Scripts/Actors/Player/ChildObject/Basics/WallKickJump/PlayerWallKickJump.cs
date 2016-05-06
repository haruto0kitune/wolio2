using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class PlayerWallKickJump : MonoBehaviour
{
    GameObject Player;
    Rigidbody2D PlayerRigidbody2D;
    PlayerState PlayerState;
    PlayerMotion PlayerMotion;
    Key Key;

    void Awake()
    {
        Player = GameObject.Find("Test");
        PlayerRigidbody2D = Player.GetComponent<Rigidbody2D>();
        PlayerState = Player.GetComponent<PlayerState>();
        PlayerMotion = Player.GetComponent<PlayerMotion>();
        Key = Player.GetComponent<Key>();
    }

    void Start()
    {
        //Player
        this
            .OnTriggerStay2DAsObservable()
            .Where(x => x.gameObject.layer == LayerMask.NameToLayer("Field"))
            .Where(x => PlayerState.IsJumping.Value)
            .Where(x => Key.Vertical.Value == 1f)
            .Subscribe(_ => WallKickJump(60, 10));
    }

    void WallKickJump(int Angle, float Radius)
    {
        Vector2 Vector;

        // Make velocity reset
        PlayerRigidbody2D.velocity = Vector2.zero;

        // Convert Polar to Rectangular
        if (PlayerState.FacingRight.Value)
        {
            Vector = Utility.PolarToRectangular2D(Angle, Radius);
            Vector = new Vector2(Vector.x * -1, Vector.y);
        }
        else
        {
            Vector = Utility.PolarToRectangular2D(Angle, Radius);
        }

        PlayerMotion.Turn();
        PlayerRigidbody2D.velocity = Vector;
    }
}
