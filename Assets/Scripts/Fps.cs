using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

public class Fps : MonoBehaviour
{
    public Text Text;

    void Start()
    {
        FPSCounter.Current.SubscribeToText(Text);
    }
}