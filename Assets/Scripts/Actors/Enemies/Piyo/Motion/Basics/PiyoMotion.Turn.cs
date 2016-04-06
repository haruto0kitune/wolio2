using UnityEngine;
using System.Collections;

public partial class PiyoMotion : MonoBehaviour
{
    public void Turn()
    {
        Utility.Flip(SpriteRenderer);
    }
}
