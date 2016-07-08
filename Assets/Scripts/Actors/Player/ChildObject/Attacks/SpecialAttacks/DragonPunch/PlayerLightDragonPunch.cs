using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Attacks.SpecialAttacks
{
    public class PlayerLightDragonPunch : MonoBehaviour
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
        GameObject DragonPunchHitBox;
        BoxCollider2D HitBox;
        [SerializeField]
        GameObject DragonPunchHurtBox;
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

        bool wasCanceled;

        void Awake()
        {
            Rigidbody2D = Player.GetComponent<Rigidbody2D>();
            Animator = Player.GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            CircleCollider2D = GetComponent<CircleCollider2D>();
            PlayerState = Player.GetComponent<PlayerState>();
            HitBox = DragonPunchHitBox.GetComponent<BoxCollider2D>();
            HurtBox = DragonPunchHurtBox.GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            // Animation
            #region EnterDragonPunch
            ObservableStateMachineTrigger
                .OnStateEnterAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.LightDragonPunch"))
                .Do(x => Debug.Log("Enter DragonPunch"))
                .Subscribe(_ => coroutineStore = StartCoroutine(LightDragonPunchCoroutine()));
            #endregion
            #region DragonPunch->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.LightDragonPunch"))
                .Where(x => hasFinished && PlayerState.IsGrounded.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsLightDragonPunch", false);
                    Animator.SetBool("IsStanding", true);
                    hasFinished = false;
                    PlayerState.hasInputedLightDragonPunchCommand.Value = false;
                });
            #endregion
            #region DragonPunch->SupineJumpingDamage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.LightDragonPunch"))
                .Where(x => PlayerState.WasSupineAttributeAttacked.Value && PlayerState.WasKnockdownAttributeAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsLightDragonPunch", false);
                    Animator.SetBool("IsSupineJumpingDamage", true);
                });
            #endregion
            #region DragonPunch->ProneJumpingDamage
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.LightDragonPunch"))
                .Where(x => PlayerState.WasProneAttributeAttacked.Value && PlayerState.WasKnockdownAttributeAttacked.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsLightDragonPunch", false);
                    Animator.SetBool("IsProneJumpingDamage", true);
                });
            #endregion

            // Collision
            this.ObserveEveryValueChanged(x => wasCanceled)
                .Where(x => x)
                .Subscribe(_ => Cancel());

            this.ObserveEveryValueChanged(x => PlayerState.WasAttacked.Value)
                .Where(x => PlayerState.IsDragonPunch.Value)
                .Subscribe(_ => wasCanceled = _);

            this.UpdateAsObservable()
                .Where(x => !gameObject.activeSelf)
                .Subscribe(x => Debug.Log("DragonPunch Active: " + gameObject.activeSelf));

            
            // Damage
            DragonPunchHitBox.OnTriggerEnter2DAsObservable()
                .Where(x => x.gameObject.tag == "Enemy/HurtBox")
                .Subscribe(_ =>
                {
                    _.gameObject.GetComponent<DamageManager>().ApplyDamage(damageValue, hitRecovery, hitStop, isTechable, hasKnockdownAttribute, attackAttribute, knockdownAttribute);
                    HitBox.enabled = false;
                });

        }

        IEnumerator LightDragonPunchCoroutine()
        {
            Debug.Log("DragonPunch");
            // Startup
            BoxCollider2D.enabled = true;
            CircleCollider2D.enabled = true;
            HitBox.enabled = true;

            for (int i = 0; i < startup; i++)
            {
                yield return null;
            }

            // Active
            if (PlayerState.FacingRight.Value)
            {
                Rigidbody2D.velocity = new Vector2(1f, 10f);
            }
            else
            {
                Rigidbody2D.velocity = new Vector2(-1f, 10f);
            }

            for (int i = 0; i < active; i++)
            {
                if (active - 6 <= i) Rigidbody2D.velocity = new Vector2(0, Rigidbody2D.velocity.y);
                yield return null;
            }

            // Recovery
            while (!PlayerState.IsGrounded.Value)
            {
                yield return null;
            }

            hasFinished = true;
            yield return null;

            BoxCollider2D.enabled = false;
            CircleCollider2D.enabled = false;
            HitBox.enabled = false;
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