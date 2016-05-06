using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using System.Linq;
using Player;

public class study1 : MonoBehaviour
{
    IState State;
    //State State2;

    void Start()
    {
        State = new State();
        //State2 = new State();
    }

    void Update()
    {
        State.Action();
        //State2.Action();
    }
}
