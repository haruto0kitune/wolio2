using UnityEngine;
using System;
using System.Collections;

public enum AttackAttribute
{
    low,
    overhead,
    high
}

public enum KnockdownAttribute
{
    supineKnockdown,
    proneKnockdown
}

[Flags]
public enum ControlMode
{
    ActionMode = 1 << 0,
    FightingMode = 1 << 1
}