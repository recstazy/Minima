using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyTargetable
{
    void UpdateTargets(List<DamageTarget> targets);
}
