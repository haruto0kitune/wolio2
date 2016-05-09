using UnityEngine;
using UnityEngine.Scripting;
using System.Collections;
using System.IO;
using UnityStandardAssets.CrossPlatformInput;
using UniRx;
using UniRx.Triggers;

namespace Wolio
{
    public class GameCommand : MonoBehaviour
    {
        public bool A;
        public bool B;
        public bool C;
        public bool D;
        public bool E;

        public bool Left;
        public bool Right;
        public bool Up;
        public bool Down;

        private GameController GameController;

        void Awake()
        {
            var json = File.ReadAllText("test.json");
            GameController = JsonUtility.FromJson<GameController>(json);
        }

        void Start()
        {
            this.UpdateAsObservable()
                .Subscribe(_ => A = CrossPlatformInputManager.GetButton(GameController.A));

            this.UpdateAsObservable()
                .Subscribe(_ => B = CrossPlatformInputManager.GetButton(GameController.B));
            
            this.UpdateAsObservable()
                .Subscribe(_ => C = CrossPlatformInputManager.GetButton(GameController.C));

            this.UpdateAsObservable()
                .Subscribe(_ => D = CrossPlatformInputManager.GetButton(GameController.D));

            this.UpdateAsObservable()
                .Subscribe(_ => E = CrossPlatformInputManager.GetButton(GameController.E));

            this.UpdateAsObservable()
                .Subscribe(_ => Left = CrossPlatformInputManager.GetButton(GameController.Left));
            
            this.UpdateAsObservable()
                .Subscribe(_ => Right = CrossPlatformInputManager.GetButton(GameController.Right));

            this.UpdateAsObservable()
                .Subscribe(_ => Up = CrossPlatformInputManager.GetButton(GameController.Up));

            this.UpdateAsObservable()
                .Subscribe(_ => Down = CrossPlatformInputManager.GetButton(GameController.Down));
        }
    }
}