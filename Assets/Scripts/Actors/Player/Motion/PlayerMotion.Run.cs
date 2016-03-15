using UnityEngine;
using System.Collections;

public partial class PlayerMotion : MonoBehaviour
{
    public void Run(float Horizontal, float MaxSpeed)
    {
        PlayerState.IsRunning.Value = true;
        Rigidbody2D.velocity = new Vector2(Horizontal * MaxSpeed, Rigidbody2D.velocity.y);
    }

    public void ExitRun()
    {
        PlayerState.IsRunning.Value = false;
    }
}
