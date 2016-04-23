using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class SetFPS : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 60;
        Time.captureFramerate = 60;
    }

    void Start()
    {
 
    }
}
