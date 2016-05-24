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
        }

        // Execute DamageManager
        public IEnumerator Damage(int damageValue, int recovery)
        {
            // StartUp
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
            }

            // Finish
            BoxCollider2D.enabled = false;
            CircleCollider2D.enabled = false;
            PlayerStandingDamageHurtBoxTrigger.enabled = false;
            Key.IsAvailable.Value = true;
            PlayerState.WasAttacked.Value = false;
        }
    }
}