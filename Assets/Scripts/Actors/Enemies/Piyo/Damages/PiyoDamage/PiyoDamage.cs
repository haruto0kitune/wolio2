using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Piyo.Damages
{
    public class PiyoDamage : MonoBehaviour, IDamage
    {
        [SerializeField]
        GameObject Piyo;
        [SerializeField]
        GameObject PiyoDamageHurtBox;
        Animator Animator;
        ObservableStateMachineTrigger ObservableStateMachineTrigger;
        PiyoState PiyoState;
        PiyoStatus Status;
        Rigidbody2D PiyoRigidbody2D;
        BoxCollider2D BoxCollider2D;
        BoxCollider2D PiyoDamageHurtBoxTrigger;
        bool wasAttackedDuringDamage = false;
        bool isKnockBack = false;
        int knockBackFrame;
        Coroutine damageCoroutineStore;
        
        void Awake()
        {
            Animator = Piyo.GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            PiyoState = Piyo.GetComponent<PiyoState>();
            Status = Piyo.GetComponent<PiyoStatus>();
            PiyoRigidbody2D = Piyo.GetComponent<Rigidbody2D>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            PiyoDamageHurtBoxTrigger = PiyoDamageHurtBox.GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            // Animation
            #region Damage->Run
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.PiyoDamage"))
                .Where(x => !PiyoState.WasAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsRunning", true);
                    Animator.SetBool("IsDamaged", false);
                });
            #endregion
            #region Damage->Damage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.PiyoDamage"))
                .Where(x => wasAttackedDuringDamage)
                .Subscribe(_ =>
                {
                    Animator.Play("PiyoDamage", Animator.GetLayerIndex("Base Layer"), 0.0f);
                });
            #endregion

            // Motion (KnockBack)
            this.FixedUpdateAsObservable()
                .Where(x => isKnockBack) 
                .Subscribe(x =>
                {
                    knockBackFrame++;

                    if (PiyoState.FacingRight.Value)
                    {
                        PiyoRigidbody2D.velocity = new Vector2(-1f, 0);
                    }
                    else
                    {
                        PiyoRigidbody2D.velocity = new Vector2(1f, 0);
                    }

                    if(knockBackFrame == 10)
                    {
                        isKnockBack = false;
                        knockBackFrame = 0;
                    }
                });
        }

        public void Damage(int damageValue, int recovery, int hitStop)
        {
            if(damageCoroutineStore == null)
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
            isKnockBack = true;
            BoxCollider2D.enabled = true;
            PiyoDamageHurtBoxTrigger.enabled = true;
            PiyoState.WasAttacked.Value = true;

            // Apply Damage
            Status.Hp.Value -= damageValue;

            // Recover
            //for (int i = 0; i < recovery; i++)
            //{
            //    yield return null;
            //}
            for (int i = 0; i < hitStop; i++)
            {
                yield return null;
            }

            // Finish
            //
            // When transtion to next state, collider enabled is off.
            // if not, PiyoDamage becomes strange motion.
            PiyoState.WasAttacked.Value = false;
            wasAttackedDuringDamage = false;

            yield return null;
            
            BoxCollider2D.enabled = false;
            PiyoDamageHurtBoxTrigger.enabled = false;
        }
    }
}
