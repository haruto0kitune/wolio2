using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UniRx;
using UniRx.Triggers;

namespace MyUtility
{
    public class AnimatorPauser : MonoBehaviour
    {
        static List<AnimatorPauser> Pausers;
        private Animator Animator;
        private ReactiveProperty<bool> isPausing;

        void Awake()
        {
            Pausers = new List<AnimatorPauser>();
            Animator = GetComponent<Animator>();
            isPausing = new ReactiveProperty<bool>();
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
                obj.Animator.speed = 0;
            }
        }

        public static void Resume()
        {
            foreach (var obj in Pausers)
            {
                obj.Animator.speed = 1;
            }
        }
    }
}