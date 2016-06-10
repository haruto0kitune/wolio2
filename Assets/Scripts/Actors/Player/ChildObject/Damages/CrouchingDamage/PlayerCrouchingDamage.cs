﻿using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Damages
{
    public class PlayerCrouchingDamage : MonoBehaviour, IDamage
    {
        [SerializeField]
        GameObject Player;
        [SerializeField]
        GameObject PlayerCrouchingDamageHurtBox;
        Animator Animator;
        ObservableStateMachineTrigger ObservableStateMachineTrigger;
        PlayerState PlayerState;
        Status Status;
        Rigidbody2D PlayerRigidbody2D;
        Key Key;
        BoxCollider2D BoxCollider2D;
        BoxCollider2D PlayerCrouchingDamageHurtBoxTrigger;
        bool wasAttackedDuringDamage = false;
        int knockBackFrame;
        Coroutine damageCoroutineStore;
        
        void Awake()
        {
            Animator = Player.GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            PlayerState = Player.GetComponent<PlayerState>();
            Status = Player.GetComponent<Status>();
            PlayerRigidbody2D = Player.GetComponent<Rigidbody2D>();
            Key = Player.GetComponent<Key>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            PlayerCrouchingDamageHurtBoxTrigger = PlayerCrouchingDamageHurtBox.GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            // Animation
            #region CrouchingDamage->Crouch
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.CrouchingDamage"))
                .Where(x => !PlayerState.WasAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCrouching", true);
                    Animator.SetBool("IsCrouchingDamage", false);
                });
            #endregion
        }

        public void Damage(int damageValue, int recovery, int hitStop, bool isTechable = false)
        {
            if (damageCoroutineStore == null)
            {
                damageCoroutineStore = StartCoroutine(DamageCoroutine(damageValue, recovery, hitStop, isTechable));
            }
            else
            {
                StopCoroutine(damageCoroutineStore);
                damageCoroutineStore = StartCoroutine(DamageCoroutine(damageValue, recovery, hitStop, isTechable));
                wasAttackedDuringDamage = true;
            }
        }

        // Execute DamageManager
        public IEnumerator DamageCoroutine(int damageValue, int recovery, int hitStop, bool isTechable = false)
        {
            // StartUp
            BoxCollider2D.enabled = true;
            PlayerCrouchingDamageHurtBoxTrigger.enabled = true;
            Key.IsAvailable.Value = false;
            PlayerState.WasAttacked.Value = true;

            // Apply Damage
            Status.Hp.Value -= damageValue;

            // HitStop
            var x = 1;

            for (int i = 0; i < hitStop; i++)
            {
                PlayerRigidbody2D.velocity = new Vector2(x, 0);
                x *= -1;
                yield return null;
            }

            // Knockback
            for (int i = 0;i < recovery; i++)
            {
                if (PlayerState.FacingRight.Value)
                {
                    PlayerRigidbody2D.velocity = new Vector2(-1f, 0);
                }
                else
                {
                    PlayerRigidbody2D.velocity = new Vector2(1f, 0);
                }

                yield return null;
            }

            // Finish
            //
            // When transtion to next state, collider enabled is off.
            // if not, PlayerCrouchingDamage becomes strange motion.
            PlayerState.WasAttacked.Value = false;
            wasAttackedDuringDamage = false;

            yield return null;
            
            BoxCollider2D.enabled = false;
            PlayerCrouchingDamageHurtBoxTrigger.enabled = false;
            Key.IsAvailable.Value = true;
        }
    }
}