using UnityEngine;
using System.Collections;

public class Utility : MonoBehaviour
{
    public void Flip()
    {
        GetComponent<SpriteRenderer>().flipX = !(GetComponent<SpriteRenderer>().flipX);
    }
}