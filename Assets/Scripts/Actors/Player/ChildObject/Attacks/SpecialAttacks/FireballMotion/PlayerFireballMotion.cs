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
        GameObject HurtBox;
        BoxCollider2D HurtBoxTrigger;
        bool hasFinished;
        Coroutine coroutineStore;
        [SerializeField]
        int startup;
        [SerializeField]
        int recovery;

        void Awake()
        {
            Animator = Player.GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            CircleCollider2D = GetComponent<CircleCollider2D>();
            PlayerState = Player.GetComponent<PlayerState>();
            HurtBoxTrigger = HurtBox.GetComponent<BoxCollider2D>();
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
        }

        IEnumerator FireballMotionCoroutine()
        {
            // Startup
            BoxCollider2D.enabled = true;
            CircleCollider2D.enabled = true;
            HurtBoxTrigger.enabled = true;

            while (Animator.GetCurrentAnimatorStateInfo(Animator.GetLayerIndex("Base Layer")).normalizedTime <= 1.0f)
            {
                yield return null;
            }

            FireballStore = Instantiate(Fireball);

            if(PlayerState.FacingRight.Value)
            {
                FireballStore.GetComponent<PlayerFireball>().Initialize(transform.position, 2, 1);
            }
            else
            {
                FireballStore.GetComponent<PlayerFireball>().Initialize(transform.position, 2, -1);
            }

            // Recovery
            for (int i = 0; i < recovery; i++)
            {
                yield return null;
            }

            BoxCollider2D.enabled = false;
            CircleCollider2D.enabled = false;
            HurtBoxTrigger.enabled = false;
            hasFinished = true;
        }
    }
}