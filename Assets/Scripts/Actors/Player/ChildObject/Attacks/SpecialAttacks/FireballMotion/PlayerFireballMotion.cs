using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Attacks.SpecialAttacks
{
    public class PlayerFireballMotion : MonoBehaviour
    {
        [SerializeField]
        GameObject Player;
        [SerializeField]
        GameObject Fireball;
        GameObject FireballStore;
        Animator Animator;
        ObservableStateMachineTrigger ObservableStateMachineTrigger;
        BoxCollider2D BoxCollider2D;
        CircleCollider2D CircleCollider2D;
        PlayerState PlayerState;
        [SerializeField]
        GameObject FireballMotionHurtBox;
        BoxCollider2D HurtBox;
        bool hasFinished;
        Coroutine coroutineStore;
        [SerializeField]
        int startup;
        [SerializeField]
        int recovery;
        bool wasCanceled;

        void Awake()
        {
            Animator = Player.GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            CircleCollider2D = GetComponent<CircleCollider2D>();
            PlayerState = Player.GetComponent<PlayerState>();
            HurtBox = FireballMotionHurtBox.GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            // Animation
            #region EnterFireballMotion
            ObservableStateMachineTrigger
                .OnStateEnterAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.FireballMotion"))
                .Subscribe(_ => coroutineStore = StartCoroutine(FireballMotionCoroutine()));
            #endregion
            #region FireballMotion->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.FireballMotion"))
                .Where(x => hasFinished)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsFireballMotion", false);
                    Animator.SetBool("IsStanding", true);
                    hasFinished = false;
                    PlayerState.hasInputedFireballMotionCommand.Value = false;
                });
            #endregion
            #region FireballMotion->StandingDamage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.FireballMotion"))
                .Where(x => PlayerState.WasAttacked.Value && !PlayerState.WasKnockdownAttributeAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsFireballMotion", false);
                    Animator.SetBool("IsStandingDamage", true);
                });
            #endregion
            #region FireballMotion->SupineJumpingDamage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.FireballMotion"))
                .Where(x => PlayerState.WasSupineAttributeAttacked.Value && PlayerState.WasKnockdownAttributeAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsFireballMotion", false);
                    Animator.SetBool("IsSupineJumpingDamage", true);
                });
            #endregion
            #region FireballMotion->ProneJumpingDamage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.FireballMotion"))
                .Where(x => PlayerState.WasProneAttributeAttacked.Value && PlayerState.WasKnockdownAttributeAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsFireballMotion", false);
                    Animator.SetBool("IsProneJumpingDamage", true);
                });
            #endregion
            
            // Collision
            this.ObserveEveryValueChanged(x => wasCanceled)
                .Where(x => x)
                .Subscribe(_ => Cancel());

            this.ObserveEveryValueChanged(x => PlayerState.WasAttacked.Value)
                .Where(x => PlayerState.IsFireballMotion.Value)
                .Subscribe(_ => wasCanceled = _);
        }

        IEnumerator FireballMotionCoroutine()
        {
            // Startup
            BoxCollider2D.enabled = true;
            CircleCollider2D.enabled = true;
            HurtBox.enabled = true;

            while (Animator.GetCurrentAnimatorStateInfo(Animator.GetLayerIndex("Base Layer")).normalizedTime <= 1.0f)
            {
                yield return null;
            }

            FireballStore = Instantiate(Fireball);
            FireballStore.transform.parent = transform;

            if(PlayerState.FacingRight.Value)
            {
                FireballStore.GetComponent<PlayerFireball>().Initialize(new Vector2(transform.position.x + 0.33f, transform.position.y), 2, 1);
            }
            else
            {
                FireballStore.GetComponent<PlayerFireball>().Initialize(new Vector2(transform.position.x - 0.33f, transform.position.y), 2, -1);
            }

            FireballStore.transform.parent = null;
            // Recovery
            for (int i = 0; i < recovery; i++)
            {
                if (i == recovery - 1) hasFinished = true;
                yield return null;
            }

            BoxCollider2D.enabled = false;
            CircleCollider2D.enabled = false;
            HurtBox.enabled = false;
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