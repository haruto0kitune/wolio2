using UnityEngine;
using System.Collections;

public interface IDamage
{
    IEnumerator Damage(int damageValue, int recover); 
}