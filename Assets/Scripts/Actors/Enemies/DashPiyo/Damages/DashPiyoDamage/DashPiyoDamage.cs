using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Enemy.DashPiyo.Damages
{
    public class DashPiyoDamage : MonoBehaviour, IDamage
    {
        [SerializeField]
        GameObject DashPiyo;
        [SerializeField]
        GameObject DashPiyoDamageHurtBox;
        Animator Animator;
        ObservableStateMachineTrigger ObservableStateMachineTrigger;
        DashPiyoState DashPiyoState;
        DashPiyoStatus Status;
        Rigidbody2D DashPiyoRigidbody2D;
        BoxCollider2D BoxCollider2D;
        BoxCollider2D DashPiyoDamageHurtBoxTrigger;
        bool wasAttackedDuringDamage = false;
        bool isKnockBack = false;
        int knockBackFrame;
        Coroutine damageCoroutineStore;

        void Awake()
        {
            Animator = DashPiyo.GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            DashPiyoState = DashPiyo.GetComponent<DashPiyoState>();
            Status = DashPiyo.GetComponent<DashPiyoStatus>();
            DashPiyoRigidbody2D = DashPiyo.GetComponent<Rigidbody2D>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            DashPiyoDamageHurtBoxTrigger = DashPiyoDamageHurtBox.GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            // Animation
            #region Damage->Run
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.DashPiyoDamage"))
                .Where(x => !DashPiyoState.WasAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsRunning", true);
                    Animator.SetBool("IsDamaged", false);
                });
            #endregion
            #region Damage->Damage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.DashPiyoDamage"))
                .Where(x => wasAttackedDuringDamage)
                .Subscribe(_ =>
                {
                    Animator.Play("DashPiyoDamage", Animator.GetLayerIndex("Base Layer"), 0.0f);
                });
            #endregion

            // Motion (KnockBack)
            this.FixedUpdateAsObservable()
                .Where(x => isKnockBack)
                .Subscribe(x =>
                {
                    knockBackFrame++;

                    if (DashPiyoState.FacingRight.Value)
                    {
                        DashPiyoRigidbody2D.velocity = new Vector2(-1f, 0);
                    }
                    else
                    {
                        DashPiyoRigidbody2D.velocity = new Vector2(1f, 0);
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
            isKnockBack = true;
            BoxCollider2D.enabled = true;
            DashPiyoDamageHurtBoxTrigger.enabled = true;
            DashPiyoState.WasAttacked.Value = true;

            // Apply Damage
            Status.Hp.Value -= damageValue;

            // Recover
            for (int i = 0; i < hitStop; i++)
            {
                yield return null;
            }

            // Finish
            //
            // When transtion to next state, collider enabled is off.
            // if not, DashPiyoDamage becomes strange motion.
            DashPiyoState.WasAttacked.Value = false;
            wasAttackedDuringDamage = false;

            yield return null;

            BoxCollider2D.enabled = false;
            DashPiyoDamageHurtBoxTrigger.enabled = false;
        }
    }
}