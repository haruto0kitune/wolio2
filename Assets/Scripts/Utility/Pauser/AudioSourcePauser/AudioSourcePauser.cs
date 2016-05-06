using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UniRx;
using UniRx.Triggers;

namespace MyUtility
{
    public class AudioSourcePauser : MonoBehaviour
    {
        static List<AudioSourcePauser> Pausers;
        private AudioSource AudioSource;

        void Awake()
        {
            Pausers = new List<AudioSourcePauser>();
            AudioSource = GetComponent<AudioSource>();
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
                obj.AudioSource.Pause();
            }
        }

        public static void Resume()
        {
            foreach (var obj in Pausers)
            {
                obj.AudioSource.UnPause();
            }
        }
    }
}