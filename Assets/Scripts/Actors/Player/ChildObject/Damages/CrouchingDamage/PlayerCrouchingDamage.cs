using UnityEngine;
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
        bool wasAttackedDuringCrouchingDamage = false;
        bool isKnockBack = false;
        int knockBackFrame;
        
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
            //When Player was attacked during CrouchingDamage
            PlayerCrouchingDamageHurtBox
                .OnTriggerEnter2DAsObservable()
                .Where(x => x.gameObject.tag == "Enemy/HitBox")
                .Do(x => Debug.Log(x.gameObject.name))
                .Subscribe(_ => wasAttackedDuringCrouchingDamage = true);

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

            // Motion (KnockBack)
            this.FixedUpdateAsObservable()
                .Where(x => isKnockBack) 
                .Subscribe(x =>
                {
                    knockBackFrame++;

                    if (PlayerState.FacingRight.Value)
                    {
                        PlayerRigidbody2D.velocity = new Vector2(-1f, 0);
                    }
                    else
                    {
                        PlayerRigidbody2D.velocity = new Vector2(1f, 0);
                    }

                    if(knockBackFrame == 3)
                    {
                        isKnockBack = false;
                        knockBackFrame = 0;
                    }
                });
        }

        // Execute DamageManager
        public IEnumerator Damage(int damageValue, int recovery)
        {
            // StartUp
            Animator.Play("CrouchingDamage", Animator.GetLayerIndex("Base Layer"), 0.0f);
            isKnockBack = true;
            BoxCollider2D.enabled = true;
            PlayerCrouchingDamageHurtBoxTrigger.enabled = true;
            Key.IsAvailable.Value = false;
            PlayerState.WasAttacked.Value = true;

            // Apply Damage
            Status.Hp.Value -= damageValue;

            // Recover
            for (int i = 0; i < recovery; i++)
            {
                yield return null;

                if(wasAttackedDuringCrouchingDamage)
                {
                    wasAttackedDuringCrouchingDamage = false;
                    yield break;
                }
            }

            // Finish
            //
            // When transtion to next state, collider enabled is off.
            // if not, PlayerStandingDamage becomes strange motion.
            PlayerState.WasAttacked.Value = false;

            yield return null;
            
            BoxCollider2D.enabled = false;
            PlayerCrouchingDamageHurtBoxTrigger.enabled = false;
            Key.IsAvailable.Value = true;
        }
    }
}