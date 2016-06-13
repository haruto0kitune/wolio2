using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Basics
{
    public class PlayerAirTech : MonoBehaviour
    {
        [SerializeField]
        GameObject Player;
        Animator Animator;
        ObservableStateMachineTrigger ObservableStateMachineTrigger;
        Key Key;
        PlayerState PlayerState;
        Rigidbody2D PlayerRigidbody2D;
        BoxCollider2D BoxCollider2D;

        void Awake()
        {
            Animator = Player.GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            Key = Player.GetComponent<Key>();
            PlayerState = Player.GetComponent<PlayerState>();
            PlayerRigidbody2D = Player.GetComponent<Rigidbody2D>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            // Animation
            #region AirTech->Jump
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.AirTech"))
                .Where(x => x.StateInfo.normalizedTime >= 1)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsAirTech", false);
                    Animator.SetBool("IsJumping", true);
                });
            #endregion

            // Motion
            ObservableStateMachineTrigger
                .OnStateEnterAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.AirTech"))
                .Where(x => PlayerState.canAirTech.Value)
                .Subscribe(_ =>
                {
                    if (Key.Horizontal.Value >= 1f)
                    {
                        var Vector = Utility.PolarToRectangular2D(60, 3);
                        PlayerRigidbody2D.velocity = Vector;
                    }
                    else if(Key.Horizontal.Value <= -1f)
                    {
                        var Vector = Utility.PolarToRectangular2D(120, 3);
                        PlayerRigidbody2D.velocity = Vector;
                    }
                    else if(Key.Horizontal.Value == 0f)
                    {
                        var Vector = Utility.PolarToRectangular2D(90, 3);
                        PlayerRigidbody2D.velocity = Vector;
                    }
                });
        }
    }
}
