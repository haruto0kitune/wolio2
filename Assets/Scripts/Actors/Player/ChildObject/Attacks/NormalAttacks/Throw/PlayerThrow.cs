using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Attacks.NormalAttacks.Throws
{
    public class PlayerThrow : MonoBehaviour
    {
        [SerializeField]
        GameObject Player;
        Rigidbody2D Rigidbody2D;
        Animator Animator;
        ObservableStateMachineTrigger ObservableStateMachineTrigger;
        BoxCollider2D BoxCollider2D;
        CircleCollider2D CircleCollider2D;
        PlayerState PlayerState;
        bool hasFinished;
        Coroutine coroutineStore;
        [SerializeField]
        int startup;
        [SerializeField]
        int active;
        [SerializeField]
        int recovery;
        [SerializeField]
        int damageValue;
        [SerializeField]
        int hitRecovery;
        [SerializeField]
        int hitStop;
        [SerializeField]
        bool isTechable;
        [SerializeField]
        bool hasKnockdownAttribute;
        [SerializeField]
        AttackAttribute attackAttribute;
        [SerializeField]
        KnockdownAttribute knockdownAttribute;

        public GameObject Enemy { set; get; }
        public DamageManager DamageManager { set; get; }
        bool wasCanceled;

        void Awake()
        {
            Rigidbody2D = Player.GetComponent<Rigidbody2D>();
            Animator = Player.GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            CircleCollider2D = GetComponent<CircleCollider2D>();
            PlayerState = Player.GetComponent<PlayerState>();
        }

        void Start()
        {
            // Animation
            #region EnterThrow
            ObservableStateMachineTrigger
                .OnStateEnterAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Throw"))
                .Do(x => Debug.Log("Enter Throw"))
                .Subscribe(_ => coroutineStore = StartCoroutine(ThrowCoroutine()));
            #endregion
            #region Throw->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Throw"))
                .Where(x => hasFinished && PlayerState.IsGrounded.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsThrowing", false);
                    Animator.SetBool("IsStanding", true);
                    hasFinished = false;
                });
            #endregion
        }

        IEnumerator ThrowCoroutine()
        {
            // Startup
            Animator.speed = 1f;
            BoxCollider2D.enabled = true;
            CircleCollider2D.enabled = true;

            if (PlayerState.FacingRight.Value)
            {
                Enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(-1, Enemy.GetComponent<Rigidbody2D>().velocity.y);
            }
            else
            {
                Enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(1, Enemy.GetComponent<Rigidbody2D>().velocity.y);
            }

            for (int i = 0; i < startup; i++)
            {
                yield return null;
            }

            // Active
            //DamageManager.ApplyDamage(damageValue, hitRecovery, hitStop, isTechable, hasKnockdownAttribute, attackAttribute, knockdownAttribute);

            if (PlayerState.FacingRight.Value)
            {
                Enemy.GetComponent<Rigidbody2D>().velocity = Utility.PolarToRectangular2D(60, 3f);
            }
            else
            {
                Enemy.GetComponent<Rigidbody2D>().velocity = Utility.PolarToRectangular2D(120, 3f);
            }
           
            // Recovery
            for (int i = 0; i < recovery; i++)
            {
                yield return null;
            }

            hasFinished = true;
            yield return null;

            BoxCollider2D.enabled = false;
            CircleCollider2D.enabled = false;
            Enemy = null;
            DamageManager = null;
        }
    }
}