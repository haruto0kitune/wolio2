using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Damages
{
    public class PlayerStandingDamage : MonoBehaviour, IDamage
    {
        [SerializeField]
        GameObject Player;
        [SerializeField]
        GameObject PlayerStandingDamageHurtBox;
        Animator Animator;
        ObservableStateMachineTrigger ObservableStateMachineTrigger;
        PlayerState PlayerState;
        Status Status;
        Rigidbody2D PlayerRigidbody2D;
        Key Key;
        BoxCollider2D BoxCollider2D;
        CircleCollider2D CircleCollider2D;
        BoxCollider2D PlayerStandingDamageHurtBoxTrigger;
        bool wasAttackedDuringStandingDamage = false;
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
            CircleCollider2D = GetComponent<CircleCollider2D>();
            PlayerStandingDamageHurtBoxTrigger = PlayerStandingDamageHurtBox.GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            //When Player was attacked during StandingDamage
            PlayerStandingDamageHurtBox
                .OnTriggerEnter2DAsObservable()
                .Where(x => x.gameObject.tag == "Enemy/HitBox")
                .ThrottleFirstFrame(1)
                .Subscribe(_ => wasAttackedDuringStandingDamage = true);

            // Animation
            #region StandingDamage->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.StandingDamage"))
                .Where(x => !PlayerState.WasAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", true);
                    Animator.SetBool("IsStandingDamage", false);
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
            Animator.Play("StandingDamage", Animator.GetLayerIndex("Base Layer"), 0.0f);
            isKnockBack = true;
            BoxCollider2D.enabled = true;
            CircleCollider2D.enabled = true;
            PlayerStandingDamageHurtBoxTrigger.enabled = true;
            Key.IsAvailable.Value = false;
            PlayerState.WasAttacked.Value = true;

            // Apply Damage
            Status.Hp.Value -= damageValue;

            // Recover
            for (int i = 0; i < recovery; i++)
            {
                yield return null;
                
                if(wasAttackedDuringStandingDamage)
                {
                    wasAttackedDuringStandingDamage = false;
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
            CircleCollider2D.enabled = false;
            PlayerStandingDamageHurtBoxTrigger.enabled = false;
            Key.IsAvailable.Value = true;
        }
    }
}