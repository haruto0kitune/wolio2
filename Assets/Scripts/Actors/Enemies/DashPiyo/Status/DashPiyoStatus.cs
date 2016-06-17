using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Wolio.Actor.Enemy.DashPiyo
{
    public class DashPiyoStatus : MonoBehaviour
    {
        [InspectorDisplay]
        public IntReactiveProperty Hp;

        void Start()
        {

        }
    }
}
