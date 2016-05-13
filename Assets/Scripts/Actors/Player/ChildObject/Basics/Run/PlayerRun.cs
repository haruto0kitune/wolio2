using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Player
{
    public class PlayerRun : MonoBehaviour
    {
        PlayerState PlayerState;
        BoxCollider2D BoxCollider2D;
        CircleCollider2D CircleCollider2D;

        void Awake()
        {
            PlayerState = GameObject.Find("Test").GetComponent<PlayerState>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            CircleCollider2D = GetComponent<CircleCollider2D>();
        }

        void Start()
        {
            this.UpdateAsObservable()
                .Where(x => BoxCollider2D.enabled == true || CircleCollider2D.enabled == true)
                .Subscribe(_ =>
                {
                    Debug.Log(BoxCollider2D.enabled);
                    Debug.Log(CircleCollider2D.enabled);
                });

            PlayerState.IsRunning
                .Where(x => x)
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = true;
                    CircleCollider2D.enabled = true;
                });

            PlayerState.IsRunning
                .Where(x => !x)
                .Subscribe(_ =>
                {
                    BoxCollider2D.enabled = false;
                    CircleCollider2D.enabled = false;
                });
        }
    }
}
