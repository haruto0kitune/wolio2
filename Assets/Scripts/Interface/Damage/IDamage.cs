using UnityEngine;
using System.Collections;

public interface IDamage
{
    void Damage(int damageValue, int recovery, int hitStop, bool isTechable = false);
    IEnumerator DamageCoroutine(int damageValue, int recovery, int hitStop, bool isTechable = false);
}