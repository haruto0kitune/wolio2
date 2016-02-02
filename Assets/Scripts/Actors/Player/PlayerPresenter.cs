using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Key))]
public class PlayerPresenter : MonoBehaviour
{
    Player Player;
    Key Key;

    void Awake()
    {
        Player = GetComponent<Player>();
        Key = GetComponent<Key>();
    }

    private void Start()
    {
        UpdateAsObservables();
    }

    private void UpdateAsObservables()
    {
        this.UpdateAsObservable()
            .Subscribe(_ => Player.Run(Key.Horizontal));

        this.UpdateAsObservable()
            .Where(x => Key.Vertical == 1)
            .Subscribe(_ => Player.Jump());
    }
}