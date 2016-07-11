using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Attacks.NormalAttacks.Throws
{
    public class PlayerGrab : MonoBehaviour
    {
        [SerializeField]
        GameObject Player;
        Rigidbody2D Rigidbody2D;
        Animator Animator;
        ObservableStateMachineTrigger ObservableStateMachineTrigger;
        BoxCollider2D BoxCollider2D;
        CircleCollider2D CircleCollider2D;
        PlayerState PlayerState;
        [SerializeField]
        GameObject PlayerThrow;
        [SerializeField]
        GameObject GrabHitBox;
        BoxCollider2D HitBox;
        [SerializeField]
        GameObject GrabHurtBox;
        BoxCollider2D HurtBox;
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

        bool hitGrab;
        bool wasCanceled;

        void Awake()
        {
            Rigidbody2D = Player.GetComponent<Rigidbody2D>();
            Animator = Player.GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            CircleCollider2D = GetComponent<CircleCollider2D>();
            PlayerState = Player.GetComponent<PlayerState>();
            HitBox = GrabHitBox.GetComponent<BoxCollider2D>();
            HurtBox = GrabHurtBox.GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            // Animation
            #region EnterGrab
            ObservableStateMachineTrigger
                .OnStateEnterAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Grab"))
                .Do(x => Debug.Log("Enter Grab"))
                .Subscribe(_ => coroutineStore = StartCoroutine(GrabCoroutine()));
            #endregion
            #region Grab->Throw
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Grab"))
                .Where(x => hitGrab) 
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsGrabbing", false);
                    Animator.SetBool("IsThrowing", true);
                    hitGrab = false;
                });
            #endregion
            #region Grab->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Grab"))
                .Where(x => hasFinished) 
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsGrabbing", false);
                    Animator.SetBool("IsStanding", true);
                    hasFinished = false;
                });
            #endregion
            #region Grab->SupineJumpingDamage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Grab"))
                .Where(x => PlayerState.WasSupineAttributeAttacked.Value && PlayerState.WasKnockdownAttributeAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsGrabbing", false);
                    Animator.SetBool("IsSupineJumpingDamage", true);
                });
            #endregion
            #region Grab->ProneJumpingDamage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Grab"))
                .Where(x => PlayerState.WasProneAttributeAttacked.Value && PlayerState.WasKnockdownAttributeAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsGrabbing", false);
                    Animator.SetBool("IsProneJumpingDamage", true);
                });
            #endregion

            // Collision
            this.ObserveEveryValueChanged(x => wasCanceled)
                .Where(x => x)
                .Subscribe(_ => Cancel());

            this.ObserveEveryValueChanged(x => PlayerState.WasAttacked.Value)
                .Where(x => PlayerState.IsGrabbing.Value)
                .Subscribe(_ => wasCanceled = _);

            // Damage
            GrabHitBox.OnTriggerEnter2DAsObservable()
                .Where(x => x.gameObject.tag == "Enemy/HurtBox")
                .Do(x => Debug.Log("hitGrab"))
                .Subscribe(_ =>
                {
                    //_.gameObject.GetComponent<DamageManager>().ApplyDamage(damageValue, hitRecovery, hitStop, isTechable, hasKnockdownAttribute, attackAttribute, knockdownAttribute);
                    PlayerThrow.GetComponent<PlayerThrow>().Enemy = _.gameObject.GetComponent<DamageManager>().Actor;
                    PlayerThrow.GetComponent<PlayerThrow>().DamageManager = _.gameObject.GetComponent<DamageManager>();
                    _.gameObject.GetComponent<DamageManager>().Actor.GetComponent<IState>().WasAttacked.Value = true;
                    hitGrab = true;
                    HitBox.enabled = false;
                });
        }

        IEnumerator GrabCoroutine()
        {
            // Startup
            Animator.speed = 1f;
            BoxCollider2D.enabled = true;
            CircleCollider2D.enabled = true;
            HitBox.enabled = true;

            for (int i = 0; i < startup; i++)
            {
                yield return null;
            }

            // Active
            for (int i = 0; i < active; i++)
            {
                yield return null;
            }

            HitBox.enabled = false;

            // Recovery
            for (int i = 0; i < recovery; i++)
            {
                yield return null;
            }

            hasFinished = true;
            yield return null;

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