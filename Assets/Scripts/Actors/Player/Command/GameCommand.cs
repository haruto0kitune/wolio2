using UnityEngine;
using UnityEngine.Scripting;
using System.Collections;
using System.IO;
using UnityStandardAssets.CrossPlatformInput;
using UniRx;
using UniRx.Triggers;

namespace Wolio
{
    public class GameCommand : MonoBehaviour
    {
        void Start()
        {
            var json = File.ReadAllText("test.json");
            var jsonObject = JsonUtility.FromJson<GameController>(json);
            Debug.Log(jsonObject.Right);
        }

        void Update()
        {

        }
    }
}