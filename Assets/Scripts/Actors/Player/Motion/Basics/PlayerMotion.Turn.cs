using UnityEngine;
using System.Collections;

public partial class PlayerMotion
{
    public void Turn()
    {
        Utility.Flip(SpriteRenderer);
    }
}
