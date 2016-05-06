using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UniRx;
using UniRx.Triggers;

namespace MyUtility
{
    public class AudioSourcePauserManager : MonoBehaviour
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
                .Subscribe(_ => AudioSourcePauser.Pause());

            isPausing
                .Skip(1)
                .Where(x => !x)
                .Subscribe(_ => AudioSourcePauser.Resume());
        }
    }
}