using UnityEngine;
using System.Collections;

public partial class PlayerMotion : MonoBehaviour
{
    public void Turn()
    {
        Utility.Flip(SpriteRenderer);
    }
}
