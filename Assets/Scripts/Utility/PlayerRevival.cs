using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerRevival : MonoBehaviour
{
    private ReactiveProperty<bool> R;
    private Vector3 InitialTransformPosition;
    private GameObject Player;

    void Awake()
    {
        R = new ReactiveProperty<bool>();
        Player = GameObject.Find("Test");
        InitialTransformPosition = Player.transform.position;
    }

    void Start()
    {
        this.UpdateAsObservable()
            .Subscribe(_ => R.Value = CrossPlatformInputManager.GetButton("R"));

        R.Where(x => x).Subscribe(_ => Instantiate(Resources.Load("Prefab/Actors/Test"), InitialTransformPosition, Quaternion.identity));
    }
}
