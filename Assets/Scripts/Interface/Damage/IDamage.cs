using UnityEngine;
using System.Collections;

public interface IDamage
{
    void Damage(int damageValue, int recovery, int hitStop, bool isTechable, bool hasKnockdownAttribute, AttackAttribute attackAttribute);
    IEnumerator DamageCoroutine(int damageValue, int recovery, int hitStop, bool isTechable, bool hasKnockdownAttribute, AttackAttribute attackAttribute);
}