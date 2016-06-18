using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.FlyingPiyo.Damages
{
    public class FlyingPiyoDamage : MonoBehaviour, IDamage
    {
        [SerializeField]
        GameObject FlyingPiyo;
        [SerializeField]
        GameObject FlyingPiyoDamageHurtBox;
        Animator Animator;
        ObservableStateMachineTrigger ObservableStateMachineTrigger;
        FlyingPiyoState FlyingPiyoState;
        FlyingPiyoStatus Status;
        Rigidbody2D FlyingPiyoRigidbody2D;
        BoxCollider2D BoxCollider2D;
        BoxCollider2D FlyingPiyoDamageHurtBoxTrigger;
        bool wasAttackedDuringDamage = false;
        bool isKnockBack = false;
        int knockBackFrame;
        Coroutine damageCoroutineStore;

        void Awake()
        {
            Animator = FlyingPiyo.GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            FlyingPiyoState = FlyingPiyo.GetComponent<FlyingPiyoState>();
            Status = FlyingPiyo.GetComponent<FlyingPiyoStatus>();
            FlyingPiyoRigidbody2D = FlyingPiyo.GetComponent<Rigidbody2D>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            FlyingPiyoDamageHurtBoxTrigger = FlyingPiyoDamageHurtBox.GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            // Animation
            #region Damage->Fly
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.FlyingPiyoDamage"))
                .Where(x => !FlyingPiyoState.WasAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsFlying", true);
                    Animator.SetBool("IsDamaged", false);
                });
            #endregion

            // Motion (KnockBack)
            this.FixedUpdateAsObservable()
                .Where(x => isKnockBack)
                .Subscribe(x =>
                {
                    knockBackFrame++;

                    if (FlyingPiyoState.FacingRight.Value)
                    {
                        FlyingPiyoRigidbody2D.velocity = new Vector2(-1f, 0);
                    }
                    else
                    {
                        FlyingPiyoRigidbody2D.velocity = new Vector2(1f, 0);
                    }

                    if (knockBackFrame == 10)
                    {
                        isKnockBack = false;
                        knockBackFrame = 0;
                    }
                });
        }

        public void Damage(int damageValue, int recovery, int hitStop, bool isTechable, bool hasKnockdownAttribute, AttackAttribute attackAttribute, KnockdownAttribute knockdownAttribute)
        {
            if(damageCoroutineStore == null)
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
            Animator.Play("FlyingPiyoDamage", Animator.GetLayerIndex("Base Layer"), 0.0f);
            isKnockBack = true;
            BoxCollider2D.enabled = true;
            FlyingPiyoDamageHurtBoxTrigger.enabled = true;
            FlyingPiyoState.WasAttacked.Value = true;

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
            // if not, FlyingPiyoDamage becomes strange motion.
            FlyingPiyoState.WasAttacked.Value = false;
            wasAttackedDuringDamage = false;

            yield return null;

            BoxCollider2D.enabled = false;
            FlyingPiyoDamageHurtBoxTrigger.enabled = false;
        }
    }
}