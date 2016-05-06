using UnityEngine;
using System.Collections;
using Player;

public class State : IState
{
    public void Action()
    {
        OnAction();
    }

    private void OnAction()
    {
        Debug.Log("StateUpdate");
    }
}
