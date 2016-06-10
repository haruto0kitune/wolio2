using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;
using Wolio.Actor.Player;

namespace Wolio
{
    public class PlayerDebugInformation : MonoBehaviour
    {
        [SerializeField]
        private GameObject Player;
        private PlayerState PlayerState;
        private Status Status;
        private Rigidbody2D Rigidbody2D;

        [SerializeField]
        private Text Text;
        [SerializeField]
        private Text Text1;
        [SerializeField]
        private Text Text2;
        [SerializeField]
        private Text Text3;
        [SerializeField]
        private Text Text4;
        [SerializeField]
        private Text Text5;
        [SerializeField]
        private Text Text6;

        void Awake()
        {
            PlayerState = Player.GetComponent<PlayerState>();
            Status = Player.GetComponent<Status>();
            Rigidbody2D = Player.GetComponent<Rigidbody2D>();
        }
        void Start()
        {
            FPSCounter.Current.SubscribeToText(Text);
            PlayerState.IsClimbable.SubscribeToText(Text1);
            PlayerState.IsClimbing.SubscribeToText(Text2);
            this.ObserveEveryValueChanged(x => Rigidbody2D.velocity.x).SubscribeToText(Text3);
            this.ObserveEveryValueChanged(x => Rigidbody2D.velocity.y).SubscribeToText(Text4);
            PlayerState.IsCrouching.SubscribeToText(Text5);
            Status.Hp.SubscribeToText(Text6);
        }
    }
}
