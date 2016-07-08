using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Basics
{
    public class PlayerCreep : MonoBehaviour
    {
        Animator Animator;
        ObservableStateMachineTrigger ObservableStateMachineTrigger;
        PlayerState PlayerState;
        Rigidbody2D PlayerRigidbody2D;
        Key Key;
        BoxCollider2D BoxCollider2D;
        BoxCollider2D HurtBox;
        [SerializeField]
        float Speed;

        void Awake()
        {
            Animator = GameObject.Find("Test").GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            PlayerState = GameObject.Find("Test").GetComponent<PlayerState>();
            PlayerRigidbody2D = GameObject.Find("Test").GetComponent<Rigidbody2D>();
            Key = GameObject.Find("Test").GetComponent<Key>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            HurtBox = GameObject.Find("CreepHurtBox").GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            //Animation
            #region Creep->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Creep"))
                .Where(x => Key.Horizontal.Value == 0 && Key.Vertical.Value == 0)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsStanding", true);
                    Animator.SetBool("IsCreeping", false);
                });
            #endregion
            #region Creep->Run
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Creep"))
                .Where(x => Key.Horizontal.Value != 0 && Key.Vertical.Value == 0)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsRunning", true);
                    Animator.SetBool("IsCreeping", false);
                });
            #endregion
            #region Creep->Crouch 
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Creep"))
                .Where(x => Key.Horizontal.Value == 0 && Key.Vertical.Value == -1)
                .Subscribe(_ =>
                {
                    Debug.Log("Creep->Crouch");
                    Animator.SetBool("IsCrouching", true);
                    Animator.SetBool("IsCreeping", false);
                });
            #endregion
            #region Creep->LightDragonPunch
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Creep"))
                .Where(x => PlayerState.hasInputedLightDragonPunchCommand.Value)
                .Subscribe(_ =>
                {
                    Debug.Log("Creep->DragonPunch");
                    Animator.SetBool("IsCreeping", false);
                    Animator.SetBool("IsLightDragonPunch", true);
                });
            #endregion
            #region Creep->MiddleDragonPunch
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Creep"))
                .Where(x => PlayerState.hasInputedMiddleDragonPunchCommand.Value)
                .Subscribe(_ =>
                {
                    Debug.Log("Creep->DragonPunch");
                    Animator.SetBool("IsCreeping", false);
                    Animator.SetBool("IsMiddleDragonPunch", true);
                });
            #endregion
            #region Creep->HighDragonPunch
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Creep"))
                .Where(x => PlayerState.hasInputedHighDragonPunchCommand.Value)
                .Subscribe(_ =>
                {
                    Debug.Log("Creep->DragonPunch");
                    Animator.SetBool("IsCreeping", false);
                    Animator.SetBool("IsHighDragonPunch", true);
                });
            #endregion

            PlayerState.IsCrouching
                .Where(x => x)
                .Subscribe(x => Debug.Log("IsCrouching: true"));

            PlayerState.IsCreeping
                .Where(x => x)
                .Subscribe(x => Debug.Log("IsCreeping: true"));

            //Motion
            this.FixedUpdateAsObservable()
                .Where(x => PlayerState.canCreep.Value)
                .Where(x => Key.Horizontal.Value != 0 && Key.Vertical.Value == -1)
                .Subscribe(_ => this.Creep(Key.Horizontal.Value, Speed));

            //Collision
            this.ObserveEveryValueChanged(x => Animator.GetBool("IsCreeping"))
                .Where(x => x)
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = true;
                    HurtBox.enabled = true;
                });

            this.ObserveEveryValueChanged(x => Animator.GetBool("IsCreeping"))
                .Where(x => !x)
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = false;
                    HurtBox.enabled = false;
                    PlayerRigidbody2D.velocity = Vector2.zero;
                });
        }

        public void Creep(float Horizontal, float CreepSpeed)
        {
            PlayerRigidbody2D.velocity = new Vector2(Horizontal * CreepSpeed, PlayerRigidbody2D.velocity.y);
        }
    }
}
