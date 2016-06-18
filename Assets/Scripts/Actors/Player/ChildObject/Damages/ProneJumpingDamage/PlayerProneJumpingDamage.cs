using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Damages
{
    public class PlayerProneJumpingDamage : MonoBehaviour, IDamage
    {
        [SerializeField]
        GameObject Player;
        [SerializeField]
        GameObject PlayerProneJumpingDamageHurtBox;
        Animator Animator;
        ObservableStateMachineTrigger ObservableStateMachineTrigger;
        PlayerState PlayerState;
        Status Status;
        Rigidbody2D PlayerRigidbody2D;
        Key Key;
        BoxCollider2D BoxCollider2D;
        BoxCollider2D PlayerProneJumpingDamageHurtBoxTrigger;
        bool wasAttackedDuringDamage = false;
        int knockBackFrame;
        Coroutine damageCoroutineStore;
        bool hasFinishedHitStop;

        void Awake()
        {
            Animator = Player.GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            PlayerState = Player.GetComponent<PlayerState>();
            Status = Player.GetComponent<Status>();
            PlayerRigidbody2D = Player.GetComponent<Rigidbody2D>();
            Key = Player.GetComponent<Key>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            PlayerProneJumpingDamageHurtBoxTrigger = PlayerProneJumpingDamageHurtBox.GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            // Animation
            #region EnterProneJumpingDamage
            ObservableStateMachineTrigger
                .OnStateEnterAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.ProneJumpingDamage"))
                .Subscribe(_ => Animator.speed = 0f);
            #endregion
            #region ProneJumpingDamage->AirTech
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.ProneJumpingDamage"))
                .Where(x => PlayerState.canAirTech.Value)
                .Where(x => Key.Z)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsAirTech", true);
                    Animator.SetBool("IsProneJumpingDamage", false);
                });
            #endregion
            #region ProneJumpingDamage->ProneKnockdown
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.ProneJumpingDamage"))
                .Where(x => PlayerState.IsGrounded.Value)
                .Where(x => hasFinishedHitStop)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsProneKnockdown", true);
                    Animator.SetBool("IsProneJumpingDamage", false);
                });
            #endregion
        }

        public void Damage(int damageValue, int recovery, int hitStop, bool isTechable, bool hasKnockdownAttribute, AttackAttribute attackAttribute, KnockdownAttribute knockdownAttribute)
        {
            if (damageCoroutineStore == null)
            {
                damageCoroutineStore = StartCoroutine(DamageCoroutine(damageValue, recovery, hitStop, isTechable, hasKnockdownAttribute, attackAttribute, knockdownAttribute));
            }
            else
            {
                StopCoroutine(damageCoroutineStore);
                damageCoroutineStore = StartCoroutine(DamageCoroutine(damageValue, recovery, hitStop, isTechable, hasKnockdownAttribute, attackAttribute, knockdownAttribute));
                wasAttackedDuringDamage = true;
            }
        }

        // Execute DamageManager
        public IEnumerator DamageCoroutine(int damageValue, int recovery, int hitStop, bool isTechable, bool hasKnockdownAttribute, AttackAttribute attackAttribute, KnockdownAttribute knockdownAttribute)
        {
            // StartUp
            BoxCollider2D.enabled = true;
            PlayerProneJumpingDamageHurtBoxTrigger.enabled = true;
            PlayerState.WasAttacked.Value = true;
            PlayerState.WasProneAttributeAttacked.Value = true;
            PlayerState.WasKnockdownAttributeAttacked.Value = hasKnockdownAttribute;
            PlayerState.canAirTech.Value = false;

            // Apply Damage
            Status.Hp.Value -= damageValue;

            // HitStop
            hasFinishedHitStop = false;
            var x = 0.1f;
            PlayerRigidbody2D.isKinematic = true;

            for (int i = 0; i < hitStop; i++)
            {
                PlayerRigidbody2D.velocity = new Vector2(x, 0);
                x *= -1;
                yield return null;
            }

            PlayerRigidbody2D.isKinematic = false;
            Animator.speed = 1f;

            // Knockback
            var Vector = Utility.PolarToRectangular2D(90, 3);

            if (PlayerState.FacingRight.Value)
            {
                PlayerRigidbody2D.velocity = new Vector2(Vector.x * -1, Vector.y);
            }
            else
            {
                PlayerRigidbody2D.velocity = new Vector2(Vector.x, Vector.y);
            }

            for (int i = 0; i < recovery; i++)
            {
                yield return null;
                hasFinishedHitStop = true;
            }

            // AirTechable Time
            PlayerState.canAirTech.Value = true;

            while (!Key.Z)
            {
                if (PlayerState.IsGrounded.Value)
                {
                    break;
                }

                yield return null;
            }

            // Finish
            //
            // When transtion to next state, collider enabled is off.
            // if not, PlayerProneJumpingDamage becomes strange motion.
            PlayerState.WasAttacked.Value = false;
            PlayerState.WasProneAttributeAttacked.Value = false;
            PlayerState.WasKnockdownAttributeAttacked.Value = false;
            wasAttackedDuringDamage = false;
            hasFinishedHitStop = false;

            yield return null;

            BoxCollider2D.enabled = false;
            PlayerProneJumpingDamageHurtBoxTrigger.enabled = false;
        }
    }
}