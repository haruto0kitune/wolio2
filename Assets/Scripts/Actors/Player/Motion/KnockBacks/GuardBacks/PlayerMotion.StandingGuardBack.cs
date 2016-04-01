using UnityEngine;
using System.Collections;

public partial class PlayerMotion : MonoBehaviour
{
    public IEnumerator StandingGuardBack()
    {
        var VelocityStore = Rigidbody2D.velocity;

        for (int i = 0; i < 3; i++)
        {

        }
        yield return null;
    }
}
