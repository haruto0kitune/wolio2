using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

[RequireComponent(typeof(PlayerState))]
[RequireComponent(typeof(PlayerConfig))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Key))]
public class PlayerPresenter : MonoBehaviour
{
    PlayerState PlayerState;
    PlayerConfig PlayerConfig;
    PlayerMovement PlayerMovement;
    Key Key;

    void Awake()
    {
        PlayerState = GetComponent<PlayerState>();
        PlayerConfig = GetComponent<PlayerConfig>();
        PlayerMovement = GetComponent<PlayerMovement>();
        Key = GetComponent<Key>();
    }

    private void Start()
    {
        UpdateAsObservables();
    }

    private void UpdateAsObservables()
    {
        this.UpdateAsObservable()
            .Subscribe(_ => PlayerMovement.Run(Key.Horizontal));

        this.UpdateAsObservable()
            .Where(x => Key.Vertical == 1)
            .Subscribe(_ => PlayerMovement.Jump());
    }
}