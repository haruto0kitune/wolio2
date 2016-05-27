using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class Status : MonoBehaviour
{
    [InspectorDisplay]
    public IntReactiveProperty Hp;

    void Start()
    {

    }
}
