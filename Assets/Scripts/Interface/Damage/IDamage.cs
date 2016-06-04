using UnityEngine;
using System.Collections;

public interface IDamage
{
    void Damage(int damageValue, int recovery);
    IEnumerator DamageCoroutine(int damageValue, int recovery); 
}