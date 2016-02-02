using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UniRx;
using UniRx.Triggers;

public class PlayerControl : MonoBehaviour
{
    private float Direction;
    private float Jump;
    private bool Attack;
    private bool Dash;
    private bool Left;
    private bool Light;
    private bool Guard;
    private bool WallKickJump;
    private bool duringWKJ;

    private void Start()
    {
        UpdateAsObservables();
    }

    private void UpdateAsObservables()
    {
        this.UpdateAsObservable()
            .Subscribe(_ => Direction = CrossPlatformInputManager.GetAxisRaw("Horizontal"));

        this.UpdateAsObservable()
            .Subscribe(_ => Jump = CrossPlatformInputManager.GetAxisRaw("Vertical"));
    }
}