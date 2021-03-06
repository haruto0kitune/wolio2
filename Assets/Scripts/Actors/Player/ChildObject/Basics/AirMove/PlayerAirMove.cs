﻿using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Basics
{
    public class PlayerAirMove : MonoBehaviour
    {
        [SerializeField]
        GameObject Actor;
        PlayerState PlayerState;
        Key Key;
        Rigidbody2D PlayerRigidbody2D;
        [SerializeField]
        float MaxSpeed;
        [SerializeField]
        float Force;

        void Awake()
        {
            PlayerState = Actor.GetComponent<PlayerState>();
            Key = Actor.GetComponent<Key>();
            PlayerRigidbody2D = Actor.GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            //Motion
            this.FixedUpdateAsObservable()
                .Where(x => (PlayerState.IsActionModeJumping.Value || PlayerState.IsWallKickJumping.Value || PlayerState.IsFalling.Value) && (PlayerState.controlMode == ControlMode.ActionMode)) 
                .Where(x => !PlayerState.IsGrounded.Value)
                .Where(x => PlayerState.canAirMove.Value)
                .Where(x => Key.Horizontal.Value != 0)
                .Subscribe(_ => this.AirMove(Key.Horizontal.Value));
        }

        public void AirMove(float Horizontal)
        {
            PlayerRigidbody2D.AddForce(new Vector2(/*Force*/Parameter.GetPlayerParameter().PlayerBasics.AirMove.Force * Horizontal, 0f));

            if (PlayerRigidbody2D.velocity.x > /*MaxSpeed*/Parameter.GetPlayerParameter().PlayerBasics.AirMove.MaxSpeed)
            {
                PlayerRigidbody2D.velocity = new Vector2(/*MaxSpeed*/Parameter.GetPlayerParameter().PlayerBasics.AirMove.MaxSpeed, PlayerRigidbody2D.velocity.y);
            }
            else if(PlayerRigidbody2D.velocity.x < /*-MaxSpeed*/-Parameter.GetPlayerParameter().PlayerBasics.AirMove.MaxSpeed)
            {
                PlayerRigidbody2D.velocity = new Vector2(/*-MaxSpeed*/-Parameter.GetPlayerParameter().PlayerBasics.AirMove.MaxSpeed, PlayerRigidbody2D.velocity.y);
            }
        }
    }
}