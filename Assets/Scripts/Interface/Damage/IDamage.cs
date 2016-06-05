using UnityEngine;
using System.Collections;

public interface IDamage
{
    void Damage(int damageValue, int recovery, int hitStop);
    IEnumerator DamageCoroutine(int damageValue, int recovery, int hitStop);
}