using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Damages
{
    public class PlayerJumpingDamage : MonoBehaviour, IDamage
    {
        [SerializeField]
        GameObject Player;
        [SerializeField]
        GameObject PlayerJumpingDamageHurtBox;
        Animator Animator;
        ObservableStateMachineTrigger ObservableStateMachineTrigger;
        PlayerState PlayerState;
        Status Status;
        Rigidbody2D PlayerRigidbody2D;
        Key Key;
        BoxCollider2D BoxCollider2D;
        BoxCollider2D PlayerJumpingDamageHurtBoxTrigger;
        bool wasAttackedDuringDamage = false;
        bool isKnockBack = false;
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
            PlayerJumpingDamageHurtBoxTrigger = PlayerJumpingDamageHurtBox.GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            // Animation
            #region JumpingDamage->Jump
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.JumpingDamage"))
                .Where(x => !PlayerState.WasAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsJumping", true);
                    Animator.SetBool("IsJumpingDamage", false);
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

        public void Damage(int damageValue, int recovery, int hitStop)
        {
            if (damageCoroutineStore == null)
            {
                damageCoroutineStore = StartCoroutine(DamageCoroutine(damageValue, recovery, hitStop));
            }
            else
            {
                StopCoroutine(damageCoroutineStore);
                damageCoroutineStore = StartCoroutine(DamageCoroutine(damageValue, recovery, hitStop));
                wasAttackedDuringDamage = true;
            }
        }
        
        // Execute DamageManager
        public IEnumerator DamageCoroutine(int damageValue, int recovery, int hitStop)
        {
            // StartUp
            Animator.Play("JumpingDamage", Animator.GetLayerIndex("Base Layer"), 0.0f);
            isKnockBack = true;
            BoxCollider2D.enabled = true;
            PlayerJumpingDamageHurtBoxTrigger.enabled = true;
            Key.IsAvailable.Value = false;
            PlayerState.WasAttacked.Value = true;

            // Apply Damage
            Status.Hp.Value -= damageValue;

            // Recover
            for (int i = 0; i < recovery; i++)
            {
                yield return null;
            }

            // Finish
            //
            // When transtion to next state, collider enabled is off.
            // if not, PlayerJumpingDamage becomes strange motion.
            PlayerState.WasAttacked.Value = false;
            wasAttackedDuringDamage = false;

            yield return null;
            
            BoxCollider2D.enabled = false;
            PlayerJumpingDamageHurtBoxTrigger.enabled = false;
            Key.IsAvailable.Value = true;
        }
    }
}
