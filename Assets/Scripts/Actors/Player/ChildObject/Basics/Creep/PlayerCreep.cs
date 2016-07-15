using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player.Basics
{
    public class PlayerCreep : MonoBehaviour
    {
        [SerializeField]
        GameObject Actor;
        Animator Animator;
        ObservableStateMachineTrigger ObservableStateMachineTrigger;
        PlayerState PlayerState;
        Rigidbody2D PlayerRigidbody2D;
        Key Key;
        BoxCollider2D BoxCollider2D;
        [SerializeField]
        GameObject CreepHurtBox;
        BoxCollider2D HurtBox;
        [SerializeField]
        GameObject CreepCeilingCheckBox;
        BoxCollider2D CeilingCheckBox;
        [SerializeField]
        float Speed;
        bool canTransition;

        void Awake()
        {
            Animator = Actor.GetComponent<Animator>();
            ObservableStateMachineTrigger = Animator.GetBehaviour<ObservableStateMachineTrigger>();
            PlayerState = Actor.GetComponent<PlayerState>();
            PlayerRigidbody2D = Actor.GetComponent<Rigidbody2D>();
            Key = Actor.GetComponent<Key>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            HurtBox = CreepHurtBox.GetComponent<BoxCollider2D>();
            CeilingCheckBox = CreepCeilingCheckBox.GetComponent<BoxCollider2D>();
            canTransition = true;
        }

        void Start()
        {
            //Animation
            #region Creep->Stand
            ObservableStateMachineTrigger
                .OnStateUpdateAsObservable()
                .Where(x => x.StateInfo.IsName("Base Layer.Creep"))
                .Where(x => canTransition)
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
                .Where(x => canTransition)
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
                .Where(x => canTransition)
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
                .Where(x => PlayerState.canDragonPunch.Value)
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
                .Where(x => PlayerState.canDragonPunch.Value)
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
                .Where(x => PlayerState.canDragonPunch.Value)
                .Where(x => PlayerState.hasInputedHighDragonPunchCommand.Value)
                .Subscribe(_ =>
                {
                    Animator.SetBool("IsCreeping", false);
                    Animator.SetBool("IsHighDragonPunch", true);
                });
            #endregion

            //Motion
            this.FixedUpdateAsObservable()
                .Where(x => PlayerState.canCreep.Value)
                .Where(x => Key.Horizontal.Value != 0 && Key.Vertical.Value == -1)
                .Subscribe(_ => this.Creep(Key.Horizontal.Value, Speed));

            this.FixedUpdateAsObservable()
                .Where(x => !canTransition)
                .Where(x => Key.Horizontal.Value == 0)
                .Subscribe(_ => this.Creep(Key.Horizontal.Value, Speed));

            //Collision
            this.ObserveEveryValueChanged(x => Animator.GetBool("IsCreeping"))
                .Where(x => x)
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = true;
                    HurtBox.enabled = true;
                    CeilingCheckBox.enabled = true;
                });

            this.ObserveEveryValueChanged(x => Animator.GetBool("IsCreeping"))
                .Where(x => !x)
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = false;
                    HurtBox.enabled = false;
                    CeilingCheckBox.enabled = false;
                    PlayerRigidbody2D.velocity = Vector2.zero;
                });

            // Trigger
            CeilingCheckBox.OnTriggerStay2DAsObservable()
                .Where(x => x.gameObject.tag == "Field"
                         || x.gameObject.tag == "Hard Platform"
                         || x.gameObject.tag == "Wall")
                .Subscribe(_ => canTransition = false);

            CeilingCheckBox.OnTriggerExit2DAsObservable()
                .Where(x => x.gameObject.tag == "Field"
                         || x.gameObject.tag == "Hard Platform"
                         || x.gameObject.tag == "Wall")
                .Subscribe(_ => canTransition = true);
        }

        public void Creep(float Horizontal, float CreepSpeed)
        {
            PlayerRigidbody2D.velocity = new Vector2(Horizontal * CreepSpeed, PlayerRigidbody2D.velocity.y);
        }
    }
}
