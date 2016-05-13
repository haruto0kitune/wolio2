using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player
{
    public class Key : MonoBehaviour
    {
        public ReactiveProperty<float> Horizontal;
        public ReactiveProperty<float> Vertical;

        public bool A;
        public bool S;
        public bool D;
        public bool Z;
        public bool X;
        public bool C;
        public bool LeftShift;

        public bool Space;

        public ReactiveProperty<bool> IsAvailable;

        private void Awake()
        {
            Horizontal = new ReactiveProperty<float>();
            Vertical = new ReactiveProperty<float>();
            IsAvailable = new ReactiveProperty<bool>(true);
        }

        private void Start()
        {
            UpdateAsObservables();
        }

        private void UpdateAsObservables()
        {
            this.UpdateAsObservable()
                .Where(x => IsAvailable.Value)
                .Subscribe(_ => Horizontal.Value = CrossPlatformInputManager.GetAxisRaw("Horizontal"));

            this.UpdateAsObservable()
                .Where(x => IsAvailable.Value)
                .Subscribe(_ => Vertical.Value = CrossPlatformInputManager.GetAxisRaw("Vertical"));

            this.UpdateAsObservable()
                .Where(x => IsAvailable.Value)
                .Subscribe(_ => A = CrossPlatformInputManager.GetButtonDown("A"));

            this.UpdateAsObservable()
                .Where(x => IsAvailable.Value)
                .Subscribe(_ => S = CrossPlatformInputManager.GetButtonDown("S"));

            this.UpdateAsObservable()
                .Where(x => IsAvailable.Value)
                .Subscribe(_ => D = CrossPlatformInputManager.GetButtonDown("D"));

            this.UpdateAsObservable()
                .Where(x => IsAvailable.Value)
                .Subscribe(_ => Z = CrossPlatformInputManager.GetButtonDown("Z"));

            this.UpdateAsObservable()
                .Where(x => IsAvailable.Value)
                .Subscribe(_ => X = CrossPlatformInputManager.GetButtonDown("X"));

            this.UpdateAsObservable()
                .Where(x => IsAvailable.Value)
                .Subscribe(_ => C = CrossPlatformInputManager.GetButtonDown("C"));

            this.UpdateAsObservable()
                .Where(x => IsAvailable.Value)
                .Subscribe(_ => LeftShift = CrossPlatformInputManager.GetButton("LeftShift"));

            this.UpdateAsObservable()
                .Where(x => IsAvailable.Value)
                .Subscribe(_ => Space = CrossPlatformInputManager.GetButton("Space"));
        }
    }
}