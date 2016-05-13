using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio
{
    public class MainCamera : MonoBehaviour
    {
        GameObject Player;

        void Awake()
        {
            Player = GameObject.Find("Test");
        }

        void Start()
        {
            this.ObserveEveryValueChanged(x => Player.transform.position)
                .Subscribe(_ => transform.position = (Vector2)_);
        }
    }
}
