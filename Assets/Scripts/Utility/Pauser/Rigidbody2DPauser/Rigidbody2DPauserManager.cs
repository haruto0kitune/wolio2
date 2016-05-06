using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UniRx;
using UniRx.Triggers;

namespace MyUtility
{
    public class Rigidbody2DPauserManager : MonoBehaviour
    {
        private ReactiveProperty<bool> isPausing;

        void Awake()
        {
            isPausing = new ReactiveProperty<bool>();
        }

        void Start()
        {
            this.ObserveEveryValueChanged(x => CrossPlatformInputManager.GetAxisRaw("Horizontal"))
                .Where(x => x == 1)
                .Subscribe(_ => isPausing.Value = !isPausing.Value);

            isPausing
                .Skip(1)
                .Where(x => x)
                .Subscribe(_ => Rigidbody2DPauser.Pause());

            isPausing
                .Skip(1)
                .Where(x => !x)
                .Subscribe(_ => Rigidbody2DPauser.Resume());
        }
    }
}