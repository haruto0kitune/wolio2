using UnityEngine;
using System.Collections;

public partial class PlayerMotion : MonoBehaviour
{
    public void Turn(/*float Horizontal*/)
    {
        //if ((Horizontal > 0 && !(GetComponent<PlayerState>().FacingRight.Value)) ||
        //    (Horizontal < 0 && GetComponent<PlayerState>().FacingRight.Value))
        //{
        //    GetComponent<PlayerState>().FacingRight.Value = !(GetComponent<PlayerState>().FacingRight.Value);
            GetComponent<Utility>().Flip();
        //}
    }
}
