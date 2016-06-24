using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public Stack<string> inputHistory;

        private void Awake()
        {
            Horizontal = new ReactiveProperty<float>();
            Vertical = new ReactiveProperty<float>();
            IsAvailable = new ReactiveProperty<bool>(true);
            inputHistory = new Stack<string>(16);
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
                .Subscribe(_ => 
                {
                    if(Horizontal.Value == -1)
                    {
                        inputHistory.Push("left");
                    }
                    else if(Horizontal.Value == 1)
                    {
                        inputHistory.Push("right");
                    }
                });

            this.UpdateAsObservable()
                .Where(x => IsAvailable.Value)
                .Subscribe(_ => Vertical.Value = CrossPlatformInputManager.GetAxisRaw("Vertical"));

            this.UpdateAsObservable()
                .Subscribe(_ => 
                {
                    if(Vertical.Value == -1)
                    {
                        inputHistory.Push("down");
                    }
                    else if(Vertical.Value == 1)
                    {
                        inputHistory.Push("up");
                    }
                });

            this.UpdateAsObservable()
                .Where(x => IsAvailable.Value)
                .Subscribe(_ => A = CrossPlatformInputManager.GetButtonDown("A"));

            this.UpdateAsObservable()
                .Where(x => A)
                .Subscribe(_ => inputHistory.Push("A"));

            this.UpdateAsObservable()
                .Where(x => IsAvailable.Value)
                .Subscribe(_ => S = CrossPlatformInputManager.GetButtonDown("S"));

            this.UpdateAsObservable()
                .Where(x => S)
                .Subscribe(_ => inputHistory.Push("S"));

            this.UpdateAsObservable()
                .Where(x => IsAvailable.Value)
                .Subscribe(_ => D = CrossPlatformInputManager.GetButtonDown("D"));

            this.UpdateAsObservable()
                .Where(x => D)
                .Subscribe(_ => inputHistory.Push("D"));

            this.UpdateAsObservable()
                .Where(x => IsAvailable.Value)
                .Subscribe(_ => Z = CrossPlatformInputManager.GetButtonDown("Z"));

            this.UpdateAsObservable()
                .Where(x => Z)
                .Subscribe(_ => inputHistory.Push("Z"));

            this.UpdateAsObservable()
                .Where(x => IsAvailable.Value)
                .Subscribe(_ => X = CrossPlatformInputManager.GetButtonDown("X"));

            this.UpdateAsObservable()
                .Where(x => X)
                .Subscribe(_ => inputHistory.Push("X"));

            this.UpdateAsObservable()
                .Where(x => IsAvailable.Value)
                .Subscribe(_ => C = CrossPlatformInputManager.GetButtonDown("C"));

            this.UpdateAsObservable()
                .Where(x => C)
                .Subscribe(_ => inputHistory.Push("C"));

            this.UpdateAsObservable()
                .Where(x => IsAvailable.Value)
                .Subscribe(_ => LeftShift = CrossPlatformInputManager.GetButton("LeftShift"));

            this.UpdateAsObservable()
                .Where(x => IsAvailable.Value)
                .Subscribe(_ => Space = CrossPlatformInputManager.GetButton("Space"));

            this.UpdateAsObservable()
                .Where(x => inputHistory.Count != 0)
                .Where(x => Input.GetButtonDown("Space"))
                .Subscribe(_ => Debug.Log(inputHistory.Pop()));

            this.ObserveEveryValueChanged(x => inputHistory.Count)
                .Subscribe(_ => Debug.Log("Count: " + inputHistory.Count));

            this.LateUpdateAsObservable()
                .Where(x => inputHistory.Count > 15)
                .Subscribe(_ =>
                {
                    var tempStack = new Stack<string>(15);

                    for (int i = 0; i < 15; i++)
                    {
                        tempStack.Push(inputHistory.Pop());
                    }

                    inputHistory.Clear();

                    for (int i = 0; i < 15; i++)
                    {
                        inputHistory.Push(tempStack.Pop());
                    }
                });
        }
    }
}