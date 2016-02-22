using UnityEngine;
using System.Collections;

public partial class PlayerMotion : MonoBehaviour
{
    public void Turn()
    {
        GetComponent<Utility>().Flip();
    }
}
