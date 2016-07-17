using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Basics
{
    public class PlayerLand : MonoBehaviour
    {
        [SerializeField]
        GameObject Actor;
        Animator Animator;
        ObservableStateMachineTrigger ObservableStateMachineTrigger;
        PlayerState PlayerState;
        Key Key;
        BoxCollider2D BoxCollider2D;
        CircleCollider2D CircleCollider2D;
        [SerializeField]
        GameObject PlayerLandHurtBox;
        BoxCollider2D HurtBox;
        bool hasFinished;
        [SerializeField]
        int recovery;
        Coroutine coroutineStore;
        bool wasCanceled;

        void Awake()
        {
            Animator = Actor.GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            PlayerState = Actor.GetComponent<PlayerState>();
            Key = Actor.GetComponent<Key>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            CircleCollider2D = GetComponent<CircleCollider2D>();
            HurtBox = PlayerLandHurtBox.GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            // Animation
            #region Land->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Land"))
                .Where(x =>  hasFinished)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsLanding", false);
                    Animator.SetBool("IsStanding", true);
                    hasFinished = false;
                });
            #endregion
            #region Land->StandingDamage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Land"))
                .Where(x => PlayerState.WasAttacked.Value && !PlayerState.WasKnockdownAttributeAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsLanding", false);
                    Animator.SetBool("IsStandingDamage", true);
                });
            #endregion
            #region Land->SupineJumpingDamage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Land"))
                .Where(x => PlayerState.WasSupineAttributeAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsLanding", false);
                    Animator.SetBool("IsSupineJumpingDamage", true);
                });
            #endregion
            #region Land->ProneJumpingDamage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Land"))
                .Where(x => PlayerState.WasProneAttributeAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsLanding", false);
                    Animator.SetBool("IsProneJumpingDamage", true);
                });
            #endregion
            #region Land->Jump
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Land"))
                .Where(x => hasFinished)
                .Where(x => PlayerState.canFightingModeJump.Value)
                .Where(x => Key.Vertical.Value == 1)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsLanding", false);
                    Animator.SetBool("IsFightingModeJumping", true);
                });
            #endregion

            // Motion
            ObservableStateMachineTrigger
                .OnStateEnterAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Land"))
                .Subscribe(_ => coroutineStore = StartCoroutine(Land()));

            // Collision
            this.ObserveEveryValueChanged(x => Animator.GetBool("IsLanding"))
                .Where(x => x)
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = true;
                    CircleCollider2D.enabled = true;
                    HurtBox.enabled = true;
                });

            this.ObserveEveryValueChanged(x => Animator.GetBool("IsLanding"))
                .Where(x => !x)
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = false;
                    CircleCollider2D.enabled = false;
                    HurtBox.enabled = false;
                });

            // Cancel
            this.ObserveEveryValueChanged(x => wasCanceled)
                .Where(x => x)
                .Subscribe(_ => Cancel());

            this.ObserveEveryValueChanged(x => PlayerState.WasAttacked.Value)
                .Where(x => x)
                .Subscribe(_ => wasCanceled = _);
        }

        IEnumerator Land()
        {
            for (int i = 0; i < recovery; i++)
            {
                if (Animator.GetCurrentAnimatorStateInfo(Animator.GetLayerIndex("Base Layer")).normalizedTime == 1)
                {
                    hasFinished = true;
                    break;
                }
                yield return null;
            }

            hasFinished = true;
        }

        void Cancel()
        {
            StopCoroutine(coroutineStore);

            // Collision disable
            BoxCollider2D.enabled = false;
            CircleCollider2D.enabled = false;
            HurtBox.enabled = false;

            wasCanceled = false;
        }
    }
}