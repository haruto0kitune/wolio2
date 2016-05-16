using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Attacks.NormalAttacks.StandingAttacks
{
    public class PlayerStandingHighAttack : MonoBehaviour
    {
        Animator Animator;
        ObservableStateMachineTrigger ObservableStateMachineTrigger;
        PlayerState PlayerState;
        Rigidbody2D PlayerRigidbody2D;
        Key Key;
        BoxCollider2D BoxCollider2D;
        CircleCollider2D CircleCollider2D;
        BoxCollider2D HitBox;
        BoxCollider2D HurtBox;
        [SerializeField]
        int Startup;
        [SerializeField]
        int Active;
        [SerializeField]
        int Recovery;

        void Awake()
        {
            Animator = GameObject.Find("Test").GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            PlayerState = GameObject.Find("Test").GetComponent<PlayerState>();
            PlayerRigidbody2D = GameObject.Find("Test").GetComponent<Rigidbody2D>();
            Key = GameObject.Find("Test").GetComponent<Key>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            CircleCollider2D = GetComponent<CircleCollider2D>();
            HitBox = GameObject.Find("StandingHighAttackHitBox").GetComponent<BoxCollider2D>();
            HurtBox = GameObject.Find("StandingHighAttackHurtBox").GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            //Animation
            #region StandingMiddleAttack
            ObservableStateMachineTrigger
                 .OnStateEnterAsObservable()
                 .Where(x => x.StateInfo.IsName("Base Layer.StandingHighAttack"))
                 .Subscribe(_ => Animator.speed = 0);
            #endregion

            //Collision and Motion
            this.ObserveEveryValueChanged(x => Animator.GetBool("IsStandingHighAttack"))
                .Where(x => x)
                .Subscribe(_ => StartCoroutine(Attack()));
        }

        public IEnumerator Attack()
        {
            #region Startup
            BoxCollider2D.enabled = true;
            CircleCollider2D.enabled = true;
            HurtBox.enabled = true;

            for (int i = 0; i < Startup; i++)
            {
                yield return null;
            }
            #endregion
            #region Active
            Animator.speed = 1;
            HitBox.enabled = true;

            for (var i = 0; i < Active; i++)
            {
                yield return null;
            }

            HitBox.enabled = false;
            #endregion
            #region Recovery
            for (int i = 0; i < Recovery; i++)
            {
                yield return null;
            }

            BoxCollider2D.enabled = false;
            CircleCollider2D.enabled = false;
            HurtBox.enabled = false;
            #endregion
            #region StandingHighAttack->Stand
            Animator.SetBool("IsStanding", true);
            Animator.SetBool("IsStandingHighAttack", false);
            #endregion
        }
    }
}