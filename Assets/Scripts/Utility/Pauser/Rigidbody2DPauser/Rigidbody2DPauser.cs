using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UniRx;
using UniRx.Triggers;

namespace MyUtility
{
    public class Rigidbody2DPauser : MonoBehaviour
    {
        static List<Rigidbody2DPauser> Pausers;
        private Rigidbody2D Rigidbody2D;
        private Vector2 VelocityStore;
        private float AngularVelocityStore;

        void Awake()
        {
            Pausers = new List<Rigidbody2DPauser>();
            Rigidbody2D = GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            Pausers.Add(this);
        }

        void OnDestroy()
        {
            Pausers.Remove(this);
        }

        public static void Pause()
        {
            foreach (var obj in Pausers)
            {
                obj.VelocityStore = obj.Rigidbody2D.velocity;
                obj.AngularVelocityStore = obj.Rigidbody2D.angularVelocity;
                obj.Rigidbody2D.Sleep();
            }
        }

        public static void Resume()
        {
            foreach (var obj in Pausers)
            {
                obj.Rigidbody2D.WakeUp();
                obj.Rigidbody2D.velocity = obj.VelocityStore;
                obj.Rigidbody2D.angularVelocity = obj.AngularVelocityStore;
            }
        }
    }
}
