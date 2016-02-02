using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UniRx;
using UniRx.Triggers;

public class Key : MonoBehaviour
{
    public float Horizontal;
    public float Vertical;

    public bool A;
    public bool S;
    public bool D;
    public bool Z;
    public bool X;
    public bool C;

    public bool Space;

    private void Start()
    {
        UpdateAsObservables();
    }

    private void UpdateAsObservables()
    {
        this.UpdateAsObservable()
            .Subscribe(_ => Horizontal = CrossPlatformInputManager.GetAxisRaw("Horizontal"));

        this.UpdateAsObservable()
            .Subscribe(_ => Vertical = CrossPlatformInputManager.GetAxisRaw("Vertical"));

        this.UpdateAsObservable()
            .Subscribe(_ => A = CrossPlatformInputManager.GetButton("A"));

        this.UpdateAsObservable()
            .Subscribe(_ => S = CrossPlatformInputManager.GetButton("S"));

        this.UpdateAsObservable()
            .Subscribe(_ => D = CrossPlatformInputManager.GetButton("D"));

        this.UpdateAsObservable()
            .Subscribe(_ => Z = CrossPlatformInputManager.GetButton("Z"));

        this.UpdateAsObservable()
            .Subscribe(_ => X = CrossPlatformInputManager.GetButton("X"));

        this.UpdateAsObservable()
            .Subscribe(_ => C = CrossPlatformInputManager.GetButton("C"));

        this.UpdateAsObservable()
            .Subscribe(_ => Space = CrossPlatformInputManager.GetButton("Space"));
    }
}